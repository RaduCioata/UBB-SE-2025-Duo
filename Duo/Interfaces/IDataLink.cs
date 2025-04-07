using System.Data;
using Microsoft.Data.SqlClient;

namespace Duo.Interfaces
{
    /// <summary>
    /// Defines the contract for database access operations.
    /// </summary>
    public interface IDataLink
    {
        /// <summary>
        /// Executes a stored procedure that returns a DataTable.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>A DataTable containing the results of the query.</returns>
        DataTable ExecuteReader(string storedProcedureName, SqlParameter[]? parameters = null);

        /// <summary>
        /// Executes a stored procedure that returns a scalar value.
        /// </summary>
        /// <typeparam name="T">The type of scalar value to return.</typeparam>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>The scalar result of the query.</returns>
        T ExecuteScalar<T>(string storedProcedureName, SqlParameter[]? parameters = null);

        /// <summary>
        /// Executes a stored procedure that does not return a result set.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional SQL parameters for the stored procedure.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string storedProcedureName, SqlParameter[]? parameters = null);
    }
} 