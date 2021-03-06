﻿using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
            MainVM.Ldt.empId = empID;

            MainVM.Mr.worker.RunWorkerAsync();
            MainVM.Ldt.worker.RunWorkerAsync();
            
            this.ucEmployee.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucCustSupp.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
            //this.ucCustSupp.BackToSelectCustomerClicked += selectCustomer_BtnClicked;

            this.ucServices.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;
           
            this.ucLocation.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;


            this.ucProduct.SaveCloseButtonClicked += saveCloseBtn_SaveCloseButtonClicked;

            this.ucSelectSalesQuote.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucSalesQuote.SaveCloseButtonClicked += saveCloseSalesQuoteForm; 
            this.ucSalesQuote.ConvertToInvoice += convertToInvoice_BtnClicked;
            this.ucSalesQuote.SelectCustomer += selectCustomer_BtnClicked;
            this.ucSalesQuote.SelectItem += selectItem_BtnClicked;
            this.ucSalesQuote.PrintSalesQuote += printSalesQuote_BtnCliked;

            this.ucPurchaseOrder.SelectCustomer += selectCustomer_BtnClicked;

            this.ucPurchaseOrder.SelectSalesQuote += selectSalesQuote_BtnClicked;
            this.ucPurchaseOrder.SaveCloseButtonClicked += saveClosePurchaseOrderForm;
            this.ucPurchaseOrder.PrintPurchaseOrder += printPurchaseOrder_BtnClicked;

            this.ucAddItem.SaveCloseButtonClicked += saveCloseOther_BtnClicked;

            this.ucSelectCustomer.AddNewCustomer += addNewCustomer_BtnClicked;
            this.ucSelectCustomer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucNoticeOfEmployment.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucInvoiceForm.SaveCloseButtonClicked += saveCloseInvoiceForm;
            this.ucInvoiceForm.PrintSalesInvoice += printSalesInvoice_BtnCliked;

            this.ucInvoicePaymentHist.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;
            this.ucInvoicePaymentHist.ReceivePaymentButtonClicked += receive_BtnClicked;
            this.ucInvoicePaymentHist.PrintReceipt += printReceipt_BtnCliked;

            this.ucInvoicePaymentForm.SaveClosePaymentForm += saveClosePaymentForm_BtnClicked;
            this.ucInvoicePaymentForm.PrintReceipt += printReceipt_BtnCliked;

            this.ucOfficialReceipt.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;
            this.ucPurchaseOrderViewer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucSaleInvoiceViewer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucService.SelectServiceButtonClicked += selectService_BtnClicked;
            this.ucSelectService.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;
            this.ucService.SaveCloseButtonClicked += saveCloseServiceForm;

            this.ucSalesQuoteViewer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            
            this.ucSaleInvoiceViewer.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;

            this.ucService.AssignEmployeeButtonClicked += assignEmp_BtnClicked;
            this.ucAssignEmp.SaveCloseOtherButtonClicked += saveCloseOther_BtnClicked;
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
        }

        #region Custom Events

        private void assignEmp_BtnClicked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucAssignEmp))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void selectService_BtnClicked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            ucService.Visibility = Visibility.Collapsed;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSelectService))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void convertToInvoice_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isPaymentInvoice = true;
            MainVM.isView = false;
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Billing and Collection";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }

            foreach(UIElement obj in transOrderGrid.Children)
            {
                if (transOrderGrid.Children.IndexOf(obj) == 0)
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }

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

        private void printSalesInvoice_BtnCliked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSaleInvoiceViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void printSalesQuote_BtnCliked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSalesQuoteViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void printPurchaseOrder_BtnClicked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucPurchaseOrderViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

       private void printReceipt_BtnCliked(object sender, EventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucOfficialReceipt))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void saveClosePaymentForm_BtnClicked(object sender, EventArgs e)
        {
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucInvoicePaymentForm))
                {
                    Grid.SetZIndex((obj), 0);
                    obj.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void receive_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucInvoicePaymentForm))
                {
                    Grid.SetZIndex((obj), 1);
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void addNewCustomer_BtnClicked(object sender, EventArgs e)
        {
            MainVM.isEdit = false;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            formGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 0);
            Grid.SetZIndex((formGridBg), 1);
            if (ucSalesQuote.IsVisible)
                MainVM.isNewTrans = true;
            else if (ucPurchaseOrder.IsVisible)
                MainVM.isNewPurchaseOrder = true;
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

        private void saveCloseServiceForm(object sender, EventArgs e)
        {
            foreach (var obj in serviceGrid.Children)
            {
                if (obj is Grid)
                {
                    if (((Grid)obj).Equals(serviceGridHome))
                    {
                        headerLbl.Content = "Service Management";
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }
            refreshData();
        }

        private void saveCloseSalesQuoteForm(object sender, EventArgs e)
        {
            MainVM.isNewTrans = false;
            foreach (var obj in transQuotationGrid.Children)
            {
                if (obj is Grid)
                {
                    if (((Grid)obj).Equals(quotationsGridHome))
                    {
                        headerLbl.Content = "Trasanction - Sales Order";
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }
            refreshData();
        }

        private void saveCloseInvoiceForm(object sender, EventArgs e)
        {
            headerLbl.Content = "Billing and Collection";
            MainVM.isNewTrans = false;
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            billingGrid.Visibility = Visibility.Visible;
            foreach (UIElement obj in billingGrid.Children)
            {
                if (billingGrid.Children.IndexOf(obj) == 0)
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            refreshData();
        }

        private void saveClosePurchaseOrderForm(object sender, EventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in transOrderGrid.Children)
            {
                if (obj is Grid)
                {
                    if (transOrderGrid.Children.IndexOf(obj) == 1)
                    {
                        headerLbl.Content = "Trasanction - Purchase Order";
                        ((Grid)obj).Visibility = Visibility.Visible;
                    }
                }
                else
                    ((UserControl)obj).Visibility = Visibility.Collapsed;

            }

            refreshData();

            
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
            MainVM.Ldt.worker.RunWorkerAsync();
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            Grid.SetZIndex((otherGridBg), 1);
            Grid.SetZIndex((formGridBg), 0);
            foreach (var obj in formGridBg.Children)
            {
                if (obj is UserControl)
                    ((UserControl)obj).Visibility = Visibility.Collapsed;
                else if (obj is Grid)
                    ((Grid)obj).Visibility = Visibility.Collapsed;

            }
           
        }

        private void saveCloseOther_BtnClicked(object sender, EventArgs e)
        {
           
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            if (ucSelectSalesQuote.IsVisible)
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
            else if (ucInvoicePaymentHist.IsVisible)
            {
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
            else if (ucSelectService.IsVisible)
            {
                foreach (var element in serviceGrid.Children)
                {
                    if (element is UserControl)
                    {
                        if (!(((UserControl)element).Equals(ucService)))
                        {
                            ((UserControl)element).Visibility = Visibility.Collapsed;
                        }
                        else
                            ((UserControl)element).Visibility = Visibility.Visible;
                    }
                }
            }

            otherGridBg.Visibility = Visibility.Collapsed;
            foreach (UIElement obj in otherGridBg.Children)
            {
                obj.Visibility = Visibility.Collapsed;

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

        void closeAllOtherGridForm()
        {
            otherGridBg.Visibility = Visibility.Collapsed;
            foreach (UIElement obj in otherGridBg.Children)
            {
                obj.Visibility = Visibility.Collapsed;

            }
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (UIElement obj in formGridBg.Children)
            {
                obj.Visibility = Visibility.Collapsed;

            }
        }

        #region Side Menu Buttons

        private void dashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            closeAllOtherGridForm();
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
            MainVM.resetValueofVariables();
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
            closeAllOtherGridForm();
        }

        private void ordersSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
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
            closeAllOtherGridForm();
        }

        private void billsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Billing and Collection";
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
            closeAllOtherGridForm();
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 3)
                {
                    headerLbl.Content = "Service Management";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in serviceGrid.Children)
            {
                if (serviceGrid.Children.IndexOf(obj) == 0)
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void reportsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (reportsSubMenuGrid.IsVisible)
                reportsSubMenuGrid.Visibility = Visibility.Collapsed;
            else
                reportsSubMenuGrid.Visibility = Visibility.Visible;

        }

        private void queriesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (queriesSubMenuGrid.IsVisible)
                queriesSubMenuGrid.Visibility = Visibility.Collapsed;
            else
                queriesSubMenuGrid.Visibility = Visibility.Visible;
        }

        private void salesReportBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 4)
                {
                    
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in reportsGrid.Children)
            {
                if (reportsGrid.Children.IndexOf(obj) == 0)
                {
                    headerLbl.Content = "Reports - Sales Report";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void purchaseReportBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 4)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in reportsGrid.Children)
            {
                if (reportsGrid.Children.IndexOf(obj) == 1)
                {
                    headerLbl.Content = "Reports - Purchase Report";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void serviceReportBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 4)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in reportsGrid.Children)
            {
                if (reportsGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Reports - Service Report";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void manageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (manageSubMenugrid.IsVisible)
                manageSubMenugrid.Visibility = Visibility.Collapsed;
            else
                manageSubMenugrid.Visibility = Visibility.Visible;
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            closeAllOtherGridForm();
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
            MainVM.isNewSupplier = true;
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
                foreach (UIElement obj in ucProduct.productDetailsFormGrid1.Children)
                {
                    if (!(obj is Label))
                    {
                        obj.IsEnabled = false;
                    }
                }
            }
            else if (manageServicesGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (UIElement obj in formGridBg.Children)
                {
                    if (obj.Equals(ucServices))
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;

                }
                ucServices.saveCloseButtonGrid.Visibility = Visibility.Collapsed;
                ucServices.editCloseButtonGrid.Visibility = Visibility.Visible;
                foreach (UIElement obj in ucServices.serviceForm.Children)
                {
                    obj.IsEnabled = false;
                }
            }
            else if (manageLocationsGrid.IsVisible)
            {
                Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                foreach (UIElement obj in formGridBg.Children)
                {
                    if (obj.Equals(ucLocation))
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;

                }
                ucLocation.saveCancelGrid2.Visibility = Visibility.Collapsed;
                ucLocation.editCloseGrid2.Visibility = Visibility.Visible;
                foreach (UIElement obj in ucServices.serviceForm.Children)
                {
                    obj.IsEnabled = false;
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



        

        #endregion

        #region Order Management - Sales Quotes

        private void transQuotationAddBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MainVM.isNewSalesQuote = true;
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
            
        }

        

        private void viewQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MainVM.isViewSalesQuote = true;
            foreach (UIElement element in transQuotationGrid.Children)
            {
                if (transQuotationGrid.Children.IndexOf(element) == 1)
                    element.Visibility = Visibility.Visible;
                else
                    element.Visibility = Visibility.Collapsed;
            }
        }

        private void editQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEditSalesQuote = true;
            foreach (UIElement element in transQuotationGrid.Children)
            {
                if (transQuotationGrid.Children.IndexOf(element) == 1)
                    element.Visibility = Visibility.Visible;
                else
                    element.Visibility = Visibility.Collapsed;
            }

        }

        private void deleteQuoteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `sales_quote_t` SET `isDeleted`= 1 WHERE sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "';";
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

        private void printSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSalesQuoteViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void genContractBtn_Click(object sender, RoutedEventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucNoticeOfEmployment))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
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


        private void viewInvoiceRecord_Click(object sender, RoutedEventArgs e)
        {

            MainVM.isView = true;
            MainVM.isPaymentInvoice = true;
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
            foreach(UIElement obj in billingGrid.Children)
            {
                if(billingGrid.Children.IndexOf(obj) == 1)
                    obj.Visibility = Visibility.Visible;
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            
        }

        private void printSalesInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSaleInvoiceViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }

        private void receivePaymentBtn_Click(object sender, RoutedEventArgs e)
        {

            MainVM.isPaymentInvoice = true;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucInvoicePaymentHist))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
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

        private void viewPurchaseOrderBtn_Click(object sender, RoutedEventArgs e)
        {

            MainVM.isViewPurchaseOrder = true;
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

        private void editPurchaseOrderBtn_Click(object sender, RoutedEventArgs e)
        {

            MainVM.isEditPurchaseOrder = true;
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

        private void deletePurchaseOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `purchase_order_t` SET `isDeleted`= 1 WHERE PONumChar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "';";
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

        private void printPurchaseOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            otherGridBg.Visibility = Visibility.Visible;
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucPurchaseOrderViewer))
                {
                    obj.Visibility = Visibility.Visible;
                }
            }
        }


        #endregion
        #region Service Management
        private void newServiceSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isNewSchedule = true;
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            otherGridBg.Visibility = Visibility.Visible;
            Grid.SetZIndex((otherGridBg), 1);
            foreach (UIElement obj in otherGridBg.Children)
            {
                if (obj.Equals(ucSelectService))
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                {
                    obj.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void viewSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isViewSchedule = true;
            foreach (var element in serviceGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Equals(ucService)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
        }

        private void editSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isEditSchedule = true;
            foreach (var element in serviceGrid.Children)
            {
                if (element is UserControl)
                {
                    if (!(((UserControl)element).Equals(ucService)))
                    {
                        ((UserControl)element).Visibility = Visibility.Collapsed;
                    }
                    else
                        ((UserControl)element).Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        private void selectedReportBtnClicked(object sender, RoutedEventArgs e)
        {

            //foreach (var obj in containerGrid.Children)
            //{
            //    ((Grid)obj).Visibility = Visibility.Collapsed;
            //}
            //reportsGrid.Visibility = Visibility.Visible;
            //foreach (var obj in reportsGrid.Children)
            //{
                
            //}
            //foreach (var obj in transOrderGrid.Children)
            //{
            //    if (obj is Grid)
            //    {
                    
            //        ((Grid)obj).Visibility = Visibility.Visible;
            //    }
            //    else
            //        ((UserControl)obj).Visibility = Visibility.Collapsed;

            //}
        }
        #region Queries

        private void QueiresConBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 0)
                {
                    headerLbl.Content = "Queires - Frequent Contractor";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void QueiresEmpBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 1)
                {
                    headerLbl.Content = "Queires - Frequent Employee";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void QueiresItems_Click(object sender, RoutedEventArgs e)
        {
           MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 2)
                {
                    headerLbl.Content = "Queires - Most Items Availed";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void QueiresService_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 5)
                {
                    headerLbl.Content = "Queires - Most Sevice Availed";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void QueiresCustomer_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 4)
                {
                    headerLbl.Content = "Queires - Most Loyal Customer";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }

        private void QueiresSupplier_Click(object sender, RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            foreach (UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 5)
                {

                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            foreach (UIElement obj in queriesGrid.Children)
            {
                if (queriesGrid.Children.IndexOf(obj) == 3)
                {
                    headerLbl.Content = "Queires - Top Supplier";
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
            }
            closeAllOtherGridForm();
        }
        #endregion
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            searchuery();

        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchuery();
        }

        void searchuery()
        {
            if (transQuotationGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.SalesQuotes.Where(x => x.sqNoChar_.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    quotationsDataGrid.ItemsSource = items;
                }
                else
                {
                    quotationsDataGrid.ItemsSource = MainVM.SalesQuotes;
                }
            }
            else if (transOrderGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.PurchaseOrder.Where(x => x.PONumChar.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    purchaseOrderDg.ItemsSource = items;
                }
                else
                {
                    purchaseOrderDg.ItemsSource = MainVM.PurchaseOrder;
                }
            }
            else if (serviceGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = from ss in MainVM.ServiceSchedules_
                                join ass in MainVM.AvailedServices on ss.ServiceAvailedID equals ass.AvailedServiceID
                                where ass.SqNoChar.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1
                                select ss;


                    scheduledServiceDg.ItemsSource = items;
                }
                else
                {
                    scheduledServiceDg.ItemsSource = MainVM.ServiceSchedules_;
                }
            }
            else if (manageLocationsGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.Regions.Where(x => x.RegionName.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    regionListDg.ItemsSource = items;
                }
                else
                {
                    regionListDg.ItemsSource = MainVM.Regions;
                }
            }
            else if (manageUnitsGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.Units.Where(x => x.UnitName.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    unitListDg.ItemsSource = items;
                }
                else
                {
                    unitListDg.ItemsSource = MainVM.Units;
                }
            }
            else if (manageServicesGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.ServicesList.Where(x => x.ServiceName.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    serviceTypeDg.ItemsSource = items;
                }
                else
                {
                    serviceTypeDg.ItemsSource = MainVM.ServicesList;
                }
            }
            else if (manageProductListGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.ProductList.Where(x => x.ItemName.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    productListDg.ItemsSource = items;
                }
                else
                {
                    productListDg.ItemsSource = MainVM.ProductList;
                }
            }
            else if (manageEmployeeGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    if (MainVM.isContractor)
                    {
                        var items = MainVM.Contractor.Where(x => (x.EmpFname + " " + x.EmpMiddleInitial + " " + x.EmpLName).IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);
                        manageEmployeeDataGrid.ItemsSource = items;
                    }
                    else
                    {
                        var items = MainVM.Employees.Where(x => (x.EmpFname + " " + x.EmpMiddleInitial + " " + x.EmpLName).IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);
                        manageEmployeeDataGrid.ItemsSource = items;
                    }
                }
                else
                {
                    if (MainVM.isContractor)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Contractor;
                    }
                    else
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Employees;
                    }
                }
            }
            else if (manageCustomerGrid.IsVisible)
            {
                if (!(String.IsNullOrWhiteSpace(MainVM.SearchQuery)))
                {
                    var items = MainVM.Suppliers.Where(x => x.CompanyName.IndexOf(MainVM.SearchQuery, StringComparison.CurrentCultureIgnoreCase) != -1);

                    manageCustomerDataGrid.ItemsSource = items;
                }
                else
                {
                    manageCustomerDataGrid.ItemsSource = MainVM.Suppliers;
                }
            }
        }

        private void userManualBtn_Click(object sender, RoutedEventArgs e)
        {
            string locationToSavePdf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources\\USER-MANUAL.pdf");  
            Process.Start(locationToSavePdf);
        }
    }
}
