﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        public String PostalCode { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public object cbItem { get; set; }
        public object CityName { get; set; }
        
        public String EmailAddress { get; set; }
        public String PhoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String ContactValue { get; set; }
        public String RepTitle { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public String DateStarted { get; set; }
        public String DateEnded { get; set; }

        public String ProductName { get; set; }
        public String ProductDesc { get; set; }
        public String ProductUnit { get; set; }
        public decimal ProductPrice { get; set; }
        public int Edit { get; set; }

        //-----REPRESENTATIVE
        protected ObservableCollection<Representative> representative = new ObservableCollection<Representative>();

        protected Representative selectedRepresentative = null;

        public ObservableCollection<Representative> Representatives
        {
            get { return representative; }
            set { representative = value; }
        }

        public Representative SelectedRepresentative
        {
            get { return selectedRepresentative; }
            set { SetProperty(ref selectedRepresentative, value); }
        }
        //-----END OF REPRESENTATIVE

        //-----CUSTOMER/ Supplier
        protected ObservableCollection<Customer> allCustomerSupplier =
            new ObservableCollection<Customer>();

        public ObservableCollection<Customer> AllCustomerSupplier
        {
            get { return allCustomerSupplier; }
            set { allCustomerSupplier = value; }
        }

        protected ObservableCollection<Customer> customers =
            new ObservableCollection<Customer>();

        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        protected ObservableCollection<Customer> suppliers =
            new ObservableCollection<Customer>();

        public ObservableCollection<Customer> Suppliers
        {
            get { return suppliers; }
            set { suppliers = value; }
        }

        protected Customer selectedCustomerSupplier = null;

        public Customer SelectedCustomerSupplier
        {
            get { return selectedCustomerSupplier; }
            set { SetProperty(ref selectedCustomerSupplier, value); }
        }



        //-----Employee/contractor

        protected ObservableCollection<Employee> allEmployeesContractor =
            new ObservableCollection<Employee>();

        public ObservableCollection<Employee> AllEmployeesContractor
        {
            get { return allEmployeesContractor; }
            set { allEmployeesContractor = value; }
        }

        protected ObservableCollection<Employee> employees =
            new ObservableCollection<Employee>();

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        protected ObservableCollection<Employee> contractor =
            new ObservableCollection<Employee>();

        public ObservableCollection<Employee> Contractor
        {
            get { return contractor; }
            set { contractor = value; }
        }

        protected Employee selectedEmployeeContractor = null;

        public Employee SelectedEmployeeContractor
        {
            get { return selectedEmployeeContractor; }
            set { SetProperty(ref selectedEmployeeContractor, value); }
        }
        //-----End of Employee
        
        //-----End of contractor

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
        //----- Product Category
        protected ObservableCollection<ItemType> productCategory = new ObservableCollection<ItemType>();

        protected ItemType selectedProductCategory = null;

        public ObservableCollection<ItemType> ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; }
        }

        public ItemType SelectedProductCategory
        {
            get { return selectedProductCategory; }
            set { SetProperty(ref selectedProductCategory, value); }
        }
        //-----services
        protected ObservableCollection<Service> servicesList = new ObservableCollection<Service>();

        protected Service selectedservice = null;

        public ObservableCollection<Service> ServicesList
        {
            get { return servicesList; }
            set { servicesList = value; }
        }

        public Service SelectedService
        {
            get { return selectedservice; }
            set { SetProperty(ref selectedservice, value); }
        }

        //----- Sales Quote
        protected ObservableCollection<SalesQuote> salesQuote = new ObservableCollection<SalesQuote>();

        protected SalesQuote selectedSalesQuote = null;

        public ObservableCollection<SalesQuote> SalesQuotes
        {
            get { return salesQuote; }
            set { salesQuote = value; }
        }

        public SalesQuote SelectedSalesQuote
        {
            get { return selectedSalesQuote; }
            set { SetProperty(ref selectedSalesQuote, value); }
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
        //-----Additional Fees
        protected ObservableCollection<AdditionalFee> additionalFees = new ObservableCollection<AdditionalFee>();

        protected AdditionalFee selectedAdditionalFee = null;

        public ObservableCollection<AdditionalFee> AdditionalFees
        {
            get { return additionalFees; }
            set { additionalFees = value; }
        }

        public AdditionalFee SelectedAdditionalFee
        {
            get { return selectedAdditionalFee; }
            set { SetProperty(ref selectedAdditionalFee, value); }
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
