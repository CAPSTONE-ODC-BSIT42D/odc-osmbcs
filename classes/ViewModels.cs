using MySql.Data.MySqlClient;
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
            isEdit = false;

        }

        private LoadDataToUI ldt = new LoadDataToUI();
        public LoadDataToUI Ldt
        {
            get { return ldt; }
        }

        public object cbItem { get; set; }

        public decimal DecimalTextBox_;
        public decimal DecimalTextBox { get; set; }
        public int IntegerTextBox_;
        public int IntegerTextBox { get; set; }
        public string StringTextBox_;
        public string StringTextBox { get; set; }
        public DateTime DatePickerBox_ = DateTime.Now;
        public DateTime DatePickerBox
        {
            get { return DatePickerBox_; }
            set { SetProperty(ref DatePickerBox_, value); }
        }
        #region Customer/Supplier
        public object CompanyType;
        public object CompanyType_
        {
            get { return CompanyType; }
            set { SetProperty(ref CompanyType, value); }
        }

        public string CompanyName;
        public string CompanyName_
        {
            get { return CompanyName; }
            set { SetProperty(ref CompanyName, value); }
        }

        public string CompanyAddress;
        public string CompanyAddress_
        {
            get { return CompanyAddress; }
            set { SetProperty(ref CompanyAddress, value); }
        }

        public string CompanyCity;
        public string CompanyCity_
        {
            get { return CompanyCity; }
            set { SetProperty(ref CompanyCity, value); }
        }

        public object CompanyProvince;
        public object CompanyProvince_
        {
            get { return CompanyProvince; }
            set { SetProperty(ref CompanyProvince, value); }
        }

        public string CompanyPostalCode;
        public string CompanyPostalCode_
        {
            get { return CompanyPostalCode; }
            set { SetProperty(ref CompanyPostalCode, value); }
        }


        public string CompanyEmail;
        public string CompanyEmail_
        {
            get { return CompanyEmail; }
            set { SetProperty(ref CompanyEmail, value); }
        }

        public string CompanyTelephone;
        public string CompanyTelephone_
        {
            get { return CompanyTelephone; }
            set { SetProperty(ref CompanyTelephone, value); }
        }

        public string CompanyMobile;
        public string CompanyMobile_
        {
            get { return CompanyMobile; }
            set { SetProperty(ref CompanyMobile, value); }
        }

        public string RepTitle;
        public string RepTitle_
        {
            get { return RepTitle; }
            set { SetProperty(ref RepTitle, value); }
        }

        public string RepFName;
        public string RepFName_
        {
            get { return RepFName; }
            set { SetProperty(ref RepFName, value); }
        }

        public string RepLName;
        public string RepLName_
        {
            get { return RepLName; }
            set { SetProperty(ref RepLName, value); }
        }

        public string RepMName;
        public string RepMName_
        {
            get { return RepMName; }
            set { SetProperty(ref RepMName, value); }
        }

        public string RepEmail;
        public string RepEmail_
        {
            get { return RepEmail; }
            set { SetProperty(ref RepEmail, value); }
        }

        public string RepTelephone;
        public string RepTelephone_
        {
            get { return RepTelephone; }
            set { SetProperty(ref RepTelephone, value); }
        }

        public string RepMobile;
        public string RepMobile_
        {
            get { return RepMobile; }
            set { SetProperty(ref RepMobile, value); }
        }
        #endregion
        protected Employee LoginEmployee = null;

        public Employee LoginEmployee_
        {
            get { return LoginEmployee; }
            set { SetProperty(ref LoginEmployee, value); }
        }

        public int Edit { get; set; }

        public bool isEdit { get; set; }
        
        public bool isNewTrans{ get; set; }

        public bool isNewPurchaseOrder { get; set; }

        public bool isPaymentInvoice { get; set; }

        public bool isViewHome { get; set; }

        public bool isNewRecord { get; set; }

        public bool isContractor { get; set; }

        public int lastNumber { get; set; }

        public string Ddl;
        public string Ddl_
        {
            get { return Ddl; }
            set { SetProperty(ref Ddl, value); }
        }

        private string PaymentID_;
        public string PaymentID
        {
            get { return PaymentID_; }
            set { SetProperty(ref PaymentID_, value); }
        }

        //Invoice Variables
        private decimal VatableSales_;
        public decimal VatableSale {
            get { return VatableSales_; }
            set { SetProperty(ref VatableSales_, value); }
        }

        public decimal VatExemptedSales_;
        public decimal VatExemptedSales
        {
            get { return VatExemptedSales_; }
            set { SetProperty(ref VatExemptedSales_, value); }
        }

        public decimal TotalSalesWithOutDp_;
        public decimal TotalSalesWithOutDp
        {
            get { return TotalSalesWithOutDp_; }
            set { SetProperty(ref TotalSalesWithOutDp_, value); }
        }

        public decimal ZeroRatedSales_;
        public decimal ZeroRatedSales {
            get { return ZeroRatedSales_; }
            set { SetProperty(ref ZeroRatedSales_, value); }
        }

        public decimal VatAmount_;
        public decimal VatAmount {
            get { return VatAmount_; }
            set { SetProperty(ref VatAmount_, value); }
        }

        public decimal TotalSales_;
        public decimal TotalSales {
            get { return TotalSales_; }
            set { SetProperty(ref TotalSales_, value); }
        }

        public decimal TotalSalesNoVat_;
        public decimal TotalSalesNoVat
        {
            get { return TotalSalesNoVat_; }
            set { SetProperty(ref TotalSalesNoVat_, value); }
        }

        public decimal TotalDue_;
        public decimal TotalDue {
            get { return TotalDue_; }
            set { SetProperty(ref TotalDue_, value); }
        }

        public decimal TotalDueNoVat_;
        public decimal TotalDueNoVat
        {
            get { return TotalDueNoVat_; }
            set { SetProperty(ref TotalDueNoVat_, value); }
        }

        public decimal DiscountAmount_;
        public decimal DiscountAmount {
            get { return DiscountAmount_; }
            set { SetProperty(ref DiscountAmount_, value); }
        }

        public decimal WithHoldingTax_;
        public decimal WithHoldingTax {
            get { return WithHoldingTax_; }
            set { SetProperty(ref WithHoldingTax_, value); }
        }
        


        public String SearchQuery { get; set; }

        #region Customer/Supplier
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

        #endregion

        #region Employee/Contractor
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

        #endregion



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
        #region Service - Service Scheduling - Phases

        protected ObservableCollection<Service> servicesList = new ObservableCollection<Service>();
        public ObservableCollection<Service> ServicesList
        {
            get { return servicesList; }
            set { SetProperty(ref servicesList, value); }
        }

        protected Service selectedservice = null;
        public Service SelectedService
        {
            get { return selectedservice; }
            set { SetProperty(ref selectedservice, value); }
        }

        protected ObservableCollection<AvailedService> availedServices = new ObservableCollection<AvailedService>();

        protected AvailedService selectedAvailedServices = null;

        public ObservableCollection<AvailedService> AvailedServices
        {
            get { return availedServices; }
            set { availedServices = value; }
        }

        public AvailedService SelectedAvailedServices
        {
            get { return selectedAvailedServices; }
            set { SetProperty(ref selectedAvailedServices, value); }
        }


        protected ObservableCollection<Phase> phases = new ObservableCollection<Phase>();
        public ObservableCollection<Phase> Phases
        {
            get { return phases; }
            set { SetProperty(ref phases, value); }
        }



        protected Phase selectedPhase = null;
        public Phase SelectedPhase
        {
            get { return selectedPhase; }
            set { SetProperty(ref selectedPhase, value); }
        }

        protected ObservableCollection<PhaseGroup> phasegroup = new ObservableCollection<PhaseGroup>();
        public ObservableCollection<PhaseGroup> PhasesGroup
        {
            get { return phasegroup; }
            set { SetProperty(ref phasegroup, value); }
        }
        

        protected PhaseGroup selectedPhaseGroup = null;
        public PhaseGroup SelectedPhaseGroup
        {
            get { return selectedPhaseGroup; }
            set { SetProperty(ref selectedPhaseGroup, value); }
        }

        protected ObservableCollection<ServiceSchedule> ServiceSchedules = new ObservableCollection<ServiceSchedule>();

        protected ServiceSchedule SelectedServiceSchedule = null;

        public ObservableCollection<ServiceSchedule> ServiceSchedules_
        {
            get { return ServiceSchedules; }
            set { ServiceSchedules = value; }
        }

        public ServiceSchedule SelectedServiceSchedule_
        {
            get { return SelectedServiceSchedule; }
            set { SetProperty(ref SelectedServiceSchedule, value); }
        }
        
        protected ObservableCollection<Employee> AssignedEmployees = new ObservableCollection<Employee>();

        public ObservableCollection<Employee> AssignedEmployees_
        {
            get { return AssignedEmployees; }
            set { AssignedEmployees = value; }
        }

        protected ObservableCollection<Employee> AvailableEmployees =
            new ObservableCollection<Employee>();

        public ObservableCollection<Employee> AvailableEmployees_
        {
            get { return AvailableEmployees; }
            set { AvailableEmployees = value; }
        }

        #endregion

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

        //----- Sales Invoice

        protected ObservableCollection<SalesInvoice> salesInvoice = new ObservableCollection<SalesInvoice>();

        protected SalesInvoice selectedSalesInvoice = null;

        public ObservableCollection<SalesInvoice> SalesInvoice
        {
            get { return salesInvoice; }
            set { salesInvoice = value; }
        }

        public SalesInvoice SelectedSalesInvoice
        {
            get { return selectedSalesInvoice; }
            set { SetProperty(ref selectedSalesInvoice, value); }
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

        //----- Invoice Item
        protected ObservableCollection<InvoiceItem> InvoiceItems_ = new ObservableCollection<InvoiceItem>();

        public ObservableCollection<InvoiceItem> InvoiceItems
        {
            get { return InvoiceItems_; }
            set { InvoiceItems_ = value; }
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
        //-----Added Items

        protected ObservableCollection<AvailedItem> availedItems = new ObservableCollection<AvailedItem>();

        public ObservableCollection<AvailedItem> AvailedItems
        {
            get { return availedItems; }
            set { availedItems = value; }
        }

        protected AvailedItem selectedAvailedItem = null;

        public AvailedItem SelectedAvailedItem
        {
            get { return selectedAvailedItem; }
            set { SetProperty(ref selectedAvailedItem, value); }
        }
        //-----Added Services

        //----- Units

        protected ObservableCollection<Markup_History> markupHist = new ObservableCollection<Markup_History>();
        

        public ObservableCollection<Markup_History> MarkupHist
        {
            get { return markupHist; }
            set { markupHist = value; }
        }
        


        //----- Units

        protected ObservableCollection<Unit> units = new ObservableCollection<Unit>();

        protected Unit selectedUnit = null;

        public ObservableCollection<Unit> Units
        {
            get { return units; }
            set { units = value; }
        }

        public Unit SelectedUnit
        {
            get { return selectedUnit; }
            set { SetProperty(ref selectedUnit, value); }
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

        //----- Regions

        protected ObservableCollection<Region> regions = new ObservableCollection<Region>();

        protected Region selectedRegion = null;

        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            set { regions = value; }
        }

        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set { SetProperty(ref selectedRegion, value); }
        }


       
        

        //Payments Hist

        

        protected PaymentT SelectedPaymentH = null;

        public PaymentT SelectedPaymentH_
        {
            get { return SelectedPaymentH; }
            set { SetProperty(ref SelectedPaymentH, value); }
        }
    }
}
