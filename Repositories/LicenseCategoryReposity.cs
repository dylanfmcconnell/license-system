using Dapper;
using Microsoft.Data.SqlClient;

public class LicenseCategoryRepository : IRepository<LicenseCategory, int>
{
    private readonly DatabaseConnection _db;

    public LicenseCategoryRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<LicenseCategory?> GetEntityById(int id)
    {
        // Manual SQL access no Dapper for practice

        using var connection = _db.GetConnection();

        var sql = """
        SELECT 
            c.Id, c.Name, c.Description,
            t.Id, t.Name, t.CategoryId, t.LicenseClassName, t.ExpirationTime, t.Fee, t.Description
        FROM LicenseCategory c
        LEFT JOIN LicenseType t ON c.Id = t.CategoryId
        WHERE c.Id = @Id
        """;

        using var command = new SqlCommand(sql, (SqlConnection)connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        LicenseCategory? category = null;

        while (await reader.ReadAsync())
        {
            if (category == null)
            {
                category = new LicenseCategory
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                };
            }
            
            if (!reader.IsDBNull(3))
            {   
                var className = reader.GetString(6);
                var licenseClass = Type.GetType($"LicenseSystem.Models.{className}") ?? typeof(License);

                category.LicenseTypes.Add(new LicenseType
                {
                    Id = reader.GetInt32(3),
                    Name = reader.GetString(4),
                    Category = category,
                    LicenseClass = licenseClass,
                    ExpirationTime = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    Fee = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                    Description = reader.IsDBNull(9) ? null : reader.GetString(9),
                });
            }
        }

        return category;
    }

    public async Task<IEnumerable<LicenseCategory>> GetAllEntities()
    {
        // Manual SQL access no Dapper for practice

        using var connection = _db.GetConnection();

        var sql = """
        SELECT 
            c.Id, c.Name, c.Description,
            t.Id, t.Name, t.CategoryId, t.LicenseClassName, t.ExpirationTime, t.Fee, t.Description
        FROM LicenseCategory c
        LEFT JOIN LicenseType t ON c.Id = t.CategoryId
        """;

        using var command = new SqlCommand(sql, (SqlConnection)connection);
        using var reader = await command.ExecuteReaderAsync();

        var categoriesDict = new Dictionary<int, LicenseCategory>();

        while (await reader.ReadAsync())
        {
            var categoryId = reader.GetInt32(0);
            
            if (!categoriesDict.TryGetValue(categoryId, out var category))
            {
                category = new LicenseCategory
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                };
                categoriesDict[categoryId] = category;
            }
            
            if (!reader.IsDBNull(3))
            {
                var className = reader.GetString(6);
                var licenseClass = Type.GetType($"LicenseSystem.Models.{className}") ?? typeof(License);

                category.LicenseTypes.Add(new LicenseType
                {
                    Id = reader.GetInt32(3),
                    Name = reader.GetString(4),
                    Category = categoriesDict[categoryId],
                    LicenseClass = licenseClass,
                    ExpirationTime = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    Fee = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                    Description = reader.IsDBNull(9) ? null : reader.GetString(9),
                });
            }
        }

        return categoriesDict.Values;
    }
    
    public async Task<bool> AddEntity(LicenseCategory entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            INSERT INTO LicenseCategory (Name, Description)
            VALUES (@Name, @Description)
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { entity.Name, entity.Description });
        
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateEntity(LicenseCategory entity)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            UPDATE LicenseCategory
            SET Name = @Name, Description = @Description
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { entity.Name, entity.Description, entity.Id });
        
        return rowsAffected == 1;
    }  

    public async Task<bool> DeleteEntity(int id)
    {
        using var connection = _db.GetConnection();
        
        var sql = """
            DELETE FROM LicenseCategory
            WHERE Id = @Id
        """;
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { id });
        
        return rowsAffected == 1;
    }
}