using Dapper;

public class LicenseRepository : IRepository<License, string>
{
    private readonly DatabaseConnection _db;

    public LicenseRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<License?> GetEntityById(string id)
    {
        using var connection = _db.GetConnection();
        
        var sql = "SELECT * FROM License WHERE Id = @id";
        var license = await connection.QueryFirstOrDefaultAsync<License>(sql, new { id });
        
        if (license == null) return null;
        
        var applicantSql = "SELECT * FROM Applicant WHERE Id = @ApplicantId";
        license.Applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(applicantSql, new { license.ApplicantId });
        
        var licenseTypeSql = "SELECT * FROM LicenseType WHERE Id = @LicenseTypeId";
        license.LicenseType = await connection.QueryFirstOrDefaultAsync<LicenseType>(licenseTypeSql, new { license.LicenseTypeId });
        
        return license;
    }

    public async Task<IEnumerable<License>> GetAllEntities()
    {
        using var connection = _db.GetConnection();
        
        var sql = "SELECT * FROM License";
        var licenses = (await connection.QueryAsync<License>(sql)).ToList();

        var applicantIds = licenses.Select(l => l.ApplicantId).Distinct().ToList();
        var applicantsSql = "SELECT * FROM Applicant WHERE Id IN @Ids";
        var applicants = (await connection.QueryAsync<Applicant>(applicantsSql, new { Ids = applicantIds })).ToDictionary(a => a.Id);
        
        var licenseTypeIds = licenses.Select(l => l.LicenseTypeId).Distinct().ToList();
        var licenseTypesSql = "SELECT * FROM LicenseType WHERE Id IN @Ids";
        var licenseTypes = (await connection.QueryAsync<LicenseType>(licenseTypesSql, new { Ids = licenseTypeIds })).ToDictionary(lt => lt.Id);
        
        foreach (var license in licenses)
        {
            if (applicants.TryGetValue(license.ApplicantId, out var applicant))
                license.Applicant = applicant;
            
            if (licenseTypes.TryGetValue(license.LicenseTypeId, out var licenseType))
                license.LicenseType = licenseType;
        }
        
        return licenses;
    }

    public async Task<bool> AddEntity(License entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            INSERT INTO License (Id, LicenseTypeId, ApplicantId, FirstName, LastName, Address, DateOfBirth, IssueDate, Expiration, Status)
            VALUES (@Id, @LicenseTypeId, @ApplicantId, @FirstName, @LastName, @Address, @DateOfBirth, @IssueDate, @Expiration, @Status)
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new 
        { 
            entity.Id,
            entity.LicenseTypeId,
            entity.ApplicantId,
            entity.FirstName,
            entity.LastName,
            entity.Address,
            entity.DateOfBirth,
            entity.IssueDate,
            Expiration = entity.ExpirationDate,
            Status = entity.Status.ToString()
        });
        
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateEntity(License entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            UPDATE License
            SET LicenseTypeId = @LicenseTypeId, ApplicantId = @ApplicantId, FirstName = @FirstName, LastName = @LastName,
                Address = @Address, DateOfBirth = @DateOfBirth, IssueDate = @IssueDate, Expiration = @Expiration, Status = @Status
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new 
        { 
            entity.LicenseTypeId,
            entity.ApplicantId,
            entity.FirstName,
            entity.LastName,
            entity.Address,
            entity.DateOfBirth,
            entity.IssueDate,
            Expiration = entity.ExpirationDate,
            Status = entity.Status.ToString(),
            entity.Id
        });
        
        return rowsAffected == 1;
    }

    public async Task<bool> DeleteEntity(string id)
    {
        using var connection = _db.GetConnection();
        
        var sql = "DELETE FROM License WHERE Id = @id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { id });
        
        return rowsAffected == 1;
    }
}