using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationWebApp
{
    public class Entities
    {
    }
    #region Category

    public class Category
    {
        public Category() { }
        public int CtryId { get; set; }
        public string? CtryName { get; set; }
        public string? CtryDesc { get; set; }
    }
    public class SynckCategory_Response_Message
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
        /// <summary>
        /// Gets or sets the datatable
        /// </summary>        
        public DataTable dataTable { get; set; }

    }
    #endregion

}
