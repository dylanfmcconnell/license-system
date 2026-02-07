using Dapper;

/// <summary>
/// Repository implementation for managing <see cref="LicenseType"/> entities in the database.
/// </summary>
public sealed class LicenseTypeRepository : IRepository<LicenseType, int>
{
    private readonly DatabaseConnection _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseTypeRepository"/> class.
    /// </summary>
    /// <param name="db">The database connection provider.</param>
    public LicenseTypeRepository(DatabaseConnection db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<LicenseType?> GetEntityById(int id)
    {
        using var connection = _db.GetConnection();

        var sql = "SELECT * FROM LicenseType WHERE Id = @id";
        var licenseType = await connection.QueryFirstOrDefaultAsync<LicenseType>(sql, new { id });

        if (licenseType == null)
            return null;

        var categorySql = "SELECT * FROM LicenseCategory WHERE Id = @CategoryId";
        licenseType.Category = await connection.QueryFirstOrDefaultAsync<LicenseCategory>(
            categorySql,
            new { licenseType.CategoryId }
        );

        licenseType.LicenseClass =
            Type.GetType($"LicenseSystem.Models.{licenseType.LicenseClassName}") ?? typeof(License);

        return licenseType;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LicenseType>> GetAllEntities()
    {
        using var connection = _db.GetConnection();

        var sql = "SELECT * FROM LicenseType";
        var licenseTypes = (await connection.QueryAsync<LicenseType>(sql)).ToList();

        var categoryIds = licenseTypes.Select(lt => lt.CategoryId).Distinct().ToList();
        var categoriesSql = "SELECT * FROM LicenseCategory WHERE Id IN @Ids";
        var categories = (
            await connection.QueryAsync<LicenseCategory>(categoriesSql, new { Ids = categoryIds })
        ).ToDictionary(c => c.Id);

        foreach (var licenseType in licenseTypes)
        {
            if (categories.TryGetValue(licenseType.CategoryId, out var category))
            {
                licenseType.Category = category;
            }
            licenseType.LicenseClass =
                Type.GetType($"LicenseSystem.Models.{licenseType.LicenseClassName}")
                ?? typeof(License);
        }

        return licenseTypes;
    }

    /// <inheritdoc />
    public async Task<LicenseType?> AddEntity(LicenseType entity)
    {
        using var connection = _db.GetConnection();

        var sql = """
                INSERT INTO LicenseType (Name, CategoryId, LicenseClassName, ExpirationTime, Fee, Description)
                VALUES (@Name, @CategoryId, @LicenseClassName, @ExpirationTime, @Fee, @Description);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        var newId = await connection.QuerySingleOrDefaultAsync<int?>(
            sql,
            new
            {
                entity.Name,
                entity.CategoryId,
                LicenseClassName = entity.LicenseClass?.Name ?? "License",
                entity.ExpirationTime,
                entity.Fee,
                entity.Description,
            }
        );

        if (newId == null)
            return null;

        entity.Id = newId.Value;
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateEntity(LicenseType entity)
    {
        using var connection = _db.GetConnection();

        var sql = """
                UPDATE LicenseType
                SET Name = @Name, CategoryId = @CategoryId, LicenseClassName = @LicenseClassName, 
                    ExpirationTime = @ExpirationTime, Fee = @Fee, Description = @Description
                WHERE Id = @Id
            """;

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                entity.Name,
                entity.CategoryId,
                LicenseClassName = entity.LicenseClass?.Name ?? "License",
                entity.ExpirationTime,
                entity.Fee,
                entity.Description,
                entity.Id,
            }
        );

        return rowsAffected == 1;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteEntity(int id)
    {
        using var connection = _db.GetConnection();

        var sql = "DELETE FROM LicenseType WHERE Id = @id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { id });

        return rowsAffected == 1;
    }
}
