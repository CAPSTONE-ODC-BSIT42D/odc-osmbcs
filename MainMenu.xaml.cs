using Microsoft.Win32;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public static MainViewModel MainVM = new MainViewModel();
        public MainMenu()
        {
            InitializeComponent();
            this.DataContext = MainVM;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            contactTypeCb.SelectedIndex = 0;
            contactTypeCb1.SelectedIndex = 0;
            contactDetailsMobileTb.IsEnabled = false;
            contactDetailsPhoneTb.IsEnabled = false;
            contactDetailsEmailTb.IsEnabled = false;
            contactDetailsMobileTb1.IsEnabled = false;
            contactDetailsPhoneTb1.IsEnabled = false;
            contactDetailsEmailTb1.IsEnabled = false;
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                empProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
            setManageCustomerGridControls();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (!visual.IsDescendantOf(saleSubMenuGrid) && !visual.IsDescendantOf(manageSubMenugrid))
                saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            //if (!visual.IsDescendantOf(manageEmployeeDataGrid))
            //{
            //    if (manageEmployeeDataGrid.SelectedItems.Count > 0)
            //    {
            //        manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnEditBtnEmp)].Visibility = Visibility.Hidden;
            //        manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnDelBtnEmp)].Visibility = Visibility.Hidden;
            //    }
            //}
            //if (!visual.IsDescendantOf(manageSupplierDataGrid))
            //{
            //    if (manageSupplierDataGrid.SelectedItems.Count > 0)
            //    {
            //        manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnEditSuppBtn)].Visibility = Visibility.Hidden;
            //        manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnDeleteSuppBtn)].Visibility = Visibility.Hidden;
            //    }
            //}
            //if (!visual.IsDescendantOf(manageCustomeDataGrid))
            //{
            //    if (manageCustomeDataGrid.SelectedItems.Count > 0)
            //    {
            //        manageCustomeDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnEditBtnCont)].Visibility = Visibility.Hidden;
            //        manageCustomeDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnDelBtnCont)].Visibility = Visibility.Hidden;
            //    }
            //}
        }

        /*-----------------MENU BAR BUTTONS-------------------*/

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else if (result == MessageBoxResult.No)
            {

            }
        }

        private void salesBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Visible;
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            servicesGrid.Visibility = Visibility.Visible;
        }

        private void reportsBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            reportsGrid.Visibility = Visibility.Visible;
        }

        private void dashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
        }

        private void manageBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Visible;
        }

        /*-----------------END OF MENU BAR BUTTONS-------------------*/

        /*-----------------SUB MENU BAR BUTTONS-------------------*/

        private void quotesSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionGrid.Visibility = Visibility.Visible;
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            for (int x = 0; x<transactionGrid.Children.Count; x++)
            {
                transactionGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionQuotationsGrid.Visibility = Visibility.Visible;
            quotationsGridHome.Visibility = Visibility.Visible;

        }

        private void ordersSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < transactionGrid.Children.Count; x++)
            {
                transactionGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionOrdersGrid.Visibility = Visibility.Visible;
        }

        private void invoiceSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < transactionGrid.Children.Count; x++)
            {
                transactionGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionInvoiceGrid.Visibility = Visibility.Visible;
        }

        private void recieptSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < transactionGrid.Children.Count; x++)
            {
                transactionGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transactionReceiptGrid.Visibility = Visibility.Visible;
        }

        /*-----------------END OF SUB MENU BAR BUTTONS-------------------*/


        /*-----------------TRANSTACTION-------------------*/
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.Customers.Where(x => x.CompanyName.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()));
            var observable = new ObservableCollection<Customer>(linqResults);
            selectCustomerDg.ItemsSource = observable;
        }

        private void transQuoteAddBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            selectCustomerGrid.Visibility = Visibility.Visible;
        }

        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            addRequestionGrid.Visibility = Visibility.Visible;
        }

        private void transRequestBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel this transaction?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
                {
                    transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                selectCustomerGrid.Visibility = Visibility.Visible;
            }
            else if (result == MessageBoxResult.Cancel)
            {

            }
        }
        
        private void paymentCustomRb_Checked(object sender, RoutedEventArgs e)
        {
            downPercentTb.IsEnabled = true;
            paymentDpLbl.IsEnabled = true;
        }

        private void paymentCustomRb_Unchecked(object sender, RoutedEventArgs e)
        {
            downPercentTb.IsEnabled = false;
            paymentDpLbl.IsEnabled = false;
        }

        private void transRequestNext_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            makeSalesQuoteGrid.Visibility = Visibility.Visible;
        }

        private void transQuotationFormBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save this progress?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
                {
                    transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                addRequestionGrid.Visibility = Visibility.Visible;
            }
            else if (result == MessageBoxResult.No)
            {

            }

        }

        private void transReqAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            addNewItem newItem = new addNewItem();
            newItem.ShowDialog();
            //foreach (Representative rep in MainVM.Representatives)
            //{

            //}
        }

        private void transQuotationSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            viewQuotationGrid.Visibility = Visibility.Visible;
        }

        private void transViewSaveOnlyBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            quotationsGridHome.Visibility = Visibility.Visible;
        }

        private void transViewSaveSend_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            quotationsGridHome.Visibility = Visibility.Visible;
        }

        private void transViewBackBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            makeSalesQuoteGrid.Visibility = Visibility.Visible;
        }

        private void validtycustomRd_Checked(object sender, RoutedEventArgs e)
        {
            ValidityCustom.IsEnabled = true;
            validtycustomlbl.IsEnabled = true;
        }

        private void validtycustomRd_Unchecked(object sender, RoutedEventArgs e)
        {
            ValidityCustom.IsEnabled = false;
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

        private void orderFormBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to cancel this transaction?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                for (int x = 1; x < transactionOrdersGrid.Children.Count; x++)
                {
                    transactionOrdersGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                transOrdersGridHome.Visibility = Visibility.Visible;
            }
            else if (result == MessageBoxResult.No)
            {

            }
        }

        private void transOrdersAddBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionOrdersGrid.Children.Count; x++)
            {
                transactionOrdersGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            orderFormGrid.Visibility = Visibility.Visible;
        }

        private void transOrderSaveOnly_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionOrdersGrid.Children.Count; x++)
            {
                transactionOrdersGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transOrdersGridHome.Visibility = Visibility.Visible;
        }

        private void transOrderSaveSend_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionOrdersGrid.Children.Count; x++)
            {
                transactionOrdersGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            transOrdersGridHome.Visibility = Visibility.Visible;
        }

        /*-----------------END OF TRANSTACTION-------------------*/
        

        /*-----------------MANAGE CUSTOMER-------------------*/

        private void customerManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < manageGrid.Children.Count; x++)
            {
                manageGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageCustomerGrid.Visibility = Visibility.Visible;
        }

        private void setManageCustomerGridControls()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Customers.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Customers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString() , CompanyProvinceName = dr["locProvince"].ToString() ,CompanyProvinceID = dr["companyProvinceID"].ToString() });
                }
                dbCon.Close();
            }
        }

        private void btnEditCust_Click(object sender, RoutedEventArgs e)
        {
            loadCustSuppDetails();
            compType = 0;
            manageCustomerGrid.Visibility = Visibility.Hidden;
            companyDetailsGrid.Visibility = Visibility.Visible;
            companyDetailsHeader.Content = "Manage Customer - Edit Customer";
            isEdit = true;
        }

        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            if (manageCustomeDataGrid.SelectedItems.Count > 0)
            {
                String id = MainVM.SelectedCustomer.CompanyID;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `cust_supp_t` SET `isDeleted`= 1 WHERE companyID = '"+id+"';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            setManageCustomerGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
                setManageCustomerGridControls();
            }
        }

        

        private void manageCustomerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            manageCustomerGrid.Visibility = Visibility.Hidden;
            companyDetailsGrid.Visibility = Visibility.Visible;
            companyDetailsHeader.Content = "Manage Customer - New Customer";
            compType = 0;
        }

        private void manageCustomeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        
        private void manageCustomerGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            setManageCustomerGridControls();
        }
        

        /*-----------------END OF MANAGE CUSTOMER-------------------*/
        /*
        /*
        /*
        /*
        /*
        /*-----------------MANAGE EMPLOYEE-------------------------*/


        private void manageEmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void manageEmployeeDataGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void setManageEmployeeGridControls()
        {

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM position_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                empPostionCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT jobID, jobName FROM job_title_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                empJobCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT a.empID, a.empFName,a.empLname, a.empMI, a.empAddinfo, a.empAddress, a.empCity, a.empProvinceID, a.empUserName, b.locprovince, a.positionID ,c.positionName, a.jobID, d.empPic, d.empSignature " +
                    "FROM emp_cont_t a  " +
                    "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
                    "JOIN position_t c ON a.positionID = c.positionid " +
                    "JOIN emp_pic_t d ON a.empID = d.empID " +
                    //"JOIN job_title_t d ON a.jobID = d.jobID " +
                    "WHERE isDeleted = 0 AND empType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Employees.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    if (dr["empPic"].Equals(DBNull.Value))
                    {
                        MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpUserName = dr["empUserName"].ToString() });
                    }
                    else
                    {
                        MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), EmpPic = (byte[])dr["empPic"], EmpUserName = dr["empUserName"].ToString() });
                    }
                    
                }
                dbCon.Close();
            }
        }

        private void manageEmployeeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            manageEmployeeGrid.Visibility = Visibility.Hidden;
            employeeDetailsGrid.Visibility = Visibility.Visible;
            employeeDetailsHeader.Content = "Manage Employee - New Employee";
            empType = 0;
            employeeOnlyGrid.Visibility = Visibility.Visible;
            contractorOnlyGrid.Visibility = Visibility.Collapsed;
            empJobCb.IsEnabled = false;
            empDateStarted.IsEnabled = false;
            empDateEnded.IsEnabled = false;
            setManageEmployeeGridControls();
        }

        private void btnEditEmp_Click(object sender, RoutedEventArgs e)
        {
            setManageEmployeeGridControls();
            manageEmployeeGrid.Visibility = Visibility.Hidden;
            employeeDetailsGrid.Visibility = Visibility.Visible;
            employeeDetailsHeader.Content = "Manage Employee - Edit Employee";
            empType = 0;
            isEdit = true;
            employeeOnlyGrid.Visibility = Visibility.Visible;
            contractorOnlyGrid.Visibility = Visibility.Collapsed;
            empJobCb.IsEnabled = false;
            empDateStarted.IsEnabled = false;
            empDateEnded.IsEnabled = false;
        }

        private void btnDeleteEmp_Click(object sender, RoutedEventArgs e)
        {
            if (manageEmployeeDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageEmployeeDataGrid.Columns[0].GetCellContent(manageEmployeeDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `emp_cont_t` SET `isDeleted`= 1 WHERE empID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Employee record successfully deleted!");
                            setManageEmployeeGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
                setManageCustomerGridControls();
            }
        }

        private void employeeManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            setManageEmployeeGridControls();
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < manageGrid.Children.Count; x++)
            {
                manageGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageEmployeeGrid.Visibility = Visibility.Visible;

        }

        /*-----------------END OF MANAGE EMPLOYEE-------------------*/
        /*
        /*
        /*
        /*-----------------MANAGE CONTRACTOR-------------------------*/
        
        private void contractorManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < manageGrid.Children.Count; x++)
            {
                manageGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageContractorGrid.Visibility = Visibility.Visible;
            setManageEmployeeGridControls();
        }

        private void btnEditCont_Click(object sender, RoutedEventArgs e)
        {
            setManageContractorGridControls();
            manageContractorGrid.Visibility = Visibility.Hidden;
            employeeDetailsGrid.Visibility = Visibility.Visible;
            employeeDetailsHeader.Content = "Manage Contractor - Edit Contractor";
            empType = 1;
            isEdit = true;
            contractorOnlyGrid.Visibility = Visibility.Visible;
            empJobCb.IsEnabled = true;
            empDateStarted.IsEnabled = true;
            empDateEnded.IsEnabled = true;

        }

        private void btnDeleteCont_Click(object sender, RoutedEventArgs e)
        {
            if (manageContractorDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageContractorDataGrid.Columns[0].GetCellContent(manageContractorDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this Contractor record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        string query = "UPDATE `contractor_t` SET `isDeleted`= 1 WHERE contID = '" + id + "';";
                        if (dbCon.insertQuery(query, conn))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            setManageContractorGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
                setManageContractorGridControls();
            }
        }

        private void manageContractorGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //setManageContractorGridControls();
        }

        private void setManageContractorGridControls()
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                string query = query = "";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, conn);
                DataSet fromDb = new DataSet();
                //dataAdapter.Fill(fromDb, "t");
                //manageContractorDataGrid.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT a.empID, a.empFName,a.empLname, a.empMI, a.empAddinfo, a.empAddress, a.empCity, a.empProvinceID, b.locprovince, a.positionID ,c.positionName, a.jobID, d.jobName " +
                    "FROM emp_cont_t a  " +
                    "JOIN provinces_t b ON a.empProvinceID = b.locProvinceId " +
                    "JOIN position_t c ON a.positionID = c.positionid " +
                    "JOIN job_title_t d ON a.jobID = d.jobID " +
                    "WHERE a.isDeleted = 0 AND a.empType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Employees.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Employees.Add(new Employee() { EmpID = dr["empID"].ToString(), EmpFname = dr["empFName"].ToString(), EmpLName = dr["empLname"].ToString(), EmpMiddleInitial = dr["empMI"].ToString(), EmpAddInfo = dr["empAddInfo"].ToString(), EmpAddress = dr["empAddress"].ToString(), EmpCity = dr["empCity"].ToString(), EmpProvinceID = dr["empProvinceID"].ToString(), EmpProvinceName = dr["locprovince"].ToString(), PositionID = dr["positionID"].ToString(), PositionName = dr["positionName"].ToString(), JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString() });
                }
                dbCon.Close();
            }
        }

        private void manageContractorAddBtn_Click(object sender, RoutedEventArgs e)
        {
            setManageContractorGridControls();
            manageContractorGrid.Visibility = Visibility.Hidden;
            employeeDetailsGrid.Visibility = Visibility.Visible;
            employeeDetailsHeader.Content = "Manage Contractor - New Contractor";
            empType = 1;
            employeeOnlyGrid.Visibility = Visibility.Collapsed;
            contractorOnlyGrid.Visibility = Visibility.Visible;
            empJobCb.IsEnabled = true;
            empDateStarted.IsEnabled = true;
            empDateEnded.IsEnabled = true;

        }

        private void manageContractorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /*-----------------END OF MANAGE CONTRACTOR-------------------*/
        /*
        /*
        /*
        /**/
        /*
        /*
        /*-----------------MANAGE SUPPLIER-------------------------*/


        private void supplierManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < manageGrid.Children.Count; x++)
            {
                manageGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageSupplierGrid.Visibility = Visibility.Visible;
        }

        private void manageSupplierGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            setManageSupplierGridControls();
        }

        private void setManageSupplierGridControls()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, cs.companyAddress,cs.companyCity,p.locProvince,cs.companyProvinceID " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainVM.Customers.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.Customers.Add(new Customer() { CompanyID = dr["companyID"].ToString(), CompanyName = dr["companyName"].ToString(), CompanyDesc = dr["companyAddInfo"].ToString(), CompanyAddress = dr["companyAddress"].ToString(), CompanyCity = dr["companyCity"].ToString(), CompanyProvinceName = dr["locProvince"].ToString(), CompanyProvinceID = dr["companyProvinceID"].ToString() });
                }
                dbCon.Close();
            }
            
        }

        private void btnEditSupp_Click(object sender, RoutedEventArgs e)
        {
            loadCustSuppDetails();
            compType = 1;
            manageSupplierGrid.Visibility = Visibility.Hidden;
            companyDetailsGrid.Visibility = Visibility.Visible;
            companyDetailsHeader.Content = "Manage Supplier - Edit Supplier";
            isEdit = true;
        }

        private void btnDeleteSupp_Click(object sender, RoutedEventArgs e)
        {
            if (manageSupplierDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageSupplierDataGrid.Columns[0].GetCellContent(manageSupplierDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this supplier record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `cust_supp_t` SET `isDeleted`= 1 WHERE companyID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                        }
                        setManageSupplierGridControls();
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                    setManageSupplierGridControls();
                }
            }
        }

        private void manageSupplierAddbtn_Click(object sender, RoutedEventArgs e)
        {
            clearSupplierFields();
            manageSupplierGrid.Visibility = Visibility.Hidden;
            companyDetailsGrid.Visibility = Visibility.Visible;
            companyDetailsHeader.Content = "Manage Supplier - New Supplier";
            compType = 1;
        }

        private void clearSupplierFields()
        {
            custCompanyNameTb.Clear();
            custCityTb.Clear();
            descriptionTb1.Clear();
            custAddressTb.Clear();
            custProvinceCb.SelectedValue = -1;
        }

        private void manageSupplierDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /*-----------------END OF MANAGE SUPPLIER-------------------*/
        /*
        /*
        /*
        /*
        /*
        /*-----------------MANAGE UTILITIES-------------------------*/

        private void settingsManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageGrid.Visibility = Visibility.Visible;
            for (int x = 0; x < manageGrid.Children.Count; x++)
            {
                manageGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            manageSettingsGrid.Visibility = Visibility.Visible;
            setManageSettingsGridControl();
        }

        private void setManageSettingsGridControl()
        {
            setListBoxControls();
        }

        private void settingsEmployeeGridBtn_Click(object sender, RoutedEventArgs e)
        {
            manageEmployeeSettings manageEmployeeSettings = new manageEmployeeSettings();
            manageEmployeeSettings.ShowDialog();
        }

        private void settingsServicesGridBtn_Click(object sender, RoutedEventArgs e)
        {
            manageServicesSettings manageServicesSettings = new manageServicesSettings();
            manageServicesSettings.ShowDialog();
        }

        private void settingsInvetoryBtn_Click(object sender, RoutedEventArgs e)
        {
            manageProductSettings manageProductSettings = new manageProductSettings();
            manageProductSettings.ShowDialog();
        }

        private static String dbname = "odc_db";

        //EMPLOYEE PART
        private void addEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (employeePositionLb.Items.Contains(empPosNewTb.Text))
            {
                MessageBox.Show("Employee position already exists");
            }
            else
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Employee Poisition successfully added");
                        empPosNewTb.Clear();
                        setListBoxControls();
                        dbCon.Close();
                    }
                }
            }

        }

        private void saveEmpPosBtn_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("Already in the list.");
                }
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Employee Poisition saved");
                        empPosNewTb.Clear();
                        setListBoxControls();
                        dbCon.Close();
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
                        string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Employee position successfully deleted.");
                            setListBoxControls();
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

            if (employeePositionLb.SelectedItems.Count > 0)
            {

                empPosNewTb.Text = MainMenu.MainVM.SelectedEmpPosition.PositionName;
            }
            else
            {
                MessageBox.Show("Please select an employee position first.");
            }
            dbCon.Close();
        }

        //CONTRACTOR PART
        private void addContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (contJobLb.Items.Contains(contNewJobTb.Text))
            {
                MessageBox.Show("Contractor  already exists");
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
                        setListBoxControls();
                        dbCon.Close();
                    }
                }
            }

        }

        private void saveContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
            {
                MessageBox.Show("Contractor  must be filled");
            }
            else
            {
                if (contJobLb.Items.Contains(contNewJobTb.Text))
                {
                    MessageBox.Show("Already in the list.");
                }
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `odc_db`.`job_title_t` set `jobName` = '" + contNewJobTb.Text + "' where positionID = '" + MainMenu.MainVM.SelectedJobTitle.JobID + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Employee Poisition saved");
                        contNewJobTb.Clear();
                        setListBoxControls();
                        dbCon.Close();
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
                        string query = "DELETE FROM `odc_db`.`job_title_t` WHERE `positionID`='" + MainMenu.MainVM.SelectedJobTitle.JobID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Employee position successfully deleted.");
                            setListBoxControls();
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

        private void editContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;

            if (contJobLb.SelectedItems.Count > 0)
            {

                contNewJobTb.Text = MainMenu.MainVM.SelectedJobTitle.JobName;
            }
            else
            {
                MessageBox.Show("Please select an employee position first.");
            }
            dbCon.Close();
        }

        private void setListBoxControls()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM POSITION_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainMenu.MainVM.EmpPosition.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainMenu.MainVM.EmpPosition.Add(new EmpPosition() { PositionID = dr["positionid"].ToString(), PositionName = dr["positionName"].ToString() });
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
                MainMenu.MainVM.EmpPosition.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainMenu.MainVM.ContJobTitle.Add(new ContJobName() { JobID = dr["jobID"].ToString(), JobName = dr["jobName"].ToString() });
                }
                dbCon.Close();
            }
        }

        /*-----------------END OF MANAGE UTILITIES-------------------*/
        /*
        /*
        /*
        /*
        /**/


        private void servicesDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /*----------------------------------------*/
        private int compType = 0;
        private bool isEdit = false;
        private string compID = "";
        private void saveCustBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Do you wish to save this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                custDataToDb();

            }
            else if (result == MessageBoxResult.Cancel)
            {
                clearCompanyDetailsGrid();
            }

        }
        
        private void cancelCustBtn_Click(object sender, RoutedEventArgs e)
        {
            clearCompanyDetailsGrid();
        }
        

        private void custCompanyNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(custCompanyNameTb) == true)
                saveCustBtn.IsEnabled = false;
            else validateCustomerDetailsTextBoxes();

        }

        private void locationAddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(custAddressTb) == true)
                saveCustBtn.IsEnabled = false;
            else validateCustomerDetailsTextBoxes();
        }

        private void locationCityTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(custCityTb) == true)
                saveCustBtn.IsEnabled = false;
            else validateCustomerDetailsTextBoxes();
        }

        private void contactDetailsEmailTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true)
            {
                saveCustBtn.IsEnabled = false;
                saveCustContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsEmailTb.Text;
                saveCustContactBtn.IsEnabled = true;
                validateCustomerDetailsTextBoxes();
            }

        }

        private void contactDetailsPhoneTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true)
            {
                saveCustBtn.IsEnabled = false;
                saveCustContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsPhoneTb.Text;
                saveCustContactBtn.IsEnabled = true;
                validateCustomerDetailsTextBoxes();
            }
        }

        private void contactDetailsMobileTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true)
            {
                saveCustBtn.IsEnabled = false;
                saveCustContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsMobileTb.Text;
                saveCustContactBtn.IsEnabled = true;
                validateCustomerDetailsTextBoxes();
            }
        }

        private string contactDetail = "";
        private void contactTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactTypeCb.SelectedIndex==0)
            {
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = false;
                contactDetailsEmailTb.IsEnabled = false;
                clearContactsBoxes();
            }
            else if (contactTypeCb.SelectedIndex == 1)
            {
                contactDetailsEmailTb.Visibility = Visibility.Visible;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = false;
                contactDetailsEmailTb.IsEnabled = true;
                clearContactsBoxes();
            }
            else if (contactTypeCb.SelectedIndex == 2)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Visible;
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = true;
                contactDetailsEmailTb.IsEnabled = false;
                clearContactsBoxes();

            }
            else if (contactTypeCb.SelectedIndex == 3)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.Visibility = Visibility.Visible;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.IsEnabled = true;
                contactDetailsPhoneTb.IsEnabled = false;
                contactDetailsEmailTb.IsEnabled = false;
                clearContactsBoxes();
            }
        }

        private void addNewCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true))
            {
                if (contactTypeCb.SelectedIndex != 0)
                {
                    if (contactTypeCb.SelectedIndex == 1)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsEmailTb.Text))
                        {

                            MainVM.CustContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb.SelectedIndex == 2)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsPhoneTb.Text))
                        {

                            MainVM.CustContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb.SelectedIndex == 3)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsMobileTb.Text))
                        {
                            MainVM.CustContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a contact type.");
                }
            }
            else
            {
                MessageBox.Show("Please resolve the error first.");
            }
        }

        private void btnEditCustCont_Click(object sender, RoutedEventArgs e)
        {
            if (custContactDg.SelectedItem != null)
            {
                contactTypeCb.SelectedIndex = int.Parse(MainVM.SelectedCustContact.ContactTypeID);
                if (MainVM.SelectedCustContact.ContactTypeID.Equals("1"))
                {
                    contactDetailsEmailTb.Text = MainVM.SelectedCustContact.ContactDetails;
                }
                else if (MainVM.SelectedCustContact.ContactTypeID.Equals("2"))
                {
                   contactDetailsPhoneTb.Text = MainVM.SelectedCustContact.ContactDetails;
                }
                else if (MainVM.SelectedCustContact.ContactTypeID.Equals("3"))
                {
                    contactDetailsMobileTb.Text = MainVM.SelectedCustContact.ContactDetails;
                }
                saveCustContactBtn.Visibility = Visibility.Visible;
                cancelCustContactBtn.Visibility = Visibility.Visible;
            }
          
        }

        private void saveCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true))
            {
                if (contactTypeCb.SelectedIndex != 0)
                {
                    if (contactTypeCb.SelectedIndex == 1)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsEmailTb.Text))
                        {

                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb.SelectedIndex == 2)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsPhoneTb.Text))
                        {

                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb.SelectedIndex == 3)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsMobileTb.Text))
                        {
                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a contact type.");
                }
            }
            else
            {
                MessageBox.Show("Please resolve the error first.");
            }
        }

        private void cancelCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            contactTypeCb.SelectedIndex = 0;
            cancelCustContactBtn.Visibility = Visibility.Hidden;
            saveCustContactBtn.Visibility = Visibility.Hidden;
            clearContactsBoxes();
        }

        int newCust = 0;
        int newSupp = 0;
        private void btnDeleteCustCont_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this contact record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                MainVM.CustContacts.Remove(MainVM.SelectedCustContact);
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void custContactDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        public String[] repDetails;
        private void newRepresentativeBtn_Click(object sender, RoutedEventArgs e)
        {
            RepresentativeWindow repWindow = new RepresentativeWindow();
            MainVM.CustRepresentatives.Add(new Representative());
            customerRepresentativeDg.SelectedIndex = customerRepresentativeDg.Items.Count - 1;
            repWindow.ShowDialog();
            validateCustomerDetailsTextBoxes();
            
            
        }
        private void customerRepresentativeDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        

        private void editBtnCustContRep_Click(object sender, RoutedEventArgs e)
        {
            RepresentativeWindow repWindow = new RepresentativeWindow();
            repWindow.isEdit = true;
            repWindow.ShowDialog();
            validateCustomerDetailsTextBoxes();
        }

        private void delBtnCustContRep_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this Representative record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                int selectIndex = custContactDg.SelectedIndex;
                MainVM.CustRepresentatives.RemoveAt(selectIndex);
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }
        private void custDataToDb()
        {
            var dbCon = DBConnection.Instance();
            string[] proc = { "", "", "", "" };

            if (!isEdit)
            {
                proc[0] = "INSERT_COMPANY";
            }
            else
            {
                proc[0] = "UPDATE_COMPANY";
            }
            try
            {
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(proc[0], conn);
                    //INSERT NEW CUSTOMER TO DB;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyName", custCompanyNameTb.Text);
                    cmd.Parameters["@companyName"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@addInfo", custAddInfoTb.Text);
                    cmd.Parameters["@addInfo"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@address", custAddressTb.Text);
                    cmd.Parameters["@address"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@city", custCityTb.Text);
                    cmd.Parameters["@city"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@province", custProvinceCb.SelectedValue);
                    cmd.Parameters["@province"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@compType", compType);
                    cmd.Parameters["@compType"].Direction = ParameterDirection.Input;
                    if (!isEdit)
                    {
                        cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                        cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        compID = cmd.Parameters["@insertedid"].Value.ToString();
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@compID", compID);
                        cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    foreach (Contact cont in MainVM.CustContacts)
                    {
                        if (cont.TableID == null)
                        {
                            cmd = new MySqlCommand("INSERT_COMPANY_CONT", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@contactType", cont.ContactTypeID);
                            cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@contactDetail", cont.ContactDetails);
                            cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@compID", compID);
                            cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE_COMPANY_CONT", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@contactType", cont.ContactTypeID);
                            cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@contactDetail", cont.ContactDetails);
                            cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@tableID", cont.TableID);
                            cmd.Parameters["@tableID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }


                    }
                    //INSERT REPRESENTATIVE TO DB;
                    string repID = "";
                    foreach (Representative rep in MainVM.CustRepresentatives)
                    {
                        if (rep.RepresentativeID == null)
                        {
                            cmd = new MySqlCommand("INSERT_REP", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@repLName", rep.RepLastName);
                            cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repFName", rep.RepFirstName);
                            cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repMName", rep.RepMiddleName);
                            cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@custID", compID);
                            cmd.Parameters["@custID"].Direction = ParameterDirection.Input;
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            repID = cmd.Parameters["@insertedid"].Value.ToString();
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE_REP", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@repLName", rep.RepLastName);
                            cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repFName", rep.RepFirstName);
                            cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repMName", rep.RepMiddleName);
                            cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repID", rep.RepresentativeID);
                            cmd.Parameters["@repID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        //INSERT CONTACTS OF REPRESENTATIVE TO DB;
                        foreach (Contact repcont in rep.ContactsOfRep)
                        {
                            if (repcont.TableID == null)
                            {
                                cmd = new MySqlCommand("INSERT_REP_CONT", conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@contactType", repcont.ContactTypeID);
                                cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@contactDetail", repcont.ContactDetails);
                                cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@repID", repID);
                                cmd.Parameters["@repId"].Direction = ParameterDirection.Input;
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {

                                cmd = new MySqlCommand("UPDATE_REP_CONT", conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@contactType", repcont.ContactTypeID);
                                cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@contactDetail", repcont.ContactDetails);
                                cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@tableID", repcont.TableID);
                                cmd.Parameters["@tableID"].Direction = ParameterDirection.Input;
                                cmd.ExecuteNonQuery();
                            }


                            if (!isEdit)
                            {

                            }
                            else
                            {

                            }
                        }

                    }
                    clearCompanyDetailsGrid();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
        private void clearCompanyDetailsGrid()
        {
            
            MainVM.CustContacts.Clear();
            MainVM.RepContacts.Clear();
            MainVM.CustRepresentatives.Clear();
            MainVM.Name = "";
            MainVM.Address = "";
            MainVM.City = "";
            MainVM.Number = "";
            MainVM.Email = "";
            MainVM.CityName = "";
            MainVM.EmailAddress = "";
            MainVM.PhoneNumber = "";
            MainVM.MobileNumber = "";
            Validation.ClearInvalid((custCompanyNameTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((custAddressTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((custCityTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
            isEdit = false;
            if (compType == 0)
            {
                compType = 0;
                manageCustomerGrid.Visibility = Visibility.Visible;
                companyDetailsGrid.Visibility = Visibility.Hidden;
                setManageCustomerGridControls();
            }
            else if (compType == 1)
            {
                compType = 0;
                manageSupplierGrid.Visibility = Visibility.Visible;
                companyDetailsGrid.Visibility = Visibility.Hidden;
                setManageSupplierGridControls();
            }
        }
        private void validateCustomerDetailsTextBoxes()
        {
            if (String.IsNullOrWhiteSpace(custCompanyNameTb.Text) || String.IsNullOrWhiteSpace(custAddressTb.Text) || String.IsNullOrWhiteSpace(custCityTb.Text) || custProvinceCb.SelectedIndex == -1 || MainVM.CustContacts.Count == 0 || MainVM.CustRepresentatives.Count == 0)
            {
                saveCustBtn.IsEnabled = false;
            }
            else
            {
                saveCustBtn.IsEnabled = true;
            }
        }



        private void loadCustSuppDetails()
        {
            
            try
            {
                var dbCon = DBConnection.Instance();
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();
                    compID = MainVM.SelectedCustomer.CompanyID;
                    custCompanyNameTb.Text = MainVM.SelectedCustomer.CompanyName;
                    custAddInfoTb.Text = MainVM.SelectedCustomer.CompanyDesc;
                    custAddressTb.Text = MainVM.SelectedCustomer.CompanyAddress;
                    custCityTb.Text = MainVM.SelectedCustomer.CompanyCity;
                    custProvinceCb.SelectedIndex = int.Parse(MainVM.SelectedCustomer.CompanyProvinceID)-1;
                    string query = "SELECT representativeID," +
                        "repTitle," +
                        "repLName," +
                        "repFName," +
                        "repMInitial " +
                        "FROM representative_t " +
                        "WHERE companyID = '" + compID + "'; ";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, conn);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        MainVM.CustRepresentatives.Add(new Representative() { RepFirstName = dr["repFName"].ToString(), RepLastName = dr["RepLName"].ToString(), RepMiddleName = dr["RepMInitial"].ToString(), RepresentativeID = dr["representativeID"].ToString() });
                        query = "SELECT rc.tableID," +
                        "rc.repID," +
                        "rc.contactTypeID," +
                        "rc.contactValue," +
                        "cont.contactType" +
                        " FROM rep_t_contact_t rc" +
                        " JOIN contacts_t cont" +
                        " ON cont.contactTypeID = rc.contactTypeID" +
                        " WHERE rc.repID = '" + dr["representativeID"].ToString() + "';";
                        dataAdapter = dbCon.selectQuery(query, conn);
                        fromDb = new DataSet();
                        fromDbTable = new DataTable();
                        dataAdapter.Fill(fromDb, "t");
                        fromDbTable = fromDb.Tables["t"];
                        MainVM.SelectedRepresentative = MainVM.CustRepresentatives.Last();
                        foreach (DataRow dr1 in fromDbTable.Rows)
                        {
                            MainVM.SelectedRepresentative.ContactsOfRep.Add(new Contact() { TableID = dr1["tableID"].ToString(), ContactDetails = dr1["contactValue"].ToString(), ContactType = dr1["contactType"].ToString(), ContactTypeID = dr1["contactTypeID"].ToString() });
                        }
                    }

                    query = "SELECT cs.tableID," +
                        "cs.compID," +
                        "cs.contactTypeID," +
                        "cs.contactValue," +
                        "cont.contactType" +
                        " FROM cust_supp_t_contact_t cs" +
                        " JOIN contacts_t cont" +
                        " ON cont.contactTypeID = cs.contactTypeID" +
                        " WHERE cs.compID = '" + compID + "';";
                    dataAdapter = dbCon.selectQuery(query, conn);
                    fromDb = new DataSet();
                    fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        MainVM.CustContacts.Add(new Contact() { TableID = dr["tableID"].ToString(), ContactDetails = dr["contactValue"].ToString(), ContactType = dr["contactType"].ToString(), ContactTypeID = dr["contactTypeID"].ToString() });
                    }

                }
                validateCustomerDetailsTextBoxes();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        private void clearContactsBoxes()
        {
            contactDetailsPhoneTb.Text = "";
            contactDetailsEmailTb.Text = "";
            contactDetailsMobileTb.Text = "";
            contactDetailsPhoneTb1.Text = "";
            contactDetailsEmailTb1.Text = "";
            contactDetailsMobileTb1.Text = "";
            Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsPhoneTb1).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsEmailTb1).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsMobileTb1).GetBindingExpression(TextBox.TextProperty));
        }

        

        private void validateEmployeeTextBoxes()
        {
            if (empType == 0)
            {
                if (!isEdit)
                {
                    if (String.IsNullOrWhiteSpace(empFirstNameTb.Text) || String.IsNullOrWhiteSpace(empLastNameTb.Text) || String.IsNullOrWhiteSpace(empCityTb.Text) || String.IsNullOrWhiteSpace(empUserNameTb.Text) || String.IsNullOrWhiteSpace(empPasswordTb.Password) || empPostionCb.SelectedIndex == -1 || empProvinceCb.SelectedIndex == -1)
                    {
                        saveEmpBtn.IsEnabled = false;
                    }
                    else
                    {
                        saveEmpBtn.IsEnabled = true;
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(empFirstNameTb.Text) || String.IsNullOrWhiteSpace(empLastNameTb.Text) || String.IsNullOrWhiteSpace(empCityTb.Text) || empPostionCb.SelectedIndex == -1 || empProvinceCb.SelectedIndex == -1)
                    {
                        saveEmpBtn.IsEnabled = false;
                    }
                    else
                    {
                        saveEmpBtn.IsEnabled = true;
                    }
                }
                
            }
            else
            {
                if (String.IsNullOrWhiteSpace(empFirstNameTb.Text) || String.IsNullOrWhiteSpace(empLastNameTb.Text) || String.IsNullOrWhiteSpace(empMiddleInitialTb.Text) || String.IsNullOrWhiteSpace(empCityTb.Text) || empJobCb.SelectedIndex == -1 || empProvinceCb.SelectedIndex == -1 || String.IsNullOrWhiteSpace(empDateStarted.Text) || String.IsNullOrWhiteSpace(empDateEnded.Text))
                {
                    saveEmpBtn.IsEnabled = false;
                }
                else
                {
                    saveEmpBtn.IsEnabled = true;
                }
            }
            
        }

        private void empFirstNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empFirstNameTb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void empMiddleInitialTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empMiddleInitialTb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void empLastNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empLastNameTb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void empAddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empAddressTb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void empCityTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empCityTb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void empProvinceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(empProvinceCb) == true)
                saveEmpBtn.IsEnabled = false;
            else validateEmployeeTextBoxes();
        }

        private void contactTypeCb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactTypeCb1.SelectedIndex == 0)
            {
                contactDetailsMobileTb1.IsEnabled = false;
                contactDetailsPhoneTb1.IsEnabled = false;
                contactDetailsEmailTb1.IsEnabled = false;
                clearContactsBoxes();
            }
            else if (contactTypeCb1.SelectedIndex == 1)
            {
                contactDetailsEmailTb1.Visibility = Visibility.Visible;
                contactDetailsMobileTb1.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb1.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb1.IsEnabled = false;
                contactDetailsPhoneTb1.IsEnabled = false;
                contactDetailsEmailTb1.IsEnabled = true;
                clearContactsBoxes();
            }
            else if (contactTypeCb1.SelectedIndex == 2)
            {
                contactDetailsEmailTb1.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb1.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb1.Visibility = Visibility.Visible;
                contactDetailsMobileTb1.IsEnabled = false;
                contactDetailsPhoneTb1.IsEnabled = true;
                contactDetailsEmailTb1.IsEnabled = false;
                clearContactsBoxes();

            }
            else if (contactTypeCb1.SelectedIndex == 3)
            {
                contactDetailsEmailTb1.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb1.Visibility = Visibility.Visible;
                contactDetailsPhoneTb1.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb1.IsEnabled = true;
                contactDetailsPhoneTb1.IsEnabled = false;
                contactDetailsEmailTb1.IsEnabled = false;
                clearContactsBoxes();
            }
        }

        private void contactDetailsEmailTb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb1) == true)
            {
                saveEmpBtn.IsEnabled = false;
                saveEmpContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsEmailTb1.Text;
                saveEmpContactBtn.IsEnabled = true;
                validateEmployeeTextBoxes();
            }

        }

        private void contactDetailsPhoneTb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb1) == true)
            {
                saveEmpBtn.IsEnabled = false;
                saveEmpContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsPhoneTb1.Text;
                saveEmpContactBtn.IsEnabled = true;
                validateEmployeeTextBoxes();
            }
        }

        private void contactDetailsMobileTb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb1) == true)
            {
                saveEmpBtn.IsEnabled = false;
                saveEmpContactBtn.IsEnabled = false;
            }
            else
            {
                contactDetail = contactDetailsMobileTb1.Text;
                saveEmpContactBtn.IsEnabled = true;
                validateEmployeeTextBoxes();
            }
        }

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
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void empContactsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addNewEmpContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb1) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb1) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb1) == true))
            {
                if (contactTypeCb1.SelectedIndex != 0)
                {
                    if (contactTypeCb1.SelectedIndex == 1)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsEmailTb1.Text))
                        {

                            MainVM.EmpContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb1.SelectedIndex == 2)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsPhoneTb1.Text))
                        {

                            MainVM.EmpContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb1.SelectedIndex == 3)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsMobileTb1.Text))
                        {
                            MainVM.EmpContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a contact type.");
                }
            }
            else
            {
                MessageBox.Show("Please resolve the error first.");
            }
        }

        private void editEmpContBtn_Click(object sender, RoutedEventArgs e)
        {
            if (empContactsDg.SelectedItem != null)
            {
                contactTypeCb1.SelectedIndex = int.Parse(MainVM.SelectedEmpContact.ContactTypeID);
                if (MainVM.SelectedEmpContact.ContactTypeID.Equals("1"))
                {
                    contactDetailsEmailTb1.Text = MainVM.SelectedEmpContact.ContactDetails;
                    
                }
                else if (MainVM.SelectedEmpContact.ContactTypeID.Equals("2"))
                {
                    contactDetailsPhoneTb1.Text = MainVM.SelectedEmpContact.ContactDetails;
                }
                else if (MainVM.SelectedEmpContact.ContactTypeID.Equals("3"))
                {
                    contactDetailsMobileTb1.Text = MainVM.SelectedEmpContact.ContactDetails;
                }
                addNewEmpContactBtn.Visibility = Visibility.Hidden;
                cancelEmpContactBtn.Visibility = Visibility.Visible;
                saveEmpContactBtn.Visibility = Visibility.Visible;
            }
        }

        private void delEmpContBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this contact?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                MainVM.EmpContacts.RemoveAt(empContactsDg.SelectedIndex);
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void saveEmpContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true))
            {
                if (contactTypeCb1.SelectedIndex != 0)
                {
                    if (contactTypeCb1.SelectedIndex == 1)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsEmailTb1.Text))
                        {

                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb1.SelectedIndex == 2)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsPhoneTb1.Text))
                        {

                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                    if (contactTypeCb1.SelectedIndex == 3)
                    {
                        if (!String.IsNullOrWhiteSpace(contactDetailsMobileTb1.Text))
                        {
                            MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb1.SelectedIndex.ToString(), ContactType = contactTypeCb1.SelectedValue.ToString(), ContactDetails = contactDetail });
                            clearContactsBoxes();
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a contact type.");
                }
            }
            else
            {
                MessageBox.Show("Please resolve the error first.");
            }
        }

        public void clearContactTextbox()
        {
            contactTypeCb1.SelectedIndex = 0;
            MainVM.SelectedEmpContact.ContactDetails = contactDetail;
            validateEmployeeTextBoxes();
            clearContactsBoxes();
            cancelEmpContactBtn.Visibility = Visibility.Hidden;
            saveEmpContactBtn.Visibility = Visibility.Hidden;
            addNewEmpContactBtn.Visibility = Visibility.Visible;
        }

        private void cancelEmpContactBtn_Click(object sender, RoutedEventArgs e)
        {
            contactTypeCb1.SelectedIndex = 0;
            cancelEmpContactBtn.Visibility = Visibility.Hidden;
            saveEmpContactBtn.Visibility = Visibility.Hidden;
            addNewEmpContactBtn.Visibility = Visibility.Visible;
            clearContactsBoxes();
        }

        private void empJobCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validateEmployeeTextBoxes();
        }

        private void dateStarted_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateEmployeeTextBoxes();
        }

        private void dateEnded_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateEmployeeTextBoxes();
        }

        private void empPostionCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (empPostionCb.SelectedIndex>0)
            {
                validateEmployeeTextBoxes();
            }
            
        }

        private void empUserNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool usernameExist = false;
            TextBox txtBox = sender as TextBox;
            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(txtBox, TextBox.TextProperty);

            BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(txtBox, TextBox.TextProperty);
            foreach (Employee emp in MainVM.Employees)
            {
                if (emp.EmpUserName.Equals(empUserNameTb.Text))
                {
                    usernameExist = true;
                }
            }
            if (usernameExist)
            {
                

                ValidationError validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                validationError.ErrorContent = "The username is already exist.";
                Validation.MarkInvalid(bindingExpressionBase, validationError);
            }
            else
            {
                Validation.ClearInvalid(bindingExpressionBase);
                validateEmployeeTextBoxes();

            }


        }

        private void empPasswordTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            validateEmployeeTextBoxes();
        }



        private void saveEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to save this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                empDataToDb();

            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void cancelEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            clearEmployeeDetailsGrid();
        }

        

        private string empId;
        private int empType = 0;
        private void empDataToDb()
        {
            var dbCon = DBConnection.Instance();
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
                    
                    SecureString passwordsalt = empPasswordTb.SecurePassword;
                    foreach(Char c in "$w0rdf!$h")
                    {
                        passwordsalt.AppendChar(c);
                    }
                    passwordsalt.MakeReadOnly();

                    cmd.Parameters.AddWithValue("@upassword", SecureStringToString(passwordsalt));
                    cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@positionID", empPostionCb.SelectedValue);
                    cmd.Parameters["@positionID"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@empType", empType);
                    cmd.Parameters["@empType"].Direction = ParameterDirection.Input;

                    if (empType == 1)
                    {
                        cmd.Parameters.AddWithValue("@jobID", empJobCb);
                        cmd.Parameters["@jobID"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@dateFrom", empDateStarted);
                        cmd.Parameters["@dateFrom"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@dateTo", empDateEnded);
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
                        cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                        cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        empId = cmd.Parameters["@insertedid"].Value.ToString();
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@empID", compID);
                        cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    foreach (Contact cont in MainVM.EmpContacts)
                    {
                        if (cont.TableID == null)
                        {
                            cmd = new MySqlCommand("INSERT_EMPLOYEE_CONT", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@contactType", cont.ContactTypeID);
                            cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@contactDetail", cont.ContactDetails);
                            cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@empID", empId);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE_EMPLOYEE_CONT", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@contactType", cont.ContactTypeID);
                            cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@contactDetail", cont.ContactDetails);
                            cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@tableID", cont.TableID);
                            cmd.Parameters["@tableID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (!isEdit)
                    {
                        cmd = new MySqlCommand("INSERT_EMPLOYEE_PIC_T", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@picBLOB", picdata);
                        cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@sigBLOB", null);
                        cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@empID", empId);
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
                        cmd.Parameters.AddWithValue("@empID", empId);
                        cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    clearEmployeeDetailsGrid();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        private void clearEmployeeDetailsGrid()
        {
            isEdit = false;
            MainVM.EmpContacts.Clear();
            empFirstNameTb.Clear();
            empLastNameTb.Clear();
            empMiddleInitialTb.Clear();
            empAddressTb.Clear();
            empCityTb.Clear();
            empProvinceCb.SelectedIndex = 0;
            empPostionCb.SelectedIndex = 0;
            empUserNameTb.Clear();
            empPasswordTb.Clear();
            empImage.Source = null;
            if (empType == 0)
            {
                empType = 0;
                manageEmployeeGrid.Visibility = Visibility.Visible;
                employeeDetailsGrid.Visibility = Visibility.Hidden;
                setManageEmployeeGridControls();
            }
            else if (empType == 1)
            {
                empType= 0;
                manageContractorGrid.Visibility = Visibility.Visible;
                employeeDetailsGrid.Visibility = Visibility.Hidden;
                setManageContractorGridControls();
            }
        }
        private void loadEmpContDetails()
        {

            try
            {
                var dbCon = DBConnection.Instance();
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();
                    empFirstNameTb.Text = MainVM.SelectedEmployee.EmpFname;
                    empMiddleInitialTb.Text = MainVM.SelectedEmployee.EmpMiddleInitial;
                    empLastNameTb.Text = MainVM.SelectedEmployee.EmpLName;
                    empAddressTb.Text = MainVM.SelectedEmployee.EmpAddress;
                    empCityTb.Text = MainVM.SelectedEmployee.EmpCity;
                    empProvinceCb.SelectedIndex = int.Parse(MainVM.SelectedEmployee.EmpProvinceID);
                    empPostionCb.SelectedIndex = int.Parse(MainVM.SelectedEmployee.PositionID);
                    employeeOnlyGrid.Visibility = Visibility.Hidden;
                    if (MainVM.SelectedEmployee.EmpPic!=null)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(MainVM.SelectedEmployee.EmpPic))
                        {
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.StreamSource = ms;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();
                            // Assign the Source property of your image
                            empImage.Source = imageSource;
                        }
                    }
                    string query = "SELECT cs.tableID," +
                        "cs.empId," +
                        "cs.contactTypeID," +
                        "cs.contactValue," +
                        "cont.contactType" +
                        " FROM employee_t_contact_t cs" +
                        " JOIN contacts_t cont" +
                        " ON cont.contactTypeID = cs.contactTypeID" +
                        " WHERE cs.compID = '" + MainVM.SelectedEmployee.EmpID + "';";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, conn);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        MainVM.EmpContacts.Add(new Contact() { TableID = dr["tableID"].ToString(), ContactDetails = dr["contactValue"].ToString(), ContactType = dr["contactType"].ToString(), ContactTypeID = dr["contactTypeID"].ToString() });
                    }

                }
                validateEmployeeTextBoxes();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
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

        

        private void manageSupplierSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.Customers.Where(x => x.CompanyName.ToLower().Contains(manageSupplierSearchTb.Text.ToLower()));
            var observable = new ObservableCollection<Customer>(linqResults);
            manageSupplierDataGrid.ItemsSource = observable;
        }

        private void manageSupplierSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(manageSupplierSearchTb.Text.Count() == 0)
            {
                manageSupplierDataGrid.ItemsSource = MainVM.Customers;
            }
        }

        
    }
    internal class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private string _lineNo;
        public string lineNo
        {
            get { return _lineNo; }
            set { SetField(ref _lineNo, value, "Line No."); }
        }
        private string _itemCode;
        public string itemCode
        {
            get { return _itemCode; }
            set { SetField(ref _itemCode, value, "Item Code"); }
        }
        private string _desc;
        public string desc
        {
            get { return _desc; }
            set { SetField(ref _desc, value, "Description"); }
        }
        private string _unit;
        public string unit
        {
            get { return _unit; }
            set { SetField(ref _unit, value, "Unit"); }
        }
        private int _qty;
        public int qty
        {
            get { return _qty; }
            set { SetField(ref _qty, value, "Quantity"); }
        }
        private decimal _unitprice;
        public decimal unitPrice
        {
            get { return _unitprice; }
            set { SetField(ref _unitprice, value, "Unit Price"); }
        }
        private decimal _totalAmount;
        public decimal totalAmount
        {
            get { return _totalAmount; }
            set { SetField(ref _totalAmount, value, "Total Amount"); }
        }
    }

    

}
