using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucSalesQuote.xaml
    /// </summary>
    public partial class ucSalesQuote : UserControl
    {
        public ucSalesQuote()
        {
            InitializeComponent();
        }

        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SaveCloseButtonClicked;
        public event EventHandler ConvertToInvoice;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnConvertToInvoice(RoutedEventArgs e)
        {
            var handler = ConvertToInvoice;
            if (handler != null)
                handler(this, e);
        }

        private void transRequestBack_Click(object sender, RoutedEventArgs e)
        {
            if (newRequisitionGrid.IsVisible)
            {
                foreach (var obj in newRequisitionGridForm.Children)
                {
                    if (obj is TextBox)
                        ((TextBox)obj).IsEnabled = true;
                    else if (obj is Xceed.Wpf.Toolkit.DecimalUpDown)
                        ((Xceed.Wpf.Toolkit.DecimalUpDown)obj).IsEnabled = true;
                    else if (obj is Button)
                        ((Button)obj).IsEnabled = true;
                    else if (obj is DataGrid)
                        ((DataGrid)obj).IsEnabled = true;
                }
                OnSaveCloseButtonClicked(e);

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
                foreach (var element in newRequisitionGrid.Children)
                {
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
                }
                if (!validationError)
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
                    SalesQuoteDocument df = new SalesQuoteDocument();
                    document = df.CreateDocument("SalesQuote", "asdsadsa");
                    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                    saleQuoteViewer.pagePreview.Ddl = ddl;
                }
                else
                    MessageBox.Show("No items on the list.");
            }
            else if (viewQuotationGrid.IsVisible)
            {
                transRequestNext.Content = "Next";
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = document;
                renderer.RenderDocument();
                string filename = @"d:\test\" + MainVM.SelectedSalesQuote.sqNoChar_ + ".pdf";

                saveSalesQuoteToDb();
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "" + MainVM.SelectedSalesQuote.sqNoChar_;
                dlg.DefaultExt = ".pdf";
                dlg.Filter = "Text documents (.pdf)|*.pdf";
                if (dlg.ShowDialog() == true)
                {
                    filename = dlg.FileName;
                    renderer.PdfDocument.Save(filename);
                }
                OnSaveCloseButtonClicked(e);
            }
        }
        private void selectCustomerBtn_Click(object sender, RoutedEventArgs e)
        {

            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is Grid)
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
        }
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.Customers.Where(x => x.CompanyName.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()));
            var observable = new ObservableCollection<Customer>(linqResults);
            selectCustomerDg.ItemsSource = observable;
        }


        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }
        private void transReqAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is Grid)
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
        }

        private void feesBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is Grid)
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
                    MainVM.isEdit = true;
                    break;
                }
                else
                {
                    feeTypeCb.SelectedIndex = feeTypeCb.Items.Count - 1;
                    otherFeenameTb.Text = MainVM.SelectedAdditionalFee.FeeName;
                    feeCostTb.Value = MainVM.SelectedAdditionalFee.FeePrice;
                    addSaveAdditionalFeesBtn.Content = "Save";
                    MainVM.isEdit = true;
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
            if (MainVM.isEdit)
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
            MainVM.isEdit = false;
        }

        private void saveAdditionalFees_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
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
                closeModals();
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


                    MainVM.AddedServices.Add(new AddedService() { TableNoChar = serviceNoChar, ServiceID = MainVM.SelectedService.ServiceID, ProvinceID = MainVM.SelectedProvince.ProvinceID, Address = serviceAddressTb.Text, City = serviceCityTb.Text, TotalCost = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice });

                    MainVM.RequestedItems.Add(new RequestedItem() { lineNo = (MainVM.RequestedItems.Count + 1).ToString(), itemCode = serviceNoChar, itemName = MainVM.SelectedService.ServiceName, desc = serviceDescTb.Text, itemTypeName = "Service", itemType = 1, qty = 1, unitPrice = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmount = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, totalAmountMarkUp = MainVM.SelectedService.ServicePrice + MainVM.SelectedProvince.ProvincePrice, qtyEditable = false });

                    closeModals();
                }

            }
            computePrice();
        }

        private void closeModals()
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
            foreach (var obj in formGridBg.Children)
            {
                if (obj is Grid)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
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
            else if ((bool)deliveryCustomRd.IsChecked)
                estDel = int.Parse(deliveryDaysTb.Value.ToString());

            if (!(bool)validityDefaultRd.IsChecked)
            {
                valid = int.Parse(validityTb.Value.ToString());
            }

            DateTime endDate = new DateTime();
            endDate = DateTime.Now.AddDays(valid);

            DateTime deliveryDate = new DateTime();
            deliveryDate = DateTime.Now.AddDays(estDel);

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
            stringChars += "SQ";
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

        }

        void saveSalesQuoteToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO `odc_db`.`sales_quote_t` " + "(`sqNoChar`,`custID`,`quoteSubject`,`priceNote`,`deliveryDate`,`estDelivery`,`validityDays`,`validityDate`,`otherTerms`,`VAT`,`vatIsExcluded`,`paymentIsLanded`,`paymentCurrency`,`status`,`termsDays`,`termsDP`,`penaltyAmt`,`penaltyPerc`,`markUpPercent`,`discountPercent`)" +
                    " VALUES " +
                    "('" + MainVM.SelectedSalesQuote.sqNoChar_ + "','" +
                    MainVM.SelectedSalesQuote.custID_ + "','" +
                    MainVM.SelectedSalesQuote.quoteSubject_ + "','" +
                    MainVM.SelectedSalesQuote.priceNote_ + "','" +
                    MainVM.SelectedSalesQuote.deliveryDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesQuote.estDelivery_ + "','" +
                    MainVM.SelectedSalesQuote.validityDays_ + "','" +
                    MainVM.SelectedSalesQuote.validityDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesQuote.otherTerms_ + "','" +
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
                        foreach (RequestedItem item in MainVM.RequestedItems)
                        {
                            if (item.itemType == 0)
                            {
                                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ItemCode.Equals(item.itemCode)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`items_availed_t`(`sqNoChar`,`itemCode`,`itemQnty`,`totalCost`)" +
                                    " VALUES " +
                                    "('" + MainVM.SelectedSalesQuote.sqNoChar_ + "', '" + MainVM.SelectedProduct.ItemCode + "','" + item.qty + "', '" + item.totalAmount + "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                            }
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
                                    MainVM.SelectedAddedService.TotalCost + "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                                foreach (AdditionalFee af in MainVM.SelectedAddedService.AdditionalFees)
                                {
                                    query = "INSERT INTO `odc_db`.`fees_per_transaction_t`(`serviceNoChar`,`feeName`,`feeValue`)" +
                                    " VALUES " +
                                    "('" + item.itemCode + "', '" + af.FeeName + "', '" + af.FeePrice + "');";
                                    noError = dbCon.insertQuery(query, dbCon.Connection);
                                }
                            }
                        }
                        if (noError)
                        {
                            MessageBox.Show("Successfully added.");

                        }

                        else
                            MessageBox.Show("Theres an error occured in saving the record");
                    }
                }
            }
        }

        void loadSalesQuoteToUi()
        {
            MainVM.SelectedCustomerSupplier = MainVM.Customers.Where(x => x.CompanyID.Equals(MainVM.SelectedSalesQuote.custID_.ToString())).FirstOrDefault();
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
                    totalAmount = item.TotalCost,
                    itemType = 0,
                    unitPrice = MainVM.SelectedProduct.CostPrice
                    
                });
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
            }
            foreach(var obj in newRequisitionGridForm.Children)
            {
                if (obj is TextBox)
                    ((TextBox)obj).IsEnabled = false;
                else if (obj is Xceed.Wpf.Toolkit.DecimalUpDown)
                    ((Xceed.Wpf.Toolkit.DecimalUpDown)obj).IsEnabled = false;
                else if (obj is Button)
                    ((Button)obj).IsEnabled = false;
                else if (obj is DataGrid)
                    ((DataGrid)obj).IsEnabled = false;
            }
            computePrice();
        }
        private void generatePDFBtn_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var element in transQuoatationGridForm.Children)
            //{
            //    if (element is Grid)
            //    {
            //        if (!(((Grid)element).Name.Equals(viewQuotationGrid.Name)))
            //        {
            //            ((Grid)element).Visibility = Visibility.Collapsed;
            //        }
            //        else
            //            ((Grid)element).Visibility = Visibility.Visible;
            //    }
            //}
            //if (MainVM.RequestedItems.Count != 0)
            //{
            //    transRequestNext.Content = "Save";
            //    salesQuoteToMemory();
            //    SalesQuoteDocument df = new SalesQuoteDocument();
            //    document = df.CreateDocument("SalesQuote", "asdsadsa");
            //    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            //    saleQuoteViewer.pagePreview.Ddl = ddl;
            //}
            //else
            //    MessageBox.Show("No items on the list.");
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }

        private void convertToInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            OnConvertToInvoice(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                if (MainVM.SelectedSalesQuote != null)
                    loadSalesQuoteToUi();
                else
                {
                    foreach (var obj in newRequisitionGridForm.Children)
                    {
                        if (obj is TextBox)
                            ((TextBox)obj).IsEnabled = true;
                        else if (obj is Xceed.Wpf.Toolkit.DecimalUpDown)
                            ((Xceed.Wpf.Toolkit.DecimalUpDown)obj).IsEnabled = true;
                        else if (obj is Button)
                            ((Button)obj).IsEnabled = true;
                        else if (obj is DataGrid)
                            ((DataGrid)obj).IsEnabled = true;
                    }
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
                closeModals();

            }
            
        }
    }
    
}
