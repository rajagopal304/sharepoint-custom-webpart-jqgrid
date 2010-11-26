using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;

namespace CustomWebPart.Code.Helpers
{
    public class SPObjectModelHelper
    {
        internal const string LIST_NAME = "DemoCustomers";

        internal static SPList GetListIfExists(SPWeb web, string listName)
        {
            try
            {
                SPList list = web.Lists[listName];
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}