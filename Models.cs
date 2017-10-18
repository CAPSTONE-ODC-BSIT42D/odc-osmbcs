using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype2
{
    
    public class Customer : ViewModelEntity
    {
        public Customer()
        {

        }
        protected string companyID;
        protected string companyName;
        protected string companyDesc;
        protected string companyAddress;
        protected string companyCity;
        protected string companyProvinceID;
        protected string companyProvinceName;
        protected string companyPostalCode;
        protected string companyEmail;
        protected string companyTelephone;
        protected string companyMobile;
        protected string companyType;
        protected string title;
        protected string firstname;
        protected string middlename;
        protected string lastname;
        protected string fullname;
        protected string emailAddress;
        protected string telephone;
        protected string mobilephone;

        public string CompanyID
        {
            get { return companyID; }
            set { SetProperty(ref companyID, value); }
        }
        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty(ref companyName, value); }
        }
        public string CompanyDesc
        {
            get { return companyDesc; }
            set { SetProperty(ref companyDesc, value); }
        }
        public string CompanyAddress
        {
            get { return companyAddress; }
            set { SetProperty(ref companyAddress, value); }
        }
        public string CompanyCity
        {
            get { return companyCity; }
            set { SetProperty(ref companyCity, value); }
        }
        public string CompanyProvinceID
        {
            get { return companyProvinceID; }
            set { SetProperty(ref companyProvinceID, value); }
        }

        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { SetProperty(ref companyPostalCode, value); }
        }
        public string CompanyProvinceName
        {
            get { return companyProvinceName; }
            set { SetProperty(ref companyProvinceName, value); }
        }

        public string CompanyEmail
        {
            get { return companyEmail; }
            set { SetProperty(ref companyEmail, value); }
        }

        public string CompanyTelephone
        {
            get { return companyTelephone; }
            set { SetProperty(ref companyTelephone, value); }
        }

        public string CompanyMobile
        {
            get { return companyMobile; }
            set { SetProperty(ref companyMobile, value); }
        }

        public string CompanyType
        {
            get { return companyType; }
            set { SetProperty(ref companyType, value); }
        }

        public string RepTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

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

        public string RepEmail
        {
            get { return emailAddress; }
            set { SetProperty(ref emailAddress, value); }
        }

        public string RepTelephone
        {
            get { return telephone; }
            set { SetProperty(ref telephone, value); }
        }

        public string RepMobile
        {
            get { return mobilephone; }
            set { SetProperty(ref mobilephone, value); }
        }

        public string RepFullName
        {
            get { return fullname; }
            set { SetProperty(ref fullname, value); }
        }
    }

    public class Employee : ViewModelEntity
    {
        public Employee()
        {

        }
        protected string empID;
        protected string empTitle;
        protected string empFName;
        protected string empLName;
        protected string empMI;
        protected string empAddInfo;
        protected string empAddress;
        protected string empCity;
        protected string empProvinceID;
        protected string empProvinceName;
        protected string positionID;
        protected string positionName;
        protected string empEmail;
        protected string empTelephone;
        protected string empMobile;
        protected string empUsername;
        protected string empPassword;
        protected string jobID;
        protected string jobName;
        protected string empDateFrom;
        protected string empDateTo;
        protected string empType;
        protected byte[] empPic;
        protected byte[] empSig;
        

        public string EmpID
        {
            get { return empID; }
            set { SetProperty(ref empID, value); }
        }
        public string EmpTitle
        {
            get { return empTitle; }
            set { SetProperty(ref empTitle, value); }
        }

        public string EmpFname
        {
            get { return empFName; }
            set { SetProperty(ref empFName, value); }
        }

        public string EmpLName
        {
            get { return empLName; }
            set { SetProperty(ref empLName, value); }
        }

        public string EmpMiddleInitial
        {
            get { return empMI; }
            set { SetProperty(ref empMI, value); }
        }

        public string EmpAddInfo
        {
            get { return empAddInfo; }
            set { SetProperty(ref empAddInfo, value); }
        }

        public string EmpAddress
        {
            get { return empAddress; }
            set { SetProperty(ref empAddress, value); }
        }

        public string EmpCity
        {
            get { return empCity; }
            set { SetProperty(ref empCity, value); }
        }

        public string EmpProvinceID
        {
            get { return empProvinceID; }
            set { SetProperty(ref empProvinceID, value); }
        }

        public string EmpProvinceName
        {
            get { return empProvinceName; }
            set { SetProperty(ref empProvinceName, value); }
        }

        public string PositionID
        {
            get { return positionID; }
            set { SetProperty(ref positionID, value); }
        }

        public string PositionName
        {
            get { return positionName; }
            set { SetProperty(ref positionName, value); }
        }

        public string EmpEmail
        {
            get { return empEmail; }
            set { SetProperty(ref empEmail, value); }
        }

        public string EmpTelephone
        {
            get { return empTelephone; }
            set { SetProperty(ref empTelephone, value); }
        }

        public string EmpMobile
        {
            get { return empMobile; }
            set { SetProperty(ref empMobile, value); }
        }

        public string EmpUserName
        {
            get { return empUsername; }
            set { SetProperty(ref empUsername, value); }
        }

        public string EmpPassword
        {
            get { return empPassword; }
            set { SetProperty(ref empPassword, value); }
        }

        public string JobID
        {
            get { return jobID; }
            set { SetProperty(ref jobID, value); }
        }

        public string JobName
        {
            get { return jobName; }
            set { SetProperty(ref jobName, value); }
        }

        public string EmpDateFrom
        {
            get { return empDateFrom; }
            set { SetProperty(ref empDateFrom, value); }
        }

        public string EmpDateTo
        {
            get { return empDateTo; }
            set { SetProperty(ref empDateFrom, value); }
        }

        public string EmpType
        {
            get { return empType; }
            set { SetProperty(ref empType, value); }
        }

        public byte[] EmpPic
        {
            get { return empPic; }
            set { SetProperty(ref empPic, value); }
        }

        public byte[] EmpSig
        {
            get { return empSig; }
            set { SetProperty(ref empSig, value); }
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
        protected string tableID;
        protected int id;
        protected bool isDefault;
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
        public string TableID
        {
            get { return tableID; }
            set { SetProperty(ref tableID, value); }
        }

        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { SetProperty(ref isDefault, value); }
        }

    }

    public class EmpPosition : ViewModelEntity
    {
        public EmpPosition()
        {

        }

        protected string positionid;
        protected string positionName;

        public string PositionID
        {
            get { return positionid; }
            set { SetProperty(ref positionid, value); }
        }

        public string PositionName
        {
            get { return positionName; }
            set { SetProperty(ref positionName, value); }
        }
    }

    public class ContJobName : ViewModelEntity
    {
        public ContJobName()
        {

        }

        protected string jobid;
        protected string jobName;

        public string JobID
        {
            get { return jobid; }
            set { SetProperty(ref jobid, value); }
        }

        public string JobName
        {
            get { return jobName; }
            set { SetProperty(ref jobName, value); }
        }
    }

    public class Province : ViewModelEntity
    {
        public Province()
        {

        }
        protected int provinceID;
        protected string provinceName;
        protected decimal provincePrice;
        public int ProvinceID
        {
            get { return provinceID; }
            set { SetProperty(ref provinceID, value); }
        }
        public string ProvinceName
        {
            get { return provinceName; }
            set { SetProperty(ref provinceName, value); }
        }
        public decimal ProvincePrice
        {
            get { return provincePrice; }
            set { SetProperty(ref provincePrice, value); }
        }
    }

    public class ItemType : ViewModelEntity
    {
        public ItemType()
        {

        }

        protected int typeID;

        public int TypeID
        {
            get { return typeID; }
            set { SetProperty(ref typeID, value); }
        }

        protected string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { SetProperty(ref typeName, value); }
        }

    }

    public class Item : ViewModelEntity
    {
        public Item()
        {

        }
        protected bool isChecked;
        protected string itemCode;
        protected string itemName;
        protected string itemDesc;
        protected decimal costPrice;
        protected string unit;
        protected int quantity;
        protected string typeID;
        protected string supplierID;
        protected string typeName;
        protected string supplierName;

        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        public string ItemCode
        {
            get { return itemCode; }
            set { SetProperty(ref itemCode, value); }
        }

        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); }
        }
        public string ItemDesc
        {
            get { return itemDesc; }
            set { SetProperty(ref itemDesc, value); }
        }
        public decimal CostPrice
        {
            get { return costPrice; }
            set { SetProperty(ref costPrice, value); }
        }
        public string Unit
        {
            get { return unit; }
            set { SetProperty(ref unit, value); }
        }
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
        public string TypeID
        {
            get { return typeID; }
            set { SetProperty(ref typeID, value); }
        }
        public string TypeName
        {
            get { return typeName; }
            set { SetProperty(ref typeName, value); }
        }
        public string SupplierID
        {
            get { return supplierID; }
            set { SetProperty(ref supplierID, value); }
        }
        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty(ref supplierName, value); }
        }
    }

    public class Service : ViewModelEntity
    {
        public Service()
        {

        }

        protected string serviceID;
        protected string serviceName;
        protected string serviceDesc;
        protected decimal servicePrice;

        public string ServiceID
        {
            get { return serviceID; }
            set { SetProperty(ref serviceID, value); }
        }

        public string ServiceName
        {
            get { return serviceName; }
            set { SetProperty(ref serviceName, value); }
        }

        public string ServiceDesc
        {
            get { return serviceDesc; }
            set { SetProperty(ref serviceDesc, value); }
        }

        public decimal ServicePrice
        {
            get { return servicePrice; }
            set { SetProperty(ref servicePrice, value); }
        }
    }

    public class AddedService : ViewModelEntity
    {
        public AddedService()
        {

        }
        protected string tableNoChar;
        protected string serviceID;
        protected string serviceName;
        protected decimal servicePrice;
        protected string address;
        protected string city;
        protected string provinceName;
        protected int provinceID;
        protected decimal totalCost;

        public string TableNoChar
        {
            get { return tableNoChar; }
            set { SetProperty(ref tableNoChar, value); }
        }

        public string ServiceID
        {
            get { return serviceID; }
            set { SetProperty(ref serviceID, value); }
        }
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }
        public int ProvinceID
        {
            get { return provinceID; }
            set { SetProperty(ref provinceID, value); }
        }

        public decimal TotalCost
        {
            get { return totalCost; }
            set { SetProperty(ref totalCost, value); }
        }

        protected ObservableCollection<AdditionalFee> additionalFees = new ObservableCollection<AdditionalFee>();

        public ObservableCollection<AdditionalFee> AdditionalFees
        {
            get { return additionalFees; }
            set { SetProperty(ref additionalFees, value); }
        }
    }

    public class AddedItem : ViewModelEntity
    {
        public AddedItem()
        {

        }
        protected int tableID;
        protected string sqNoChar;
        protected string itemCode;
        protected int itemQty;
        protected decimal totalCost;

        public int TableID
        {
            get { return tableID; }
            set { SetProperty(ref tableID, value); }
        }

        public string SqNoChar
        {
            get { return sqNoChar; }
            set { SetProperty(ref sqNoChar, value); }
        }

        public string ItemCode
        {
            get { return itemCode; }
            set { SetProperty(ref itemCode, value); }
        }

        public int ItemQty
        {
            get { return itemQty; }
            set { SetProperty(ref itemQty, value); }
        }

        public decimal TotalCost
        {
            get { return totalCost; }
            set { SetProperty(ref totalCost, value); }
        }
    }

    public class AdditionalFee : ViewModelEntity
    {
        public AdditionalFee()
        {

        }

        protected string feeName;
        protected decimal feePrice;
        protected string serviceNoChar;

        public string ServiceNoChar
        {
            get { return serviceNoChar; }
            set { SetProperty(ref serviceNoChar, value); }
        }


        public string FeeName
        {
            get { return feeName; }
            set { SetProperty(ref feeName, value); }
        }

        public decimal FeePrice
        {
            get { return feePrice; }
            set { SetProperty(ref feePrice, value); }
        }
    }

    public class RequestedItem : ViewModelEntity
    {
        public RequestedItem()
        {

        }
        private string _lineNo;
        public string lineNo
        {
            get { return _lineNo; }
            set { SetProperty(ref _lineNo, value); }
        }
        private string _itemCode;
        public string itemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }
        private string _itemName;
        public string itemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
        }
        private string _desc;
        public string desc
        {
            get { return _desc; }
            set { SetProperty(ref _desc, value); }
        }
        private string _itemTypeName;
        public string itemTypeName
        {
            get { return _itemTypeName; }
            set { SetProperty(ref _itemTypeName, value); }
        }

        private int _itemType;
        public int itemType
        {
            get { return _itemType; }
            set { SetProperty(ref _itemType, value); }
        }

        private int _qty;
        public int qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }
        private decimal _unitprice;
        public decimal unitPrice
        {
            get { return _unitprice; }
            set { SetProperty(ref _unitprice, value); }
        }
        private decimal _unitpricemarkup;
        public decimal unitPriceMarkUp
        {
            get { return _unitpricemarkup; }
            set { SetProperty(ref _unitpricemarkup, value); }
        }

        private decimal _totalAmount;
        public decimal totalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
        }


        private decimal _totalAmountMarkUp;
        public decimal totalAmountMarkUp
        {
            get { return _totalAmountMarkUp; }
            set { SetProperty(ref _totalAmountMarkUp, value); }
        }

        private bool _qtyEditable;
        public bool qtyEditable
        {
            get { return _qtyEditable; }
            set { SetProperty(ref _qtyEditable, value); }
        }

        protected ObservableCollection<AdditionalFee> _additionalFees = new ObservableCollection<AdditionalFee>();

        public ObservableCollection<AdditionalFee> additionalFees
        {
            get { return _additionalFees; }
            set { _additionalFees = value; }
        }
    }

    public class InvoiceItem : ViewModelEntity
    {
        public InvoiceItem()
        {

        }
        private string _lineNo;
        public string lineNo
        {
            get { return _lineNo; }
            set { SetProperty(ref _lineNo, value); }
        }
        private string _itemCode;
        public string itemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }
        private string _itemName;
        public string itemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
        }
        private string _desc;
        public string desc
        {
            get { return _desc; }
            set { SetProperty(ref _desc, value); }
        }

        private int _itemType;
        public int itemType
        {
            get { return _itemType; }
            set { SetProperty(ref _itemType, value); }
        }

        private int _qty;
        public int qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal _unitPrice;
        public decimal unitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        private decimal _totalAmount;
        public decimal totalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
        }

        protected ObservableCollection<AdditionalFee> _additionalFees = new ObservableCollection<AdditionalFee>();

        public ObservableCollection<AdditionalFee> additionalFees
        {
            get { return _additionalFees; }
            set { _additionalFees = value; }
        }
    }

    public class SalesQuote : ViewModelEntity
    {
        public SalesQuote()
        {

        }

        protected string sqNoChar;
        public string sqNoChar_
        {
            get { return sqNoChar; }
            set { SetProperty(ref sqNoChar, value); }
        }
        protected DateTime dateOfIssue;
        public DateTime dateOfIssue_
        {
            get { return dateOfIssue; }
            set { SetProperty(ref dateOfIssue, value); }
        }
        protected int custID;
        public int custID_
        {
            get { return custID; }
            set { SetProperty(ref custID, value); }
        }
        protected string custName;
        public string custName_
        {
            get { return custName; }
            set { SetProperty(ref custName, value); }
        }
        protected int custRepID;
        public int custRepID_
        {
            get { return custRepID; }
            set { SetProperty(ref custRepID, value); }
        }
        protected string quoteSubject;
        public string quoteSubject_
        {
            get { return quoteSubject; }
            set { SetProperty(ref quoteSubject  , value); }
        }
        protected string body1;
        public string body1_
        {
            get { return body1; }
            set { SetProperty(ref body1, value); }
        }
        protected string priceNote;
        public string priceNote_
        {
            get { return priceNote; }
            set { SetProperty(ref priceNote, value); }
        }
        protected int ptID;
        public int ptID_
        {
            get { return ptID; }
            set { SetProperty(ref ptID, value); }
        }
        protected DateTime deliveryDate;
        public DateTime deliveryDate_
        {
            get { return deliveryDate; }
            set { SetProperty(ref deliveryDate, value); }
        }
        protected int estDelivery;
        public int estDelivery_
        {
            get { return estDelivery; }
            set { SetProperty(ref estDelivery, value); }
        }
        protected int validityDays;
        public int validityDays_
        {
            get { return validityDays; }
            set { SetProperty(ref validityDays, value); }
        }
        protected DateTime validityDate;
        public DateTime validityDate_
        {
            get { return validityDate; }
            set { SetProperty(ref validityDate, value); }
        }
        protected string otherTerms;
        public string otherTerms_
        {
            get { return otherTerms; }
            set { SetProperty(ref otherTerms, value); }
        }
        protected string body2;
        public string body2_
        {
            get { return body2; }
            set { SetProperty(ref body2, value); }
        }
        protected DateTime expiration;
        public DateTime expiration_
        {
            get { return expiration; }
            set { SetProperty(ref expiration, value); }
        }
        protected decimal vat;
        public decimal vat_
        {
            get { return vat; }
            set { SetProperty(ref vat, value); }
        }
        protected bool vatexcluded;
        public bool vatexcluded_
        {
            get { return vatexcluded; }
            set { SetProperty(ref vatexcluded, value); }
        }
        protected bool paymentIsLanded;
        public bool paymentIsLanded_
        {
            get { return paymentIsLanded; }
            set { SetProperty(ref paymentIsLanded, value); }
        }
        protected string paymentCurrency;
        public string paymentCurrency_
        {
            get { return paymentCurrency; }
            set { SetProperty(ref paymentCurrency, value); }
        }
        protected string deliveryAddress;
        public string deliveryAddress_
        {
            get { return deliveryAddress; }
            set { SetProperty(ref deliveryAddress, value); }
        }
        protected string status;
        public string status_
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }
        protected int termsDays;
        public int termsDays_
        {
            get { return termsDays; }
            set { SetProperty(ref termsDays, value); }
        }
        protected int termsDP;
        public int termsDP_
        {
            get { return termsDP; }
            set { SetProperty(ref termsDP, value); }
        }
        protected decimal penaltyAmt;
        public decimal penaltyAmt_
        {
            get { return penaltyAmt; }
            set { SetProperty(ref penaltyAmt, value); }
        }
        protected decimal penaltyPercent;
        public decimal penaltyPercent_
        {
            get { return penaltyPercent; }
            set { SetProperty(ref penaltyPercent, value); }
        }
        protected int warrantyDays;
        public int warrantyDays_
        {
            get { return warrantyDays; }
            set { SetProperty(ref warrantyDays, value); }
        }
        protected string additionalTerms;
        public string additionalTerms_
        {
            get { return additionalTerms; }
            set { SetProperty(ref additionalTerms, value); }
        }

        protected decimal markUpPercent;
        public decimal markUpPercent_
        {
            get { return markUpPercent; }
            set { SetProperty(ref markUpPercent, value); }
        }

        protected decimal discountPercent;
        public decimal discountPercent_
        {
            get { return discountPercent; }
            set { SetProperty(ref discountPercent, value); }
        }

        protected ObservableCollection<AddedService> addedServices = new ObservableCollection<AddedService>();

        public ObservableCollection<AddedService> AddedServices
        {
            get { return addedServices; }
            set { addedServices = value; }
        }

        protected ObservableCollection<AddedItem> addedItem = new ObservableCollection<AddedItem>();

        public ObservableCollection<AddedItem> AddedItems
        {
            get { return addedItem; }
            set { addedItem = value; }
        }
    }

    public class SalesInvoice : ViewModelEntity
    {
        public SalesInvoice()
        {

        }

        protected string invoiceNo;
        public string invoiceNo_
        {
            get { return invoiceNo; }
            set { SetProperty(ref invoiceNo, value); }
        }

        protected int custID;
        public int custID_
        {
            get { return custID; }
            set { SetProperty(ref custID, value); }
        }

        protected int empID;
        public int empID_
        {
            get { return empID; }
            set { SetProperty(ref empID, value); }
        }

        protected string sqNoChar;
        public string sqNoChar_
        {
            get { return sqNoChar; }
            set { SetProperty(ref sqNoChar, value); }
        }

        protected string tin;
        public string tin_
        {
            get { return tin; }
            set { SetProperty(ref tin, value); }
        }

        protected string busStyle;
        public string busStyle_
        {
            get { return busStyle; }
            set { SetProperty(ref busStyle, value); }
        }

        protected DateTime dateOfIssue;
        public DateTime dateOfIssue_
        {
            get { return dateOfIssue; }
            set { SetProperty(ref dateOfIssue, value); }
        }

        protected DateTime dueDate;
        public DateTime dueDate_
        {
            get { return dueDate; }
            set { SetProperty(ref dueDate, value); }
        }

        protected int terms;
        public int terms_
        {
            get { return terms; }
            set { SetProperty(ref terms, value); }
        }

        protected string purchaseOrderNumber;
        public string purchaseOrderNumber_
        {
            get { return purchaseOrderNumber; }
            set { SetProperty(ref purchaseOrderNumber, value); }
        }

        protected string paymentStatus;
        public string paymentStatus_
        {
            get { return paymentStatus; }
            set { SetProperty(ref paymentStatus, value); }
        }

        protected decimal vat;
        public decimal vat_
        {
            get { return vat; }
            set { SetProperty(ref vat, value); }
        }

        protected decimal sc_pwd_discount;
        public decimal sc_pwd_discount_
        {
            get { return sc_pwd_discount; }
            set { SetProperty(ref sc_pwd_discount, value); }
        }

        protected decimal withholdingTax;
        public decimal withholdingTax_
        {
            get { return withholdingTax; }
            set { SetProperty(ref withholdingTax, value); }
        }

        public string notes;
        public string notes_
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }

    }

    public class PaymentHist : ViewModelEntity
    {
        public PaymentHist()
        {

        }

        protected int custHistID;
        public int custHistID_
        {
            get { return custHistID; }
            set { SetProperty(ref custHistID, value); }
        }

        protected DateTime paymentDate;
        public DateTime paymentDate_
        {
            get { return paymentDate; }
            set { SetProperty(ref paymentDate, value); }
        }

        protected decimal custBalance;
        public decimal custBalance_
        {
            get { return custBalance; }
            set { SetProperty(ref custBalance, value); }
        }

        protected string paymentStatus;
        public string paymentStatus_
        {
            get { return paymentStatus; }
            set { SetProperty(ref paymentStatus, value); }
        }

        protected int invoiceNo;
        public int invoiceNo_
        {
            get { return invoiceNo; }
            set { SetProperty(ref invoiceNo, value); }
        }
    }

    public class ServiceSchedule : ViewModelEntity
    {
        public ServiceSchedule()
        {

        }

        protected string serviceSchedNoChar;
        public string serviceSchedNoChar_
        {
            get { return serviceSchedNoChar; }
            set { SetProperty(ref serviceSchedNoChar, value); }
        }

        protected int invoiceNo;
        public int invoiceNo_
        {
            get { return invoiceNo; }
            set { SetProperty(ref invoiceNo, value); }
        }

        protected string serviceStatus;
        public string serviceStatus_
        {
            get { return serviceStatus; }
            set { SetProperty(ref serviceStatus, value); }
        }

        protected DateTime dateStarted;
        public DateTime dateStarted_
        {
            get { return dateStarted; }
            set { SetProperty(ref dateStarted, value); }

        }

        protected DateTime dateEnded;
        public DateTime dateEnded_
        {
            get { return dateEnded; }
            set { SetProperty(ref dateEnded, value); }
        }

        protected string schedNotes;
        public string schedNotes_
        {
            get { return schedNotes; }
            set { SetProperty(ref schedNotes, value); }
        }

        protected ObservableCollection<AssignedEmployee> assignedEmployees = new ObservableCollection<AssignedEmployee>();
        public ObservableCollection<AssignedEmployee> assignedEmployees_
        {
            get { return assignedEmployees; }
            set { SetProperty(ref assignedEmployees, value); }
        }
    }

    public class AssignedEmployee : ViewModelEntity
    {
        public AssignedEmployee()
        {

        }

        protected int TableID;
        public int TableID_
        {
            get { return TableID; }
            set { SetProperty(ref TableID, value); }
        }

        protected string serviceSqNoChar;
        public string serviceSqNoChar_
        {
            get { return serviceSqNoChar; }
            set { SetProperty(ref serviceSqNoChar, value); }
        }

        protected int EmpID;
        public int EmpID_
        {
            get { return EmpID; }
            set { SetProperty(ref EmpID, value); }
        }
    }
}
