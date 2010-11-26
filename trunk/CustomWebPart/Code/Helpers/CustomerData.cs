using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;

namespace CustomWebPart.Code.Helpers
{
   
    public class CustomersData
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Records { get; set; }
        public List<CustomerEntity> Rows { get; set; }
    }

    public class CustomerEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }
        public string OrdersHistory { get; set; }
        public string Registered { get; set; }
        public string Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int ModifiedByID { get; set; }

    }

    public class CustomersGUItHelper
    {
        public List<CustomerEntity> Entities
        {
            get
            {
                return list;
            }
        }
        public int TotalCount { get; set; }

        private List<CustomerEntity> list = new List<CustomerEntity>();

        public void ReadAllCustomers(int start, int limit, string sortedField, string sortdirection) 
        {
            GetRegisterdCustomers();

            if (sortdirection.ToLower() == "asc")
                this.Sort(true, sortedField);
            else
                this.Sort(false, sortedField);

            TotalCount = list.Count;

            //filter
            list = list.Skip(start).Take(limit).ToList<CustomerEntity>();
        }

        public void Search(int page, int rows, string searchField, string searchString, string searchOper)
        {


            list.Clear();
            GetRegisterdCustomers();

            switch (searchOper)
            {
                case "eq":
                    {
                        list = EqualSearch(list, searchField, searchString);
                        TotalCount = list.Count;
                        list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerEntity>();
                        break;
                    }
                case "ne":
                    {
                        list = NotEqualsSearch(list, searchField, searchString);
                        TotalCount = list.Count;
                        list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerEntity>();
                        break;
                    }
                case "cn":
                    {
                        list = ContainsSearch(list, searchField, searchString);
                        TotalCount = list.Count;
                        list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerEntity>();
                        break;
                    }
                default: break;
            }

        }

        #region search
        private List<CustomerEntity> EqualSearch(List<CustomerEntity> collection, string field, string searchString)
        {
            switch (field)
            {
                case "Name":
                    {
                        collection = collection.Where(u => u.Name.Equals(searchString)).ToList<CustomerEntity>();
                        break;
                    }
            }
            return collection;
        }

        private List<CustomerEntity> NotEqualsSearch(List<CustomerEntity> collection, string field, string searchString)
        {
            switch (field)
            {
                case "Name":
                    {
                        collection = collection.Where(u => u.Name != searchString).ToList<CustomerEntity>();
                        break;
                    }
            }
            return collection;
        }

        private List<CustomerEntity> ContainsSearch(List<CustomerEntity> collection, string field, string searchString)
        {
            switch (field)
            {
                case "Name":
                    {
                        collection = collection.Where(u => u.Name.Contains(searchString)).ToList<CustomerEntity>();
                        break;
                    }
            }
            return collection;
        }

        #endregion
        private void Sort(bool asc, string sortField)
        {

            switch (sortField)
            {
                case "Name":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.Name).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.Name).ToList<CustomerEntity>();
                        break;
                    }
                case "EmailAddress":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.EmailAddress).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.EmailAddress).ToList<CustomerEntity>();
                        break;
                    }
                case "Phone":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.Phone).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.Phone).ToList<CustomerEntity>();
                        break;
                    }
                case "Notes":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.Notes).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.Notes).ToList<CustomerEntity>();
                        break;
                    }
                case "Registered":
                    {
                        if (asc)
                            list = list.OrderBy(usmEntity => usmEntity.Registered).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(usmEntity => usmEntity.Registered).ToList<CustomerEntity>();
                        break;
                    }
                case "Modified":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.Modified).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.Modified).ToList<CustomerEntity>();
                        break;
                    }
                case "ModifiedBy":
                    {
                        if (asc)
                            list = list.OrderBy(customerEntity => customerEntity.ModifiedBy).ToList<CustomerEntity>();
                        else
                            list = list.OrderByDescending(customerEntity => customerEntity.ModifiedBy).ToList<CustomerEntity>();
                        break;
                    }
            }
        }

        private void GetRegisterdCustomers()
        {
            SPList registeredUsersList = SPObjectModelHelper.GetListIfExists(SPContext.Current.Web, SPObjectModelHelper.LIST_NAME);
            if (registeredUsersList == null) return;
            SPListItemCollection items = registeredUsersList.Items;
         

            foreach (SPListItem item in items)
            {
                
                string modifiedByValue = string.Empty;
                SPFieldUser userField = (SPFieldUser)item.Fields.GetField("Modified By");
                SPFieldUserValue fieldValue = (SPFieldUserValue)userField.GetFieldValue(item["Modified By"].ToString());
                SPUser spUser = fieldValue.User;

                modifiedByValue = spUser.Name;
    

                CustomerEntity ce = new CustomerEntity()
                {
                    ID = item.ID,
                    Name = item["First Name"].ToString() + " " + item["Last Name"].ToString(),
                    EmailAddress = item["Email Address"].ToString(),
                    Phone = item["Phone"].ToString(),
                    Notes = item["Notes"].ToString(),
                    OrdersHistory = "(...)",
                    Registered = item["Created"].ToString(),
                    Modified = item["Modified"].ToString(),
                    ModifiedBy = modifiedByValue,
                    ModifiedByID = spUser.ID
                };

                list.Add(ce);
            }
        }
    }
}