using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype2
{
    #region Minor Models
    public class EmpPosition : ViewModelEntity
    {
        public EmpPosition()
        {

        }

        protected int positionid;
        protected string positionName;

        public int PositionID
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

        protected int jobid;
        protected string jobName;

        public int JobID
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
        protected int regionID;
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
        public int RegionID
        {
            get { return regionID; }
            set { SetProperty(ref regionID, value); }
        }
    }

    public class Region : ViewModelEntity
    {
        public Region()
        {

        }

        protected int regionID;

        public int RegionID
        {
            get { return regionID; }
            set { SetProperty(ref regionID, value); }
        }

        protected string regionName;
        public string RegionName
        {
            get { return regionName; }
            set { SetProperty(ref regionName, value); }
        }

        protected decimal ratePrice;
        public decimal RatePrice
        {
            get { return ratePrice; }
            set { SetProperty(ref ratePrice, value); }
        }

        protected ObservableCollection<Province> provinces = new ObservableCollection<Province>();

        public ObservableCollection<Province> Provinces
        {
            get { return provinces; }
            set { SetProperty(ref provinces, value); }
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

    public class AdditionalFee : ViewModelEntity
    {
        public AdditionalFee()
        {

        }

        protected int feeID;
        protected int servicesAvailedID;
        protected string feeName;
        protected decimal feePrice;

        public int FeeID
        {
            get { return feeID; }
            set { SetProperty(ref feeID, value); }
        }

        public int ServicesAvailedID
        {
            get { return servicesAvailedID; }
            set { SetProperty(ref servicesAvailedID, value); }
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

    public class Phase : ViewModelEntity
    {
        public Phase()
        {

        }

        protected int phaseID;

        public int PhaseID
        {
            get { return phaseID; }
            set { SetProperty(ref phaseID, value); }
        }

        protected string phaseName;

        public string PhaseName
        {
            get { return phaseName; }
            set { SetProperty(ref phaseName, value); }
        }

        protected string phaseDesc;
        public string PhaseDesc
        {

            get { return phaseDesc; }
            set { SetProperty(ref phaseDesc, value); }
        }

        protected int sequenceNo;
        public int SequenceNo
        {

            get { return sequenceNo; }
            set { SetProperty(ref sequenceNo, value); }
        }

        protected int phaseGroupID;
        public int PhaseGroupID
        {

            get { return phaseGroupID; }
            set { SetProperty(ref phaseGroupID, value); }
        }

        protected bool firstItem;
        public bool FirstItem
        {
            get { return firstItem; }
            set { SetProperty(ref firstItem, value); }
        }

        protected bool lastItem;
        public bool LastItem
        {
            get { return lastItem; }
            set { SetProperty(ref lastItem, value); }
        }

        protected bool isModified;
        public bool IsModified
        {
            get { return isModified; }
            set { SetProperty(ref isModified, value); }
        }
    }


    public class PhaseGroup : ViewModelEntity
    {
        public PhaseGroup()
        {

        }

        protected int phaseGroupID;

        public int PhaseGroupID
        {
            get { return phaseGroupID; }
            set { SetProperty(ref phaseGroupID, value); }
        }

        protected string phaseGroupName;

        public string PhaseGroupName
        {
            get { return phaseGroupName; }
            set { SetProperty(ref phaseGroupName, value); }
        }

        protected string phaseDesc;
        public string PhaseGroupDesc
        {

            get { return phaseDesc; }
            set { SetProperty(ref phaseDesc, value); }
        }

        protected int sequenceNo;
        public int SequenceNo
        {

            get { return sequenceNo; }
            set { SetProperty(ref sequenceNo, value); }
        }

        protected int serviceID;
        public int ServiceID
        {

            get { return serviceID; }
            set { SetProperty(ref serviceID, value); }
        }

        protected bool isModified;
        public bool IsModified
        {
            get { return isModified; }
            set { SetProperty(ref isModified, value); }
        }

        protected ObservableCollection<Phase> phaseItems = new ObservableCollection<Phase>();
        public ObservableCollection<Phase> PhaseItems
        {
            get { return phaseItems; }
            set { SetProperty(ref phaseItems, value); }
        }

    }

    public class Phases_Per_Service : ViewModelEntity
    {
        public Phases_Per_Service()
        {

        }

        protected int id;

        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
        
        protected int phaseID;
        public int PhaseID
        {

            get { return phaseID; }
            set { SetProperty(ref phaseID, value); }
        }

        protected int serviceSchedID;
        public int ServiceSchedID
        {

            get { return serviceSchedID; }
            set { SetProperty(ref serviceSchedID, value); }
        }

        protected string status;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

    }


    public class Unit : ViewModelEntity
    {
        public Unit()
        {

        }

        protected int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        protected string unitName;
        public string UnitName
        {
            get { return unitName; }
            set { SetProperty(ref unitName, value); }
        }

        protected string unitShorthand;
        public string UnitShorthand
        {
            get { return unitShorthand; }
            set { SetProperty(ref unitShorthand, value); }
        }
    }

    public class Markup_History : ViewModelEntity
    {
        public Markup_History()
        {

        }

        protected int markupID;
        public int MarkupID
        {
            get { return markupID; }
            set { SetProperty(ref markupID, value); }
        }

        protected decimal markupPerc;
        public decimal MarkupPerc
        {
            get { return markupPerc; }
            set { SetProperty(ref markupPerc, value); }
        }

        protected DateTime dateEffective;
        public DateTime DateEffective
        {
            get { return dateEffective; }
            set { SetProperty(ref dateEffective, value); }
        }

        protected int itemID;
        public int ItemID
        {
            get { return itemID; }
            set { SetProperty(ref itemID, value); }
        }
    }

    #endregion
    #region Major Models
    public class Customer : ViewModelEntity
    {
        public Customer()
        {

        }
        protected int companyID;
        protected string companyName;
        protected string companyDesc;
        protected string companyAddress;
        protected string companyCity;
        protected int companyProvinceID;
        protected string companyPostalCode;
        protected string companyEmail;
        protected string companyTelephone;
        protected string companyMobile;
        protected int companyType;
        protected string title;
        protected string firstname;
        protected string middlename;
        protected string lastname;
        protected string fullname;
        protected string emailAddress;
        protected string telephone;
        protected string mobilephone;

        public int CompanyID
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
        public int CompanyProvinceID
        {
            get { return companyProvinceID; }
            set { SetProperty(ref companyProvinceID, value); }
        }

        public string CompanyPostalCode
        {
            get { return companyPostalCode; }
            set { SetProperty(ref companyPostalCode, value); }
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

        public int CompanyType
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
        protected int empID;
        protected string empTitle;
        protected string empFName;
        protected string empLName;
        protected string empMI;
        protected int positionID;
        protected string empUsername;
        protected string empAddress;
        protected string empPassword;
        protected int jobID;
        protected DateTime empDateFrom;
        protected DateTime empDateTo;
        protected int empType;

        public int EmpID
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

        public string EmpAddress
        {
            get { return empAddress; }
            set { SetProperty(ref empAddress, value); }
        }

        public int PositionID
        {
            get { return positionID; }
            set { SetProperty(ref positionID, value); }
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

        public int JobID
        {
            get { return jobID; }
            set { SetProperty(ref jobID, value); }
        }

        public DateTime EmpDateFrom
        {
            get { return empDateFrom; }
            set { SetProperty(ref empDateFrom, value); }
        }

        public DateTime EmpDateTo
        {
            get { return empDateTo; }
            set { SetProperty(ref empDateFrom, value); }
        }

        public int EmpType
        {
            get { return empType; }
            set { SetProperty(ref empType, value); }
        }

        protected bool hasAccess;
        public bool HasAccess
        {
            get { return hasAccess; }
            set { SetProperty(ref hasAccess, value); }
        }
    }

    public class Item : ViewModelEntity
    {
        public Item()
        {

        }

        protected int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        protected string itemName;
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); }
        }

        protected string itemDesc;
        public string ItemDesc
        {
            get { return itemDesc; }
            set { SetProperty(ref itemDesc, value); }
        }

        protected decimal markupPerc;
        public decimal MarkUpPerc
        {
            get { return markupPerc; }
            set { SetProperty(ref markupPerc, value); }
        }

        protected int unitId;
        public int UnitID
        {
            get { return unitId; }
            set { SetProperty(ref unitId, value); }
        }

        protected int typeID;
        public int TypeID
        {
            get { return typeID; }
            set { SetProperty(ref typeID, value); }
        }

        protected int supplierID;
        public int SupplierID
        {
            get { return supplierID; }
            set { SetProperty(ref supplierID, value); }
        }

        protected DateTime dateEffective;
        public DateTime DateEffective
        {
            get { return dateEffective; }
            set { SetProperty(ref dateEffective, value); }
        }

    }

    public class Service : ViewModelEntity
    {
        public Service()
        {

        }

        protected int serviceID;
        protected string serviceName;
        protected string serviceDesc;
        protected decimal servicePrice;

        public int ServiceID
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

        protected ObservableCollection<PhaseGroup> phaseGroups = new ObservableCollection<PhaseGroup>();
        public ObservableCollection<PhaseGroup> PhaseGroups
        {
            get { return phaseGroups; }
            set { SetProperty(ref phaseGroups, value); }
        }
    }

    public class AvailedService : ViewModelEntity
    {
        public AvailedService()
        {

        }
        protected int availedServiceID;
        public int AvailedServiceID
        {
            get { return availedServiceID; }
            set { SetProperty(ref availedServiceID, value); }
        }

        protected int serviceID;
        public int ServiceID
        {
            get { return serviceID; }
            set { SetProperty(ref serviceID, value); }
        }

        protected int provinceID;
        public int ProvinceID
        {
            get { return provinceID; }
            set { SetProperty(ref provinceID, value); }
        }

        protected string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }


        protected string city;
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        protected string sqNoChar;
        public string SqNoChar
        {
            get { return sqNoChar; }
            set { SetProperty(ref sqNoChar, value); }
        }

        protected decimal totalCost;
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

    public class AvailedItem : ViewModelEntity
    {
        public AvailedItem()
        {

        }
        protected int availedItemID;
        protected string sqNoChar;
        protected int itemID;
        protected int itemQty;
        protected decimal totalCost;

        public int AvailedItemID
        {
            get { return availedItemID; }
            set { SetProperty(ref availedItemID, value); }
        }

        public string SqNoChar
        {
            get { return sqNoChar; }
            set { SetProperty(ref sqNoChar, value); }
        }

        public int ItemID
        {
            get { return itemID; }
            set { SetProperty(ref itemID, value); }
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

    public class RequestedItem : ViewModelEntity
    {
        public RequestedItem()
        {

        }
        private int _lineNo;
        public int lineNo
        {
            get { return _lineNo; }
            set { SetProperty(ref _lineNo, value); }
        }
        protected int _availedItemID;
        public int availedItemID
        {
            get { return _availedItemID; }
            set { SetProperty(ref _availedItemID, value); }
        }

        private int _itemID;
        public int itemID
        {
            get { return _itemID; }
            set { SetProperty(ref _itemID, value); }
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

        private decimal _unitPriceMarkUp;
        public decimal unitPriceMarkUp
        {
            get { return _unitPriceMarkUp; }
            set { SetProperty(ref _unitPriceMarkUp, value); }
        }
        

        private decimal _totalAmount;
        public decimal totalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
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
        
        protected ObservableCollection<AvailedService> availedServices = new ObservableCollection<AvailedService>();

        public ObservableCollection<AvailedService> AvailedServices
        {
            get { return availedServices; }
            set { availedServices = value; }
        }

        protected ObservableCollection<AvailedItem> availedItems = new ObservableCollection<AvailedItem>();

        public ObservableCollection<AvailedItem> AvailedItems
        {
            get { return availedItems; }
            set { availedItems = value; }
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

    public class PurchaseOrder : ViewModelEntity
    {
        public PurchaseOrder()
        {

        }
        public string notes;
        public string notes_
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
    }

    public class PaymentT : ViewModelEntity
    {
        public PaymentT()
        {

        }

        protected int SIpaymentID;
        public int SIpaymentID_
        {
            get { return SIpaymentID; }
            set { SetProperty(ref SIpaymentID, value); }
        }

        protected decimal SIpaymentAmount;
        public decimal SIpaymentAmount_
        {
            get { return SIpaymentAmount; }
            set { SetProperty(ref SIpaymentAmount, value); }
        }

        protected DateTime SIpaymentDate;
        public DateTime SIpaymentDate_
        {
            get { return SIpaymentDate; }
            set { SetProperty(ref SIpaymentDate, value); }
        }

        protected string SIpaymentMethod;
        public string SIpaymentMethod_
        {
            get { return SIpaymentMethod; }
            set { SetProperty(ref SIpaymentMethod, value); }
        }

        protected string SIcheckNo;
        public string SIcheckNo_
        {
            get { return SIcheckNo; }
            set { SetProperty(ref SIcheckNo, value); }
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

        protected ObservableCollection<Employee> assignedEmployees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> assignedEmployees_
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

        protected ObservableCollection<Employee> allEmployeesContractor =
            new ObservableCollection<Employee>();

        public ObservableCollection<Employee> AllEmployeesContractor
        {
            get { return allEmployeesContractor; }
            set { allEmployeesContractor = value; }
        }
    }
    #endregion
}
