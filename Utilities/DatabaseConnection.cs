using System;
using Microsoft.Data.SqlClient;
using System.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

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