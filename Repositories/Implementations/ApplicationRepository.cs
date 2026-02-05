using Dapper;

public sealed class ApplicationRepository : IRepository<Application, int>
{
    private readonly DatabaseConnection _db;

    public ApplicationRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<Application?> GetEntityById(int id)
    {
        using var connection = _db.GetConnection();
        
        var sql = "SELECT * FROM Application WHERE Id = @id";
        var application = await connection.QueryFirstOrDefaultAsync<Application>(sql, new { id });
        
        if (application == null) return null;
        
        var applicantSql = "SELECT * FROM Applicant WHERE Id = @ApplicantId";
        application.Applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(applicantSql, new { application.ApplicantId });
        
        var licenseTypeSql = "SELECT * FROM LicenseType WHERE Id = @LicenseTypeId";
        application.LicenseType = await connection.QueryFirstOrDefaultAsync<LicenseType>(licenseTypeSql, new { application.LicenseTypeId });
        
        if (application.LicenseId != null)
        {
            var licenseSql = "SELECT * FROM License WHERE Id = @LicenseId";
            application.License = await connection.QueryFirstOrDefaultAsync<License>(licenseSql, new { application.LicenseId });
        }
        
        return application;
    }

    public async Task<IEnumerable<Application>> GetAllEntities()
    {
        using var connection = _db.GetConnection();
        
        var sql = "SELECT * FROM Application";
        var applications = (await connection.QueryAsync<Application>(sql)).ToList();
        
        var applicantIds = applications.Select(a => a.ApplicantId).Distinct().ToList();
        var applicantsSql = "SELECT * FROM Applicant WHERE Id IN @Ids";
        var applicants = (await connection.QueryAsync<Applicant>(applicantsSql, new { Ids = applicantIds })).ToDictionary(a => a.Id);
        
        var licenseTypeIds = applications.Select(a => a.LicenseTypeId).Distinct().ToList();
        var licenseTypesSql = "SELECT * FROM LicenseType WHERE Id IN @Ids";
        var licenseTypes = (await connection.QueryAsync<LicenseType>(licenseTypesSql, new { Ids = licenseTypeIds })).ToDictionary(lt => lt.Id);
        
        var licenseIds = applications.Where(a => a.LicenseId != null).Select(a => a.LicenseId).Distinct().ToList();
        var licenses = new Dictionary<string, License>();
        if (licenseIds.Any())
        {
            var licensesSql = "SELECT * FROM License WHERE Id IN @Ids";
            licenses = (await connection.QueryAsync<License>(licensesSql, new { Ids = licenseIds })).ToDictionary(l => l.Id);
        }
        
        foreach (var application in applications)
        {
            if (applicants.TryGetValue(application.ApplicantId, out var applicant))
                application.Applicant = applicant;
            
            if (licenseTypes.TryGetValue(application.LicenseTypeId, out var licenseType))
                application.LicenseType = licenseType;
            
            if (application.LicenseId != null && licenses.TryGetValue(application.LicenseId, out var license))
                application.License = license;
        }
        
        return applications;
    }

    public async Task<Application?> AddEntity(Application entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            INSERT INTO Application (ApplicantId, LicenseTypeId, SubmissionDate, ApprovedStatus, ApprovedDate, LicenseId, Fee, DeliveryAddress)
            VALUES (@ApplicantId, @LicenseTypeId, @SubmissionDate, @ApprovedStatus, @ApprovedDate, @LicenseId, @Fee, @DeliveryAddress);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;
        
        var newId = await connection.QuerySingleOrDefaultAsync<int?>(sql, new
        {
            entity.ApplicantId,
            entity.LicenseTypeId,
            entity.SubmissionDate,
            entity.ApprovedStatus,
            entity.ApprovedDate,
            entity.LicenseId,
            entity.Fee,
            entity.DeliveryAddress
        });
    
        if (newId == null) return null;
        
        entity.Id = newId.Value;
        return entity;
    }

    public async Task<bool> UpdateEntity(Application entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            UPDATE Application
            SET ApplicantId = @ApplicantId, LicenseTypeId = @LicenseTypeId, SubmissionDate = @SubmissionDate,
                ApprovedStatus = @ApprovedStatus, ApprovedDate = @ApprovedDate, LicenseId = @LicenseId, Fee = @Fee, DeliveryAddress = @DeliveryAddress
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new 
        { 
            entity.ApplicantId,
            entity.LicenseTypeId,
            entity.SubmissionDate,
            entity.ApprovedStatus,
            entity.ApprovedDate,
            entity.LicenseId,
            entity.Fee,
            entity.DeliveryAddress,
            entity.Id
        });
        
        return rowsAffected == 1;
    }

    public async Task<bool> DeleteEntity(int id)
    {
        using var connection = _db.GetConnection();
        
        var sql = "DELETE FROM Application WHERE Id = @id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { id });
        
        return rowsAffected == 1;
    }
}