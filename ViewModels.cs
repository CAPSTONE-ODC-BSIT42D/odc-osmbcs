using System;
using System.Collections;
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

        public String Name { get; set; }
        public String AdditionalInfo { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public object cbItem { get; set; }
        public object CityName { get; set; }
        public String EmailAddress { get; set; }
        public String PhoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String ContactValue { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
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

        protected Customer selectedCustomer = null;

        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { SetProperty(ref selectedCustomer, value); }
        }

        //-----CUSTOMER
        protected ObservableCollection<Customer> suppliers =
            new ObservableCollection<Customer>();

        protected Customer selectedSupplier = null;

        public ObservableCollection<Customer> Suppliers
        {
            get { return suppliers; }
            set { suppliers = value; }
        }

        public Customer SelectedSupplier
        {
            get { return selectedSupplier; }
            set { SetProperty(ref selectedSupplier, value); }
        }

        //-----Employee
        protected ObservableCollection<Employee> employees =
            new ObservableCollection<Employee>();

        protected Employee selectedEmployee = null;

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { SetProperty(ref selectedEmployee, value); }
        }
        //-----End of Employee

        //-----Selected CustContact
        protected ObservableCollection<Contact> empcontacts = new ObservableCollection<Contact>();

        protected Contact selectedContact = null;

        public ObservableCollection<Contact> EmpContacts
        {
            get { return empcontacts; }
            set { empcontacts = value; }
        }

        public Contact SelectedEmpContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
        }
        //----- END of Selected CustContact

        //-----Selected CustContact
        protected ObservableCollection<Contact> custcontact = new ObservableCollection<Contact>();

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
        //----- END of Selected CustContact

        //----- RepContacts
        protected ObservableCollection<Contact> repcontact = new ObservableCollection<Contact>();

        public ObservableCollection<Contact> RepContacts
        {
            get { return repcontact; }
            set { repcontact = value; }
        }

        public Contact SelectedRepContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
        }
        //----- END of REpContacts

        //----- Employee Position
        protected ObservableCollection<EmpPosition> empPosition = new ObservableCollection<EmpPosition>();

        protected EmpPosition selectedEmpPosition = null;

        public ObservableCollection<EmpPosition> EmpPosition
        {
            get { return empPosition; }
            set { empPosition = value; }
        }

        public EmpPosition SelectedEmpPosition
        {
            get { return selectedEmpPosition; }
            set { SetProperty(ref selectedEmpPosition, value); }
        }
        //----- End of Employee Position

        //----- Contractor Job Title
        protected ObservableCollection<ContJobName> contJobTitle = new ObservableCollection<ContJobName>();

        protected ContJobName selectedJobTitle = null;

        public ObservableCollection<ContJobName> ContJobTitle
        {
            get { return contJobTitle; }
            set { contJobTitle = value; }
        }

        public ContJobName SelectedJobTitle
        {
            get { return selectedJobTitle; }
            set { SetProperty(ref selectedJobTitle, value); }
        }
        //----- End of Contractor Job Title

        //----- Product Lsit
        protected ObservableCollection<Item> productList = new ObservableCollection<Item>();

        protected Item selectedProduct = null;

        public ObservableCollection<Item> ProductList
        {
            get { return productList; }
            set { productList = value; }
        }

        public Item SelectedProduct
        {
            get { return selectedProduct; }
            set { SetProperty(ref selectedProduct, value); }
        }
        //----- Requested Item
        protected ObservableCollection<RequestedItem> requestedItems = new ObservableCollection<RequestedItem>();

        protected RequestedItem selectedRequestedItem = null;

        public ObservableCollection<RequestedItem> RequestedItems
        {
            get { return requestedItems; }
            set { requestedItems = value; }
        }

        public RequestedItem SelectedRequestedItem
        {
            get { return selectedRequestedItem; }
            set { SetProperty(ref selectedRequestedItem, value); }
        }

        //----- Provinces

        protected ObservableCollection<Province> provinces = new ObservableCollection<Province>();

        protected Province selectedProvince = null;

        public ObservableCollection<Province> Provinces
        {
            get { return provinces; }
            set { provinces = value; }
        }

        public Province SelectedProvince
        {
            get { return selectedProvince; }
            set { SetProperty(ref selectedProvince, value); }
        }
    }
}
