// See https://aka.ms/new-console-template for more information
using IntegrationWebApp;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Linq;
using static Program;

public partial class Program
{



    public static void Main(string[] args)
    {
        int isSyncked = 0;
        bool connection = NetworkInterface.GetIsNetworkAvailable();
        if (connection == true)
        {
            Console.WriteLine("Internet connection is available.So start to sync master data...");
            //Syncking using API.....
            //synckUsingAPI();
            //Syncking using direct SQL connection
            //1.category
            GetLiveCategory("category");


        }
        else
        {
            Console.WriteLine("Network is not available");
        }
        Console.ReadLine();
    }
    #region Synck using SQL-SQL...
    public static void GetLiveCategory(string category)
    {
        BLL bLL = new BLL();        
         bLL.GetSynckData(category);      

    }

    #endregion end Synck using SQL-SQL...


    #region Synck Using API....

    public static void synckUsingAPI()
    {
        string APIUrl;
        int isSyncked = 0;
        Task<HttpResponseMessage> responseMessages;
        Category_Response_Message responseMessage = new Category_Response_Message();
        Console.WriteLine("Start-------------Call a Category Details:-------- ");
        APIUrl = "https://localhost:7064/api/Category/GetCategory";
        int categoryId = 0;
        var input = new
        {
            categoryId = 0,
        };
        string tokenId = string.Empty;
        responseMessages = Get(input, GetHeaders(tokenId), APIUrl);
        if (responseMessages != null)
        {
            if (responseMessages.Id == 20)
            {
                responseMessage = JsonConvert.DeserializeObject<Category_Response_Message>(responseMessages.Result.Content.ReadAsStringAsync().Result);
                if (responseMessage.ResultCode == 0)
                {
                    if (responseMessage.CategoryInfo != null)
                    {
                        DataTable dataTable = new DataTable("tbl_Category");
                        dataTable.Columns.Add("CtryId", typeof(int));
                        dataTable.Columns.Add("CtryName", typeof(string));
                        dataTable.Columns.Add("CtryDesc", typeof(string));
                        foreach (Category c in responseMessage.CategoryInfo)
                        {
                            DataRow dr = dataTable.NewRow();
                            dr["CtryId"] = Convert.ToString(c.CtryId);
                            dr["CtryName"] = Convert.ToString(c.CtryName);
                            dr["CtryDesc"] = Convert.ToString(c.CtryDesc);
                            dataTable.Rows.Add(dr);
                        }
                        Console.WriteLine("Category is .....    ");
                        foreach (DataRow theRow in dataTable.Rows)
                        {
                            Console.WriteLine("CtryId--> {0}  CtryName--> {1}  CtryDesc-->{2} ", Convert.ToInt32(theRow["CtryId"]), theRow["CtryName"], theRow["CtryDesc"]);
                        }
                        isSyncked = SynMaster(dataTable);
                        if (isSyncked == 1)
                        {
                            Console.WriteLine("Syncked successfully");
                        }
                        else
                        {
                            Console.WriteLine("Some error while syncking");
                        }

                        Console.WriteLine("Category is .....End    ");

                    }
                }
            }

        }

        Console.WriteLine("End-------------Call a Category Details:-------- ");
    }
    public static async Task<HttpResponseMessage> Get(Dictionary<string, string> headers, string url)
    {
        HttpResponseMessage response = null;
        try
        {
            using (var client = new HttpClient())
            {
                var cst = new CancellationTokenSource();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
                response = await client.SendAsync(request, cst.Token);
            }
        }
        catch (HttpRequestException)
        {

        }
        catch (Exception ex)
        {
            string m = ex.Message;
        }
        return response;
    }
    public static async Task<HttpResponseMessage> Get(object param, Dictionary<string, string> headers, string url)
    {
        HttpResponseMessage response = null;
        try
        {
            HttpClientHandler insecureHandler = GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                var cst = new CancellationTokenSource();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
                if (param != null)
                {
                    var content = JsonConvert.SerializeObject(param);
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                response = await client.SendAsync(request, cst.Token);
            }
        }
        catch (HttpRequestException ex)
        {

        }
        catch (Exception exe)
        {

        }
        return response;
    }
    public static HttpClientHandler GetInsecureHandler()
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert.Issuer.Equals("CN=localhost"))
                return true;
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
        return handler;
    }
    public static Dictionary<string, string> GetHeaders(string Token)
    {
        var headers = new Dictionary<string, string>();

        return headers;
    }
    public static int SynMaster(DataTable dataTable)
    {
        int isSyncked = 0;
        try
        {
            string consString = "Data Source=DESKTOP-0STJ8IO;Initial Catalog=Study;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("Ins_Category"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tbl_Category", dataTable);
                    cmd.Parameters.AddWithValue("@ReturnCode", 0);
                    cmd.Parameters.AddWithValue("@ReturnDesc", string.Empty);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }


            isSyncked = 1;
        }
        catch (Exception ex)
        {

            Console.WriteLine("Exception While Syncking master data....Exception is=" + ex.Message);
        }
        return isSyncked;
    }

    public class Category
    {
        public Category() { }
        public int CtryId { get; set; }
        public string? CtryName { get; set; }
        public string? CtryDesc { get; set; }
    }
    public class Category_Response_Message
    {
        /// <summary>
        /// Gets or sets the result code
        /// </summary>        
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the Message for the result
        /// </summary>

        public string? Message { get; set; }

        public Category[]? CategoryInfo { get; set; }
    }
    #endregion End Synck Using API.... 
}




