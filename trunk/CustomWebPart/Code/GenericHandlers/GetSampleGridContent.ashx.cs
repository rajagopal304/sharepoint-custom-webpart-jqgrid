using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using CustomWebPart.Code.Helpers;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CustomWebPart.Code.GenericHandlers
{
    public class GetSampleGridContent : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;

            var page = Convert.ToInt32(context.Request.Form["page"]);
            var rows = Convert.ToInt32(context.Request.Form["rows"]);
            string direction = context.Request.Form["sord"];
            string sortField = context.Request.Form["sidx"].ToString();

            bool isSearch = Convert.ToBoolean(context.Request.Form["_search"]);

            CustomersGUItHelper customerGUIManager = new CustomersGUItHelper();
            if (true == isSearch)
            {
                customerGUIManager.Search(page, rows, context.Request.Form["searchField"].ToString(), context.Request.Form["searchString"].ToString(), context.Request.Form["searchOper"].ToString());
            }
            else
            {
                customerGUIManager.ReadAllCustomers((page - 1) * rows, rows, sortField, direction);
            }

            CustomersData jsonData = new CustomersData();
            jsonData.Rows = customerGUIManager.Entities;
            jsonData.Page = page;
            jsonData.Records = customerGUIManager.TotalCount;
            jsonData.Total = (int)Math.Ceiling((float)customerGUIManager.TotalCount / (float)rows);

            MemoryStream stream = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CustomersData));

            ser.WriteObject(stream, jsonData); 

            stream.Position = 0;

            StreamReader sr = new StreamReader(stream);

            var json = sr.ReadToEnd();

            context.Response.Write(json);
            context.Response.End();
        }
    }
}