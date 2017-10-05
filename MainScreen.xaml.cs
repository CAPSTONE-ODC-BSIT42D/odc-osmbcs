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
        private readonly BackgroundWorker worker = new BackgroundWorker();
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            foreach(var obj in formGridBg.Children)
            {
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
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { loadDataToUi();}));
            

            
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

            MainVM.SelectedCustomerSupplier = null;
            MainVM.SelectedEmployeeContractor = null;

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
                    MainVM.Provinces.Add(new Province() { ProvinceID = (int)dr["locProvinceID"], ProvinceName = dr["locProvince"].ToString(), ProvincePrice = decimal.Parse(dr["locPrice"].ToString()) });
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
                        MainVM.ProductList.Add(new Item() { ItemCode = dr["itemCode"].ToString(),ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(),TypeName = MainVM.SelectedProductCategory.TypeName, Quantity = 1, SupplierID = dr["supplierID"].ToString(), SupplierName = MainVM.SelectedCustomerSupplier.CompanyName });
                    }
                    else
                        MainVM.ProductList.Add(new Item() { ItemCode = dr["itemCode"].ToString(), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), TypeName = MainVM.SelectedProductCategory.TypeName, Quantity = 1, SupplierID = dr["supplierID"].ToString()});
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
                    MainVM.Representatives.Add(new Representative(){ RepresentativeID = dr["representativeID"].ToString(), RepTitle = dr["repTitle"].ToString(), RepFirstName = dr["repFName"].ToString(), RepMiddleName = dr["repMInitial"].ToString(), RepLastName = dr["repLName"].ToString(), RepEmail = dr["repEmail"].ToString(), RepTelephone = dr["repTelephone"].ToString(), RepMobile = dr["repMobile"].ToString() });
                }
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM emp_cont_t a  " +
                    "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
                    "JOIN position_t c ON a.positionID = c.positionid " +
                    "JOIN emp_pic_t d ON a.empID = d.empID " +
                    "WHERE isDeleted = 0 AND empType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    if (dr["empPic"].Equals(DBNull.Value))
                    {
                        MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpType = dr["empType"].ToString() });
                        MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpType = dr["empType"].ToString()});
                    }
                    else
                    {
                        MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = (byte[])dr["empPic"], EmpType = dr["empType"].ToString() });
                        MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpPic = (byte[])dr["empPic"], EmpType = dr["empType"].ToString() });
                    }

                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * " +
                    "FROM emp_cont_t a  " +
                    "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
                    //"JOIN position_t c ON a.positionID = c.positionid " +
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
                    MainVM.Contractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpType = dr["empType"].ToString() });
                    MainVM.AllEmployeesContractor.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString(), EmpEmail = dr["empEmail"].ToString(), EmpMobile = dr["empMobile"].ToString(), EmpTelephone = dr["empTelephone"].ToString(), EmpType = dr["empType"].ToString() });
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
                    if(MainVM.SelectedCustomerSupplier != null)
                    {
                        MainVM.SalesQuotes.Add(new SalesQuote() { sqNoChar_ = dr["sqNoChar"].ToString(), dateOfIssue_ = dateOfIssue, custID_ = int.Parse(dr["custID"].ToString()), custRepID_ = int.Parse(dr["custRepID"].ToString()), custName_ = MainVM.SelectedCustomerSupplier.CompanyName, quoteSubject_ = dr["quoteSubject"].ToString(), priceNote_ = dr["priceNote"].ToString(), deliveryDate_ = deliveryDate, estDelivery_ = int.Parse(dr["estDelivery"].ToString()), validityDays_ = int.Parse(dr["validityDays"].ToString()), validityDate_ = validityDate, otherTerms_ = dr["otherTerms"].ToString(), expiration_ = expiration, vat_ = Decimal.Parse(dr["vat"].ToString()), vatexcluded_ = vatIsExcluded, paymentIsLanded_ = paymentIsLanded, paymentCurrency_ = dr["paymentCurrency"].ToString(), status_ = dr["status"].ToString(), termsDays_ = int.Parse(dr["termsDays"].ToString()), termsDP_ = int.Parse(dr["termsDP"].ToString()), penaltyAmt_ = Decimal.Parse(dr["penaltyAmt"].ToString()), penaltyPercent_ = int.Parse(dr["penaltyPerc"].ToString()), markUpPercent_ = decimal.Parse(dr["markUpPercent"].ToString()), discountPercent_ = decimal.Parse(dr["discountPercent"].ToString()) });

                    }
                }
                query = "SELECT * FROM services_availed_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                fromDb = new DataSet();
                fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                query = "SELECT * FROM services_availed_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb3 = new DataSet();
                DataTable fromDbTable3 = new DataTable();
                dataAdapter.Fill(fromDb3, "t");
                fromDbTable3 = fromDb.Tables["t"];
                query = "SELECT * FROM fees_per_transaction_t;";
                dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb2 = new DataSet();
                DataTable fromDbTable2 = new DataTable();
                dataAdapter.Fill(fromDb2, "t");
                fromDbTable2 = fromDb2.Tables["t"];
                foreach (SalesQuote sq in MainVM.SalesQuotes)
                {
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        if (dr["sqNoChar"].ToString().Equals(sq.sqNoChar_))
                        {
                            MainVM.SelectedAddedService = (new AddedService(){ TableNoChar = dr["tableNoChar"].ToString(), ServiceID = dr["serviceID"].ToString(), ProvinceID = int.Parse(dr["provinceID"].ToString()), City = dr["city"].ToString(), Address = dr["address"].ToString(), TotalCost = decimal.Parse(dr["totalCost"].ToString()) });
                            foreach(DataRow dr2 in fromDbTable2.Rows)
                            {
                                if (dr["tableNoChar"].ToString().Equals(dr2["servicesNochar"].ToString()))
                                {
                                    MainVM.SelectedAddedService.AdditionalFees.Add(new AdditionalFee() { ServiceNoChar = dr["serviceNoChar"].ToString(), FeeName = dr["feeName"].ToString(), FeePrice = decimal.Parse(dr["feeValue"].ToString())});
                                }
                            }
                            sq.AddedServices.Add(MainVM.SelectedAddedService);
                        }
                    }
                    foreach(DataRow dr3 in fromDbTable3.Rows)
                    {
                        if (dr3["sqNoChar"].ToString().Equals(sq.sqNoChar_))
                        {
                            sq.AddedItems.Add(new AddedItem() { TableID = int.Parse(dr3["tableID"].ToString()), SqNoChar = dr3["sqNoChar"].ToString(), ItemCode = dr3["itemCode"].ToString(), ItemQty = int.Parse(dr3["itemQnty"].ToString()), TotalCost = decimal.Parse(dr3["totalCost"].ToString())});
                        }
                    }

                        
                }
                dbCon.Close();
            }
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

                if (!((Grid)obj).Name.Equals("settingsFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }
                
            }
            formGridBg.Visibility = Visibility.Visible;

        }

        private void closeSideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in formGridBg.Children)
            {

                ((Grid)obj).Visibility = Visibility.Collapsed;

            }
            formGridBg.Visibility = Visibility.Collapsed;
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
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            transQuotationGrid.Visibility = Visibility.Visible;
            foreach (var obj in transQuotationGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            quotationsGridHome.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
            headerLbl.Content = "Trasanction - Sales Quote";
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

                if (!((Grid)obj).Name.Equals("settingsFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
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

                if (!((Grid)obj).Name.Equals("employeeDetailsFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
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

                if (!((Grid)obj).Name.Equals("companyDetailsFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
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

                if (!((Grid)obj).Name.Equals("productDetailsFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void generateProductCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(productNameTb.Text))
            {
                MessageBox.Show("For meaningful product code, enter product name first");
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string productCode = "";
                var random = new Random();

                for (int i = 0; i < 4; i++)
                {
                    productCode += chars[random.Next(chars.Length)];
                }
                productCode += "-";

                for (int i = 0; i < 4; i++)
                {
                    productCode += chars[random.Next(chars.Length)];
                }
                productCode += MainVM.ProductList.Count + 1;

                productCodeTb.Text = productCode;
            }
            else
            {
                if (productNameTb.Text.Length > 4)
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string productCode = "";
                    var random = new Random();

                    for (int i = 0; i < 4; i++)
                    {
                        productCode += chars[random.Next(chars.Length)];
                    }
                    productCode += "-";

                    productCode += productNameTb.Text.Trim().Substring(0, 4).ToUpper();
                    productCode += MainVM.ProductList.Count + 1;

                    productCodeTb.Text = productCode;
                }
                else
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string productCode = "";
                    var random = new Random();

                    for (int i = 0; i < 4; i++)
                    {
                        productCode += chars[random.Next(chars.Length)];
                    }
                    productCode += "-";

                    productCode += productNameTb.Text.Trim().ToUpper();
                    productCode += MainVM.ProductList.Count + 1;

                    productCodeTb.Text = productCode;
                }
            }
        }

        private bool validationError = false;

        byte[] picdata;
        byte[] sigdata;
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    empImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    picdata = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }

            }
        }

        private void viewRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            formGridBg.Visibility = Visibility.Visible;
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("companyDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                saveCancelGrid.Visibility = Visibility.Collapsed;
                editCloseGrid.Visibility = Visibility.Visible;
                foreach (var element in companyDetailsFormGrid1.Children)
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
                loadRecordToFields();
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("employeeDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                saveCancelGrid1.Visibility = Visibility.Collapsed;
                editCloseGrid1.Visibility = Visibility.Visible;
                foreach (var element in employeeDetailsFormGrid1.Children)
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
                loadRecordToFields();
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("productDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                saveCancelGrid2.Visibility = Visibility.Collapsed;
                editCloseGrid2.Visibility = Visibility.Visible;
                foreach (var element in productDetailsFormGrid1.Children)
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
                loadRecordToFields();
            }

        }

        private void editRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            formGridBg.Visibility = Visibility.Visible;
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("companyDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                loadRecordToFields();
                isEdit = true;
                saveCancelGrid.Visibility = Visibility.Visible;
                editCloseGrid.Visibility = Visibility.Collapsed;
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("employeeDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                loadRecordToFields();
                isEdit = true;
                saveCancelGrid1.Visibility = Visibility.Visible;
                editCloseGrid1.Visibility = Visibility.Collapsed;
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (var obj in formGridBg.Children)
                {

                    if (!((Grid)obj).Name.Equals("productDetailsFormGrid"))
                    {
                        ((Grid)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }

                }
                loadRecordToFields();
                isEdit = true;
                saveCancelGrid2.Visibility = Visibility.Visible;
                editCloseGrid2.Visibility = Visibility.Collapsed;
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

        private void editCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in companyDetailsFormGrid1.Children)
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
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyTelephone))
            {
                companyTelCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyMobile))
            {
                companyMobCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyEmail))
            {
                companyEmailCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepTelephone))
            {
                repTelCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepMobile))
            {
                repMobCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepEmail))
            {
                repEmailCb.IsChecked = true;
            }
            editCloseGrid.Visibility = Visibility.Collapsed;
            saveCancelGrid.Visibility = Visibility.Visible;
            isEdit = true;
            
        }

        private void editEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in employeeDetailsFormGrid1.Children)
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
            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpTelephone))
            {
                empTelCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpMobile))
            {
                empMobCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpEmail))
            {
                empEmailCb.IsChecked = true;
            }
            saveCancelGrid1.Visibility = Visibility.Visible;
            editCloseGrid1.Visibility = Visibility.Collapsed;
            isEdit = true;
        }
        private void editProductBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in productDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).IsEnabled = true;
                }
                if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled = true;
                }
                if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                }
            }
            saveCancelGrid2.Visibility = Visibility.Visible;
            editCloseGrid2.Visibility = Visibility.Collapsed;
            isEdit = true;
        }

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                if (companyDetailsFormGrid.IsVisible)
                {
                    foreach (var element in companyDetailsFormGrid1.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            Validation.ClearInvalid(expression);
                            if (((TextBox)element).IsEnabled)
                            {
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((TextBox)element);
                            }
                            else
                            {

                            }
                            
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((ComboBox)element);
                        }
                    }
                }
                else if (employeeDetailsFormGrid.IsVisible)
                {
                    foreach (var element in employeeDetailsFormGrid1.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            Validation.ClearInvalid(expression);
                            if (((TextBox)element).IsEnabled)
                            {
                                
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((TextBox)element);
                            }
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((ComboBox)element);
                        }
                    }
                }
                else if (productDetailsFormGrid.IsVisible)
                {
                    foreach (var element in productDetailsFormGrid1.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            
                            if (expression!=null)
                            {
                                Validation.ClearInvalid(expression);
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((TextBox)element);
                            }
                        }
                        if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                        {
                            BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                            Validation.ClearInvalid(expression);
                            if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                            {

                                expression.UpdateSource();
                                validationError = Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element);
                            }
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            if (expression != null)
                            {
                                expression.UpdateSource();
                                validationError = Validation.GetHasError((ComboBox)element);
                            }
                            
                        }
                    }
                }
                if (!validationError)
                saveDataToDb();
                isEdit = false;
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
            
        }
        
        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                resetFieldsValue();
            }
            else if (result == MessageBoxResult.No)
            {
            }
            
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            resetFieldsValue();

        }

        private void employeeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employeeType.SelectedIndex == 0)
            {
                contractorOnlyGrid.Visibility = Visibility.Collapsed;
                employeeOnlyGrid.Visibility = Visibility.Visible;
            }
            if(employeeType.SelectedIndex == 1)
            {
                contractorOnlyGrid.Visibility = Visibility.Visible;
                employeeOnlyGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void contactDetails_Checked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;
            if (propertyName.Equals(empTelCb.Name))
            {
                if((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empTelCb.IsChecked = false;
                }
                else
                {
                    if (empTelephoneTb.IsEnabled)
                    {
                        empTelephoneTb.IsEnabled = false;
                        empTelephoneTb.Text = "";
                    }
                }
                
            }
            else if (propertyName.Equals(empMobCb.Name))
            {
                if ((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empMobCb.IsChecked = false;
                }
                else
                {
                    if (empMobileNumberTb.IsEnabled)
                    {
                        empMobileNumberTb.IsEnabled = false;
                        empMobileNumberTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(empEmailCb.Name))
            {
                if ((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empEmailCb.IsChecked = false;
                }
                else
                {
                    if (empEmailAddressTb.IsEnabled)
                    {
                        empEmailAddressTb.IsEnabled = false;
                        empEmailAddressTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(companyTelCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyTelCb.IsChecked = false;
                }
                else
                {
                    if (companyTelephoneTb.IsEnabled)
                    {
                        companyTelephoneTb.IsEnabled = false;
                        companyTelephoneTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(companyMobCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyMobCb.IsChecked = false;
                }
                else
                {
                    if (companyMobileTb.IsEnabled)
                    {
                        companyMobileTb.IsEnabled = false;
                        companyMobileTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(companyEmailCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyEmailCb.IsChecked = false;
                }
                else
                {
                    if (companyEmailTb.IsEnabled)
                    {
                        companyEmailTb.IsEnabled = false;
                        companyEmailTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repTelCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repTelCb.IsChecked = false;
                }
                else
                {
                    if (repTelephoneTb.IsEnabled)
                    {
                        repTelephoneTb.IsEnabled = false;
                        repTelephoneTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repMobCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repMobCb.IsChecked = false;
                }
                else
                {
                    if (repMobileTb.IsEnabled)
                    {
                        repMobileTb.IsEnabled = false;
                        repMobileTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repEmailCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repEmailCb.IsChecked = false;
                }
                else
                {
                    if (repEmailTb.IsEnabled)
                    {
                        repEmailTb.IsEnabled = false;
                        repEmailTb.Text = "";
                    }
                }
            }
        }

        private void contactDetail_Unchecked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;
            if (propertyName.Equals(empTelCb.Name))
            {
                if (!empTelephoneTb.IsEnabled)
                    empTelephoneTb.IsEnabled = true;

            }
            else if (propertyName.Equals(empMobCb.Name))
            {
                if (!empMobileNumberTb.IsEnabled)
                    empMobileNumberTb.IsEnabled = true;
            }
            else if (propertyName.Equals(empEmailCb.Name))
            {
                if (!empEmailAddressTb.IsEnabled)
                    empEmailAddressTb.IsEnabled = true;
            }
            else if (propertyName.Equals(companyTelCb.Name))
            {
                if (!companyTelephoneTb.IsEnabled)
                    companyTelephoneTb.IsEnabled = true;
            }
            else if (propertyName.Equals(companyMobCb.Name))
            {
                if (!companyMobileTb.IsEnabled)
                    companyMobileTb.IsEnabled = true;
            }
            else if (propertyName.Equals(companyEmailCb.Name))
            {
                if (!companyEmailTb.IsEnabled)
                    companyEmailTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repTelCb.Name))
            {
                if (!repTelephoneTb.IsEnabled)
                    repTelephoneTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repMobCb.Name))
            {
                if (!repMobileTb.IsEnabled)
                    repMobileTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repEmailCb.Name))
            {
                if (!repEmailTb.IsEnabled)
                    repEmailTb.IsEnabled = true;
            }
        }

        private bool isEdit = false;

        private void loadRecordToFields()
        {
            if (companyDetailsFormGrid.IsVisible)
            {
                companyTypeCb.SelectedIndex = int.Parse(MainVM.SelectedCustomerSupplier.CompanyType);
                companyNameTb.Text = MainVM.SelectedCustomerSupplier.CompanyName;
                companyDescriptionTb.Text = MainVM.SelectedCustomerSupplier.CompanyDesc;
                companyAddressTb.Text = MainVM.SelectedCustomerSupplier.CompanyAddress;
                companyCityTb.Text = MainVM.SelectedCustomerSupplier.CompanyCity;
                companyProvinceCb.SelectedValue = MainVM.SelectedCustomerSupplier.CompanyProvinceID;
                companyPostalCode.Text = MainVM.SelectedCustomerSupplier.CompanyPostalCode;
                companyEmailTb.Text = MainVM.SelectedCustomerSupplier.CompanyEmail;
                companyTelephoneTb.Text = MainVM.SelectedCustomerSupplier.CompanyTelephone;
                companyMobileTb.Text = MainVM.SelectedCustomerSupplier.CompanyMobile;
                if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyTelephone))
                {
                    companyTelCb.IsChecked = true;
                }
                
                if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyMobile))
                {
                    companyMobCb.IsChecked = true;
                }
                if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyEmail))
                {
                    companyEmailCb.IsChecked = true;
                }
                MainVM.SelectedRepresentative = MainVM.Representatives.Where(x => x.RepresentativeID.Equals(MainVM.SelectedCustomerSupplier.RepresentativeID)).FirstOrDefault();
                representativeTitle.Text = MainVM.SelectedRepresentative.RepTitle;
                repFirstNameTb.Text = MainVM.SelectedRepresentative.RepFirstName;
                repMiddleInitialTb.Text = MainVM.SelectedRepresentative.RepMiddleName;
                repLastNameTb.Text = MainVM.SelectedRepresentative.RepLastName;
                repEmailTb.Text = MainVM.SelectedRepresentative.RepEmail;
                repTelephoneTb.Text = MainVM.SelectedRepresentative.RepTelephone;
                repMobileTb.Text = MainVM.SelectedRepresentative.RepMobile;
                if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepTelephone))
                {
                    repTelCb.IsChecked = true;
                }
                if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepMobile))
                {
                    repMobCb.IsChecked = true;
                }
                if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepEmail))
                {
                    repMobCb.IsChecked = true;
                }
            }
            else if (employeeDetailsFormGrid.IsVisible)
            {
                employeeType.SelectedIndex = int.Parse(MainVM.SelectedEmployeeContractor.EmpType);
                empFirstNameTb.Text = MainVM.SelectedEmployeeContractor.EmpFname;
                empLastNameTb.Text = MainVM.SelectedEmployeeContractor.EmpLName;
                empMiddleInitialTb.Text = MainVM.SelectedEmployeeContractor.EmpMiddleInitial;
                empAddressTb.Text = MainVM.SelectedEmployeeContractor.EmpAddress;
                empCityTb.Text = MainVM.SelectedEmployeeContractor.EmpCity;
                empProvinceCb.SelectedValue = MainVM.SelectedEmployeeContractor.EmpProvinceID;
                var stride = (empImage.ActualWidth * PixelFormats.Rgba64.BitsPerPixel + 7) / 8;
                if (MainVM.SelectedEmployeeContractor.EmpPic != null)
                    empImage.Source = BitmapSource.Create((int)empImage.Width, (int)empImage.Height, 96, 96, PixelFormats.Rgba64, null,MainVM.SelectedEmployeeContractor.EmpPic,(int)stride);
                
                if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpTelephone))
                {
                    empTelCb.IsChecked = true;
                }
                if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpMobile))
                {
                    empMobCb.IsChecked = true;
                }
                if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpEmail))
                {
                    empEmailCb.IsChecked = true;
                }
                empTelephoneTb.Text = MainVM.SelectedEmployeeContractor.EmpTelephone;
                empMobileNumberTb.Text = MainVM.SelectedEmployeeContractor.EmpMobile;
                empEmailAddressTb.Text = MainVM.SelectedEmployeeContractor.EmpEmail;
                if (employeeType.SelectedIndex == 0)
                {
                    empPostionCb.SelectedValue = MainVM.SelectedEmployeeContractor.PositionID;
                    empUserNameTb.Text = MainVM.SelectedEmployeeContractor.EmpUserName;
                    empPasswordTb.IsEnabled = false;
                }
                else if(employeeType.SelectedIndex == 1)
                {
                    empJobCb.SelectedValue = MainVM.SelectedEmployeeContractor.JobID;
                    empDateStarted.Text = MainVM.SelectedEmployeeContractor.EmpDateTo;
                    empDateEnded.Text = MainVM.SelectedEmployeeContractor.EmpDateFrom;
                }
            }
            else if (productDetailsFormGrid.IsVisible)
            {
                productNameTb.Text = MainVM.SelectedProduct.ItemName;
                productDescTb.Text = MainVM.SelectedProduct.ItemDesc;
                categoryCb.SelectedValue = MainVM.SelectedProduct.TypeID;
                costPriceTb.Value = MainVM.SelectedProduct.CostPrice;
                unitTb.Text = MainVM.SelectedProduct.Unit;
                supplierCb.SelectedValue = MainVM.SelectedProduct.SupplierID;
            }
        }
        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            if (companyDetailsFormGrid.IsVisible)
            {
                string[] proc = { "", "", "", "" };

                if (!isEdit)
                {
                    proc[0] = "INSERT_COMPANY";
                    proc[1] = "INSERT_REP";
                }
                else
                {
                    proc[0] = "UPDATE_COMPANY";
                    proc[1] = "UPDATE_REP";
                }
                try
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(proc[0], conn);
                        string repID = "";
                        cmd = new MySqlCommand(proc[1], conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@repTitle", representativeTitle.Text);
                        cmd.Parameters["@repTitle"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repLName", repLastNameTb.Text);
                        cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repFName", repFirstNameTb.Text);
                        cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repMName", repMiddleInitialTb.Text);
                        cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repEmail", repEmailTb.Text);
                        cmd.Parameters["@repEmail"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repTelephone", repTelephoneTb.Text);
                        cmd.Parameters["@repTelephone"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repMobile", repMobileTb.Text);
                        cmd.Parameters["@repMobile"].Direction = ParameterDirection.Input;
                        if (!isEdit)
                        { 
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            repID = cmd.Parameters["@insertedid"].Value.ToString();
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@repID", MainVM.SelectedCustomerSupplier.RepresentativeID);
                            cmd.Parameters["@repID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }

                        cmd = new MySqlCommand(proc[0], conn);
                        //INSERT NEW CUSTOMER TO DB;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@companyName", companyNameTb.Text);
                        cmd.Parameters["@companyName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@addInfo", companyDescriptionTb.Text);
                        cmd.Parameters["@addInfo"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@address", companyAddressTb.Text);
                        cmd.Parameters["@address"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@city", companyCityTb.Text);
                        cmd.Parameters["@city"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@province", companyProvinceCb.SelectedValue);
                        cmd.Parameters["@province"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyPostalCode", companyPostalCode.Text);
                        cmd.Parameters["@companyPostalCode"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyEmail", companyEmailTb.Text);
                        cmd.Parameters["@companyEmail"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyTelephone", companyTelephoneTb.Text);
                        cmd.Parameters["@companyTelephone"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyMobile", companyMobileTb.Text);
                        cmd.Parameters["@companyMobile"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@compType", companyTypeCb.SelectedIndex);
                        cmd.Parameters["@compType"].Direction = ParameterDirection.Input;
                        if (!isEdit)
                        {
                            cmd.Parameters.AddWithValue("@repID", repID);
                            cmd.Parameters["@repID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@compID", MainVM.SelectedCustomerSupplier.CompanyID);
                            cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    resetFieldsValue();
                    loadDataToUi();
                }
            }
            else if (employeeDetailsFormGrid.IsVisible)
            {
               
                string[] proc = { "", "", "", "" };

                if (!isEdit)
                {
                    proc[0] = "INSERT_EMPLOYEE";
                }
                else
                {
                    proc[0] = "UPDATE_EMPLOYEE";
                }

                try
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        conn.Open();
                        string empID = "";
                        MySqlCommand cmd = new MySqlCommand(proc[0], conn);
                        //INSERT NEW EMPLOYEE TO DB;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fName", empFirstNameTb.Text);
                        cmd.Parameters["@fName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@lName", empLastNameTb.Text);
                        cmd.Parameters["@lName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@middleInitial", empMiddleInitialTb.Text);
                        cmd.Parameters["@middleInitial"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@address", empAddressTb.Text);
                        cmd.Parameters["@address"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@city", empCityTb.Text);
                        cmd.Parameters["@city"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@provinceID", empProvinceCb.SelectedValue);
                        cmd.Parameters["@provinceID"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@username", empUserNameTb.Text);
                        cmd.Parameters["@username"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@positionID", empPostionCb.SelectedValue);
                        cmd.Parameters["@positionID"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@empEmail",empEmailAddressTb.Text);
                        cmd.Parameters["@empEmail"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@empTelephone", empTelephoneTb.Text);
                        cmd.Parameters["@empTelephone"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@empMobile", empMobileNumberTb.Text);
                        cmd.Parameters["@empMobile"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@empType", employeeType.SelectedIndex);
                        cmd.Parameters["@empType"].Direction = ParameterDirection.Input;

                        if (employeeType.SelectedIndex == 1)
                        {
                            cmd.Parameters.AddWithValue("@jobID", empJobCb.SelectedValue);
                            cmd.Parameters["@jobID"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("@dateFrom", empDateStarted.Value);
                            cmd.Parameters["@dateFrom"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("@dateTo", empDateEnded.Value);
                            cmd.Parameters["@dateTo"].Direction = ParameterDirection.Input;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@jobID", DBNull.Value);
                            cmd.Parameters["@jobID"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("@dateFrom", DBNull.Value);
                            cmd.Parameters["@dateFrom"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("@dateTo", DBNull.Value);
                            cmd.Parameters["@dateTo"].Direction = ParameterDirection.Input;
                        }
                        if (!isEdit)
                        {
                            SecureString passwordsalt = empPasswordTb.SecurePassword;
                            foreach (Char c in "$w0rdf!$h")
                            {
                                passwordsalt.AppendChar(c);
                            }
                            passwordsalt.MakeReadOnly();

                            cmd.Parameters.AddWithValue("@upassword", SecureStringToString(passwordsalt));
                            cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            empID = cmd.Parameters["@insertedid"].Value.ToString();
                            
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@empID", MainVM.SelectedEmployeeContractor.EmpID);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        if (!isEdit)
                        {
                            cmd = new MySqlCommand("INSERT_EMPLOYEE_PIC_T", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@picBLOB", picdata);
                            cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@sigBLOB", null);
                            cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@empID", empID);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE_EMPLOYEE_PIC_T", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@picBLOB", picdata);
                            cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@sigBLOB", null);
                            cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@empID", MainVM.SelectedEmployeeContractor.EmpID);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    resetFieldsValue();
                    loadDataToUi();

                }
            }
            else if (productDetailsFormGrid.IsVisible)
            {
                using (MySqlConnection conn = dbCon.Connection)
                {

                    conn.Open();
                    MySqlCommand cmd = null;
                    if (!isEdit)
                    {
                        cmd = new MySqlCommand("INSERT_ITEM", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE_ITEM", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@itemNo", MainVM.SelectedProduct.ItemCode);
                        cmd.Parameters["@itemNo"].Direction = ParameterDirection.Input;
                        isEdit = false;
                    }

                    //INSERT NEW Product TO DB;

                    cmd.Parameters.AddWithValue("@itemCode", productCodeTb.Text);
                    cmd.Parameters["@itemCode"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemName", productNameTb.Text);
                    cmd.Parameters["@itemName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemDesc", productDescTb.Text);
                    cmd.Parameters["@itemDesc"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@costPrice", costPriceTb.Value);
                    cmd.Parameters["@costPrice"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@itemUnit", unitTb.Text);
                    cmd.Parameters["@itemUnit"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@typeID", categoryCb.SelectedValue);
                    cmd.Parameters["@typeID"].Direction = ParameterDirection.Input;

                    if(supplierCb.SelectedIndex == 0)
                    {
                        cmd.Parameters.AddWithValue("@supplierID", DBNull.Value);
                        cmd.Parameters["@supplierID"].Direction = ParameterDirection.Input;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@supplierID", supplierCb.SelectedValue);
                        cmd.Parameters["@supplierID"].Direction = ParameterDirection.Input;
                    }
                    cmd.ExecuteNonQuery();
                }
                resetFieldsValue();
                loadDataToUi();

            }
        }

        private void resetFieldsValue()
        {

            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            foreach (var obj in formGridBg.Children)
            {

                ((Grid)obj).Visibility = Visibility.Collapsed;

            }
            formGridBg.Visibility = Visibility.Collapsed;
            companyDetailsFormGridSv.ScrollToTop();
            employeeDetailsFormGridSv.ScrollToTop();
            productDetailsFormGridSv.ScrollToTop();
            supplierCb.SelectedIndex = 0;
            foreach (var element in companyDetailsFormGrid1.Children)
            {
                
                if (element is TextBox)
                {
                    ((TextBox)element).IsEnabled = true;
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsEnabled = true;
                    ((CheckBox)element).IsChecked = false;
                }
            }
            foreach (var element in employeeDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).IsEnabled = true;
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsEnabled = true;
                    ((CheckBox)element).IsChecked = false;
                }
            }
            foreach (var element in productDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).IsEnabled = true;
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                {
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled = true;
                    BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.ValueProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)element).Value = 0;
                }
                else if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
            }
            foreach (var element in addNewServiceForm.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsChecked = false;
                }
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
                if (element is Grid)
                {
                    if (!(((Grid)element).Name.Equals(transQuoatationGridForm.Name)))
                    {
                        ((Grid)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((Grid)element).Visibility = Visibility.Visible;
                }
            }
            
            foreach (var element in transQuoatationGridForm.Children)
            {
                if(element is Grid)
                {
                    if (!(((Grid)element).Name.Equals(newRequisitionGrid.Name)))
                    {
                        ((Grid)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((Grid)element).Visibility = Visibility.Visible;
                }
            }
            viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            newSalesQuoteBtns.Visibility = Visibility.Visible;
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.Customers.Where(x => x.CompanyName.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()));
            var observable = new ObservableCollection<Customer>(linqResults);
            selectCustomerDg.ItemsSource = observable;
        }

        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.SelectedRepresentative = MainVM.Representatives.Where(x => x.RepresentativeID.Equals(MainVM.SelectedCustomerSupplier.RepresentativeID)).FirstOrDefault();
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                ((Grid)obj).Visibility = Visibility.Collapsed;

            }
        }

        private void selectCustomerBtn_Click(object sender, RoutedEventArgs e)
        {

            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {

                if (!((Grid)obj).Name.Equals("selectCustomerGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }
        }

        private void transRequestBack_Click(object sender, RoutedEventArgs e)
        {
            if (selectCustomerGrid.IsVisible)
            {
                foreach (var element in transQuotationGrid.Children)
                {
                    if (element is Grid)
                    {
                        if (!(((Grid)element).Name.Equals(quotationsGridHome.Name)))
                        {
                            ((Grid)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((Grid)element).Visibility = Visibility.Visible;
                    }
                }
            }
            else if (termsAndConditionGrid.IsVisible)
            {
                foreach (var element in transQuoatationGridForm.Children)
                {
                    if (element is Grid)
                    {
                        if (!(((Grid)element).Name.Equals(newRequisitionGrid.Name)))
                        {
                            ((Grid)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((Grid)element).Visibility = Visibility.Visible;
                    }
                }
            }
            else if (viewQuotationGrid.IsVisible)
            {
                MainVM.SalesQuotes.Remove(MainVM.SelectedSalesQuote);
                foreach (var element in transQuoatationGridForm.Children)
                {
                    if (element is Grid)
                    {
                        if (!(((Grid)element).Name.Equals(termsAndConditionGrid.Name)))
                        {
                            ((Grid)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((Grid)element).Visibility = Visibility.Visible;
                    }
                }
            }
        }
        Document document;
        private void transRequestNext_Click(object sender, RoutedEventArgs e)
        {
            if (newRequisitionGrid.IsVisible)
            {
                foreach (var element in transQuoatationGridForm.Children)
                {
                    if (element is Grid)
                    {
                        if (!(((Grid)element).Name.Equals(termsAndConditionGrid.Name)))
                        {
                            ((Grid)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((Grid)element).Visibility = Visibility.Visible;
                    }
                }
            }

            else if (termsAndConditionGrid.IsVisible)
            {
                foreach (var element in transQuoatationGridForm.Children)
                {
                    if (element is Grid)
                    {
                        if (!(((Grid)element).Name.Equals(viewQuotationGrid.Name)))
                        {
                            ((Grid)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((Grid)element).Visibility = Visibility.Visible;
                    }
                }
                if (MainVM.RequestedItems.Count != 0)
                {
                    transRequestNext.Content = "Save";
                    salesQuoteToMemory();
                    DocumentFormat df = new DocumentFormat();
                    document = df.CreateDocument("SalesQuote", "asdsadsa");
                    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                    pagePreview.Ddl = ddl;
                }
                else
                    MessageBox.Show("No items on the list.");
            }
            else if (viewQuotationGrid.IsVisible){
                transRequestNext.Content = "Next";
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = document;
                renderer.RenderDocument();
                string filename = @"d:\test\" + MainVM.SelectedSalesQuote.sqNoChar_ + ".pdf";
                renderer.PdfDocument.Save(filename);
                saveSalesQuoteToDb();
                Process.Start(filename);
                foreach (var obj in containerGrid.Children)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                trasanctionGrid.Visibility = Visibility.Visible;
                foreach (var obj in trasanctionGrid.Children)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                transQuotationGrid.Visibility = Visibility.Visible;
                foreach (var obj in transQuotationGrid.Children)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                quotationsGridHome.Visibility = Visibility.Visible;
                settingsBtn.Visibility = Visibility.Hidden;
                headerLbl.Content = "Trasanction - Sales Quote";
            }
        }

        

        private void transReqAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {

                if (!((Grid)obj).Name.Equals("addNewItemFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }
        }

        private void feesBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {

                if (!((Grid)obj).Name.Equals("additionalFeesFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }
            MainVM.SelectedAddedService = MainVM.AddedServices.Where(x => x.TableNoChar.Equals(MainVM.SelectedRequestedItem.itemCode)).FirstOrDefault();
            //MainVM.AdditionalFees = MainVM.SelectedAddedService.AdditionalFees;
        }

        private void editFeeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (ComboBoxItem cbi in feeTypeCb.Items)
            {
                if (cbi.Content.Equals(MainVM.SelectedAdditionalFee.FeeName))
                {
                    feeTypeCb.SelectedValue = MainVM.SelectedAdditionalFee.FeeName;
                    feeCostTb.Value = MainVM.SelectedAdditionalFee.FeePrice;
                    addSaveAdditionalFeesBtn.Content = "Save";
                    isEdit = true;
                    break;
                }
                else
                {
                    feeTypeCb.SelectedIndex = feeTypeCb.Items.Count - 1;
                    otherFeenameTb.Text = MainVM.SelectedAdditionalFee.FeeName;
                    feeCostTb.Value = MainVM.SelectedAdditionalFee.FeePrice;
                    addSaveAdditionalFeesBtn.Content = "Save";
                    isEdit = true;
                    break;
                }
            }
            
        }

        private void deleteFeeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.SelectedAddedService.AdditionalFees.Remove(MainVM.SelectedAdditionalFee);
        }

        private void addSaveAdditionalFeesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                if (feeTypeCb.SelectedIndex == feeTypeCb.Items.Count - 1)
                {
                    MainVM.SelectedAdditionalFee.FeeName = otherFeenameTb.Text;
                    MainVM.SelectedAdditionalFee.FeePrice = (decimal)feeCostTb.Value;
                }
                else
                {
                    MainVM.SelectedAdditionalFee.FeeName = feeTypeCb.SelectedValue.ToString();
                    MainVM.SelectedAdditionalFee.FeePrice = (decimal)feeCostTb.Value;
                }
                addSaveAdditionalFeesBtn.Content = "Add";
            }
            else
            {
                if (feeTypeCb.SelectedIndex == feeTypeCb.Items.Count - 1)
                {
                    MainVM.SelectedAddedService.AdditionalFees.Add(new AdditionalFee() { FeeName = otherFeenameTb.Text, FeePrice = (decimal)feeCostTb.Value });
                }
                else
                {
                    MainVM.SelectedAddedService.AdditionalFees.Add(new AdditionalFee() { FeeName = feeTypeCb.SelectedValue.ToString(), FeePrice = (decimal)feeCostTb.Value });
                }
                
                
            }
            feeTypeCb.SelectedIndex = -1;
            otherFeenameTb.Text = "";
            feeCostTb.Value = 0;
            isEdit = false;
        }

        private void saveAdditionalFees_Click(object sender, RoutedEventArgs e)
        {
            //MainVM.SelectedAddedService = MainVM.AddedServices.Where(x => x.TableNoChar.Equals(MainVM.SelectedRequestedItem.itemCode)).FirstOrDefault();
            //MainVM.SelectedAddedService.AdditionalFees = MainVM.AdditionalFees;
            //MainVM.AdditionalFees.Clear();
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                if (!((Grid)obj).Name.Equals("additionalFeesGridForm"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }

            computePrice();
        }

        private void cancelAdditionalFees_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                ((Grid)obj).Visibility = Visibility.Collapsed;

            }
            computePrice();
            
        }

        private void feeTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (feeTypeCb.SelectedIndex == feeTypeCb.Items.Count - 1)
                {
                    if (!otherFeenameTb.IsVisible)
                        otherFeenameTb.Visibility = Visibility.Visible;
                }
                else
                {
                    if (otherFeenameTb.IsVisible)
                        otherFeenameTb.Visibility = Visibility.Hidden;
                }
            }
        }

        private void productRbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                for (int x = 1; x < forms.Children.Count; x++)
                {
                    forms.Children[x].Visibility = Visibility.Hidden;
                }
                product.Visibility = Visibility.Visible;
                addNewServiceForm.Visibility = Visibility.Hidden;
            }

        }

        private void productRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void serviceRbtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                for (int x = 1; x < forms.Children.Count; x++)
                {
                    forms.Children[x].Visibility = Visibility.Hidden;
                }
                product.Visibility = Visibility.Hidden;
                addNewServiceForm.Visibility = Visibility.Visible;
            }

        }

        private void serviceRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void addressOfCustomerCb_Checked(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedCustomerSupplier != null)
            {
                foreach (var element in addNewServiceForm.Children)
                {
                    if (element is TextBox)
                    {
                        if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                        {
                            serviceAddressTb.Text = MainVM.SelectedCustomerSupplier.CompanyAddress;
                            serviceAddressTb.IsEnabled = false;
                        }
                        if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                        {
                            serviceCityTb.Text = MainVM.SelectedCustomerSupplier.CompanyCity;
                            serviceCityTb.IsEnabled = false;
                        }

                    }
                    if (element is ComboBox)
                    {
                        if (((ComboBox)element).Name.Equals(serviceProvinceCb.Name))
                        {
                            serviceProvinceCb.SelectedIndex = int.Parse(MainVM.SelectedCustomerSupplier.CompanyProvinceID) - 1;
                            serviceProvinceCb.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Custoemr selected");
            }
            
        }

        private void addressOfCustomerCb_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var element in addNewServiceForm.Children)
            {
                if (element is TextBox)
                {
                    if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                    {
                        serviceAddressTb.Text = "";
                        serviceAddressTb.IsEnabled = true;
                    }
                    if (((TextBox)element).Name.Equals(serviceAddressTb.Name))
                    {
                        serviceCityTb.Text = "";
                        serviceCityTb.IsEnabled = true;
                    }

                }
                if (element is ComboBox)
                {
                    if (((ComboBox)element).Name.Equals(serviceProvinceCb.Name))
                    {
                        serviceProvinceCb.SelectedIndex = -1;
                        serviceProvinceCb.IsEnabled = true;
                    }
                }
            }
        }

        

        private void addProductBtn_Click(object sender, RoutedEventArgs e)
        {

            //Add Item In to List
            if ((bool)productRbtn.IsChecked)
            {
                foreach (Item prd in MainVM.ProductList)
                {
                    if (prd.IsChecked)
                    {
                        var linqResults = MainVM.RequestedItems.Where(x => x.itemCode.Equals(prd.ItemCode)).FirstOrDefault();
                        if (linqResults == null)
                        {
                            MainVM.RequestedItems.Add(new RequestedItem() { lineNo = (MainVM.RequestedItems.Count + 1).ToString(), itemCode = prd.ItemCode, itemName = prd.ItemName, desc = prd.ItemDesc, itemTypeName = "Product", itemType = 0, qty = prd.Quantity, unitPrice = prd.CostPrice, totalAmount = prd.Quantity * prd.CostPrice, totalAmountMarkUp = prd.Quantity * prd.CostPrice, qtyEditable = true });
                        }
                        else
                        {
                            MessageBox.Show("Already added in the list.");
                        }
                        
                    }
                }
                resetFieldsValue();
            }
            else if ((bool)serviceRbtn.IsChecked)
            {
                foreach (var element in addNewServiceForm.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        if (expression != null)
                        {
                            Validation.ClearInvalid(expression);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }
                        

                    }
                    if (element is ComboBox)
                    {
                        BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                        Validation.ClearInvalid(expression);
                        expression.UpdateSource();
                        validationError = Validation.GetHasError((ComboBox)element);
                    }
                }
                if (!validationError)
                {
                    MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(serviceTypeCb.SelectedValue.ToString())).First();
                    MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(serviceProvinceCb.SelectedValue.ToString())).First();

                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    var random = new Random();
                    string serviceNoChar = "";
                    for (int i = 0; i < 4; i++)
                    {
                        if (!(i > 3))
                        {
                            serviceNoChar += chars[random.Next(chars.Length)];
                        }
                    }
                    serviceNoChar += MainVM.SalesQuotes.Count + 1;
                    serviceNoChar += "-";
                    if (MainVM.SelectedService.ServiceName.Length > 5)
                    {
                        serviceNoChar += MainVM.SelectedService.ServiceName.Trim().Substring(0, 5).ToUpper();
                    }
                    else
                        serviceNoChar += MainVM.SelectedService.ServiceName.Trim().ToUpper();
                    serviceNoChar += "-";
                    serviceNoChar += DateTime.Now.ToString("yyyy-MM-dd");
                    

                    MainVM.AddedServices.Add(new AddedService() {TableNoChar = serviceNoChar, ServiceID = MainVM.SelectedService.ServiceID, ProvinceID = MainVM.SelectedProvince.ProvinceID, Address = serviceAddressTb.Text, City = serviceCityTb.Text, TotalCost = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice });

                    MainVM.RequestedItems.Add(new RequestedItem() { lineNo = (MainVM.RequestedItems.Count + 1).ToString(),itemCode = serviceNoChar, itemName = MainVM.SelectedService.ServiceName, desc = serviceDescTb.Text, itemTypeName = "Service", itemType = 1, qty = 1, unitPrice = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmount = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmountMarkUp = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, qtyEditable = false });
                    
                    resetFieldsValue();
                }
                
            }
            computePrice();
        }

        private void cancelAddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                if (!((Grid)obj).Name.Equals("addNewItemFormGrid"))
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
                else
                {
                    ((Grid)obj).Visibility = Visibility.Visible;
                }

            }
            computePrice();
        }

       

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addNewItemFormGrid.IsVisible)
            {
                var linqResults = MainVM.ProductList.Where(x => x.ItemName.ToLower().Contains(searchTb.Text.ToLower()));
                var observable = new ObservableCollection<Item>(linqResults);
                addGridProductListDg.ItemsSource = observable;
            }
            else if (quotationsGridHome.IsVisible)
            {
                var linqResults = MainVM.ProductList.Where(x => x.ItemName.ToLower().Contains(searchTb.Text.ToLower()));
                var observable = new ObservableCollection<Item>(linqResults);
                addGridProductListDg.ItemsSource = observable;
            }
            
        }

        private void serviceProvinceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceProvinceCb.SelectedIndex != -1)
            {
                MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == int.Parse(serviceProvinceCb.SelectedValue.ToString())).First();
                if (MainVM.SelectedProvince.ProvincePrice == 0)
                {
                    MessageBox.Show("This location has no price set. Please set it in Settings.");
                }
            }
            
        }

        private void deleteRequestedItemBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.RequestedItems.Remove(MainVM.SelectedRequestedItem);
            MainVM.AddedServices.Remove(MainVM.AddedServices.Where(x => x.TableNoChar.Equals(MainVM.SelectedRequestedItem.itemCode)).FirstOrDefault());
        }

        private void paymentCustomRb_Checked(object sender, RoutedEventArgs e)
        {
            downpaymentPercentTb.IsEnabled = true;
            paymentDpLbl.IsEnabled = true;
        }

        private void paymentCustomRb_Unchecked(object sender, RoutedEventArgs e)
        {
            downpaymentPercentTb.IsEnabled = false;
            paymentDpLbl.IsEnabled = false;
        }

        private void validtycustomRd_Checked(object sender, RoutedEventArgs e)
        {
            validityTb.IsEnabled = true;
            validtycustomlbl.IsEnabled = true;
        }

        private void validtycustomRd_Unchecked(object sender, RoutedEventArgs e)
        {
            validityTb.IsEnabled = false;
            validtycustomlbl.IsEnabled = false;
        }

        private void warrantycustomRd_Checked(object sender, RoutedEventArgs e)
        {
            warrantyDaysCustom.IsEnabled = true;
            warrantyDaysCustomLbl.IsEnabled = true;
        }

        private void warrantycustomRd_Unchecked(object sender, RoutedEventArgs e)
        {
            warrantyDaysCustom.IsEnabled = false;
            warrantyDaysCustomLbl.IsEnabled = false;
        }

        private void deliveryCustomRd_Checked(object sender, RoutedEventArgs e)
        {
            deliveryDaysCustomLbl.IsEnabled = true;
            deliveryDaysTb.IsEnabled = true;
        }

        private void deliveryCustomRd_Unchecked(object sender, RoutedEventArgs e)
        {
            deliveryDaysCustomLbl.IsEnabled = false;
            deliveryDaysTb.IsEnabled = false;
        }

        private void customPenaltyRd_Checked(object sender, RoutedEventArgs e)
        {
            customPenaltyTb.IsEnabled = true;
            customPenaltyLbl.IsEnabled = true;
        }

        private void customPenaltyRd_Unchecked(object sender, RoutedEventArgs e)
        {
            customPenaltyTb.IsEnabled = false;
            customPenaltyLbl.IsEnabled = false;
        }

        private void vatCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (vatInclusiveTb != null && vatInclusiveTb.IsEnabled == true)
            {
                vatInclusiveTb.IsEnabled = false;
            }

        }

        private void vatCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (vatInclusiveTb != null && vatInclusiveTb.IsEnabled == false)
            {
                vatInclusiveTb.IsEnabled = true;
            }
        }

        private void markupPriceTb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            computePrice();
        }

        private void discountPriceTb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            computePrice();
        }

        private void qtyTb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            computePrice();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            computePrice();
        }

        private void computePrice()
        {
            decimal totalFee = 0;
            decimal totalPrice = 0;
            
            foreach (RequestedItem item in MainVM.RequestedItems)
            {
                if (item.itemType == 0)
                {
                    item.unitPriceMarkUp = item.unitPrice + (item.unitPrice / 100 * (decimal)markupPriceTb.Value);
                    item.totalAmountMarkUp = (item.unitPriceMarkUp * item.qty) - ((item.unitPriceMarkUp * item.qty) / 100) * (decimal)discountPriceTb.Value;
                    item.totalAmount = item.unitPriceMarkUp * item.qty;

                }
                else if (item.itemType == 1)
                {
                    MainVM.SelectedAddedService = MainVM.AddedServices.Where(x => x.TableNoChar.Equals(item.itemCode)).FirstOrDefault();
                    foreach (AdditionalFee af in MainVM.SelectedAddedService.AdditionalFees)
                    {
                        if (!(af.FeePrice == 0))
                        {
                            totalFee += af.FeePrice;
                        }
                    }
                    item.unitPriceMarkUp = (item.unitPrice + totalFee) + ((item.unitPrice + totalFee) / 100 * (decimal)markupPriceTb.Value);
                    item.totalAmountMarkUp = (item.unitPrice + totalFee + (((item.unitPrice + totalFee) / 100) * (decimal)markupPriceTb.Value)) - ((item.unitPrice + totalFee) / 100) * (decimal)discountPriceTb.Value;
                    item.totalAmount = item.unitPrice + totalFee;
                }
                totalPrice += item.totalAmountMarkUp;
            }
            if (totalPriceLbl != null)
            {
                totalPriceLbl.Content = "" + totalPrice;
            }
        }

        

        void salesQuoteToMemory()
        {
            string landed = "";
            decimal vat = 0;
            string vatExc = "VAT Exclusive";
            int estDel = 0;
            int valid = 30;
            if ((bool)landedCheckBox.IsChecked)
            {
                landed = "Landed";
            }
            if (!(bool)vatCheckBox.IsChecked)
            {
                vat = (decimal)vatInclusiveTb.Value;
            }
            if ((bool)deliveryDefaultRd.IsChecked)
            {
                estDel = 30;
            }
            else if((bool)deliveryCustomRd.IsChecked)
                estDel = int.Parse(deliveryDaysTb.Value.ToString());

            if (!(bool)validityDefaultRd.IsChecked)
            {
                valid = int.Parse(validityTb.Value.ToString());
            }

            DateTime endDate = new DateTime();
            endDate = DateTime.Now;
            endDate.AddDays(valid);

            DateTime deliveryDate = new DateTime();
            deliveryDate = DateTime.Now;
            deliveryDate.AddDays(estDel);

            int downP = 50;
            if (!(bool)paymentDefaultRd.IsChecked)
                downP = int.Parse(downpaymentPercentTb.Value.ToString());

            decimal penaltyP = 0.1M;
            if (!(bool)penaltyDefaultRd.IsChecked)
                penaltyP = (decimal)customPenaltyTb.Value;
            int warr = 0;
            if ((bool)warrantyDefaultRd.IsChecked)
            {
                warr = 1;
            }
            else if ((bool)warrantycustomRd.IsChecked)
            {
                warr = int.Parse(warrantyDaysCustom.Value.ToString());
            }
            string quoteName = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbs = "01234567890123456789";
            
            var random = new Random();


            if (MainVM.SelectedCustomerSupplier.CompanyName.Length > 5)
            {
                quoteName = MainVM.SelectedCustomerSupplier.CompanyName.Trim().Substring(0, 5).ToUpper();

            }
            else
                quoteName = MainVM.SelectedCustomerSupplier.CompanyName.Trim().ToUpper();
            string stringChars = "";
            for (int i = 0; i < 4; i++)
            {
                if (!(i > 3))
                {
                    stringChars += chars[random.Next(chars.Length)];
                }
            }
            stringChars += MainVM.SalesQuotes.Count + 1;
            stringChars += "-";
            foreach (char c in quoteName)
            {
                stringChars += c;
            }
            stringChars += "-";
            stringChars += DateTime.Now.ToString("yyyy-MM-dd");

            MainVM.SelectedSalesQuote = new SalesQuote()
            {
                sqNoChar_ = stringChars,
                custID_ = int.Parse(MainVM.SelectedCustomerSupplier.CompanyID),
                custRepID_ = int.Parse(MainVM.SelectedCustomerSupplier.RepresentativeID),
                quoteSubject_ = stringChars,
                priceNote_ = "" + moneyType.SelectedValue.ToString() + ", " + landed + ", " + vatExc,
                vatexcluded_ = (bool)vatCheckBox.IsChecked,
                vat_ = vat,
                paymentIsLanded_ = (bool)landedCheckBox.IsChecked,
                paymentCurrency_ = moneyType.SelectedValue.ToString(),
                estDelivery_ = estDel,
                deliveryDate_ = deliveryDate,
                validityDays_ = valid,
                validityDate_ = endDate,
                status_ = "PENDING",
                termsDP_ = downP,
                penaltyPercent_ = penaltyP,
                warrantyDays_ = warr,
                additionalTerms_ = additionalTermsTb.Text,
                markUpPercent_ = (decimal)markupPriceTb.Value,
                discountPercent_ = (decimal)discountPriceTb.Value
            };
            MainVM.SalesQuotes.Add(MainVM.SelectedSalesQuote);
            MainVM.SelectedRepresentative = MainVM.Representatives.Where(x => x.RepresentativeID.Equals(MainVM.SelectedCustomerSupplier.RepresentativeID)).First();

        }

        void saveSalesQuoteToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO `odc_db`.`sales_quote_t` " + "(`sqNoChar`,`custID`,`custRepID`,`quoteSubject`,`priceNote`,`deliveryDate`,`estDelivery`,`validityDays`,`validityDate`,`otherTerms`,`expiration`,`VAT`,`vatIsExcluded`,`paymentIsLanded`,`paymentCurrency`,`status`,`termsDays`,`termsDP`,`penaltyAmt`,`penaltyPerc`,`markUpPercent`,`discountPercent`)" +
                    " VALUES " +
                    "('" + MainVM.SelectedSalesQuote.sqNoChar_ + "','" + 
                    MainVM.SelectedSalesQuote.custID_ + "','" + 
                    MainVM.SelectedSalesQuote.custRepID_ + "','" +
                    MainVM.SelectedSalesQuote.quoteSubject_ + "','" +
                    MainVM.SelectedSalesQuote.priceNote_ + "','" +
                    MainVM.SelectedSalesQuote.deliveryDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesQuote.estDelivery_ + "','" +
                    MainVM.SelectedSalesQuote.validityDays_ + "','" +
                    MainVM.SelectedSalesQuote.validityDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesQuote.otherTerms_ + "','" +
                    MainVM.SelectedSalesQuote.expiration_.ToString("yyyy-MM-dd") + "','" + 
                    MainVM.SelectedSalesQuote.vat_ + "'," + 
                    MainVM.SelectedSalesQuote.vatexcluded_ + "," +
                    MainVM.SelectedSalesQuote.paymentIsLanded_ + ",'" +
                    MainVM.SelectedSalesQuote.paymentCurrency_ + "','" +
                    MainVM.SelectedSalesQuote.status_ + "','" +
                    MainVM.SelectedSalesQuote.termsDays_ + "','" +
                    MainVM.SelectedSalesQuote.termsDP_ + "','" +
                    MainVM.SelectedSalesQuote.penaltyAmt_ + "','" +
                    MainVM.SelectedSalesQuote.penaltyPercent_ + "','" +
                    MainVM.SelectedSalesQuote.markUpPercent_ + "','" +
                    MainVM.SelectedSalesQuote.discountPercent_ + "'); ";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    if (dbCon.IsConnect())
                    {
                        foreach(RequestedItem item in MainVM.RequestedItems)
                        {
                            if (item.itemType == 0)
                            {
                                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ItemCode.Equals(item.itemCode)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`items_availed_t`(`sqNoChar`,`itemCode`,`itemQnty`,`totalCost`)" +
                                    " VALUES " +
                                    "(" + MainVM.SelectedSalesQuote.sqNoChar_+ ", "+ MainVM.SelectedProduct.ItemCode + "," + item.qty + ", " + item.totalAmount + ");";
                                noError = dbCon.insertQuery(query, dbCon.Connection);                            }
                            else if (item.itemType == 1)
                            {
                                
                                MainVM.SelectedAddedService = MainVM.AddedServices.Where(x => x.TableNoChar.Equals(item.itemCode)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`services_availed_t`(`tableNoChar`,`serviceID`,`provinceID`,`sqNoChar`,`city`,`address`,`totalCost`)" +
                                    " VALUES " +
                                    "('" + MainVM.SelectedAddedService.TableNoChar + "', '" + 
                                    MainVM.SelectedAddedService.ServiceID + "', '" + 
                                    MainVM.SelectedAddedService.ProvinceID + "', '" + 
                                    MainVM.SelectedSalesQuote.sqNoChar_ + "', '" + 
                                    MainVM.SelectedAddedService.City + "', '" + 
                                    MainVM.SelectedAddedService.Address + "', '" +
                                    MainVM.SelectedAddedService.TotalCost+ "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                                foreach (AdditionalFee af in MainVM.SelectedAddedService.AdditionalFees)
                                {
                                    query = "INSERT INTO `odc_db`.`fees_per_transaction_t`(`serviceNoChar`,`feeName`,`feeValue`)" +
                                    " VALUES " +
                                    "(" + item.itemCode + ", " + af.FeeName + ", " + af.FeePrice + ");";
                                    noError = dbCon.insertQuery(query, dbCon.Connection);
                                }
                            }
                        }
                        if (noError)
                        {
                            MessageBox.Show("Successfully added.");
                            loadDataToUi();
                        }
                            
                        else
                            MessageBox.Show("Theres an error occured in saving the record");
                    }
                }
            }
        }

        void loadSalesQuoteToUi()
        {
            MainVM.SelectedRepresentative = MainVM.Representatives.Where(x => x.RepresentativeID.Equals(MainVM.SelectedCustomerSupplier.RepresentativeID.ToString())).FirstOrDefault();
            MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(MainVM.SelectedSalesQuote.custID_.ToString())).FirstOrDefault();
            foreach(AddedItem item in MainVM.SelectedSalesQuote.AddedItems)
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
                    totalAmount = item.TotalCost,
                    itemType = 0,
                    unitPrice = MainVM.SelectedProduct.CostPrice,
                    
                });
            }
            foreach(AddedService service in MainVM.SelectedSalesQuote.AddedServices)
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

                });
            }
            MainVM.AddedServices = MainVM.SelectedSalesQuote.AddedServices;
            MainVM.AddedItems = MainVM.SelectedSalesQuote.AddedItems;

            computePrice();
            

        }
        private void viewQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            newSalesQuoteBtns.Visibility = Visibility.Visible;
            foreach(var obj in newRequisitionGridForm.Children)
            {
                bool isEnabled = false;
                if (obj is Button)
                    ((Button)obj).IsEnabled = isEnabled;
                else if (obj is Xceed.Wpf.Toolkit.IntegerUpDown)
                    ((Xceed.Wpf.Toolkit.IntegerUpDown)obj).IsEnabled = isEnabled;
            }
            foreach (var obj in newRequisitionGridForm.Children)
            {
                bool isEnabled = false;
                if (obj is Button)
                    ((Button)obj).IsEnabled = isEnabled;
                else if (obj is Xceed.Wpf.Toolkit.IntegerUpDown)
                    ((Xceed.Wpf.Toolkit.IntegerUpDown)obj).IsEnabled = isEnabled;
            }
            loadSalesQuoteToUi();
        }

        private void editQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            newSalesQuoteBtns.Visibility = Visibility.Visible;
        }

        private void deleteQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convertToInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
