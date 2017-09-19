using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
        public MainScreen()
        {
            InitializeComponent();
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
            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            dashboardGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
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
            MainVM.Customers.Clear();
            MainVM.Suppliers.Clear();
            MainVM.Representatives.Clear();
            MainVM.AllCustomerSupplier.Clear();
            if (dbCon.IsConnect())
            {
                string query = "SELECT locProvinceID, locProvince FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Provinces.Add(new Province() { ProvinceID = (int)dr["locProvinceID"], ProvinceName = dr["locProvince"].ToString() });
                }
                dbCon.Close();
            }

            if (dbCon.IsConnect())
            {
                string query = "SELECT locationProvinceID, locationPrice FROM location_details_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    foreach (Province prov in MainVM.Provinces)
                    {
                        if (prov.ProvinceID == (int)dr["locationProvinceID"])
                            prov.ProvincePrice = (decimal)dr["locationPrice"];
                    }
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
                string query = "SELECT * FROM service_t;";
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
                string query = "SELECT * FROM ITEM_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.SelectedCustomerSupplier = MainVM.Suppliers.Where(x => x.CompanyID.Equals(dr["supplierID"].ToString())).FirstOrDefault();
                    if (MainVM.SelectedCustomerSupplier != null)
                    {
                        MainVM.ProductList.Add(new Item() { ItemNo = dr["itemNo"].ToString(), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), Quantity = 1, SupplierID = dr["supplierID"].ToString(), SupplierName = MainVM.SelectedCustomerSupplier.CompanyName });
                    }
                    else
                        MainVM.ProductList.Add(new Item() { ItemNo = dr["itemNo"].ToString(), ItemName = dr["itemName"].ToString(), ItemDesc = dr["itemDescr"].ToString(), CostPrice = (decimal)dr["costPrice"], TypeID = dr["typeID"].ToString(), Unit = dr["itemUnit"].ToString(), Quantity = 1, SupplierID = dr["supplierID"].ToString()});
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT locationProvinceID, locationPrice FROM location_details_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    foreach (Province prov in MainVM.Provinces)
                    {
                        if (prov.ProvinceID == (int)dr["locationProvinceID"])
                            prov.ProvincePrice = (decimal)dr["locationPrice"];
                    }
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
                    MainVM.Customers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), RepresentativeID = dr["representativeID"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString() });
                    MainVM.AllCustomerSupplier.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString(), CompanyEmail = dr["companyEmail"].ToString(), CompanyTelephone = dr["companyTelephone"].ToString(), CompanyMobile = dr["companyMobile"].ToString(), RepresentativeID = dr["representativeID"].ToString(), CompanyType = dr["companyType"].ToString(), CompanyPostalCode = dr["companyPostalCode"].ToString() });
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
            settingGridBg.Visibility = Visibility.Visible;
        }

        private void closeSideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            settingGridBg.Visibility = Visibility.Collapsed;
        }

        private void quotesSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {

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
            settingGridBg.Visibility = Visibility.Visible;
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
                if (manageEmployeeGrid.IsVisible)
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
            sb.Begin(employeeDetailsFormGrid);
            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            productDetailsFormGridBg.Visibility = Visibility.Collapsed;
            if (employeeDetailsFormGridBg.IsVisible)
            {
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            }
            else
                employeeDetailsFormGridBg.Visibility = Visibility.Visible;
        }

        private void manageCustomerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(companyDetailsFormGrid);
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            productDetailsFormGridBg.Visibility = Visibility.Collapsed;
            if (companyDetailsFormGrid.IsVisible)
            {
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            }
            else
                companyDetailsFormGridBg.Visibility = Visibility.Visible;
        }

        private void manageProductAddBtn_Click(object sender, RoutedEventArgs e)
        {
            
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(productDetailsFormGrid);
            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            if (productDetailsFormGrid.IsVisible)
            {

                productDetailsFormGridBg.Visibility = Visibility.Collapsed;
            }
            else
                productDetailsFormGridBg.Visibility = Visibility.Visible;
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
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(companyDetailsFormGrid);
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                saveCancelGrid.Visibility = Visibility.Collapsed;
                editCloseGrid.Visibility = Visibility.Visible;
                if (companyDetailsFormGrid.IsVisible)
                {
                    companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    companyDetailsFormGridBg.Visibility = Visibility.Visible;
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
                sb.Begin(employeeDetailsFormGridBg);
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                saveCancelGrid1.Visibility = Visibility.Collapsed;
                editCloseGrid1.Visibility = Visibility.Visible;
                if (companyDetailsFormGrid.IsVisible)
                {
                    employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    employeeDetailsFormGridBg.Visibility = Visibility.Visible;
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
                sb.Begin(productDetailsFormGrid);
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                saveCancelGrid2.Visibility = Visibility.Collapsed;
                editCloseGrid2.Visibility = Visibility.Visible;
                if (productDetailsFormGrid.IsVisible)
                {

                    productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    productDetailsFormGridBg.Visibility = Visibility.Visible;
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
            if (manageCustomerGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(companyDetailsFormGrid);
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                if (companyDetailsFormGrid.IsVisible)
                {

                    companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    companyDetailsFormGridBg.Visibility = Visibility.Visible;
                loadRecordToFields();
                isEdit = true;
                saveCancelGrid.Visibility = Visibility.Visible;
                editCloseGrid.Visibility = Visibility.Collapsed;
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(employeeDetailsFormGrid);
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                if (employeeDetailsFormGrid.IsVisible)
                {

                    employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    employeeDetailsFormGridBg.Visibility = Visibility.Visible;
                loadRecordToFields();
                isEdit = true;
                saveCancelGrid1.Visibility = Visibility.Visible;
                editCloseGrid1.Visibility = Visibility.Collapsed;
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(productDetailsFormGrid);
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
                if (productDetailsFormGrid.IsVisible)
                {

                    productDetailsFormGridBg.Visibility = Visibility.Collapsed;
                }
                else
                    productDetailsFormGridBg.Visibility = Visibility.Visible;
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
                String id = MainVM.SelectedProduct.ItemNo;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `item_t` SET `isDeleted`= 1 WHERE itemNo = '" + id + "';";
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
            editCloseGrid.Visibility = Visibility.Collapsed;
            saveCancelGrid.Visibility = Visibility.Visible;
            isEdit = true;
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyTelephone))
            {
                companyTelCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyMobile))
            {
                companyMobCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepTelephone))
            {
                repTelCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedRepresentative.RepMobile))
            {
                repMobCb.IsChecked = true;
            }
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
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (companyDetailsFormGridBg.IsVisible)
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
                else if (employeeDetailsFormGridBg.IsVisible)
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
                else if (productDetailsFormGridBg.IsVisible)
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
            else if (result == MessageBoxResult.No)
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
            if (companyDetailsFormGridBg.IsVisible)
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
            else if (employeeDetailsFormGridBg.IsVisible)
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
            else if (productDetailsFormGridBg.IsVisible)
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
            if (companyDetailsFormGridBg.IsVisible)
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
            else if (employeeDetailsFormGridBg.IsVisible)
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
            else if (productDetailsFormGridBg.IsVisible)
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
                        cmd.Parameters.AddWithValue("@itemNo", MainVM.SelectedProduct.ItemNo);
                        cmd.Parameters["@itemNo"].Direction = ParameterDirection.Input;
                        isEdit = false;
                    }

                    //INSERT NEW Product TO DB;

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

            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            productDetailsFormGridBg.Visibility = Visibility.Collapsed;
            companyDetailsFormGridSv.ScrollToTop();
            employeeDetailsFormGridSv.ScrollToTop();
            productDetailsFormGridSv.ScrollToTop();
            supplierCb.SelectedIndex = 0;
            foreach (var element in companyDetailsFormGrid1.Children)
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
            foreach (var element in employeeDetailsFormGrid1.Children)
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
            foreach (var element in productDetailsFormGrid1.Children)
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
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
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
            if (serviceName.Text.Equals("") || servicePrice.Value == 0)
            {
                saveServiceTypeBtn.IsEnabled = false;
            }
            else
            {
                saveServiceTypeBtn.IsEnabled = true;
            }
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
                    string query = "INSERT INTO service_t (serviceName,serviceDesc,servicePrice) VALUES ('" + serviceName.Text + "','" + serviceDesc.Text + "', '" + servicePrice.Value + "')";
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
                    string query = "UPDATE `service_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + id + "'";
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
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    string query = "SELECT locationID,locationPrice FROM location_details_t " +
                        "WHERE locationProvinceId = '" + provinceCb.SelectedValue + "';";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    if (fromDbTable.Rows.Count != 0)
                    {
                        initPrice = false;
                        foreach (DataRow dr in fromDbTable.Rows)
                        {
                            locationPrice.Value = Decimal.Parse(dr["locationPrice"].ToString());
                            locationid = dr["locationId"].ToString();
                        }
                    }
                    else
                    {
                        locationPrice.Value = 0;
                        initPrice = true;
                    }
                }
            }

        }

        private void setPriceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (locationPrice.Value != null)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                MessageBoxResult result = MessageBox.Show("Do you want to save this price?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (initPrice)
                    {
                        string query = "INSERT INTO location_details_t (locationProvinceID,locationPrice) VALUES ('" + provinceCb.SelectedValue + "', '" + locationPrice.Value + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Price saved.");
                            provinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;

                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            loadDataToUi();
                        }
                    }
                    else
                    {
                        string query = "UPDATE `location_details_t` SET locationPrice = '" + locationPrice.Value + "' WHERE locationId = '" + locationid + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            //MessageBox.Show("Price updated.");
                            id = "";
                            provinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;
                            initPrice = true;
                            loadDataToUi();
                        }
                    }



                }
                else if (result == MessageBoxResult.No)
                {
                    provinceCb.SelectedValue = -1;
                    locationPrice.Value = 0;
                }
                else if (result == MessageBoxResult.Cancel)
                {
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

        
    }
}
