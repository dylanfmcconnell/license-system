using System.Data;
using Microsoft.Data.SqlClient;

/// <summary>
/// Provides database connection management for SQL Server using the configured connection string.
/// </summary>
public sealed class DatabaseConnection
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
    /// </summary>
    /// <param name="connectionString">The SQL Server connection string.</param>
    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates and opens a new SQL Server database connection.
    /// </summary>
    /// <returns>An open <see cref="IDbConnection"/> instance.</returns>
    /// <exception cref="SqlException">Thrown when the connection cannot be established.</exception>
    public IDbConnection GetConnection()
    {
        var connection = new SqlConnection(_connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
            connection.Dispose();
            throw;
        }
    }
}
