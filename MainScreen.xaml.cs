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
            worker.RunWorkerAsync();

            //loadDataToUi();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { loadDataToUi(); }));



        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
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
            MainVM.Representatives.Clear();

            MainVM.AllEmployeesContractor.Clear();
            MainVM.Employees.Clear();
            MainVM.Contractor.Clear();


            MainVM.SalesQuotes.Clear();

            

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
                string query = "SELECT * FROM services_t;";
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
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID,cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.representativeID ,cs.companyType " +
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
                    var customer = (new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), RepresentativeID = dr["representativeID"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString() });
                    MainVM.Customers.Add(customer);
                    MainVM.AllCustomerSupplier.Add(customer);
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID,cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.representativeID ,cs.companyType " +
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
                    MainVM.Suppliers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), RepresentativeID = dr["representativeID"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString() });
                    MainVM.AllCustomerSupplier.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), RepresentativeID = dr["representativeID"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString() });
                }
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT * from representative_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];

                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Representatives.Add(new Representative() { RepresentativeID = dr["representativeID"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() });
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
                    MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = dr["empType"].ToString() });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = dr["empType"].ToString() });


                }
                dbCon.Close();
            }
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
                    MainVM.Contractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = dr["empType"].ToString() });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = empPic, EmpType = dr["empType"].ToString() });
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
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out deliveryDate);

                    DateTime validityDate = new DateTime();
                    DateTime.TryParse(dr["dateOfIssue"].ToString(), out validityDate);

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

                    int custRepId;
                    int.TryParse(dr["custRepID"].ToString(), out custRepId);

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
                            custRepID_ = custRepId,
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
                            MainVM.SelectedAddedService = (new AddedService() { TableNoChar = dr["tableNoChar"].ToString(), ServiceID = dr["serviceID"].ToString(), ProvinceID = int.Parse(dr["provinceID"].ToString()), City = dr["city"].ToString(), Address = dr["address"].ToString(), TotalCost = decimal.Parse(dr["totalCost"].ToString()) });
                            foreach (DataRow dr2 in fromDbTable2.Rows)
                            {
                                if (dr2["tableNoChar"].ToString().Equals(dr2["servicesNochar"].ToString()))
                                {
                                    MainVM.SelectedAddedService.AdditionalFees.Add(new AdditionalFee() { ServiceNoChar = dr2["serviceNoChar"].ToString(), FeeName = dr2["feeName"].ToString(), FeePrice = decimal.Parse(dr2["feeValue"].ToString()) });
                                }
                            }
                            sq.AddedServices.Add(MainVM.SelectedAddedService);
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

        }

        private void reportsBtn_Click(object sender, RoutedEventArgs e)
        {

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
                if (id.Equals(""))
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
                }
                else
                {
                    string query = "UPDATE `services_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + id + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        //MessageBox.Show("Sevice type sucessfully updated");
                        id = "";
                        serviceTypeList.Visibility = Visibility.Visible;
                        serviceTypeAdd.Visibility = Visibility.Collapsed;
                        loadDataToUi();
                    }
                }

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
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `service_t` SET `isDeleted`= 1 WHERE serviceID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                        }
                    }
                    dbCon.Close();
                    serviceTypeList.Visibility = Visibility.Collapsed;
                    serviceTypeAdd.Visibility = Visibility.Visible;
                }
            }

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

        

        private void trasancnvertToInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convertToInvoice_BtnClicked(object sender, EventArgs e)
        {
            foreach (var obj in transQuotationGrid.Children)
            {
                if (obj is Grid)
                {
                    if (((Grid)obj).Equals(quotationsGridHome))
                    {
                        
                        ((Grid)obj).Visibility = Visibility.Visible;
                        settingsBtn.Visibility = Visibility.Hidden;
                    }
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }
            foreach (var element in trasanctionGrid.Children)
            {
                if (element is Grid)
                {
                    if (!(((Grid)element).Name.Equals(transInvoiceGrid.Name)))
                    {
                        ((Grid)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((Grid)element).Visibility = Visibility.Visible;
                }
            }
            headerLbl.Content = "Trasanction - Sales Invoice";
            foreach (var element in transInvoiceGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Name.Equals(ucInvoice.Name)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
            computeInvoice();
        }

        void computeInvoice()
        {
            MainVM.SelectedRepresentative = MainVM.Representatives.Where(x => x.RepresentativeID.Equals(MainVM.SelectedCustomerSupplier.RepresentativeID.ToString())).FirstOrDefault();
            MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(MainVM.SelectedSalesQuote.custID_.ToString())).FirstOrDefault();
            MainVM.RequestedItems.Clear();
            foreach (AddedItem item in MainVM.SelectedSalesQuote.AddedItems)
            {
                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ItemCode.Equals(item.ItemCode)).First();
                MainVM.RequestedItems.Add(new RequestedItem()
                {
                    lineNo = (MainVM.RequestedItems.Count + 1).ToString(),
                    itemCode = item.ItemCode,
                    desc = MainVM.SelectedProduct.ItemDesc,
                    itemName = MainVM.SelectedProduct.ItemName,
                    qty = item.ItemQty,
                    qtyEditable = true,
                    totalAmountMarkUp = item.TotalCost,
                    itemType = 0,
                    unitPrice = MainVM.SelectedProduct.CostPrice
                });
                MainVM.VatableSale += item.TotalCost;
            }
            foreach (AddedService service in MainVM.SelectedSalesQuote.AddedServices)
            {
                MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(service.ServiceID)).First();
                MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == service.ProvinceID).First();
                MainVM.RequestedItems.Add(new RequestedItem()
                {
                    lineNo = (MainVM.RequestedItems.Count + 1).ToString(),
                    itemCode = service.TableNoChar,
                    desc = MainVM.SelectedService.ServiceDesc,
                    itemName = MainVM.SelectedService.ServiceName,
                    qty = 1,
                    qtyEditable = false,
                    totalAmount = service.TotalCost,
                    itemType = 1,
                    unitPrice = service.TotalCost,
                    additionalFees = service.AdditionalFees
                });
                MainVM.VatableSale += service.TotalCost;
            }

            MainVM.VatableSale = MainVM.VatableSale - (MainVM.VatableSale * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01));
            MainVM.VatableSale = Math.Round(MainVM.VatableSale, 3);
            MainVM.TotalSalesNoVat = MainVM.VatableSale;
            MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));
            MainVM.TotalSales = MainVM.VatableSale + (MainVM.VatableSale * ((decimal)0.12));
            MainVM.TotalSales = Math.Round(MainVM.TotalSales, 3);
        }
    }
}
