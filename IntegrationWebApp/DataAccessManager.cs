using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationWebApp
{
    public  class DataAccessManager
    {
        #region Private Variables
        private const string className = "DataAccessManager";

        #endregion


        public DataTable ExecuteStoredProcedureForReadOperation(SqlConnection sqlConn, string storedProcedureName, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1200;

                if (parameters != null) { command.Parameters.AddRange(parameters); }

                try
                {
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dataTable.Load(queryResult);
                    }
                }
                catch (SqlException exception)
                {

                    throw;
                }
            }
            return dataTable;
        }

        public DataTable ExecuteStoredProcedureForReadOperation(SqlConnection sqlConn, string storedProcedureName)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1200;

                try
                {
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dataTable.Load(queryResult);
                    }
                }
                catch (SqlException exception)
                {

                    throw;
                }
            }
            return dataTable;
        }

        public void ExecuteStoredProcedureForInsertUpdateDeleteOperation(SqlConnection sqlConn, string storedProcedureName, ref SqlParameter[] parameters, SqlTransaction sqlTran)
        {
            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn, sqlTran))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        command.ExecuteNonQuery();

                        if (parameters != null)
                        {
                            foreach (SqlParameter sqlParam in parameters)
                            {
                                if (sqlParam.Direction.Equals(ParameterDirection.Output))
                                {
                                    sqlParam.Value = command.Parameters[sqlParam.ParameterName].Value;
                                }
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void ExecuteStoredProcedureForInsertUpdateDeleteOperation(SqlConnection sqlConn, string storedProcedureName, ref SqlParameter[] parameters)
        {
            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
                {
                    command.CommandTimeout = 1200;
                    //sqlConn.c

                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        command.ExecuteNonQuery();

                        if (parameters != null)
                        {
                            foreach (SqlParameter sqlParam in parameters)
                            {
                                if (sqlParam.Direction.Equals(ParameterDirection.Output))
                                {
                                    sqlParam.Value = command.Parameters[sqlParam.ParameterName].Value;
                                }
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        throw;
                    }
                }
            }
        }

        #region Param Callection methods

        public void ExecuteStoredProcedureForInsertUpdateDeleteOperation(SqlConnection sqlConn, string storedProcedureName, ref SqlParameterCollection paramCollection)
        {
            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1200;

                    if (paramCollection != null)
                    {
                        for (int i = 0; i < paramCollection.Count; i++)
                        {
                            SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                    , paramCollection[i].SqlDbType
                                                                    , paramCollection[i].Size
                                                                    , paramCollection[i].Direction
                                                                    , paramCollection[i].IsNullable
                                                                    , paramCollection[i].Precision
                                                                    , paramCollection[i].Scale
                                                                    , paramCollection[i].SourceColumn
                                                                    , DataRowVersion.Current
                                                                    , paramCollection[i].Value
                                                                    );
                            command.Parameters.Add(param1);
                        }
                    }

                    try
                    {
                        command.ExecuteNonQuery();

                        foreach (SqlParameter sqlParam in command.Parameters)
                        {
                            if (sqlParam.Direction.Equals(ParameterDirection.Output) || sqlParam.Direction.Equals(ParameterDirection.InputOutput))
                            {
                                paramCollection[sqlParam.ParameterName].Value = sqlParam.Value;
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void ExecuteStoredProcedureForInsertUpdateDeleteOperation(SqlConnection sqlConn, string storedProcedureName, ref SqlParameterCollection paramCollection, SqlTransaction sqlTran)
        {
            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn, sqlTran))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1200;
                    if (paramCollection != null)
                    {
                        for (int i = 0; i < paramCollection.Count; i++)
                        {
                            SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                    , paramCollection[i].SqlDbType
                                                                    , paramCollection[i].Size
                                                                    , paramCollection[i].Direction
                                                                    , paramCollection[i].IsNullable
                                                                    , paramCollection[i].Precision
                                                                    , paramCollection[i].Scale
                                                                    , paramCollection[i].SourceColumn
                                                                    , DataRowVersion.Current
                                                                    , paramCollection[i].Value
                                                                    );
                            command.Parameters.Add(param1);
                        }
                    }

                    try
                    {

                        command.ExecuteNonQuery();


                        if (paramCollection != null)
                        {
                            foreach (SqlParameter sqlParam in command.Parameters)
                            {
                                if (sqlParam.Direction.Equals(ParameterDirection.Output))
                                {
                                    paramCollection[sqlParam.ParameterName].Value = sqlParam.Value;
                                }
                            }
                        }

                    }
                    catch (SqlException exception)
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable ExecuteStoredProcedureForReadOperation(SqlConnection sqlConn, string storedProcedureName, SqlParameterCollection? paramCollection)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1200;
                if (paramCollection != null)
                {
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                , paramCollection[i].SqlDbType
                                                                , paramCollection[i].Size
                                                                , paramCollection[i].Direction
                                                                , paramCollection[i].IsNullable
                                                                , paramCollection[i].Precision
                                                                , paramCollection[i].Scale
                                                                , paramCollection[i].SourceColumn
                                                                , DataRowVersion.Current
                                                                , paramCollection[i].Value
                                                                );
                        command.Parameters.Add(param1);
                    }
                }

                try
                {
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dataTable.Load(queryResult);

                        foreach (SqlParameter sqlParam in command.Parameters)
                        {
                            if (sqlParam.Direction.Equals(ParameterDirection.Output))
                            {
                                paramCollection[sqlParam.ParameterName].Value = sqlParam.Value;
                            }
                        }
                    }
                }
                catch (SqlException exception)
                {
                    throw;
                }
            }
            return dataTable;
        }

        public DataTable ExecuteStoredProcedureForReadOperation(SqlConnection sqlConn, string storedProcedureName, SqlParameterCollection paramCollection, SqlTransaction sqlTran)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn, sqlTran))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (paramCollection != null)
                {
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                , paramCollection[i].SqlDbType
                                                                , paramCollection[i].Size
                                                                , paramCollection[i].Direction
                                                                , paramCollection[i].IsNullable
                                                                , paramCollection[i].Precision
                                                                , paramCollection[i].Scale
                                                                , paramCollection[i].SourceColumn
                                                                , DataRowVersion.Current
                                                                , paramCollection[i].Value
                                                                );
                        command.Parameters.Add(param1);
                    }
                }

                try
                {
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dataTable.Load(queryResult);
                    }
                }
                catch (SqlException exception)
                {
                    throw;
                }
            }
            return dataTable;
        }

        public DataSet ExecuteStoredProcedureForReadOperation(SqlConnection sqlConn, string storedProcedureName, SqlParameterCollection paramCollection, string[] tableNames)
        {
            DataSet dsRequested = new DataSet();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (paramCollection != null)
                {
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                , paramCollection[i].SqlDbType
                                                                , paramCollection[i].Size
                                                                , paramCollection[i].Direction
                                                                , paramCollection[i].IsNullable
                                                                , paramCollection[i].Precision
                                                                , paramCollection[i].Scale
                                                                , paramCollection[i].SourceColumn
                                                                , DataRowVersion.Current
                                                                , paramCollection[i].Value
                                                                );
                        command.Parameters.Add(param1);
                    }
                }

                try
                {
                    //using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    //{
                    //    adapter.Fill(dsRequested);

                    //}
                    DataTable table = null;
                    List<DataTable> tableList = new List<DataTable>();
                    foreach (string tableName in tableNames)
                    {
                        table = new DataTable(tableName);
                        tableList.Add(table);
                    }

                    dsRequested.Tables.AddRange(tableList.ToArray());
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dsRequested.Load(queryResult, LoadOption.OverwriteChanges, tableList.ToArray());
                    }
                }
                catch (SqlException exception)
                {

                    throw;
                }
            }
            return dsRequested;
        }
        public DataSet ExecuteStoredProcedureForReadOperationOutPut(SqlConnection sqlConn, string storedProcedureName, SqlParameterCollection paramCollection, string[] tableNames)
        {
            DataSet dsRequested = new DataSet();

            using (SqlCommand command = new SqlCommand(storedProcedureName, sqlConn))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (paramCollection != null)
                {
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        SqlParameter param1 = new SqlParameter(paramCollection[i].ParameterName
                                                                , paramCollection[i].SqlDbType
                                                                , paramCollection[i].Size
                                                                , paramCollection[i].Direction
                                                                , paramCollection[i].IsNullable
                                                                , paramCollection[i].Precision
                                                                , paramCollection[i].Scale
                                                                , paramCollection[i].SourceColumn
                                                                , DataRowVersion.Current
                                                                , paramCollection[i].Value
                                                                );
                        command.Parameters.Add(param1);
                    }
                }

                try
                {
                    //using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    //{
                    //    adapter.Fill(dsRequested);

                    //}
                    DataTable table = null;
                    List<DataTable> tableList = new List<DataTable>();
                    foreach (string tableName in tableNames)
                    {
                        table = new DataTable(tableName);
                        tableList.Add(table);
                    }

                    dsRequested.Tables.AddRange(tableList.ToArray());
                    using (SqlDataReader queryResult = command.ExecuteReader())
                    {
                        dsRequested.Load(queryResult, LoadOption.OverwriteChanges, tableList.ToArray());
                        foreach (SqlParameter sqlParam in command.Parameters)
                        {
                            if (sqlParam.Direction.Equals(ParameterDirection.Output))
                            {
                                paramCollection[sqlParam.ParameterName].Value = sqlParam.Value;
                            }
                        }

                    }
                }
                catch (SqlException exception)
                {

                    throw;
                }
            }
            return dsRequested;
        }




        #endregion Param Callection methods


        #region Get Connection Object

        public SqlConnection GetConnection(string strConnection, out SqlTransaction sqlTran)
        {
            SqlConnection sqlConn = null;
            sqlTran = null;

            if (!String.IsNullOrEmpty(strConnection))
            {
                sqlConn = new SqlConnection(strConnection);
                sqlConn.Open();
                sqlTran = sqlConn.BeginTransaction();
            }
            return sqlConn;
        }

        public SqlConnection GetConnection(string strConnection)
        {
            SqlConnection sqlConn = null;

            if (!String.IsNullOrEmpty(strConnection))
            {
                sqlConn = new SqlConnection(strConnection);
                sqlConn.Open();
            }
            return sqlConn;
        }

        #endregion Get Connection Object
    }
}
