using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype2
{
    public class MainViewModel : ViewModelEntity
    {
        public MainViewModel()
        {

        }

        public new String Name { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public object CityName { get; set; }
        public String EmailAddress { get; set; }
        public String PhoneNumber { get; set; }
        public String MobileNumber { get; set; }

        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public int Edit { get; set; }

        //-----REPRESENTATIVE
        protected ObservableCollection<Representative> custrepresentative = new ObservableCollection<Representative>();

        protected Representative selectedRepresentative = null;

        public ObservableCollection<Representative> CustRepresentatives
        {
            get { return custrepresentative; }
            set { custrepresentative = value; }
        }

        public Representative SelectedRepresentative
        {
            get { return selectedRepresentative; }
            set { SetProperty(ref selectedRepresentative, value); }
        }
        //-----END OF REPRESENTATIVE

        //-----CUSTOMER
        protected ObservableCollection<Customer> customers =
            new ObservableCollection<Customer>();

        protected Representative selectedCustomer = null;

        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        public Representative SelectedCustomer
        {
            get { return selectedCustomer; }
            set { SetProperty(ref selectedCustomer, value); }
        }
        //-----END OF CUSTOMER

        protected ObservableCollection<Contact> custcontact = new ObservableCollection<Contact>();

        protected Contact selectedContact = null;

        public ObservableCollection<Contact> CustContacts
        {
            get { return custcontact; }
            set { custcontact = value; }
        }

        public Contact SelectedCustContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
        }

        protected List<ObservableCollection<Contact>> contactOfRep = new List<ObservableCollection<Contact>>();

        public List<ObservableCollection<Contact>> ContactOfRep
        {
            get { return contactOfRep; }
            set { contactOfRep = value; }
        }

        protected ObservableCollection<Contact> repcontact =
            new ObservableCollection<Contact>();

        protected Contact selectedRepContact = null;

        public ObservableCollection<Contact> RepContacts
        {
            get { return repcontact; }
            set { repcontact = value; }
        }

        public Contact SelectedRepContact
        {
            get { return selectedRepContact; }
            set { SetProperty(ref selectedRepContact, value); }
        }
    }
}
