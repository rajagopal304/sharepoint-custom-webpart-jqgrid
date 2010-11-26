using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.HtmlControls;
using System.Text;


namespace CustomWebPart.Code.WebParts
{
    public class SampleJQueryWebPart : WebPart, ICallbackEventHandler
    {
        private bool _error = false;

        private const string _ascxDetailsPath = @"~/_CONTROLTEMPLATES/JQWebPart/DetailsView.ascx";
       

        private const string _jqueryMainFilePath = @"/_LAYOUTS/JQWebPart/scripts/jquery-1.4.2.min.js";
        private const string _jqueryUIFilePath = @"/_LAYOUTS/JQWebPart/scripts/jquery-ui-1.8.5.custom.min.js";
        private const string _jqueryGridLocalePath = @"/_LAYOUTS/JQWebPart/scripts/plugins/jqGrid/js/i18n/grid.locale-en.js";
        private const string _jqueryGridPath = @"/_LAYOUTS/JQWebPart/scripts/plugins/jqGrid/js/jquery.jqGrid.min.js";
        private const string _jqGridWebPartFilePath = @"/_LAYOUTS/JQWebPart/scripts/SampleWebPart.js";
        private const string _jqGridWebPartOrdersHistoryFilePath = @"/_LAYOUTS/JQWebPart/scripts/OrdersHistoryDetail.js";

        private const string _jqueryUICss = @"/_LAYOUTS/JQWebPart/styles/jquery-ui-1.8.5.custom.css";
        private const string _qGridCss = @"/_LAYOUTS/JQWebPart/scripts/plugins/jqGrid/css/ui.jqgrid.css";

        HtmlGenericControl divDetailsControlContainer;
        HtmlTable form;
        HtmlGenericControl divGridPager;

        private int selectedListItemID = 0;
        protected override void CreateChildControls()
        {

            if (!_error)
            {
                try
                {
                    base.CreateChildControls();

                   
                    divDetailsControlContainer = new HtmlGenericControl();
                    divDetailsControlContainer.ID = "divDetailsUCContainer";
                    divDetailsControlContainer.Attributes.Add("style", "display:none");
                    divDetailsControlContainer.Attributes.Add("title", "Orders history");
                   
                    form = new HtmlTable();
                    form.Attributes.Add("class", "userTable");
                    this.Controls.Add(form);

                    AddUserControlsToContent(form.Rows);

                    AddJQGridHtmlElementsToContent(form.Rows);
                   

                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();

                    
                    ClientScriptManager clientScript = this.Page.ClientScript;

                    clientScript.RegisterClientScriptBlock(typeof(Page), "PopUpJsFunctions", this.PopUpJsFunctions);

                    if (!clientScript.IsStartupScriptRegistered("jQueryJQWebPart_UI"))
                        clientScript.RegisterStartupScript(typeof(Page), "jQueryJQWebPart_UI", JQueryGUILibrary);

                    clientScript.RegisterClientScriptBlock(typeof(Page), "WebPartJsInjectVariables", this.WebPartJsInjectVariables);

                    //dynamic callback on clicking (...) in Orders history column for each row in the jquery grid
                    String cbReference = clientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "");
                    String callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
                    clientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                    
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
        }

        private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }

        private void AddUserControlsToContent(HtmlTableRowCollection rows)
        {

            Control detailsUserControl = this.Page.LoadControl(_ascxDetailsPath);
            
            HtmlTableCell cellDetailsUC = new HtmlTableCell();
            divDetailsControlContainer.Controls.Add(detailsUserControl);
            cellDetailsUC.Controls.Add(divDetailsControlContainer);
            HtmlTableRow rowDetailsUC = new HtmlTableRow();
            rowDetailsUC.Cells.Add(cellDetailsUC);

            rows.Add(rowDetailsUC);
            
        }

        private void AddJQGridHtmlElementsToContent(HtmlTableRowCollection rows)
        {
            HtmlTableCell cellJQGrid = new HtmlTableCell();
            HtmlTableRow rowJQGrid = new HtmlTableRow();

            HtmlGenericControl divJQGrid = new HtmlGenericControl();
            divJQGrid.ID = "jqGid";

            HtmlTable table = new HtmlTable();
            table.ID = "gridViewSampleList";
            table.Attributes.Add("cellpadding", "0");
            table.Attributes.Add("cellspacing", "0");
            table.Attributes.Add("class", "SAMPLE_WP_LIST");

            divJQGrid.Controls.Add(table);

            divGridPager = new HtmlGenericControl();
            divGridPager.ID = "gridViewSampleListPager";
            divGridPager.Attributes.Add("style", "text-align: center;");
            divGridPager.Attributes.Add("class", "SAMPLE_WP_PAGER");
            divGridPager.TagName = "div";

            divJQGrid.Controls.Add(divGridPager);

            cellJQGrid.Controls.Add(divJQGrid);

            rowJQGrid.Cells.Add(cellJQGrid);

            rows.Add(rowJQGrid);

        }

        /// <summary>
        /// the js fucntions that this web part uses
        /// </summary>
        public string PopUpJsFunctions
        {
            get
            {

                return @"
                <script type=""text/javascript"">
                //<![CDATA[
 
                function displayUserControl(contentDivId, selectedListItemID)
                {
                    CallServer(selectedListItemID);
                    $('#' + contentDivId).dialog();
                    
                }
 
                            
                //]]>
                </script>";
            }
        }
        public string WebPartJsInjectVariables
        {
            get
            {
                return 
                    @"<script type=""text/javascript"">" + 
                    "var varDetailsControlContainerId = '" + divDetailsControlContainer.ClientID + "';" +
                    "var varJQGridPagerID = '#" + divGridPager.ClientID + "';" +
                    "var varPageUrl = '" + HttpUtility.UrlEncode(this.Page.Request.Url.AbsoluteUri) + "';" + 
                    @"</script>";
            
            }
        
        }
       
        public string JQueryGUILibrary
        {
            get
            {
                return @"<script src='" + _jqueryMainFilePath + "' type='text/javascript'></script>" +
                        @"<script src='" + _jqueryUIFilePath + "' type='text/javascript'></script>" +
                        @"<script src='" + _jqueryGridLocalePath + "' type='text/javascript'></script>" +
                        @"<script src='" + _jqueryGridPath + "' type='text/javascript'></script>" +
                        @"<script src='" + _jqGridWebPartFilePath + "' type='text/javascript'></script>" +
                        @"<script src='" + _jqGridWebPartOrdersHistoryFilePath + "' type='text/javascript'></script>" +
                        @"<link href='" + _jqueryUICss + @"' rel=""stylesheet"" type=""text/css"" />" +
                        @"<link href='" + _qGridCss + @"' rel=""stylesheet"" type=""text/css"" />";                 

            }
        }

        #region dynamic popup user control

        /// <summary>
        /// Generates dynamic content which is rendered in the user control
        /// based on the list item ID which is passed as argument, the values of the SP Item can be fetched and rendered
        /// </summary>
        /// <param name="selectedListItemID">ID of the SP List</param>
        /// <returns>dynamically generated html</returns>
        private string DynamicOrdersHistoryForm(int selectedListItemID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<div id=""details""><input type=""hidden"" id=""selectedListItemID"" name=""selectedListItemID"" value='" + selectedListItemID + "' />");
            sb.AppendLine(@"<input type=""text"" id=""txtSampleId"" name=""txtSample"" value=""this is sample value"" />");
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        public string GetCallbackResult()
        {
            return DynamicOrdersHistoryForm(selectedListItemID);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            selectedListItemID = Convert.ToInt32(eventArgument);
        }
        #endregion
    }
}