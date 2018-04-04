using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        private MonitorRecords mr = new MonitorRecords();
        public MonitorRecords Mr
        {
            get { return mr; }
        }

        public void resetValueofVariables()
        {
            SelectedAdditionalFee = null;
            SelectedAvailedItem = null;
            SelectedAvailedServices = null;
            SelectedCustomerSupplier = null;
            SelectedEmployeeContractor = null;
            SelectedEmpPosition = null;
            SelectedJobTitle = null;
            SelectedPaymentH_ = null;
            SelectedPhase = null;
            SelectedPhaseGroup = null;
            SelectedPhasesPerService = null;
            SelectedProduct = null;
            SelectedProductCategory = null;
            SelectedProvince = null;
            SelectedPurchaseOrder = null;
            SelectedRegion = null;
            SelectedRequestedItem = null;
            SelectedSalesInvoice = null;
            SelectedSalesQuote = null;
            SelectedService = null;
            SelectedServiceSchedule_ = null;
            SelectedShipVia = null;
            SelectedUnit = null;

            isContractor = false;
            isEdit = false;

            isNewPurchaseOrder = false;
            isEditPurchaseOrder = false;
            isViewPurchaseOrder = false;

            isNewSalesQuote = false;
            isEditSalesQuote = false;
            isViewSalesQuote = false;

            isNewSchedule = false;
            isEditSchedule = false;
            isViewSchedule = false;

            isNewRecord = false;
            isNewSupplier = false;
            isNewTrans = false;
            isPaymentInvoice = false;
            isNewSched = false;
            isView = false;

            VatableSale = 0;
            VatExemptedSales = 0;
            TotalSalesWithOutDp = 0;
            ZeroRatedSales = 0;
            VatAmount = 0;
            TotalSales = 0;
            TotalSalesNoVat = 0;
            TotalDue = 0;
            TotalDueNoVat = 0;
            DiscountAmount = 0;
            WithHoldingTax = 0;
            Balance = 0;
            Downpayment = 0;

            DecimalTextBox = 0;
            IntegerTextBox = 0;
            StringTextBox = "";
            DatePickerBox = DateTime.Now;

            RequestedItems.Clear();
        }

        public object cbItem { get; set; }

        public decimal DecimalTextBox_;
        public decimal DecimalTextBox { get; set; }
        public int IntegerTextBox_;
        public int IntegerTextBox { get; set; }
        public string StringTextBox_;
        public string StringTextBox { get; set; }
        public ComboBoxItem ComboBItem { get; set; }

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

        public string BusStyle;
        public string BusStyle_
        {
            get { return BusStyle; }
            set { SetProperty(ref BusStyle, value); }
        }

        public string TaxNumber;
        public string TaxNumber_
        { 
            get { return TaxNumber; }
            set { SetProperty(ref TaxNumber, value); }
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

        public bool isNewSalesQuote { get; set; }
        public bool isViewSalesQuote { get; set; }
        public bool isEditSalesQuote { get; set; }

        public bool isNewPurchaseOrder { get; set; }
        public bool isViewPurchaseOrder { get; set; }
        public bool isEditPurchaseOrder { get; set; }

        public bool isNewSchedule { get; set; }
        public bool isEditSchedule { get; set; }

        private bool isViewSchedule_;
        public bool isViewSchedule
        {
            get { return isViewSchedule_; }
            set { SetProperty(ref isViewSchedule_, value); }
        }

        public bool isNewSupplier { get; set; }

        public bool isPaymentInvoice { get; set; }

        public bool isView { get; set; }

        public bool isNewRecord { get; set; }

        public bool isContractor { get; set; }

        public bool isNewSched { get; set; }

        public int lastNumber { get; set; }

        public string poNumChar { get; set; }

        public string invoiceId { get; set; }

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
        public decimal vatableSales_;
        public decimal VatableSale {
            get { return vatableSales_; }
            set { SetProperty(ref vatableSales_, value); }
        }

        public decimal vatExemptedSales_;
        public decimal VatExemptedSales
        {
            get { return vatExemptedSales_; }
            set { SetProperty(ref vatExemptedSales_, value); }
        }

        public decimal totalSalesWithOutDp_;
        public decimal TotalSalesWithOutDp
        {
            get { return totalSalesWithOutDp_; }
            set { SetProperty(ref totalSalesWithOutDp_, value); }
        }

        public decimal zeroRatedSales_;
        public decimal ZeroRatedSales {
            get { return zeroRatedSales_; }
            set { SetProperty(ref zeroRatedSales_, value); }
        }

        public decimal vatAmount_;
        public decimal VatAmount {
            get { return vatAmount_; }
            set { SetProperty(ref vatAmount_, value); }
        }

        public decimal totalSales_;

        public decimal TotalSales {
            get { return totalSales_; }
            set { SetProperty(ref totalSales_, value); }
        }

        public decimal totalSalesNoVat_;
        public decimal TotalSalesNoVat
        {
            get { return totalSalesNoVat_; }
            set { SetProperty(ref totalSalesNoVat_, value); }
        }

        public decimal totalDue_;
        public decimal TotalDue {
            get { return totalDue_; }
            set { SetProperty(ref totalDue_, value); }
        }

        public decimal totalDueNoVat_;
        public decimal TotalDueNoVat
        {
            get { return totalDueNoVat_; }
            set { SetProperty(ref totalDueNoVat_, value); }
        }

        public decimal discountAmount_;
        public decimal DiscountAmount {
            get { return discountAmount_; }
            set { SetProperty(ref discountAmount_, value); }
        }

        public decimal withHoldingTax_;
        public decimal WithHoldingTax {
            get { return withHoldingTax_; }
            set { SetProperty(ref withHoldingTax_, value); }
        }

        public decimal balance_;
        public decimal Balance
        {
            get { return balance_; }
            set { SetProperty(ref balance_, value); }
        }

        public decimal downpayment_;
        public decimal Downpayment
        {
            get { return downpayment_; }
            set { SetProperty(ref downpayment_, value); }
        }

        public string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set { SetProperty(ref searchQuery, value); }
        }

        protected ObservableCollection<Employee> notAvail = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> NotAvail
        {
            get { return notAvail; }
            set { notAvail = value; }
        }

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

        protected ObservableCollection<AvailedService> availedServicesList = new ObservableCollection<AvailedService>();

        public ObservableCollection<AvailedService> AvailedServicesList
        {
            get { return availedServicesList; }
            set { SetProperty(ref availedServicesList, value); }
        }

        protected ObservableCollection<AvailedService> availedServices = new ObservableCollection<AvailedService>();

        protected AvailedService selectedAvailedServices = null;

        public ObservableCollection<AvailedService> AvailedServices
        {
            get { return availedServices; }
            set { SetProperty(ref availedServices, value); }
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
        protected ObservableCollection<PurchaseOrder> PurchaseOrder_ = new ObservableCollection<PurchaseOrder>();

        protected PurchaseOrder selectedPurchaseOrder = null;

        public ObservableCollection<PurchaseOrder> PurchaseOrder
        {
            get { return PurchaseOrder_; }
            set { PurchaseOrder_ = value; }
        }

        public PurchaseOrder SelectedPurchaseOrder
        {
            get { return selectedPurchaseOrder; }
            set { SetProperty(ref selectedPurchaseOrder, value); }
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
            set { SetProperty(ref regions, value); }
        }

        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set { SetProperty(ref selectedRegion, value); }
        }

        protected ObservableCollection<ShipVia> shipVia = new ObservableCollection<ShipVia>();
        public ObservableCollection<ShipVia> ShipVia
        {
            get { return shipVia; }
            set { SetProperty(ref shipVia, value); }
        }

        protected ShipVia selectedShipVia = null;
        public ShipVia SelectedShipVia
        {
            get { return selectedShipVia; }
            set { SetProperty(ref selectedShipVia, value); }
        }
        //Payments Hist



        protected PaymentT SelectedPaymentH = null;

        public PaymentT SelectedPaymentH_
        {
            get { return SelectedPaymentH; }
            set { SetProperty(ref SelectedPaymentH, value); }
        }

        protected PhasesPerService selectedPhasesPerService = null;
        public PhasesPerService SelectedPhasesPerService
        {
            get { return selectedPhasesPerService; }
            set { SetProperty(ref selectedPhasesPerService, value); }
        }

        protected ObservableCollection<FreqItem> FrequentItems_ = new ObservableCollection<FreqItem>();
        public ObservableCollection<FreqItem> FrequentItems
        {
            get { return FrequentItems_; }
            set { SetProperty(ref FrequentItems_, value); }
        }
    }
}
