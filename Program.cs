// AI generated TESTS
using Dapper;

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=LicenseSystem;Integrated Security=true;";
var db = new DatabaseConnection(connectionString);

// Repositories
var categoryRepo = new LicenseCategoryRepository(db);
var licenseTypeRepo = new LicenseTypeRepository(db);
var applicantRepo = new ApplicantRepository(db);
var applicationRepo = new ApplicationRepository(db);
var licenseRepo = new LicenseRepository(db);

await RunTests(categoryRepo, licenseTypeRepo, applicantRepo, applicationRepo, licenseRepo);

static async Task RunTests(
    LicenseCategoryRepository categoryRepo,
    LicenseTypeRepository licenseTypeRepo,
    ApplicantRepository applicantRepo,
    ApplicationRepository applicationRepo,
    LicenseRepository licenseRepo)
{
    TestRunner.Reset();

    // ========== LicenseCategory Tests ==========
    TestRunner.Describe("LicenseCategory Repository");

    // Add
    var newCategory = new LicenseCategory { Name = $"Test Category {Guid.NewGuid()}", Description = "Test description" };
    var categoryAdded = await categoryRepo.AddEntity(newCategory);
    TestRunner.Expect(categoryAdded, "AddEntity returns true");

    // GetAll
    var allCategories = (await categoryRepo.GetAllEntities()).ToList();
    TestRunner.ExpectGreaterThan(allCategories.Count, 0, "GetAllEntities returns results");

    // Find the one we just added
    var addedCategory = allCategories.FirstOrDefault(c => c.Name == newCategory.Name);
    TestRunner.ExpectNotNull(addedCategory, "Added category exists in GetAll");

    // GetById
    if (addedCategory != null)
    {
        var fetchedCategory = await categoryRepo.GetEntityById(addedCategory.Id);
        TestRunner.ExpectNotNull(fetchedCategory, "GetEntityById returns result");
        TestRunner.ExpectEqual(addedCategory.Name, fetchedCategory?.Name, "GetEntityById returns correct entity");

        // Update
        fetchedCategory!.Description = "Updated description";
        var categoryUpdated = await categoryRepo.UpdateEntity(fetchedCategory);
        TestRunner.Expect(categoryUpdated, "UpdateEntity returns true");

        var updatedCategory = await categoryRepo.GetEntityById(fetchedCategory.Id);
        TestRunner.ExpectEqual("Updated description", updatedCategory?.Description, "UpdateEntity persists changes");

        // Delete
        var categoryDeleted = await categoryRepo.DeleteEntity(fetchedCategory.Id);
        TestRunner.Expect(categoryDeleted, "DeleteEntity returns true");

        var deletedCategory = await categoryRepo.GetEntityById(fetchedCategory.Id);
        TestRunner.ExpectNull(deletedCategory, "Deleted category no longer exists");
    }

    // ========== Applicant Tests ==========
    TestRunner.Describe("Applicant Repository");

    var newApplicant = new Applicant
    {
        FirstName = "John",
        LastName = "Doe",
        DateJoined = DateOnly.FromDateTime(DateTime.Now),
        DateOfBirth = new DateOnly(1990, 5, 15),
        Address = "123 Test St",
        Email = $"test{Guid.NewGuid()}@example.com",
        PhoneNumber = "555-1234"
    };
    var applicantAdded = await applicantRepo.AddEntity(newApplicant);
    TestRunner.Expect(applicantAdded, "AddEntity returns true");

    var allApplicants = (await applicantRepo.GetAllEntities()).ToList();
    TestRunner.ExpectGreaterThan(allApplicants.Count, 0, "GetAllEntities returns results");

    var addedApplicant = allApplicants.FirstOrDefault(a => a.Email == newApplicant.Email);
    TestRunner.ExpectNotNull(addedApplicant, "Added applicant exists in GetAll");

    if (addedApplicant != null)
    {
        var fetchedApplicant = await applicantRepo.GetEntityById(addedApplicant.Id);
        TestRunner.ExpectNotNull(fetchedApplicant, "GetEntityById returns result");
        TestRunner.ExpectEqual("John", fetchedApplicant?.FirstName, "GetEntityById returns correct entity");

        // Update
        fetchedApplicant!.FirstName = "Jane";
        var applicantUpdated = await applicantRepo.UpdateEntity(fetchedApplicant);
        TestRunner.Expect(applicantUpdated, "UpdateEntity returns true");

        var updatedApplicant = await applicantRepo.GetEntityById(fetchedApplicant.Id);
        TestRunner.ExpectEqual("Jane", updatedApplicant?.FirstName, "UpdateEntity persists changes");

        // Delete
        var applicantDeleted = await applicantRepo.DeleteEntity(fetchedApplicant.Id);
        TestRunner.Expect(applicantDeleted, "DeleteEntity returns true");

        var deletedApplicant = await applicantRepo.GetEntityById(fetchedApplicant.Id);
        TestRunner.ExpectNull(deletedApplicant, "Deleted applicant no longer exists");
    }

    // ========== LicenseType Tests ==========
    TestRunner.Describe("LicenseType Repository");

    // First we need a category for the LicenseType
    var typeCategory = new LicenseCategory { Name = $"Type Test Category {Guid.NewGuid()}" };
    await categoryRepo.AddEntity(typeCategory);
    var savedCategory = (await categoryRepo.GetAllEntities()).First(c => c.Name == typeCategory.Name);

    var newLicenseType = new LicenseType
    {
        Name = $"Test License Type {Guid.NewGuid()}",
        CategoryId = savedCategory.Id,
        Category = savedCategory,
        LicenseClass = typeof(License),
        ExpirationTime = 24,
        Fee = 50.00m,
        Description = "Test license type"
    };
    var licenseTypeAdded = await licenseTypeRepo.AddEntity(newLicenseType);
    TestRunner.Expect(licenseTypeAdded, "AddEntity returns true");

    var allLicenseTypes = (await licenseTypeRepo.GetAllEntities()).ToList();
    TestRunner.ExpectGreaterThan(allLicenseTypes.Count, 0, "GetAllEntities returns results");

    var addedLicenseType = allLicenseTypes.FirstOrDefault(lt => lt.Name == newLicenseType.Name);
    TestRunner.ExpectNotNull(addedLicenseType, "Added license type exists in GetAll");

    if (addedLicenseType != null)
    {
        var fetchedLicenseType = await licenseTypeRepo.GetEntityById(addedLicenseType.Id);
        TestRunner.ExpectNotNull(fetchedLicenseType, "GetEntityById returns result");
        TestRunner.ExpectNotNull(fetchedLicenseType?.Category, "GetEntityById loads Category relationship");
        TestRunner.ExpectEqual(24, fetchedLicenseType?.ExpirationTime, "GetEntityById returns correct ExpirationTime");

        // Update
        fetchedLicenseType!.Fee = 75.00m;
        var licenseTypeUpdated = await licenseTypeRepo.UpdateEntity(fetchedLicenseType);
        TestRunner.Expect(licenseTypeUpdated, "UpdateEntity returns true");

        var updatedLicenseType = await licenseTypeRepo.GetEntityById(fetchedLicenseType.Id);
        TestRunner.ExpectEqual(75.00m, updatedLicenseType?.Fee, "UpdateEntity persists changes");

        // Delete
        var licenseTypeDeleted = await licenseTypeRepo.DeleteEntity(fetchedLicenseType.Id);
        TestRunner.Expect(licenseTypeDeleted, "DeleteEntity returns true");
    }

    // Cleanup category
    await categoryRepo.DeleteEntity(savedCategory.Id);

    // ========== License Tests ==========
    TestRunner.Describe("License Repository");

    // Setup: need category, license type, and applicant
    var licenseCategory = new LicenseCategory { Name = $"License Test Category {Guid.NewGuid()}" };
    await categoryRepo.AddEntity(licenseCategory);
    licenseCategory = (await categoryRepo.GetAllEntities()).First(c => c.Name == licenseCategory.Name);

    var licenseType = new LicenseType
    {
        Name = $"License Test Type {Guid.NewGuid()}",
        CategoryId = licenseCategory.Id,
        Category = licenseCategory,
        LicenseClass = typeof(License),
        ExpirationTime = 12,
        Fee = 25.00m
    };
    await licenseTypeRepo.AddEntity(licenseType);
    licenseType = (await licenseTypeRepo.GetAllEntities()).First(lt => lt.Name == licenseType.Name);

    var licenseApplicant = new Applicant
    {
        FirstName = "License",
        LastName = "Tester",
        DateJoined = DateOnly.FromDateTime(DateTime.Now),
        DateOfBirth = new DateOnly(1985, 3, 20),
        Address = "456 License Ave",
        Email = $"license{Guid.NewGuid()}@test.com"
    };
    await applicantRepo.AddEntity(licenseApplicant);
    licenseApplicant = (await applicantRepo.GetAllEntities()).First(a => a.Email == licenseApplicant.Email);

    var newLicense = new License
    {
        Id = $"LIC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
        LicenseTypeId = licenseType.Id,
        LicenseType = licenseType,
        ApplicantId = licenseApplicant.Id,
        Applicant = licenseApplicant,
        FirstName = licenseApplicant.FirstName,
        LastName = licenseApplicant.LastName,
        Address = licenseApplicant.Address!,
        DateOfBirth = licenseApplicant.DateOfBirth,
        IssueDate = DateOnly.FromDateTime(DateTime.Now),
        ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(12)),
        Status = LicenseStatus.Valid
    };
    var licenseAdded = await licenseRepo.AddEntity(newLicense);
    TestRunner.Expect(licenseAdded, "AddEntity returns true");

    var fetchedLicense = await licenseRepo.GetEntityById(newLicense.Id);
    TestRunner.ExpectNotNull(fetchedLicense, "GetEntityById returns result");
    TestRunner.ExpectNotNull(fetchedLicense?.Applicant, "GetEntityById loads Applicant relationship");
    TestRunner.ExpectNotNull(fetchedLicense?.LicenseType, "GetEntityById loads LicenseType relationship");
    TestRunner.ExpectEqual(LicenseStatus.Valid, fetchedLicense?.Status, "GetEntityById returns correct Status");

    // Update
    fetchedLicense!.Status = LicenseStatus.Suspended;
    var licenseUpdated = await licenseRepo.UpdateEntity(fetchedLicense);
    TestRunner.Expect(licenseUpdated, "UpdateEntity returns true");

    var updatedLicense = await licenseRepo.GetEntityById(fetchedLicense.Id);
    TestRunner.ExpectEqual(LicenseStatus.Suspended, updatedLicense?.Status, "UpdateEntity persists changes");

    // GetAll
    var allLicenses = (await licenseRepo.GetAllEntities()).ToList();
    TestRunner.ExpectGreaterThan(allLicenses.Count, 0, "GetAllEntities returns results");

    // Delete
    var licenseDeleted = await licenseRepo.DeleteEntity(fetchedLicense.Id);
    TestRunner.Expect(licenseDeleted, "DeleteEntity returns true");

    // Cleanup
    await applicantRepo.DeleteEntity(licenseApplicant.Id);
    await licenseTypeRepo.DeleteEntity(licenseType.Id);
    await categoryRepo.DeleteEntity(licenseCategory.Id);

    // ========== Application Tests ==========
    TestRunner.Describe("Application Repository");

    // Setup
    var appCategory = new LicenseCategory { Name = $"App Test Category {Guid.NewGuid()}" };
    await categoryRepo.AddEntity(appCategory);
    appCategory = (await categoryRepo.GetAllEntities()).First(c => c.Name == appCategory.Name);

    var appLicenseType = new LicenseType
    {
        Name = $"App Test Type {Guid.NewGuid()}",
        CategoryId = appCategory.Id,
        Category = appCategory,
        LicenseClass = typeof(License),
        Fee = 30.00m
    };
    await licenseTypeRepo.AddEntity(appLicenseType);
    appLicenseType = (await licenseTypeRepo.GetAllEntities()).First(lt => lt.Name == appLicenseType.Name);

    var appApplicant = new Applicant
    {
        FirstName = "App",
        LastName = "Tester",
        DateJoined = DateOnly.FromDateTime(DateTime.Now),
        DateOfBirth = new DateOnly(1992, 7, 10),
        Address = "789 App Blvd",
        Email = $"app{Guid.NewGuid()}@test.com"
    };
    await applicantRepo.AddEntity(appApplicant);
    appApplicant = (await applicantRepo.GetAllEntities()).First(a => a.Email == appApplicant.Email);

    var newApplication = new Application
    {
        ApplicantId = appApplicant.Id,
        Applicant = appApplicant,
        LicenseTypeId = appLicenseType.Id,
        LicenseType = appLicenseType,
        SubmissionDate = DateOnly.FromDateTime(DateTime.Now),
        DeliveryAddress = "789 App Blvd",
        ApprovedStatus = ApplicationStatus.UnderReview,
        Fee = 30.00m
    };
    var applicationAdded = await applicationRepo.AddEntity(newApplication);
    TestRunner.Expect(applicationAdded, "AddEntity returns true");

    var allApplications = (await applicationRepo.GetAllEntities()).ToList();
    TestRunner.ExpectGreaterThan(allApplications.Count, 0, "GetAllEntities returns results");

    var addedApplication = allApplications.FirstOrDefault(a => a.ApplicantId == appApplicant.Id);
    TestRunner.ExpectNotNull(addedApplication, "Added application exists in GetAll");

    if (addedApplication != null)
    {
        var fetchedApplication = await applicationRepo.GetEntityById(addedApplication.Id);
        TestRunner.ExpectNotNull(fetchedApplication, "GetEntityById returns result");
        TestRunner.ExpectNotNull(fetchedApplication?.Applicant, "GetEntityById loads Applicant relationship");
        TestRunner.ExpectNotNull(fetchedApplication?.LicenseType, "GetEntityById loads LicenseType relationship");
        TestRunner.ExpectEqual(ApplicationStatus.UnderReview, fetchedApplication?.ApprovedStatus, "GetEntityById returns correct status");

        // Update
        fetchedApplication!.ApprovedStatus = ApplicationStatus.Approved;
        fetchedApplication.ApprovedDate = DateOnly.FromDateTime(DateTime.Now);
        var applicationUpdated = await applicationRepo.UpdateEntity(fetchedApplication);
        TestRunner.Expect(applicationUpdated, "UpdateEntity returns true");

        var updatedApplication = await applicationRepo.GetEntityById(fetchedApplication.Id);
        TestRunner.ExpectEqual(ApplicationStatus.Approved, updatedApplication?.ApprovedStatus, "UpdateEntity persists status change");

        // Delete
        var applicationDeleted = await applicationRepo.DeleteEntity(fetchedApplication.Id);
        TestRunner.Expect(applicationDeleted, "DeleteEntity returns true");
    }

    // Cleanup
    await applicantRepo.DeleteEntity(appApplicant.Id);
    await licenseTypeRepo.DeleteEntity(appLicenseType.Id);
    await categoryRepo.DeleteEntity(appCategory.Id);

    // ========== Summary ==========
    TestRunner.Summary();
}