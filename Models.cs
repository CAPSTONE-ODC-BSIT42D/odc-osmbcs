using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected string representativeID;
        protected bool isDefault;
        protected ObservableCollection<Contact> contactsOfRep = new ObservableCollection<Contact>();

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

        public string RepresentativeID
        {
            get { return representativeID; }
            set { SetProperty(ref representativeID, value); }
        }
        public bool IsDefault
        {
            get { return isDefault; }
            set { SetProperty(ref isDefault, value); }
        }

        public ObservableCollection<Contact> ContactsOfRep
        {
            get { return contactsOfRep; }
            set { contactsOfRep = value; }
        }

    }

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
        public string CompanyProvinceName
        {
            get { return companyProvinceName; }
            set { SetProperty(ref companyProvinceName, value); }
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
        protected string empUsername;
        protected string empPassword;
        protected string jobID;
        protected string jobName;
        protected string empDateFrom;
        protected string empDateTo;
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

    public class Item : ViewModelEntity
    {
        public Item()
        {

        }
        protected bool isChecked;
        protected string itemNo;
        protected string itemName;
        protected string itemDesc;
        protected decimal costPrice;
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

        public string ItemNo
        {
            get { return itemNo; }
            set { SetProperty(ref itemNo, value); }
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

        protected string serviceID;
        protected string serviceName;
        protected decimal servicePrice;
        protected string address;
        protected string city;
        protected string provinceName;
        protected string provinceID;
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

        public decimal ServicePrice
        {
            get { return servicePrice; }
            set { SetProperty(ref servicePrice, value); }
        }
    }

    public class AdditionalFee : ViewModelEntity
    {
        public AdditionalFee()
        {

        }

        protected string feeName;
        protected decimal feePrice;

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
        public string itemName
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
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
    }

}
