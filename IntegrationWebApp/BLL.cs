using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using static Program;
using System.Numerics;

namespace IntegrationWebApp
{
    public class BLL
    {
        #region Category
        public SynckCategory_Response_Message GetSynckData(string SynckCategory)
        {
            SynckCategory_Response_Message response = new SynckCategory_Response_Message();
            DataAccessManager dataAccessManager = new DataAccessManager();
            SqlConnection sqlConn = dataAccessManager.GetConnection(BalHelper.GetConnectionLive());
            DataTable queryResult = new DataTable();
            try
            {
                // Prepare required parameters to be passed in the stored procedure   
                SqlParameterCollection paramCollection = (SqlParameterCollection)typeof(SqlParameterCollection).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(null);
                //Input Paramters--update----

                paramCollection.Add(new SqlParameter("@SynckCategory", SqlDbType.VarChar, int.MaxValue, ParameterDirection.Input,
                                false, 0, 0, "@SynckCategory", DataRowVersion.Current, SynckCategory));

                //Output parameters
                paramCollection.Add(new SqlParameter("@ReturnCode", SqlDbType.Int, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnCode", DataRowVersion.Current, 0));

                paramCollection.Add(new SqlParameter("@ReturnDesc", SqlDbType.VarChar, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnDesc", DataRowVersion.Current, string.Empty));

                //Call DAL layer
                queryResult = dataAccessManager.ExecuteStoredProcedureForReadOperation(sqlConn, "Get_SynckCategory", paramCollection);
                response.ResultCode = (Int32)paramCollection["@ReturnCode"].Value;
                response.Message = paramCollection["@ReturnDesc"].Value.ToString();


            }
            catch (Exception ex)
            {
                response.ResultCode = 999;
                response.Message = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
                if (response.ResultCode == 0)
                {
                    try
                    {


                        //1.category
                        if (SynckCategory.ToLower() == "category".ToLower())
                        {
                            string ColumnName = "@tbl_Category";
                            string spName = "Ins_Category";
                            response = UPDSynckCategoryToclient(queryResult, ColumnName, spName);
                            if (response.ResultCode == 0)
                            {
                                DataTable dtUPDSynck = new DataTable("tbl_SynckCategory");
                                dtUPDSynck.Columns.Add("ID", typeof(int));
                                var query =
                                    from r in queryResult.AsEnumerable()
                                    select new
                                    {
                                        ID = r.Field<int>("CtryId")
                                    };
                                string json = Newtonsoft.Json.JsonConvert.SerializeObject(query.ToList());
                                dtUPDSynck = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(json);
                                if (dtUPDSynck.Rows.Count > 0)
                                {
                                    response = UPDSynckCategoryToLive(dtUPDSynck, SynckCategory);
                                }
                            }

                        }
                        //2.Product
                    }
                    catch (Exception ex)
                    {

                        response.ResultCode = 999;
                        response.Message = ex.Message.ToString();
                    }

                }

            }
            return response;
        }

        public SynckCategory_Response_Message UPDSynckCategoryToclient(DataTable dataTable, string ColumnName, string SPName)
        {
            SynckCategory_Response_Message response = new SynckCategory_Response_Message();
            DataAccessManager dataAccessManager = new DataAccessManager();
            SqlConnection sqlConn = dataAccessManager.GetConnection(BalHelper.GetConnectionStringMySql());

            try
            {
                // Prepare required parameters to be passed in the stored procedure   
                SqlParameterCollection paramCollection = (SqlParameterCollection)typeof(SqlParameterCollection).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(null);
                //Input Paramters--update----

                paramCollection.Add(new SqlParameter(ColumnName, SqlDbType.Structured, int.MaxValue, ParameterDirection.Input,
                                     false, 0, 0, ColumnName, DataRowVersion.Current, dataTable));


                //Output parameters
                paramCollection.Add(new SqlParameter("@ReturnCode", SqlDbType.Int, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnCode", DataRowVersion.Current, 0));

                paramCollection.Add(new SqlParameter("@ReturnDesc", SqlDbType.VarChar, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnDesc", DataRowVersion.Current, string.Empty));

                //Call DAL layer
                dataAccessManager.ExecuteStoredProcedureForInsertUpdateDeleteOperation(sqlConn, SPName, ref paramCollection);
                response.ResultCode = (Int32)paramCollection["@ReturnCode"].Value;
                response.Message = paramCollection["@ReturnDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                response.ResultCode = 999;
                response.Message = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();


            }
            return response;
        }
        public SynckCategory_Response_Message UPDSynckCategoryToLive(DataTable dataTable, string SynckCategory)
        {
            SynckCategory_Response_Message response = new SynckCategory_Response_Message();
            DataAccessManager dataAccessManager = new DataAccessManager();
            SqlConnection sqlConn = dataAccessManager.GetConnection(BalHelper.GetConnectionLive());

            try
            {
                // Prepare required parameters to be passed in the stored procedure   
                SqlParameterCollection paramCollection = (SqlParameterCollection)typeof(SqlParameterCollection).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(null);
                //Input Paramters--update----

                paramCollection.Add(new SqlParameter("@tbl_SynckCategory", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input,
                                     false, 0, 0, "@tbl_SynckCategory", DataRowVersion.Current, dataTable));
                paramCollection.Add(new SqlParameter("@SynckCategory", SqlDbType.VarChar, int.MaxValue, ParameterDirection.Input,
                             false, 0, 0, "@SynckCategory", DataRowVersion.Current, SynckCategory));

                //Output parameters
                paramCollection.Add(new SqlParameter("@ReturnCode", SqlDbType.Int, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnCode", DataRowVersion.Current, 0));

                paramCollection.Add(new SqlParameter("@ReturnDesc", SqlDbType.VarChar, int.MaxValue, ParameterDirection.Output,
                    false, 0, 0, "@ReturnDesc", DataRowVersion.Current, string.Empty));

                //Call DAL layer
                dataAccessManager.ExecuteStoredProcedureForInsertUpdateDeleteOperation(sqlConn, "UPD_SynckCategory", ref paramCollection);
                response.ResultCode = (Int32)paramCollection["@ReturnCode"].Value;
                response.Message = paramCollection["@ReturnDesc"].Value.ToString();
            }
            catch (Exception ex)
            {
                response.ResultCode = 999;
                response.Message = ex.Message.ToString();
            }
            finally
            {
                sqlConn.Close();
            }
            return response;
        }


        #endregion Category
    }
}
