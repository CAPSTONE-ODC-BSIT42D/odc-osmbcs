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
            
        }
        
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.ucEmployee.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucCustSupp.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucCustSupp.BackToSelectCustomerClicked += selectCustomer_BtnClicked;

            this.ucServices.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucUnit.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucLocation.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;


            this.ucProduct.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucInvoice.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucSalesQuote.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucSalesQuote.ConvertToInvoice += convertToInvoice_BtnClicked;
            this.ucSalesQuote.SelectCustomer += selectCustomer_BtnClicked;

            this.ucSelectCustomer.AddNewCustomer += addNewCustomer_BtnClicked;
            this.ucSelectCustomer.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
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
            formGridBg.Visibility = Visibility.Collapsed;
            MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
            MainVM.Ldt.worker.RunWorkerAsync();
        }

        private void addNewCustomer_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            MainVM.isNewTrans = true;
            formGridBg.Visibility = Visibility.Visible;
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
                        sb.Begin(((UserControl)obj));
                    }
                }

            }
        }


        private void saveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
            }
            formGridBg.Visibility = Visibility.Collapsed;

            MainVM.isNewTrans = false;
            MainVM.StringTextBox = null;
            MainVM.DecimalTextBox = 0;
            MainVM.IntegerTextBox = 0;
            MainVM.cbItem = null;
            MainVM.isNewRecord = false;
            MainVM.isViewHome = false;
            MainVM.isEdit = false;
            MainVM.isPaymentInvoice = false;
            closeModals();
            MainVM.Ldt.worker.RunWorkerAsync();
        }

        private void selectCustomer_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucSelectCustomer"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        //sb.Begin(((UserControl)obj));
                    }
                }

            }
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void monitoringOfData()
        {

        }

        

        private void resetValueofSelectedVariables()
        {
            MainVM.SelectedCustomerSupplier = null;
            MainVM.SelectedEmployeeContractor = null;
            MainVM.SelectedAvailedItem = null;
            MainVM.SelectedSalesQuote = null;
        }

        private void dashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
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

        //private void settingsBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (var obj in settingsGridStackPanel.Children)
        //    {
        //        if (obj is Grid)
        //        {
        //            ((Grid)obj).Visibility = Visibility.Collapsed;
        //        }
        //    }
        //    if (manageEmployeeGrid.IsVisible)
        //    {
        //        employeeSettings1.Visibility = Visibility.Visible;
        //        employeeSettings2.Visibility = Visibility.Visible;
        //    }
        //    else if (manageProductListGrid.IsVisible)
        //    {
        //        productSettings1.Visibility = Visibility.Visible;
        //    }
        //    foreach (var obj in formGridBg.Children)
        //    {
        //        if(obj is Grid)
        //        {
        //            if (!((Grid)obj).Name.Equals("settingsFormGrid"))
        //            {
        //                ((Grid)obj).Visibility = Visibility.Collapsed;
        //            }
        //            else
        //            {
        //                ((Grid)obj).Visibility = Visibility.Visible;
        //            }
        //        }
        //    }
        //    formGridBg.Visibility = Visibility.Visible;

        //}

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

        private void empManageBtn_Click(object sender, RoutedEventArgs e)
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
            headerLbl.Content = "Manage Employee";
            manageEmployeeDataGrid.ItemsSource = MainVM.Employees;
            jobName.Visibility = Visibility.Collapsed;
            position.Visibility = Visibility.Visible;
            MainVM.isContractor = false;
        }

        private void contManageBtn_Click(object sender, RoutedEventArgs e)
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
            headerLbl.Content = "Manage Contractor";
            manageEmployeeDataGrid.ItemsSource = MainVM.Contractor;
            position.Visibility = Visibility.Collapsed;
            jobName.Visibility = Visibility.Visible;
            MainVM.isContractor = true;
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
            headerLbl.Content = "Manage Supplier";
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
        }

        private void servicesManageBtn_Click(object sender, RoutedEventArgs e)
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
            manageServicesGrid.Visibility = Visibility.Visible;
            headerLbl.Content = "Manage Services";
        }


        private void unitsManageBtn_Click(object sender, RoutedEventArgs e)
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
            manageUnitsGrid.Visibility = Visibility.Visible;
            headerLbl.Content = "Manage Unit";
        }

        private void locationManageBtn_Click(object sender, RoutedEventArgs e)
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
            manageLocationsGrid.Visibility = Visibility.Visible;
            headerLbl.Content = "Manage Locations";
        }

        //private void settingsManageMenuBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
        //    sb.Begin(formGridBg);
        //    foreach (var obj in formGridBg.Children)
        //    {
        //        if (obj is Grid)
        //        {
        //            if (!((Grid)obj).Name.Equals("settingsFormGrid"))
        //            {
        //                ((Grid)obj).Visibility = Visibility.Collapsed;
        //            }
        //            else
        //            {
        //                ((Grid)obj).Visibility = Visibility.Visible;
        //            }
        //        }


        //    }
        //    formGridBg.Visibility = Visibility.Visible;
        //    foreach (var obj in settingsGridStackPanel.Children)
        //    {
        //        if (obj is Grid)
        //        {
        //            ((Grid)obj).Visibility = Visibility.Collapsed;
        //        }
        //    }
        //    serviceSettings1.Visibility = Visibility.Visible;
        //    serviceSettings2.Visibility = Visibility.Visible;
        //}

        

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
                    
                }
            }
        }


        private void manageEmployeeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            
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
                        sb.Begin(((UserControl)obj));
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

        private void manageServiceAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucServices"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        sb.Begin(((UserControl)obj));
                    }
                }


            }
            formGridBg.Visibility = Visibility.Visible;
        }


        private void manageAddUnitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucUnit"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        sb.Begin(((UserControl)obj));
                    }
                }
                

            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void managemanageAddRegionBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucLocation"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        sb.Begin(((UserControl)obj));
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
                            sb.Begin(((UserControl)obj));
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
                            sb.Begin(((UserControl)obj));
                        }
                    }

                }
                ucEmployee.saveCancelGrid1.Visibility = Visibility.Visible;
                ucEmployee.editCloseGrid1.Visibility = Visibility.Collapsed;
            }
            else if (manageProductListGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
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
                            sb.Begin(((UserControl)obj));
                        }
                    }
                }
                
                ucProduct.saveCancelGrid2.Visibility = Visibility.Visible;
                ucProduct.editCloseGrid2.Visibility = Visibility.Collapsed;
            }
            else if (manageLocationsGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                foreach (var obj in formGridBg.Children)
                {

                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Name.Equals("ucLocation"))
                        {
                            ((UserControl)obj).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ((UserControl)obj).Visibility = Visibility.Visible;
                            sb.Begin(((UserControl)obj));
                        }
                    }
                }
            }

        }

        private void deleteRecordBtn_Click(object sender, RoutedEventArgs e)
        {

            if (manageCustomerGrid.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `cust_supp_t` SET `isDeleted`= 1 WHERE companyID = '" + MainVM.SelectedCustomerSupplier.CompanyID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            MainVM.Ldt.worker.RunWorkerAsync();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `emp_cont_t` SET `isDeleted`= 1 WHERE empID = '" + MainVM.SelectedEmployeeContractor.EmpID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            MainVM.Ldt.worker.RunWorkerAsync();
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
                        string query = "UPDATE `item_t` SET `isDeleted`= 1 WHERE itemCode = '" + MainVM.SelectedProduct.ID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            MainVM.Ldt.worker.RunWorkerAsync();
                        }
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
            else if (manageLocationsGrid.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `regions_t` SET `isDeleted`= 1 WHERE id = '" + MainVM.SelectedRegion.RegionID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                            MainVM.Ldt.worker.RunWorkerAsync();
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

        ////EMPLOYEE PART
        //private void addEmpPosBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (addEmpPosBtn.Content.Equals("Save"))
        //    {
        //        var dbCon = DBConnection.Instance();
        //        dbCon.DatabaseName = dbname;
        //        if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
        //        {
        //            MessageBox.Show("Employee Position must be filled");
        //        }
        //        else
        //        {
        //            if (employeePositionLb.Items.Contains(empPosNewTb.Text))
        //            {
        //                MessageBox.Show("Employee Position already exists");
        //            }
        //            if (dbCon.IsConnect())
        //            {
        //                string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + MainVM.SelectedEmpPosition.PositionID + "'";
        //                if (dbCon.insertQuery(query, dbCon.Connection))
        //                {
        //                    MessageBox.Show("Employee Poisition saved");
        //                    addEmpPosBtn.Content = "Add";
        //                    empPosNewTb.Clear();
        //                    MainVM.Ldt.worker.RunWorkerAsync();
        //                    dbCon.Close();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string strPosition = empPosNewTb.Text;
        //        if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
        //        {
        //            MessageBox.Show("Employee Position field must be filled");
        //        }
        //        else
        //        {
        //            var dbCon = DBConnection.Instance();
        //            dbCon.DatabaseName = dbname;
        //            if (employeePositionLb.Items.Contains(empPosNewTb.Text))
        //            {
        //                MessageBox.Show("Employee position already exists");
        //            }
        //            if (dbCon.IsConnect())
        //            {
        //                if (!Regex.IsMatch(strPosition, @"[a-zA-Z -]"))
        //                {
        //                    MessageBox.Show("Special characters are not accepted");
        //                    empPosNewTb.Clear();
        //                }
        //                else
        //                {
        //                    string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
        //                    if (dbCon.insertQuery(query, dbCon.Connection))
        //                    {
        //                        {
        //                            MessageBox.Show("Employee Position successfully added");
        //                            empPosNewTb.Clear();
        //                            MainVM.Ldt.worker.RunWorkerAsync();
        //                            dbCon.Close();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}


        //private void deleteEmpPosBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (employeePositionLb.SelectedItems.Count > 0)
        //    {
        //        var dbCon = DBConnection.Instance();
        //        dbCon.DatabaseName = dbname;
        //        if (dbCon.IsConnect())
        //        {
        //            try
        //            {
        //                string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + MainVM.SelectedEmpPosition.PositionID + "';";

        //                if (dbCon.insertQuery(query, dbCon.Connection))
        //                {
        //                    dbCon.Close();
        //                    MessageBox.Show("Employee position successfully deleted.");
        //                    MainVM.Ldt.worker.RunWorkerAsync();
        //                }
        //            }
        //            catch (Exception) { throw; }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select an employee position first.");
        //    }

        //}

        //private void editEmpPosBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var dbCon = DBConnection.Instance();
        //    dbCon.DatabaseName = dbname;
        //    if (dbCon.IsConnect())
        //    {
        //        if (employeePositionLb.SelectedItems.Count > 0)
        //        {
        //            empPosNewTb.Text = MainVM.SelectedEmpPosition.PositionName;
        //            addEmpPosBtn.Content = "Save";

        //        }
        //        else
        //        {
        //            MessageBox.Show("Please select an employee position first.");
        //        }
        //    }
        //    dbCon.Close();
        //}

        ////CONTRACTOR PART
        //private void addContJobBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (addContJobBtn.Content.Equals("Save"))
        //    {
        //        var dbCon = DBConnection.Instance();
        //        dbCon.DatabaseName = dbname;
        //        if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
        //        {
        //            MessageBox.Show("Contractor Job Title field must be filled");
        //        }
        //        else
        //        {
        //            if (contJobLb.Items.Contains(contNewJobTb.Text))
        //            {
        //                MessageBox.Show("Job Title already exists");
        //            }
        //            if (dbCon.IsConnect())
        //            {
        //                string query = "UPDATE `odc_db`.`job_title_t` set `jobName` = '" + contNewJobTb.Text + "' where jobID = '" + MainVM.SelectedJobTitle.JobID + "'";
        //                if (dbCon.insertQuery(query, dbCon.Connection))
        //                {
        //                    MessageBox.Show("Job Title successfully saved");
        //                    contNewJobTb.Clear();
        //                    MainVM.Ldt.worker.RunWorkerAsync();
        //                    dbCon.Close();
        //                    contNewJobTb.Clear();
        //                    addContJobBtn.Content = "Add";


        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string strJobTitle = contNewJobTb.Text;
        //        var dbCon = DBConnection.Instance();
        //        dbCon.DatabaseName = dbname;
        //        if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
        //        {
        //            MessageBox.Show("Contractor Job Title field must be field");
        //        }
        //        else
        //        {
        //            if (contJobLb.Items.Contains(contNewJobTb.Text))
        //            {
        //                MessageBox.Show("Contractor Job Title already exists");
        //            }
        //            else
        //            {
        //                if (!Regex.IsMatch(strJobTitle, @"[a-zA-Z -]"))
        //                {
        //                    MessageBox.Show("Special Characters are not accepted");
        //                    contNewJobTb.Clear();
        //                }
        //                else
        //                {
        //                    if (dbCon.IsConnect())
        //                    {
        //                        string query = "INSERT INTO `odc_db`.`job_title_t` (`jobName`) VALUES('" + contNewJobTb.Text + "')";
        //                        if (dbCon.insertQuery(query, dbCon.Connection))
        //                        {
        //                            MessageBox.Show("Contractor Job Title successfully added");
        //                            contNewJobTb.Clear();
        //                            MainVM.Ldt.worker.RunWorkerAsync();
        //                            dbCon.Close();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        //private void deleteContJobBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (contJobLb.SelectedItems.Count > 0)
        //    {
        //        var dbCon = DBConnection.Instance();
        //        dbCon.DatabaseName = dbname;
        //        if (dbCon.IsConnect())
        //        {
        //            try
        //            {
        //                string query = "DELETE FROM `odc_db`.`job_title_t` WHERE `JobID`='" + MainVM.SelectedJobTitle.JobID + "';";

        //                if (dbCon.insertQuery(query, dbCon.Connection))
        //                {
        //                    dbCon.Close();
        //                    MessageBox.Show("Job Position successfully deleted.");
        //                    MainVM.Ldt.worker.RunWorkerAsync();
        //                }
        //            }
        //            catch (Exception) { throw; }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select a Job Position first.");
        //    }
        //}

        //private void editContJobBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var dbCon = DBConnection.Instance();
        //    dbCon.DatabaseName = dbname;

        //    if (contJobLb.SelectedItems.Count > 0)
        //    {
        //        contNewJobTb.Text = MainVM.SelectedJobTitle.JobName;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a record first.");
        //    }
        //    dbCon.Close();
        //    addContJobBtn.Content = "Save";
        //}


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
                        MainVM.Ldt.worker.RunWorkerAsync();
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
                                    MainVM.Ldt.worker.RunWorkerAsync();
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
            

        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = true;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucServices"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        sb.Begin(((UserControl)obj));
                    }
                }


            }
            formGridBg.Visibility = Visibility.Visible;
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
            MainVM.Ldt.worker.RunWorkerAsync();
        }

        private void btnEditUnit_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = true;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucUnit"))
                    {
                        ((UserControl)obj).Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((UserControl)obj).Visibility = Visibility.Visible;
                        sb.Begin(((UserControl)obj));
                    }
                }


            }
            formGridBg.Visibility = Visibility.Visible;
        }

        private void btnDeleteUnit_Click(object sender, RoutedEventArgs e)
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
                        string query = "UPDATE `unit_t` SET `isDeleted`= 1 WHERE ID = '" + MainVM.SelectedUnit.ID + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                        }
                    }
                    dbCon.Close();
                }
            }
            MainVM.Ldt.worker.RunWorkerAsync();
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
                //locationPrice.Value = MainVM.SelectedProvince.ProvincePrice;
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
                        MainVM.Ldt.worker.RunWorkerAsync();
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
