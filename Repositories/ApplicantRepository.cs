using Dapper;

public class ApplicantRepository : IRepository<Applicant, int>
{
    private readonly DatabaseConnection _db;

    public ApplicantRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<Applicant?> GetEntityById(int id)
    {
        using var connection = _db.GetConnection();
        
        var applicantSql = "SELECT * FROM Applicant WHERE Id = @id";
        var applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(applicantSql, new { id });
        
        if (applicant == null) return null;
        
        var applicationsSql = "SELECT * FROM Application WHERE ApplicantId = @Id";
        var applications = await connection.QueryAsync<Application>(applicationsSql, new { id });
        
        var licensesSql = "SELECT * FROM License WHERE ApplicantId = @Id";
        var licenses = await connection.QueryAsync<License>(licensesSql, new { id });

        applicant.Applications = [.. applications];
        applicant.Licenses = [.. licenses];

        return applicant;
    }

    public async Task<IEnumerable<Applicant>> GetAllEntities()
    {
        using var connection = _db.GetConnection();
        
        var applicantSql = "SELECT * FROM Applicant";
        var applicants = (await connection.QueryAsync<Applicant>(applicantSql)).ToList();

        var applicantIds = applicants.Select(a => a.Id).ToList();

        var applicationsSql = "SELECT * FROM Application WHERE ApplicantId IN @Ids";
        var allApplications = await connection.QueryAsync<Application>(applicationsSql, new { Ids = applicantIds });

        var licensesSql = "SELECT * FROM License WHERE ApplicantId IN @Ids";
        var allLicenses = await connection.QueryAsync<License>(licensesSql, new { Ids = applicantIds });

        foreach (var applicant in applicants)
        {
            applicant.Applications = allApplications.Where(a => a.ApplicantId == applicant.Id).ToList();
            applicant.Licenses = allLicenses.Where(l => l.ApplicantId == applicant.Id).ToList();
        }

        return applicants;
    }

    public async Task<bool> AddEntity(Applicant entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            INSERT INTO Applicant (FirstName, LastName, DateJoined, DateOfBirth, Address, Email, PhoneNumber)
            VALUES (@FirstName, @LastName, @DateJoined, @DateOfBirth, @Address, @Email, @PhoneNumber)
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { entity.FirstName, entity.LastName, entity.DateJoined, entity.DateOfBirth, entity.Address, entity.Email, entity.PhoneNumber });
        
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateEntity(Applicant entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            UPDATE Applicant
            SET FirstName = @FirstName, LastName = @LastName, DateJoined = @DateJoined, DateOfBirth = @DateOfBirth, Address = @Address, Email = @Email, PhoneNumber = @PhoneNumber
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { entity.FirstName, entity.LastName, entity.DateJoined, entity.DateOfBirth, entity.Address, entity.Email, entity.PhoneNumber, entity.Id });
        
        return rowsAffected == 1;
    }

    public async Task<bool> DeleteEntity(int id)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            DELETE FROM Applicant
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { id });
        
        return rowsAffected == 1;
    }
}
