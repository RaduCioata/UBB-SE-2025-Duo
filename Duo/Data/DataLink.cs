using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Duo.Interfaces;

namespace Duo.Data
{
    /// <summary>
    /// Provides database access operations through SQL stored procedures.
    /// </summary>
    public class DataLink : IDataLink
    {
        private SqlConnection sqlConnection;
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLink"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public DataLink(IConfiguration configuration)
        {
            string? localDataSource = configuration["LocalDataSource"];
            string? initialCatalog = configuration["InitialCatalog"];

            connectionString = "Data Source=" + localDataSource + ";" +
                              "Initial Catalog=" + initialCatalog + ";" +
                              "Integrated Security=True;" +
                              "TrustServerCertificate=True";

            try
            {
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing SQL connection: {ex.Message}");
            }
        }

        /// <summary>
        /// Opens the database connection if it's not already open.
        /// </summary>
        public void OpenConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        }

        /// <summary>
        /// Closes the database connection if it's not already closed.
        /// </summary>
        public void CloseConnection()
        {
            if (sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns a scalar value.
        /// </summary>
        /// <typeparam name="T">The type of scalar value to return.</typeparam>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>The scalar result of the query.</returns>
        public T ExecuteScalar<T>(string storedProcedureName, SqlParameter[]? parameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var result = command.ExecuteScalar();
                    if (result == DBNull.Value || result == null)
                    {
                        return default;
                    }

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing scalar query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns a DataTable.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>A DataTable containing the results of the query.</returns>
        public DataTable ExecuteReader(string storedProcedureName, SqlParameter[]? parameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing reader query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Executes a stored procedure that does not return a result set.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string storedProcedureName, SqlParameter[]? parameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing non-query: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
