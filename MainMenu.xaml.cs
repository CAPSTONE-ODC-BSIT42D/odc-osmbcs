using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
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

        public MainMenu()
        {
            InitializeComponent();
            this.DataContext = MainVM;
        }
        public static MainViewModel MainVM = new MainViewModel();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < containerGrid.Children.Count; x++)
            {
                containerGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            contactTypeCb.SelectedIndex = 0;
            contactDetailsMobileTb.IsEnabled = false;
            contactDetailsPhoneTb.IsEnabled = false;
            contactDetailsEmailTb.IsEnabled = false;
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (!visual.IsDescendantOf(saleSubMenuGrid) && !visual.IsDescendantOf(manageSubMenugrid))
                saleSubMenuGrid.Visibility = Visibility.Collapsed;
            manageSubMenugrid.Visibility = Visibility.Collapsed;
            if (!visual.IsDescendantOf(manageCustomeDataGrid))
            {
                manageCustomeDataGrid.SelectedIndex = -1;
                manageCustomeDataGrid.Columns[manageCustomeDataGrid.Columns.IndexOf(columnEditCustBtn)].Visibility = Visibility.Hidden;
                manageCustomeDataGrid.Columns[manageCustomeDataGrid.Columns.IndexOf(columnDeleteCustBtn)].Visibility = Visibility.Hidden;
            }
            if (!visual.IsDescendantOf(manageEmployeeDataGrid))
            {
                if (manageEmployeeDataGrid.SelectedItems.Count > 0)
                {
                    manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnEditBtnEmp)].Visibility = Visibility.Hidden;
                    manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnDelBtnEmp)].Visibility = Visibility.Hidden;
                }
            }
            if (!visual.IsDescendantOf(manageSupplierDataGrid))
            {
                if (manageSupplierDataGrid.SelectedItems.Count > 0)
                {
                    manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnEditSuppBtn)].Visibility = Visibility.Hidden;
                    manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnDeleteSuppBtn)].Visibility = Visibility.Hidden;
                }
            }
            if (!visual.IsDescendantOf(manageCustomeDataGrid))
            {
                if (manageCustomeDataGrid.SelectedItems.Count > 0)
                {
                    manageCustomeDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnEditBtnCont)].Visibility = Visibility.Hidden;
                    manageCustomeDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnDelBtnCont)].Visibility = Visibility.Hidden;
                }
            }
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
            setTransControlValues();

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
        private void transQuoteAddBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
            {
                transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
            }
            addRequestionGrid.Visibility = Visibility.Visible;
            setAddTransControlValues();
        }
        private void transRequestBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel this transaction?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                for (int x = 1; x < transactionQuotationsGrid.Children.Count; x++)
                {
                    transactionQuotationsGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                quotationsGridHome.Visibility = Visibility.Visible;
            }
            else if (result == MessageBoxResult.No)
            {

            }
        }

        private void setAddTransControlValues()
        {
        }

        private void setTransControlValues()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM cust_supp_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }

        }

        private void custCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = query = "SELECT CONCAT(custRepLName,' ,',custRepFname) AS custRepFullName, custRepID FROM CUSTOMER_REP_T WHERE custID = '" + custCb.SelectedValue + "';";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custRepCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void addCustBtn_Click(object sender, RoutedEventArgs e)
        {
            addCustomer addCust = new addCustomer();
            addCust.ShowDialog();
            setTransControlValues();
        }
        private void addCustRepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(custCb.SelectedIndex < 0))
            {
                RepresentativeWindow addRep = new RepresentativeWindow();
                addRep.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Select a customer first.");
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
            MessageBoxResult result = MessageBox.Show("Do you want to cancel this transaction?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
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
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, CONCAT(cs.companyAddress,', ',cs.companyCity,', ',p.locProvince) AS custLocation " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                manageCustomeDataGrid.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void setEditCustomerControls(string id)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo,cs.companyAddress ,cs.companyCity, cs.companyProvinceID" +
                    "FROM cust_supp_t cs  " +
                    "WHERE cs.companyID = '" + id + "' " +
                    "AND companyType = 0;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int locProvId = Int32.Parse(dr["companyProvinceID"].ToString());
                    custProvinceCb.SelectedIndex = locProvId - 1;
                    Name = dr["companyName"].ToString();
                    custAddInfoTb.Text = dr["companyAddInfo"].ToString();
                    MainVM.City = dr["companyCity"].ToString();
                    MainVM.Address = dr["companyAddress"].ToString();
                }
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
        }


        private void btnEditCust_Click(object sender, RoutedEventArgs e)
        {
            
            manageCustomerHomeGrid.Visibility = Visibility.Hidden;
            customerDetailsGrid.Visibility = Visibility.Visible;
            customerDetailsGrid.IsEnabled = true;
            string id = (manageCustomeDataGrid.Columns[0].GetCellContent(manageCustomeDataGrid.SelectedItem) as TextBlock).Text;
            setEditCustomerControls(id);
        }

        private void btnDeleteCust_Click(object sender, RoutedEventArgs e)
        {
            if (manageCustomeDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageCustomeDataGrid.Columns[0].GetCellContent(manageCustomeDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you want to delete this customer?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `customer_t` SET `isDeleted`= 1 WHERE custID = '"+id+"';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Successfully deleted.");
                            setManageCustomerGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.No)
                {
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
            
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(manageCustomeDataGrid))
            {
                if (manageCustomeDataGrid.SelectedItems.Count > 0)
                {
                    manageCustomeDataGrid.Columns[manageCustomeDataGrid.Columns.IndexOf(columnEditCustBtn)].Visibility = Visibility.Visible;
                    manageCustomeDataGrid.Columns[manageCustomeDataGrid.Columns.IndexOf(columnDeleteCustBtn)].Visibility = Visibility.Visible;
                }
            }
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
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(manageEmployeeDataGrid))
            {
                if (manageEmployeeDataGrid.SelectedItems.Count > 0)
                {
                    manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnEditBtnEmp)].Visibility = Visibility.Visible;
                    manageEmployeeDataGrid.Columns[manageEmployeeDataGrid.Columns.IndexOf(columnDelBtnEmp)].Visibility = Visibility.Visible;
                }
            }
        }

        private void manageEmployeeDataGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            setManageEmployeeGridControls();
        }

        private void setManageEmployeeGridControls()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                //dataAdapter.Fill(fromDb, "t");
                //manageEmployeeDataGrid.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
        }

        private void manageEmployeeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            addEmployee addEmployee = new addEmployee();
            addEmployee.ShowDialog();
            setManageEmployeeGridControls();
        }

        private void btnEditEmp_Click(object sender, RoutedEventArgs e)
        {
            if (manageEmployeeDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageEmployeeDataGrid.Columns[0].GetCellContent(manageEmployeeDataGrid.SelectedItem) as TextBlock).Text;
                editEmployee editEmployee = new editEmployee(id);
                editEmployee.ShowDialog();
                setManageEmployeeGridControls();
            }
        }

        private void btnDeleteEmp_Click(object sender, RoutedEventArgs e)
        {
            if (manageEmployeeDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageEmployeeDataGrid.Columns[0].GetCellContent(manageEmployeeDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you want to delete this customer?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `employee_t` SET `isDeleted`= 1 WHERE empID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Successfully deleted.");
                            setManageEmployeeGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.No)
                {
                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
                setManageCustomerGridControls();
            }
        }

        private void employeeManageMenuBtn_Click(object sender, RoutedEventArgs e)
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
            manageEmployeeGrid.Visibility = Visibility.Visible;
        }

        /*-----------------END OF MANAGE EMPLOYEE-------------------*/
        /*
        /*
        /*
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
                string query = "SELECT cs.companyID, cs.companyName, cs.companyAddInfo, CONCAT(cs.companyAddress,', ',cs.companyCity,', ',p.locProvince) AS custLocation " +
                    "FROM cust_supp_t cs  " +
                    "JOIN provinces_t p ON cs.companyProvinceID = p.locProvinceId " +
                    "WHERE isDeleted = 0 AND companyType = 1;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                manageSupplierDataGrid.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void btnEditSupp_Click(object sender, RoutedEventArgs e)
        {
            if (manageSupplierDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageSupplierDataGrid.Columns[0].GetCellContent(manageSupplierDataGrid.SelectedItem) as TextBlock).Text;
                editSupplier editSupplier = new editSupplier(id);
                editSupplier.ShowDialog();
                setManageSupplierGridControls();
            }
            
        }

        private void btnDeleteSupp_Click(object sender, RoutedEventArgs e)
        {
            if (manageSupplierDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageSupplierDataGrid.Columns[0].GetCellContent(manageSupplierDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you want to delete this supplier?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `supplier_t` SET `isDeleted`= 1 WHERE suppID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Successfully deleted.");
                            setManageSupplierGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.No)
                {
                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
                setManageSupplierGridControls();
            }
        }

        private void manageSupplierAddbtn_Click(object sender, RoutedEventArgs e)
        {
            manageSupplierHome.Visibility = Visibility.Hidden;
            companyDetailsGrid.Visibility = Visibility.Visible;
            companyDetailsHeader.Content = "Manage Supplier - New Supplier";
        }

        private void manageSupplierDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(manageSupplierDataGrid))
            {
                if (manageSupplierDataGrid.SelectedItems.Count > 0)
                {
                    manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnEditSuppBtn)].Visibility = Visibility.Visible;
                    manageSupplierDataGrid.Columns[manageSupplierDataGrid.Columns.IndexOf(columnDeleteSuppBtn)].Visibility = Visibility.Visible;
                }
            }
        }
        
        /*-----------------END OF MANAGE SUPPLIER-------------------*/
        /*
        /*
        /*
        /*
        /*
        /*-----------------MANAGE UTILITIES-------------------------*/

        private void utilitiesManageMenuBtn_Click(object sender, RoutedEventArgs e)
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
            manageMiscGrid.Visibility = Visibility.Visible;
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





        private void appSettingsManageMenuBtn_Click(object sender, RoutedEventArgs e)
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
            manageMiscGrid.Visibility = Visibility.Visible;
        }


        /*-----------------END OF MANAGE UTILITIES-------------------*/
        /*
        /*
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
        }

        private void btnEditCont_Click(object sender, RoutedEventArgs e)
        {
            if (manageContractorDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageContractorDataGrid.Columns[0].GetCellContent(manageContractorDataGrid.SelectedItem) as TextBlock).Text;
                editContractor editContractor = new editContractor(id);
                editContractor.ShowDialog();
                setManageContractorGridControls();
            }
        }

        private void btnDeleteCont_Click(object sender, RoutedEventArgs e)
        {
            if (manageContractorDataGrid.SelectedItems.Count > 0)
            {
                String id = (manageContractorDataGrid.Columns[0].GetCellContent(manageContractorDataGrid.SelectedItem) as TextBlock).Text;
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you want to delete this Contractor?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        string query = "UPDATE `contractor_t` SET `isDeleted`= 1 WHERE contID = '" + id + "';";
                        if (dbCon.insertQuery(query, conn))
                        {
                            MessageBox.Show("Successfully deleted.");
                            setManageContractorGridControls();
                        }
                    }

                }
                else if (result == MessageBoxResult.No)
                {
                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
        }

        private void manageContractorGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            setManageContractorGridControls();
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
        }

        private void manageContractorAddBtn_Click(object sender, RoutedEventArgs e)
        {
            addContractor addContractor = new addContractor();
            addContractor.ShowDialog();
            setManageContractorGridControls();
        }

        private void manageContractorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(manageContractorDataGrid))
            {
                if (manageContractorDataGrid.SelectedItems.Count > 0)
                {
                    manageContractorDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnEditBtnCont)].Visibility = Visibility.Visible;
                    manageContractorDataGrid.Columns[manageContractorDataGrid.Columns.IndexOf(columnDelBtnCont)].Visibility = Visibility.Visible;
                }
            }
        }

        private void servicesDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        /*-----------------END OF MANAGE CONTRACTOR-------------------*/
        /*
        /*
        /*
        /*
        /*
        /*----------------------UNIVERSAL-----------------------------*/

        /*----------------------------------------*/
        private int compType = 0;
        private void saveCustBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            MessageBoxResult result = MessageBox.Show("Do you want to save this new customer?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                
                try
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        conn.Open();
                        string proc = "INSERT_COMPANY";
                        string lastinsertedid = "";
                        MySqlCommand cmd = new MySqlCommand(proc, conn);
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
                        cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                        cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        lastinsertedid = cmd.Parameters["@insertedid"].Value.ToString();
                        foreach (Contact cont in MainVM.CustContacts)
                        {
                            proc = "INSERT_COMPANY_CONT";
                            cmd = new MySqlCommand(proc, conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@contactType", cont.ContactTypeID);
                            cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@contactDetail", cont.ContactDetails);
                            cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@compID", lastinsertedid);
                            cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        //INSERT REPRESENTATIVE TO DB;
                        foreach (Representative rep in MainVM.CustRepresentatives)
                        {
                            proc = "INSERT_REP";
                            cmd = new MySqlCommand(proc, conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@repLName", rep.RepLastName);
                            cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repFName", rep.RepFirstName);
                            cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@repMName", rep.RepMiddleName);
                            cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@custID", lastinsertedid);
                            cmd.Parameters["@custID"].Direction = ParameterDirection.Input;
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            lastinsertedid = cmd.Parameters["@insertedid"].Value.ToString();
                            //INSERT CONTACTS OF REPRESENTATIVE TO DB;
                            foreach (Contact repcont in rep.ContactsOfRep)
                            {
                                proc = "INSERT_REP_CONT";
                                cmd = new MySqlCommand(proc, conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@contactType", repcont.ContactTypeID);
                                cmd.Parameters["@contactType"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@contactDetail", repcont.ContactDetails);
                                cmd.Parameters["@contactDetail"].Direction = ParameterDirection.Input;
                                cmd.Parameters.AddWithValue("@RepID", lastinsertedid);
                                cmd.Parameters["@RepID"].Direction = ParameterDirection.Input;
                                cmd.ExecuteNonQuery();
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
            else if (result == MessageBoxResult.No)
            {
                clearCompanyDetailsGrid();
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void cancelCustBtn_Click(object sender, RoutedEventArgs e)
        {
            clearCompanyDetailsGrid();
        }

        private void clearCompanyDetailsGrid()
        {
            if (compType == 0)
            {
                manageCustomerGrid.Visibility = Visibility.Visible;
                companyDetailsGrid.Visibility = Visibility.Hidden;
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
                compType = 0;
                Validation.ClearInvalid((custCompanyNameTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((custAddressTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((custCityTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
                setManageCustomerGridControls();
            }
            else if (compType == 1)
            {
                manageSupplierGrid.Visibility = Visibility.Visible;
                companyDetailsGrid.Visibility = Visibility.Hidden;
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
                compType = 0;
                Validation.ClearInvalid((custCompanyNameTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((custAddressTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((custCityTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
                setManageSupplierGridControls();
            }
        }

        private void validateCustomerDetailsTextBoxes()
        {
            if (custCompanyNameTb.Text.Equals("") || custAddressTb.Text.Equals("") || custCityTb.Text.Equals("") || custProvinceCb.SelectedIndex == -1 || MainVM.CustContacts.Count == 0 || MainVM.CustRepresentatives.Count == 0)
            {
                saveCustBtn.IsEnabled = false;
            }
            else
            {
                saveCustBtn.IsEnabled = true;
            }
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
                    
                    MainVM.CustContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                    clearContactsBoxes();
                    contactTypeCb.SelectedIndex = 0;
                    validateCustomerDetailsTextBoxes();


                }
                else
                {
                    MessageBox.Show("Select The Type");
                }
            }
            else
                MessageBox.Show("Please resolve the error first.");

        }

        private void btnEditCustCont_Click(object sender, RoutedEventArgs e)
        {
            if (custContactDg.SelectedItem != null)
            {
                String id = (custContactDg.Columns[0].GetCellContent(custContactDg.SelectedItem) as TextBlock).Text;
                contactTypeCb.SelectedIndex = int.Parse(id);

                if (id.Equals("1"))
                {
                    MainMenu.MainVM.ContactValue = MainVM.SelectedCustContact.ContactDetails;

                }
                else if (id.Equals("2"))
                {
                    MainMenu.MainVM.ContactValue = MainVM.SelectedCustContact.ContactDetails;
                }
                else if (id.Equals("3"))
                {
                    MainMenu.MainVM.ContactValue = MainVM.SelectedCustContact.ContactDetails;
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
                    contactTypeCb.SelectedIndex = 0;
                    MainVM.SelectedCustContact.ContactDetails = contactDetail;
                    validateCustomerDetailsTextBoxes();
                    clearContactsBoxes();
                    cancelCustContactBtn.Visibility = Visibility.Hidden;
                    saveCustContactBtn.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Select The Type");
                }
            }
            else
                MessageBox.Show("Please resolve the error first.");
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
            MessageBoxResult result = MessageBox.Show("Do you want to delete this contact information?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MainVM.CustContacts.Remove(MainVM.SelectedCustContact);
            }
            else if (result == MessageBoxResult.No)
            {
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void custContactDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(custContactDg))
            {
                if (custContactDg.SelectedItems.Count > 0)
                {
                    custContactDg.Columns[custContactDg.Columns.IndexOf(columnEditBtnCustCont)].Visibility = Visibility.Visible;
                    custContactDg.Columns[custContactDg.Columns.IndexOf(columnDelBtnCustCont)].Visibility = Visibility.Visible;
                }
            }
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
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(customerRepresentativeDg))
            {
                if (customerRepresentativeDg.SelectedItems.Count > 0)
                {
                    customerRepresentativeDg.Columns[customerRepresentativeDg.Columns.IndexOf(columnEditBtnCustContRep)].Visibility = Visibility.Visible;
                    customerRepresentativeDg.Columns[customerRepresentativeDg.Columns.IndexOf(columnDelBtnCustContRep)].Visibility = Visibility.Visible;
                }
            }
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
            MessageBoxResult result = MessageBox.Show("Do you want to delete this representative?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int selectIndex = custContactDg.SelectedIndex;
                MainVM.CustRepresentatives.RemoveAt(selectIndex);
            }
            else if (result == MessageBoxResult.No)
            {
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void newCustomerGrid()
        {
            
        }

        private void newSupplierGrid()
        {
        }

        private void clearContactsBoxes()
        {
            MainMenu.MainVM.ContactValue = "";
            Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
        }

        private void transReqAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            addNewItem newItem = new addNewItem();
            newItem.ShowDialog();
            //foreach (Representative rep in MainVM.Representatives)
            //{

            //}
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
            set { SetField(ref _lineNo, value, "Line No"); }
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
