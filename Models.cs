using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype2
{
    public class Representative : ViewModelEntity
    {
        public Representative()
        {

        }
        protected string firstname;
        protected string middlename;
        protected string lastname;

        public string RepFirstName
        {
            get { return firstname; }
            set { SetProperty(ref firstname, value); }
        }
        public string RepMiddleName
        {
            get { return middlename; }
            set { SetProperty(ref middlename, value); }
        }
        public string RepLastName
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); }
        }
    }

    public class Customer : ViewModelEntity
    {
        public Customer()
        {

        }
        protected string customerName;
        protected string customerDesc;
        protected string customerAddress;
        protected string customerCity;
        protected string customerProvinceID;

        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }
        public string CustomerDesc
        {
            get { return customerDesc; }
            set { SetProperty(ref customerDesc, value); }
        }
        public string CustomerAddress
        {
            get { return customerAddress; }
            set { SetProperty(ref customerAddress, value); }
        }
        public string CustomerCity
        {
            get { return customerCity; }
            set { SetProperty(ref customerCity, value); }
        }
        public string CustomerProvinceID
        {
            get { return customerProvinceID; }
            set { SetProperty(ref customerProvinceID, value); }
        }
    }

    public class Contact : ViewModelEntity
    {
        public Contact()
        {

        }
        protected string typeid;
        protected string typename;
        protected string details;

        public string ContactTypeID
        {
            get { return typeid; }
            set { SetProperty(ref typeid, value); }
        }
        public string ContactType
        {
            get { return typename; }
            set { SetProperty(ref typename, value); }
        }
        public string ContactDetails
        {
            get { return details; }
            set { SetProperty(ref details, value); }
        }
    }

}
