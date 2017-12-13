using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Window
    {
        string empID;

        public MainScreen(string empID)
        {
            InitializeComponent();
            this.empID = empID;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            
        }

        public static Commands commands = new Commands();
        public static readonly BackgroundWorker worker = new BackgroundWorker();
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
            this.ucEmployee.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucCustSupp.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            
            this.ucProduct.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucInvoice.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucSalesQuote.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucSalesQuote.ConvertToInvoice += convertToInvoice_BtnClicked;
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            foreach (var obj in formGridBg.Children)
            {
                if(obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                else if(obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
            formGridBg.Visibility = Visibility.Collapsed;
            MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { loadDataToUi(); }));
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            
        }

        private void monitoringOfData()
        {

        }

        private void loadDataToUi()
        {
            var dbCon = DBConnection.Instance();
            MainVM.Provinces.Clear();
            MainVM.EmpPosition.Clear();
            MainVM.ContJobTitle.Clear();
            MainVM.ProductCategory.Clear();
            MainVM.ServicesList.Clear();
            MainVM.ProductList.Clear();


            MainVM.AllCustomerSupplier.Clear();
            MainVM.Customers.Clear();
            MainVM.Suppliers.Clear();

            MainVM.AllEmployeesContractor.Clear();
            MainVM.Employees.Clear();
            MainVM.Contractor.Clear();

            MainVM.RequestedItems.Clear();
            MainVM.AddedItems.Clear();
            MainVM.AddedServices.Clear();
            MainVM.AdditionalFees.Clear();

            MainVM.SalesQuotes.Clear();

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM si_payment_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                { 
                    DateTime paymentDate = new DateTime();
                    DateTime.TryParse(dr["SIpaymentDate"].ToString(), out paymentDate);
                    MainVM.PaymentList_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString()});
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT locProvinceID, locProvince, locPrice FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    if (dr["locPrice"].ToString().Equals(""))
                        MainVM.Provinces.Add(new Province() { ProvinceID = (int)dr["locProvinceID"], ProvinceName = dr["locProvince"].ToString() });
                    else
                        MainVM.Provinces.Add(new Province() { ProvinceID = (int)dr["locProvinceID"], ProvinceName = dr["locProvince"].ToString(), ProvincePrice = (decimal)dr["locPrice"] });
                }
                dbCon.Close();
            }


            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM POSITION_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.EmpPosition.Add(new EmpPosition() { PositionID = dr["positionid"].ToString(), PositionName = dr["positionName"].ToString() });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM JOB_TITLE_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ContJobTitle.Add(new ContJobName() { JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString() });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_TYPE_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ProductCategory.Add(new ItemType() { TypeID = int.Parse(dr["typeID"].ToString()), TypeName = dr["typeName"].ToString() });
                }

            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM services_t where isDeleted = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.ServicesList.Add(new Service() { ServiceID = dr["serviceID"].ToString(), ServiceName = dr["serviceName"].ToString(), ServiceDesc = dr["serviceDesc"].ToString(), ServicePrice = (decimal)dr["ServicePrice"] });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_T WHERE isDeleted = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.SelectedCustomerSupplier = MainVM.Suppliers.Where(x => x.CompanyID.Equals(dr["supplierID"].ToString())).FirstOrDefault();
                    MainVM.SelectedProductCategory = MainVM.ProductCategory.Where(x => x.TypeID == int.Parse(dr["typeID"].ToString())).FirstOrDefault();
                    if (MainVM.SelectedCustomerSupplier != null)
                    {
                        MainVM.ProductList.Add(new Item() { ItemCode = dr["itemCode"].ToString(), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), TypeName = MainVM.SelectedProductCategory.TypeName, Quantity = 1, SupplierID = dr["supplierID"].ToString(), SupplierName = MainVM.SelectedCustomerSupplier.CompanyName });
                    }
                    else
                        MainVM.ProductList.Add(new Item() { ItemCode = dr["itemCode"].ToString(), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), TypeName = MainVM.SelectedProductCategory.TypeName, Quantity = 1, SupplierID = dr["supplierID"].ToString() });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var customer = (new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() });
                    MainVM.Customers.Add(customer);
                    MainVM.AllCustomerSupplier.Add(customer);
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    var supplier = new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() };
                    MainVM.Suppliers.Add(supplier);
                    MainVM.AllCustomerSupplier.Add(supplier);
                }
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM emp_cont_t a JOIN provinces_t b ON a.empProvinceID = b.locProvinceId JOIN position_t c ON a.positionID = c.positionid  WHERE isDeleted = 0 AND empType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    byte[] empPic = null;
                    if (!dr["empPic"].Equals(DBNull.Value))
                    {
                        empPic = (byte[])dr["empPic"];
                    }
                    int empType;
                    int.TryParse(dr["empType"].ToString(), out empType);
                    MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = empType });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = empType });


                }
                dbCon.Close();
            }
            MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM emp_cont_t a  " +
                    "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
                    "JOIN job_title_t d ON a.jobID = d.jobID " +
                    "WHERE a.isDeleted = 0 AND a.empType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Contractor.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    byte[] empPic = null;
                    if (!dr["empPic"].Equals(DBNull.Value))
                    {
                        empPic = (byte[])dr["empPic"];
                    }
                    int empType;
                    int.TryParse(dr["empType"].ToString(), out empType);
                    MainVM.Contractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = empType });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = empType });
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM sales_quote_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.SalesQuotes.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateOfIssue = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out dateOfIssue);

                    DateTime deliveryDate = new DateTime();
                    DateTime.TryParse(dr["deliveryDate"].ToString(), out deliveryDate);

                    DateTime validityDate = new DateTime();
                    DateTime.TryParse(dr["validityDate"].ToString(), out validityDate);

                    DateTime expiration = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out expiration);

                    bool vatIsExcluded = false;

                    if (dr["vatIsExcluded"].ToString().Equals("1"))
                    {
                        vatIsExcluded = true;
                    }

                    bool paymentIsLanded = false;

                    if (dr["paymentIsLanded"].ToString().Equals("1"))
                    {
                        paymentIsLanded = true;
                    }
                    MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(dr["custID"].ToString())).FirstOrDefault();

                    int custId;
                    int.TryParse(dr["custID"].ToString(),out custId);

                    int estDelivery;
                    int.TryParse(dr["estDelivery"].ToString(), out estDelivery);

                    int validityDays;
                    int.TryParse(dr["validityDays"].ToString(), out validityDays);

                    decimal vat;
                    decimal.TryParse(dr["vat"].ToString(), out vat);

                    int termsDays;
                    int.TryParse(dr["termsDays"].ToString(), out termsDays);

                    int termsDP;
                    int.TryParse(dr["termsDP"].ToString(), out termsDP);

                    decimal penaltyAmt;
                    decimal.TryParse(dr["penaltyAmt"].ToString(), out penaltyAmt);

                    int penaltyPerc;
                    int.TryParse(dr["penaltyPerc"].ToString(), out penaltyPerc);

                    decimal markUpPerc;
                    decimal.TryParse(dr["markUpPercent"].ToString(), out markUpPerc);

                    decimal discountPerc;
                    decimal.TryParse(dr["discountPercent"].ToString(), out discountPerc);

                    if (MainVM.SelectedCustomerSupplier != null)
                    {
                        MainVM.SalesQuotes.Add(new SalesQuote()
                        {
                            sqNoChar_ = dr["sqNoChar"].ToString(),
                            dateOfIssue_ = dateOfIssue,
                            custID_ = custId,
                            custName_ = MainVM.SelectedCustomerSupplier.CompanyName,
                            quoteSubject_ = dr["quoteSubject"].ToString(),
                            priceNote_ = dr["priceNote"].ToString(),
                            deliveryDate_ = deliveryDate,
                            estDelivery_ = estDelivery,
                            validityDays_ = validityDays,
                            validityDate_ = validityDate,
                            otherTerms_ = dr["otherTerms"].ToString(),
                            expiration_ = expiration,
                            vat_ = vat,
                            vatexcluded_ = vatIsExcluded,
                            paymentIsLanded_ = paymentIsLanded,
                            paymentCurrency_ = dr["paymentCurrency"].ToString(),
                            status_ = dr["status"].ToString(),
                            termsDays_ = termsDays,
                            termsDP_ = termsDP,
                            penaltyAmt_ = penaltyAmt,
                            penaltyPercent_ = penaltyAmt,
                            markUpPercent_ = markUpPerc,
                            discountPercent_ = discountPerc
                        });
                    }
                }
                query = "SELECT * FROM services_availed_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                fromDb = new DataSet();
                fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                query = "SELECT * FROM fees_per_transaction_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb2 = new DataSet();
                DataTable fromDbTable2 = new DataTable();
                dataAdapter.Fill(fromDb2, "t");
                fromDbTable2 = fromDb2.Tables["t"];

                query = "SELECT * FROM items_availed_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb3 = new DataSet();
                DataTable fromDbTable3 = new DataTable();
                dataAdapter.Fill(fromDb3, "t");
                fromDbTable3 = fromDb3.Tables["t"];
                foreach (SalesQuote sq in MainVM.SalesQuotes)
                {
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        if (dr["sqNoChar"].ToString().Equals(sq.sqNoChar_))
                        {
                            MainVM.SelectedAddedService = (new AddedService() { TableNoChar = dr["tableNoChar"].ToString(), SqNoChar = dr["sqNoChar"].ToString(), ServiceID = dr["serviceID"].ToString(), ProvinceID = int.Parse(dr["provinceID"].ToString()), City = dr["city"].ToString(), Address = dr["address"].ToString(), TotalCost = decimal.Parse(dr["totalCost"].ToString()) });
                            foreach (DataRow dr2 in fromDbTable2.Rows)
                            {
                                if (dr["tableNoChar"].ToString().Equals(dr2["serviceNochar"].ToString()))
                                {
                                    MainVM.SelectedAddedService.AdditionalFees.Add(new AdditionalFee() { ServiceNoChar = dr2["serviceNoChar"].ToString(), FeeName = dr2["feeName"].ToString(), FeePrice = decimal.Parse(dr2["feeValue"].ToString()) });
                                }
                            }
                            sq.AddedServices.Add(MainVM.SelectedAddedService);
                            MainVM.AddedServices.Add(MainVM.SelectedAddedService);
                        }
                    }
                    foreach (DataRow dr3 in fromDbTable3.Rows)
                    {
                        if (dr3["sqNoChar"].ToString().Equals(sq.sqNoChar_))
                        {
                            sq.AddedItems.Add(new AddedItem() { TableID = int.Parse(dr3["tableID"].ToString()), SqNoChar = dr3["sqNoChar"].ToString(), ItemCode = dr3["itemCode"].ToString(), ItemQty = int.Parse(dr3["itemQnty"].ToString()), TotalCost = decimal.Parse(dr3["totalCost"].ToString()) });
                        }
                    }


                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM sales_invoice_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.SalesInvoice.Clear();

                //query = "SELECT * FROM payment_hist_t;";
                //MySqlDataAdapter dataAdapter2 = dbCon.selectQuery(query, dbCon.Connection);
                //DataSet fromDb2 = new DataSet();
                //DataTable fromDbTable2 = new DataTable();
                //dataAdapter2.Fill(fromDb2, "t");
                //fromDbTable2 = fromDb2.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateOfIssue = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out dateOfIssue);

                    DateTime dueDate = new DateTime();
                    DateTime.TryParse(dr["dueDate"].ToString(), out dueDate);
                    MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(dr["custID"].ToString())).FirstOrDefault();

                    int invoiceNo;
                    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                    int custId;
                    int.TryParse(dr["custID"].ToString(), out custId);

                    //int empId;
                    //int.TryParse(dr["empID"].ToString(), out empId);

                    int estDelivery;
                    int.TryParse(dr["invoiceNo"].ToString(), out estDelivery);

                    int termsDays;
                    int.TryParse(dr["termsDays"].ToString(), out termsDays);

                    decimal vat;
                    decimal.TryParse(dr["vat"].ToString(), out vat);

                    decimal sc_pwd_discount;
                    decimal.TryParse(dr["sc_pwd_discount"].ToString(), out sc_pwd_discount);

                    decimal withholdingTax;
                    decimal.TryParse(dr["withholdingTax"].ToString(), out withholdingTax);

                    MainVM.SalesInvoice.Add(new SalesInvoice() { invoiceNo_ = invoiceNo.ToString(), custID_ = custId, sqNoChar_ = dr["sqNoChar"].ToString(), tin_ = dr["tin"].ToString(), busStyle_ = dr["busStyle"].ToString(), dateOfIssue_ = dateOfIssue, terms_ = termsDays, dueDate_ = dueDate, purchaseOrderNumber_ = dr["purchaseOrderNumber"].ToString(), paymentStatus_ = dr["paymentStatus"].ToString(), vat_ = vat, sc_pwd_discount_ = sc_pwd_discount, withholdingTax_ = withholdingTax, notes_ = dr["notes"].ToString() });


                    
                }
                //foreach (DataRow dr in fromDbTable2.Rows)
                //{
                //    DateTime paymentDate = new DateTime();
                //    DateTime.TryParse(dr["paymentDate"].ToString(), out paymentDate);

                //    int invoiceNo;
                //    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                //    decimal custBalance;
                //    decimal.TryParse(dr["custBalance"].ToString(), out custBalance);
                //    MainVM.PaymentHistory_.Add(new PaymentHist() { custHistID_ = int.Parse(dr["custHistID"].ToString()), paymentDate_ = paymentDate, custBalance_ = custBalance, invoiceNo_ = invoiceNo, paymentStatus_ = dr["paymentStatus"].ToString() });
                //}
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM service_sched_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                query = "SELECT * FROM assigned_employees_t;";
                MySqlDataAdapter dataAdapter2 = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb2 = new DataSet();
                DataTable fromDbTable2 = new DataTable();
                dataAdapter2.Fill(fromDb2, "t");
                fromDbTable2 = fromDb2.Tables["t"];

                MainVM.ServiceSchedules_.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime dateStarted = new DateTime();
                    DateTime.TryParse(dr["dateStarted"].ToString(), out dateStarted);

                    DateTime dateEnded = new DateTime();
                    DateTime.TryParse(dr["dateEnded"].ToString(), out dateEnded);

                    int invoiceNo;
                    int.TryParse(dr["invoiceNo"].ToString(), out invoiceNo);

                    MainVM.SelectedServiceSchedule_ = (new ServiceSchedule() { serviceSchedNoChar_ = dr["serviceSchedNoChar"].ToString(), invoiceNo_ = invoiceNo , serviceStatus_ = dr["serviceStatus"].ToString(), dateStarted_ = dateStarted, dateEnded_ = dateEnded, schedNotes_ = dr["schedNotes"].ToString() });
                    if (fromDbTable2.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in fromDbTable2.Rows)
                        {

                            MainVM.SelectedEmployeeContractor = MainVM.AllEmployeesContractor.Where(x => x.EmpID.Equals(dr2["empID"].ToString())).FirstOrDefault();
                            MainVM.SelectedServiceSchedule_.assignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
                        }
                    }
                    
                }
                
                

                MainVM.ServiceSchedules_.Clear();
                dbCon.Close();
            }
        }

        private void resetValueofSelectedVariables()
        {
            MainVM.SelectedCustomerSupplier = null;
            MainVM.SelectedEmployeeContractor = null;
            MainVM.SelectedAddedItem = null;
            MainVM.SelectedSalesQuote = null;
        }

        private void dashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
        }

        private void salesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (salesBtnSubMenuGrid.IsVisible)
                salesBtnSubMenuGrid.Visibility = Visibility.Collapsed;
            else
                salesBtnSubMenuGrid.Visibility = Visibility.Visible;
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            serviceGrid.Visibility = Visibility.Visible;
            ucService.Visibility = Visibility.Visible;
        }

        private void reportsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            reportsGrid.Visibility = Visibility.Visible;
        }

        private void manageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (manageSubMenugrid.IsVisible)
                manageSubMenugrid.Visibility = Visibility.Collapsed;
            else
                manageSubMenugrid.Visibility = Visibility.Visible;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in settingsGridStackPanel.Children)
            {
                if (obj is Grid)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
            }
            if (manageEmployeeGrid.IsVisible)
            {
                employeeSettings1.Visibility = Visibility.Visible;
                employeeSettings2.Visibility = Visibility.Visible;
            }
            else if (manageProductListGrid.IsVisible)
            {
                productSettings1.Visibility = Visibility.Visible;
            }
            foreach (var obj in formGridBg.Children)
            {
                if(obj is Grid)
                {
                    if (!((Grid)obj).Name.Equals("settingsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
            }
            formGridBg.Visibility = Visibility.Visible;

        }

        private void closeSideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }

        private void quotesSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            trasanctionGrid.Visibility = Visibility.Visible;
            foreach (var obj in trasanctionGrid.Children)
            {
                if (obj is Grid)
                    if (((Grid)obj).Equals(transQuotationGrid))
                        ((Grid)obj).Visibility = Visibility.Visible;
                    else
                        ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            foreach (var obj in transQuotationGrid.Children)
            {
                if (obj is Grid)
                {
                    if (((Grid)obj).Equals(quotationsGridHome))
                    {
                        headerLbl.Content = "Trasanction - Sales Quote";
                        ((Grid)obj).Visibility = Visibility.Visible;
                        settingsBtn.Visibility = Visibility.Hidden;
                    }
                } 
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                        
            }
            
            
        }

        private void ordersSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void invoiceSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isViewHome = true;
            headerLbl.Content = "Trasanction - Sales Invoice";
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            trasanctionGrid.Visibility = Visibility.Visible;
            foreach (var obj in trasanctionGrid.Children)
            {
                if (obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                else if (obj is UserControl)
                    if(((UserControl)obj).Equals(ucInvoice))
                        ((UserControl)obj).Visibility = Visibility.Visible;
            }
        }

        private void empContManageBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageEmployeeGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Visible;
            headerLbl.Content = "Manage Employee/Contractor";
        }

        private void custSuppManageBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageCustomerGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
            headerLbl.Content = "Manage Customer/Supplier";
        }

        private void productManageBtn_Click(object sender, RoutedEventArgs e)
        {
            headerLbl.Content = "Manage Product List";
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageProductListGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Visible;
        }

        private void settingsManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            foreach (var obj in formGridBg.Children)
            {
                if (obj is Grid)
                {
                    if (!((Grid)obj).Name.Equals("settingsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
               

            }
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in settingsGridStackPanel.Children)
            {
                if (obj is Grid)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
            }
            serviceSettings1.Visibility = Visibility.Visible;
            serviceSettings2.Visibility = Visibility.Visible;
        }

        private void saveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            if (ucSalesQuote.IsVisible)
            {
                foreach (var obj in containerGrid.Children)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                trasanctionGrid.Visibility = Visibility.Visible;
                foreach (var obj in trasanctionGrid.Children)
                {
                    if (obj is Grid)
                        if (((Grid)obj).Equals(transQuotationGrid))
                            ((Grid)obj).Visibility = Visibility.Visible;
                        else
                            ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                foreach (var obj in transQuotationGrid.Children)
                {
                    if (obj is Grid)
                    {
                        if (((Grid)obj).Equals(quotationsGridHome))
                        {
                            headerLbl.Content = "Trasanction - Sales Quote";
                            ((Grid)obj).Visibility = Visibility.Visible;
                            settingsBtn.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                        ((UserControl)obj).Visibility = Visibility.Collapsed;

                }
            }
            if (ucInvoice.IsVisible)
                ucInvoice.Visibility = Visibility.Collapsed;
            //if (ucInvoice.IsVisible)
            //{
            //    foreach (var obj in containerGrid.Children)
            //    {
            //        ((Grid)obj).Visibility = Visibility.Collapsed;
            //    }
            //    trasanctionGrid.Visibility = Visibility.Visible;
            //    foreach (var obj in trasanctionGrid.Children)
            //    {
            //        if (obj is Grid)
            //            if (((Grid)obj).Equals(transInvoiceGrid))
            //                ((Grid)obj).Visibility = Visibility.Visible;
            //            else
            //                ((Grid)obj).Visibility = Visibility.Collapsed;
            //    }
            //    foreach (var obj in transInvoiceGrid.Children)
            //    {
            //        if (obj is Grid)
            //        {
            //            if (((Grid)obj).Equals(invoiceGridHome))
            //            {
            //                headerLbl.Content = "Trasanction - Sales Invoice";
            //                ((Grid)obj).Visibility = Visibility.Visible;
            //                settingsBtn.Visibility = Visibility.Hidden;
            //            }
            //        }
            //        else
            //            ((UserControl)obj).Visibility = Visibility.Collapsed;

            //    }
            //}
            //{
            //    foreach (var obj in containerGrid.Children)
            //    {
            //        ((Grid)obj).Visibility = Visibility.Collapsed;
            //    }
            //    trasanctionGrid.Visibility = Visibility.Visible;
            //    foreach (var obj in trasanctionGrid.Children)
            //    {
            //        if (obj is Grid)
            //            if (((Grid)obj).Equals(transInvoiceGrid))
            //                ((Grid)obj).Visibility = Visibility.Visible;
            //            else
            //                ((Grid)obj).Visibility = Visibility.Collapsed;
            //    }
            //    foreach (var obj in transInvoiceGrid.Children)
            //    {
            //        if (obj is Grid)
            //        {
            //            if (((Grid)obj).Equals(invoiceGridHome))
            //            {
            //                headerLbl.Content = "Trasanction - Sales Invoice";
            //                ((Grid)obj).Visibility = Visibility.Visible;
            //                settingsBtn.Visibility = Visibility.Hidden;
            //            }
            //        }
            //        else
            //            ((UserControl)obj).Visibility = Visibility.Collapsed;

            //    }
            //}
            MainVM.StringTextBox = null;
            MainVM.DecimalTextBox = 0;
            MainVM.IntegerTextBox = 0;
            MainVM.cbItem = null;
            MainVM.isNewRecord = false;
            MainVM.isViewHome = false;
            MainVM.isEdit = false;
            MainVM.isPaymentInvoice = false;
            closeModals();
            worker.RunWorkerAsync();
        }

        

        //Maintenance

        private void dataGridType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (manageCustomerGrid.IsVisible)
                {
                    if (compType.SelectedIndex == 0)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.AllCustomerSupplier;
                    }
                    else if (compType.SelectedIndex == 1)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.Customers;
                    }
                    else if (compType.SelectedIndex == 2)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.Suppliers;
                    }
                }
                else if (manageEmployeeGrid.IsVisible)
                {
                    if (empType.SelectedIndex == 0)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.AllEmployeesContractor;
                    }
                    else if (empType.SelectedIndex == 1)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Employees;
                    }
                    else if (empType.SelectedIndex == 2)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Contractor;
                    }
                }
            }
        }


        private void manageEmployeeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucEmployee"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }
                }
                

            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void manageCustomerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            foreach (var obj in formGridBg.Children)
            {

                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucCustSupp"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }

                }

            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void manageProductAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucProduct"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                    }
                }
                

            }
            formGridBg.Visibility = Visibility.Visible;
        }

        
        private bool validationError = false;

        private void viewRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = true;
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {
                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucCustSupp"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }

                    }

                }
                ucCustSupp.saveCancelGrid.Visibility = Visibility.Collapsed;
                ucCustSupp.editCloseGrid.Visibility = Visibility.Visible;
                foreach (var element in ucCustSupp.companyDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        ((TextBox)element).IsEnabled = false;
                    }
                    else if (element is ComboBox)
                    {
                        ((ComboBox)element).IsEnabled = false;
                    }
                    else if (element is CheckBox)
                    {
                        ((CheckBox)element).IsEnabled = false;
                    }
                }
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {
                    if(obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucEmployee"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }
                    }
                }
                ucEmployee.saveCancelGrid1.Visibility = Visibility.Collapsed;
                ucEmployee.editCloseGrid1.Visibility = Visibility.Visible;
                foreach (var element in ucEmployee.employeeDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        ((TextBox)element).IsEnabled = false;
                    }
                    else if (element is ComboBox)
                    {
                        ((ComboBox)element).IsEnabled = false;
                    }
                    else if (element is CheckBox)
                    {
                        ((CheckBox)element).IsEnabled = false;
                    }
                    else if(element is PasswordBox)
                    {
                        ((CheckBox)element).IsEnabled = false;
                    }
                    else if(element is Xceed.Wpf.Toolkit.DateTimePicker)
                    {
                        ((Xceed.Wpf.Toolkit.DateTimePicker)element).IsEnabled = false;
                    }
                }
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {
                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucProduct"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }
                    }

                }
                ucProduct.saveCancelGrid2.Visibility = Visibility.Collapsed;
                ucProduct.editCloseGrid2.Visibility = Visibility.Visible;
                foreach (var element in ucProduct.productDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        ((TextBox)element).IsEnabled = false;
                    }
                    if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                    {
                        ((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled = false;
                    }
                    if (element is ComboBox)
                    {
                        ((ComboBox)element).IsEnabled = false;
                    }
                }
            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void editRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            formGridBg.Visibility = Visibility.Visible;
            MainVM.isEdit = true;
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucCustSupp"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }

                    }
                }
                foreach (var element in ucCustSupp.companyDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        ((TextBox)element).IsEnabled = true;
                    }
                    else if (element is ComboBox)
                    {
                        ((ComboBox)element).IsEnabled = true;
                    }
                    else if (element is CheckBox)
                    {
                        ((CheckBox)element).IsEnabled = true;
                    }
                }
                ucCustSupp.saveCancelGrid.Visibility = Visibility.Visible;
                ucCustSupp.editCloseGrid.Visibility = Visibility.Collapsed;
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucEmployee"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }
                    }

                }
                foreach (var element in ucEmployee.employeeDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        ((TextBox)element).IsEnabled = true;
                    }
                    else if (element is ComboBox)
                    {
                        ((ComboBox)element).IsEnabled = true;
                    }
                    else if (element is CheckBox)
                    {
                        ((CheckBox)element).IsEnabled = true;
                    }
                    else if (element is PasswordBox)
                    {
                        ((CheckBox)element).IsEnabled = true;
                    }
                    else if (element is Xceed.Wpf.Toolkit.DateTimePicker)
                    {
                        ((Xceed.Wpf.Toolkit.DateTimePicker)element).IsEnabled = true;
                    }
                }
                ucEmployee.saveCancelGrid1.Visibility = Visibility.Visible;
                ucEmployee.editCloseGrid1.Visibility = Visibility.Collapsed;
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucProduct"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                        }
                    }
                }
                
                ucProduct.saveCancelGrid2.Visibility = Visibility.Visible;
                ucProduct.editCloseGrid2.Visibility = Visibility.Collapsed;
            }

        }

        private void deleteRecordBtn_Click(object sender, RoutedEventArgs e)
        {

            if (manageCustomerGrid.IsVisible)
            {
                String id = MainVM.SelectedCustomerSupplier.CompanyID;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `cust_supp_t` SET `isDeleted`= 1 WHERE companyID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            loadDataToUi();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                String id = MainVM.SelectedEmployeeContractor.EmpID;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `emp_cont_t` SET `isDeleted`= 1 WHERE empID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            loadDataToUi();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
            else if (manageProductListGrid.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `item_t` SET `isDeleted`= 1 WHERE itemCode = '" + MainVM.SelectedProduct.ItemCode + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            loadDataToUi();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
        }

        

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }
        public bool isEdit = false;

        
        
        public void closeModals()
        {

            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                if(obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                else if(obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;

            }
            

        }

        //Settings Grid

        private static String dbname = "odc_db";

        //EMPLOYEE PART
        private void addEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addEmpPosBtn.Content.Equals("Save"))
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
                {
                    MessageBox.Show("Employee Position must be filled");
                }
                else
                {
                    if (employeePositionLb.Items.Contains(empPosNewTb.Text))
                    {
                        MessageBox.Show("Employee Position already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + MainVM.SelectedEmpPosition.PositionID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Employee Poisition saved");
                            addEmpPosBtn.Content = "Add";
                            empPosNewTb.Clear();
                            loadDataToUi();
                            dbCon.Close();
                        }
                    }
                }
            }
            else
            {
                string strPosition = empPosNewTb.Text;
                if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
                {
                    MessageBox.Show("Employee Position field must be filled");
                }
                else
                {
                    var dbCon = DBConnection.Instance();
                    dbCon.DatabaseName = dbname;
                    if (employeePositionLb.Items.Contains(empPosNewTb.Text))
                    {
                        MessageBox.Show("Employee position already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        if (!Regex.IsMatch(strPosition, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special characters are not accepted");
                            empPosNewTb.Clear();
                        }
                        else
                        {
                            string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                {
                                    MessageBox.Show("Employee Position successfully added");
                                    empPosNewTb.Clear();
                                    loadDataToUi();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
            }

        }


        private void deleteEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employeePositionLb.SelectedItems.Count > 0)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    try
                    {
                        string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + MainVM.SelectedEmpPosition.PositionID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Employee position successfully deleted.");
                            loadDataToUi();
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select an employee position first.");
            }

        }

        private void editEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                if (employeePositionLb.SelectedItems.Count > 0)
                {
                    empPosNewTb.Text = MainVM.SelectedEmpPosition.PositionName;
                    addEmpPosBtn.Content = "Save";

                }
                else
                {
                    MessageBox.Show("Please select an employee position first.");
                }
            }
            dbCon.Close();
        }

        //CONTRACTOR PART
        private void addContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addContJobBtn.Content.Equals("Save"))
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
                {
                    MessageBox.Show("Contractor Job Title field must be filled");
                }
                else
                {
                    if (contJobLb.Items.Contains(contNewJobTb.Text))
                    {
                        MessageBox.Show("Job Title already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `odc_db`.`job_title_t` set `jobName` = '" + contNewJobTb.Text + "' where jobID = '" + MainVM.SelectedJobTitle.JobID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Job Title successfully saved");
                            contNewJobTb.Clear();
                            loadDataToUi();
                            dbCon.Close();
                            contNewJobTb.Clear();
                            addContJobBtn.Content = "Add";


                        }
                    }
                }
            }
            else
            {
                string strJobTitle = contNewJobTb.Text;
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
                {
                    MessageBox.Show("Contractor Job Title field must be field");
                }
                else
                {
                    if (contJobLb.Items.Contains(contNewJobTb.Text))
                    {
                        MessageBox.Show("Contractor Job Title already exists");
                    }
                    else
                    {
                        if (!Regex.IsMatch(strJobTitle, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special Characters are not accepted");
                            contNewJobTb.Clear();
                        }
                        else
                        {
                            if (dbCon.IsConnect())
                            {
                                string query = "INSERT INTO `odc_db`.`job_title_t` (`jobName`) VALUES('" + contNewJobTb.Text + "')";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {
                                    MessageBox.Show("Contractor Job Title successfully added");
                                    contNewJobTb.Clear();
                                    loadDataToUi();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void deleteContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            if (contJobLb.SelectedItems.Count > 0)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    try
                    {
                        string query = "DELETE FROM `odc_db`.`job_title_t` WHERE `JobID`='" + MainVM.SelectedJobTitle.JobID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Job Position successfully deleted.");
                            loadDataToUi();
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select a Job Position first.");
            }
        }

        private void editContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;

            if (contJobLb.SelectedItems.Count > 0)
            {
                contNewJobTb.Text = MainVM.SelectedJobTitle.JobName;
            }
            else
            {
                MessageBox.Show("Please select a record first.");
            }
            dbCon.Close();
            addContJobBtn.Content = "Save";
        }


        //product category
        private void deleteCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (invProductsCategoryLb.SelectedItems.Count > 0)
            {
                if (dbCon.IsConnect())
                {
                    string query = "DELETE FROM `odc_db`.`item_type_t` WHERE `typeID`='" + invProductsCategoryLb.SelectedValue + "';";
                    if (dbCon.deleteQuery(query, dbCon.Connection))
                    {
                        dbCon.Close();
                        loadDataToUi();
                        MessageBox.Show("Product Category successfully deleted");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a Product Category first.");
            }
        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addCategoryBtn.Content.Equals("Save"))
            {

            }
            else
            {
                string strCategory = invCategoryTb.Text;
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;

                if (!String.IsNullOrWhiteSpace(invCategoryTb.Text))
                {
                    if (invProductsCategoryLb.Items.Contains(invCategoryTb.Text))
                    {
                        MessageBox.Show("Product Category already exists");
                    }
                    else
                    {
                        if (!Regex.IsMatch(strCategory, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special characters are not accepted");
                            invCategoryTb.Clear();
                        }
                        else
                        {
                            if (dbCon.IsConnect())
                            {
                                string query = "INSERT INTO `odc_db`.`item_type_t` (`typeName`) VALUES('" + invCategoryTb.Text + "')";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {

                                    MessageBox.Show("Product Category successfully added");
                                    loadDataToUi();
                                    invCategoryTb.Clear();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product Category field must be filled");
                }
            }

        }

        private void editCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (invProductsCategoryLb.SelectedItems.Count > 0)
            {
                invCategoryTb.Text = MainVM.SelectedProductCategory.TypeName;
                addCategoryBtn.Content = "Save";
            }
            else
            {
                MessageBox.Show("Please select a product category first.");
            }
            dbCon.Close();
        }

        //service types
        private void addServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            serviceTypeList.Visibility = Visibility.Collapsed;
            serviceTypeAdd.Visibility = Visibility.Visible;
        }

        private void cancelServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in serviceTypeAdd.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).Value = 0;
                }
            }
            serviceTypeList.Visibility = Visibility.Visible;
            serviceTypeAdd.Visibility = Visibility.Collapsed;
            loadDataToUi();
        }

        private void serviceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strService = (sender as TextBox).Text;
            if (!Regex.IsMatch(strService, @"[a-zA-Z -]"))
            {
                MessageBox.Show("Special characters are not accepted");
            }
            //if (System.Windows.Controls.Validation.GetHasError(serviceName) == true)
            //  saveServiceTypeBtn.IsEnabled = false;
            else
            {
                validateTextBoxes();
            }

        }

        private void servicePrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateTextBoxes();
        }

        private void validateTextBoxes()
        {

        }
        private string id = "";
        private void saveServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBoxResult result = MessageBox.Show("Do you want to save this service type?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var element in serviceTypeAdd.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        if (expression != null)
                        {
                            if (((TextBox)element).IsEnabled)
                            {
                                expression.UpdateSource();
                                if (Validation.GetHasError((TextBox)element))
                                    validationError = true;
                            }
                        }
                    }
                    if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                    {
                        BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.TextProperty);
                        if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element))
                                validationError = true;
                        }
                    }
                }
                if (!validationError)
                {
                    if (!MainVM.isEdit)
                    {
                        string query = "INSERT INTO services_t (serviceName,serviceDesc,servicePrice) VALUES ('" + serviceName.Text + "','" + serviceDesc.Text + "', '" + servicePrice.Value + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Service type successfully added!");
                            serviceTypeList.Visibility = Visibility.Visible;
                            serviceTypeAdd.Visibility = Visibility.Collapsed;

                            //clearing textboxes
                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            loadDataToUi();
                        }
                        MainVM.isEdit = false;
                    }
                    else
                    {
                        string query = "UPDATE `services_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + MainVM.SelectedService.ServiceID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            //MessageBox.Show("Sevice type sucessfully updated");
                            id = "";
                            serviceTypeList.Visibility = Visibility.Visible;
                            serviceTypeAdd.Visibility = Visibility.Collapsed;
                            loadDataToUi();
                        }
                        MainVM.isEdit = false;
                    }
                }
                else
                    MessageBox.Show("Resolve the error first");
                validationError = false;
            }
            else if (result == MessageBoxResult.No)
            {
                for (int x = 1; x < serviceTypeGrid.Children.Count; x++)
                {
                    serviceTypeGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                serviceTypeList.Visibility = Visibility.Visible;
                loadDataToUi();
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (serviceTypeDg.SelectedItems.Count > 0)
            {

                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
                serviceName.Text = MainVM.SelectedService.ServiceName;
                serviceDesc.Text = MainVM.SelectedService.ServiceDesc;
                servicePrice.Value = MainVM.SelectedService.ServicePrice;
                dbCon.Close();
                MainVM.isEdit = true;
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
            }

        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (serviceTypeDg.SelectedItems.Count > 0)
            {
                id = (serviceTypeDg.Columns[0].GetCellContent(serviceTypeDg.SelectedItem) as TextBlock).Text;
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `services_t` SET `isDeleted`= 1 WHERE serviceID = '" + MainVM.SelectedService.ServiceID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                        }
                    }
                    dbCon.Close();
                }
            }
            worker.RunWorkerAsync();
        }

        bool initPrice = true;
        string locationid = "";
        private void custProvinceCb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (provinceCb.SelectedIndex == -1)
            {
                setPriceBtn.IsEnabled = false;
            }
            else
            {
                setPriceBtn.IsEnabled = true;
                MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(provinceCb.SelectedValue.ToString())).First();
                locationPrice.Value = MainVM.SelectedProvince.ProvincePrice;
            }

        }

        private void setPriceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (locationPrice.Value != null)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(provinceCb.SelectedValue.ToString())).First();
                MessageBoxResult result = MessageBox.Show("Do you want to save this price?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    string query = "UPDATE `provinces_t` SET locPrice = '" + locationPrice.Value + "' WHERE locProvinceID = '" + MainVM.SelectedProvince.ProvinceID + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Price saved.");
                        id = "";
                        provinceCb.SelectedValue = -1;
                        locationPrice.Value = 0;
                        initPrice = true;
                        loadDataToUi();
                    }



                }
                else if (result == MessageBoxResult.Cancel)
                {
                    provinceCb.SelectedValue = -1;
                    locationPrice.Value = 0;
                }
            }
            else
            {
                MessageBox.Show("Please enter the price.");
            }

        }


        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        //Transaction

        private void transQuotationAddBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in transQuotationGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Name.Equals(ucSalesQuote.Name)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
            ucSalesQuote.viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            ucSalesQuote.newSalesQuoteBtns.Visibility = Visibility.Visible;
            resetValueofSelectedVariables();
        }

        

        private void viewQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in transQuotationGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Name.Equals(ucSalesQuote.Name)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
            ucSalesQuote.viewSalesQuoteBtns.Visibility = Visibility.Visible;
            ucSalesQuote.newSalesQuoteBtns.Visibility = Visibility.Collapsed;
        }

        private void editQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            //viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            //newSalesQuoteBtns.Visibility = Visibility.Visible;
        }

        private void deleteQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convertToInvoice_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isNewRecord = true;
            headerLbl.Content = "Trasanction - Sales Invoice";
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            trasanctionGrid.Visibility = Visibility.Visible;
            foreach (var obj in trasanctionGrid.Children)
            {
                if (obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                else if (obj is UserControl)
                    if (((UserControl)obj).Equals(ucInvoice))
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
            }
        }

        private void genContractBtn_Click(object sender, RoutedEventArgs e)
        {
            ucContract.Visibility = Visibility.Visible;
        }

        
    }
}
