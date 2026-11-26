using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BankAtmSystem.DAL
{
    public static class DatabaseHelper
    {
        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        var setting = ConfigurationManager.ConnectionStrings["BankAtmDBConn"];
                        if (setting != null && !string.IsNullOrWhiteSpace(setting.ConnectionString))
                        {
                            _connectionString = setting.ConnectionString;
                        }
                    }
                    catch
                    {
                    }

                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        _connectionString = "Server=(localdb)\\mssqllocaldb;Database=BankAtmDB;Integrated Security=True;TrustServerCertificate=True;";
                    }
                }
                return _connectionString;
            }
            set => _connectionString = value;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string message)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    message = $"Đã kết nối Core Banking ({conn.Database})";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = $"Mất kết nối Core Banking ({ex.Message})";
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
