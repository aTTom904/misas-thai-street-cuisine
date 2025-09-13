namespace MisasThaiApi.Services;

using Microsoft.Data.SqlClient;
using System.Data;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<SqlConnection> GetConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = await GetConnectionAsync();
            var command = new SqlCommand("SELECT 1", connection);
            var result = await command.ExecuteScalarAsync();
            return result?.ToString() == "1";
        }
        catch
        {
            return false;
        }
    }

    public async Task InitializeDatabaseAsync()
    {
        // This method can be used to ensure tables exist
        // In production, you'd run the SQL script manually
        using var connection = await GetConnectionAsync();

        var checkTablesQuery = @"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_NAME IN ('Orders', 'OrderItems', 'Customers')";

        var command = new SqlCommand(checkTablesQuery, connection);
        var tableCount = (int)await command.ExecuteScalarAsync();

        if (tableCount < 3)
        {
            throw new InvalidOperationException(
                "Database tables not found. Please run the CreateSchema.sql script first.");
        }
    }
}