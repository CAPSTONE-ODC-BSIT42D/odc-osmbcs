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

        private static String dbname = "odc_db";

        private bool validationError = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.ucEmployee.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucCustSupp.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            //this.ucCustSupp.BackToSelectCustomerClicked += selectCustomer_BtnClicked;

            this.ucServices.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
           

            this.ucUnit.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            this.ucLocation.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;


            this.ucProduct.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            //this..SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucSalesQuote.SaveCloseButtonClicked += saveCloseSalesQuoteForm; ;
            this.ucSalesQuote.ConvertToInvoice += convertToInvoice_BtnClicked;
            this.ucSalesQuote.SelectCustomer += selectCustomer_BtnClicked;
            this.ucSalesQuote.SelectItem += selectItem_BtnClicked;

            this.ucPurchaseOrder.SelectCustomer += selectCustomer_BtnClicked;

            this.ucAddItem.SaveCloseButtonClicked += saveCloseOther_BtnClicked;

            this.ucSelectCustomer.AddNewCustomer += addNewCustomer_BtnClicked;
            this.ucSelectCustomer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucPurchaseOrder.SelectSalesQuote += selectSalesQuote_BtnClicked;

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
            otherGridBg.Visibility = Visibility.Collapsed;
            MainVM.LoginEmployee_ = MainVM.Employees.Where(x => x.EmpID.Equals(empID)).FirstOrDefault();
            MainVM.Ldt.worker.RunWorkerAsync();
            int cout = MainVM.SalesQuotes.Count;
        }

        #region Custom Events
        
        private void addNewCustomer_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            formGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((formGridBg), 1);
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

        private void selectSalesQuote_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSelectSalesQuote))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void saveCloseSalesQuoteForm(object sender, EventArgs e)
        {
            MainVM.isNewTrans = true;
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

        private void selectCustomer_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            otherGridBg.Visibility = Visibility.Visible;
            foreach (var obj in otherGridBg.Children)
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


        private void saveCloseBtn_SaveCloseButtonClicked(object sender, EventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                else if (obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;

            }

            refreshData();

            MainVM.StringTextBox = null;
            MainVM.DecimalTextBox = 0;
            MainVM.IntegerTextBox = 0;
            MainVM.cbItem = null;
            MainVM.isNewRecord = false;
            MainVM.isViewHome = false;
            MainVM.isEdit = false;
            MainVM.isPaymentInvoice = false;

        }



        private void saveCloseOther_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            otherGridBg.Visibility = Visibility.Collapsed;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSelectCustomer))
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
            if (MainVM.isPaymentInvoice)
            {
                foreach (UIElement obj in billingGrid.Children)
                {
                    if (billingGrid.Children.IndexOf(obj) == 1)
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void selectItem_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;

            otherGridBg.Visibility = Visibility.Visible;
            foreach (var obj in otherGridBg.Children)
            {
                if (obj is UserControl)
                {
                    if (!((UserControl)obj).Name.Equals("ucAddItem"))
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


        #endregion

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Side Menu Buttons

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
                        headerLbl.Content = "Order Management - Sales Quotes";
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }
        }

        private void ordersSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            trasanctionGrid.Visibility = Visibility.Visible;
            foreach (var obj in trasanctionGrid.Children)
            {
                if (obj is Grid)
                    if (((Grid)obj).Equals(transOrderGrid))
                        ((Grid)obj).Visibility = Visibility.Visible;
                    else
                        ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            foreach (var obj in transOrderGrid.Children)
            {
                if (obj is Grid)
                {
                    headerLbl.Content = "Order Management - Purchase Order";
                    ((Grid)obj).Visibility = Visibility.Visible;
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }
        }

        private void billsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Billing";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in billingGrid.Children)
            {
                if (billingGrid.Children.IndexOf(obj) == 0)
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
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

        //private void invoiceSalesMenuBtn_Click(object sender, RoutedEventArgs e)
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
        //    foreach (var obj in transQuotationGrid.Children)
        //    {
        //        if (obj is Grid)
        //        {
        //            if (((Grid)obj).Equals(invoiceGridHome))
        //            {
        //                headerLbl.Content = "Order Management - Sales Invoice";
        //                ((Grid)obj).Visibility = Visibility.Visible;
        //            }
        //        }
        //        else
        //            ((UserControl)obj).Visibility = Visibility.Collapsed;

        //    }
        //}

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

        #endregion

        #region Maintenance

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
                        //sb.Begin(((UserControl)obj));
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
            MainVM.SelectedService = new Service();
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

        #endregion

        
        #region Universal Functions
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
            else if (manageUnitsGrid.IsVisible)
            {
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
            else if (manageServicesGrid.IsVisible)
            {
                foreach (var obj in formGridBg.Children)
                {
                    if (obj is UserControl)
                    {
                        if (!((UserControl)obj).Equals(ucServices))
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
            else if (manageUnitsGrid.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (serviceTypeDg.SelectedItems.Count > 0)
                {
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
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {

                if (obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                else if (obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;

            }
        }
        public bool isEdit = false;



        public void refreshData()
        {
            MainVM.Ldt.worker.RunWorkerAsync();
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

        #endregion

        #region Order Management - Sales Quotes

        private void transQuotationAddBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isNewTrans = true;
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
            //MainVM.isNewRecord = true;
            //foreach (var obj in containerGrid.Children)
            //{
            //    ((Grid)obj).Visibility = Visibility.Collapsed;
            //}
            //trasanctionGrid.Visibility = Visibility.Visible;
            //foreach (var obj in trasanctionGrid.Children)
            //{
            //    if (obj is Grid)
            //        if (((Grid)obj).Equals(transInvoiceGrid))
            //            ((Grid)obj).Visibility = Visibility.Visible;
            //        else
            //            ((Grid)obj).Visibility = Visibility.Collapsed;
            //}
            //foreach (var obj in transInvoiceGrid.Children)
            //{
            //    if (obj is UserControl)
            //    {
            //        if (((UserControl)obj).Equals(ucInvoiceForm))
            //        {
            //            headerLbl.Content = "Order Management - Sales Invoice";
            //            ((UserControl)obj).Visibility = Visibility.Visible;
            //        }
            //    }
            //    else
            //        ((Grid)obj).Visibility = Visibility.Collapsed;

            //}
        }

        private void genContractBtn_Click(object sender, RoutedEventArgs e)
        {
            ucContract.Visibility = Visibility.Visible;
        }

        #endregion

        #region Order Management - Invoice

        private void newInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Billing";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            
            MainVM.isPaymentInvoice = true;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSelectSalesQuote))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
            //MainVM.isNewTrans = true;
            //foreach (var obj in containerGrid.Children)
            //{
            //    ((Grid)obj).Visibility = Visibility.Collapsed;
            //}
            //trasanctionGrid.Visibility = Visibility.Visible;
            //foreach (var obj in trasanctionGrid.Children)
            //{
            //    if (obj is Grid)
            //        if (((Grid)obj).Equals(transInvoiceGrid))
            //            ((Grid)obj).Visibility = Visibility.Visible;
            //        else
            //            ((Grid)obj).Visibility = Visibility.Collapsed;

            //}
            //foreach (var obj in transInvoiceGrid.Children)
            //{
            //    if (obj is UserControl)
            //    {
            //        if (((UserControl)obj).Equals(ucInvoiceForm))
            //        {
            //            headerLbl.Content = "Order Management - Sales Invoice";
            //            ((UserControl)obj).Visibility = Visibility.Visible;
            //        }
            //    }
            //    else
            //        ((Grid)obj).Visibility = Visibility.Collapsed;
            //}
            //ucSalesQuote.viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            //ucSalesQuote.newSalesQuoteBtns.Visibility = Visibility.Visible;
            //resetValueofSelectedVariables();
        }


        private void viewInvoiceRecord_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEdit = true;
        }

        private void receivePaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isPaymentInvoice = true;
        }


        #endregion

        #region Order Management - Purchase Order
        private void newPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isNewPurchaseOrder = true;
            foreach (var element in transOrderGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Equals(ucPurchaseOrder)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
        }
        #endregion
    }
}
