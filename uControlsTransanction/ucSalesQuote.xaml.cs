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
        public event EventHandler SelectCustomer;
        public event EventHandler SelectItem;

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

        protected virtual void OnSelectCustomerClicked(RoutedEventArgs e)
        {
            var handler = SelectCustomer;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnSelectItemClicked(RoutedEventArgs e)
        {
            var handler = SelectItem;
            if (handler != null)
                handler(this, e);
        }


        private void resetElements()
        {
            MainVM.RequestedItems.Clear();
            MainVM.AvailedServicesList.Clear();
            MainVM.SelectedSalesQuote = null;
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
        }


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                if (MainVM.SelectedSalesQuote != null && MainVM.isEdit || MainVM.isView)
                {
                    foreach (UIElement obj in transQuoatationGridForm.Children)
                    {
                        if (transQuoatationGridForm.Children.IndexOf(obj) == 0)
                        {
                            obj.Visibility = Visibility.Visible;
                        }
                        else
                            obj.Visibility = Visibility.Collapsed;
                    }
                    loadSalesQuoteToUi();
                    computePrice();
                }
                else
                {

                    foreach (UIElement obj in transQuoatationGridForm.Children)
                    {
                        obj.IsEnabled = true;

                        if (transQuoatationGridForm.Children.IndexOf(obj) == 0)
                        {
                            obj.Visibility = Visibility.Visible;
                        }
                        else
                            obj.Visibility = Visibility.Collapsed;
                    }

                }
                closeModals();

            }

        }


        private void transRequestBack_Click(object sender, RoutedEventArgs e)
        {
            if (newRequisitionGrid.IsVisible)
            {
                resetElements();
                OnSaveCloseButtonClicked(e);

            }
            else if (termsAndConditionGrid.IsVisible)
            {
                foreach (UIElement obj in transQuoatationGridForm.Children)
                {
                    if (transQuoatationGridForm.Children.IndexOf(obj) == 0)
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;
                }
            }
            else if (saleQuoteViewer.IsVisible)
            {
                MainVM.SalesQuotes.Remove(MainVM.SelectedSalesQuote);
                foreach (UIElement obj in transQuoatationGridForm.Children)
                {
                    if (transQuoatationGridForm.Children.IndexOf(obj) == 1)
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;
                }
            }
        }
        Document document;
        private void transRequestNext_Click(object sender, RoutedEventArgs e)
        {
            if (newRequisitionGrid.IsVisible)
            {
                if (MainVM.RequestedItems.Count != 0)
                {
                    foreach (UIElement obj in transQuoatationGridForm.Children)
                    {
                        if (transQuoatationGridForm.Children.IndexOf(obj) == 1)
                        {
                            obj.Visibility = Visibility.Visible;
                        }
                        else
                            obj.Visibility = Visibility.Collapsed;
                    }
                }
                else
                    MessageBox.Show("The requested item is empty.");
            }

            else if (termsAndConditionGrid.IsVisible)
            {
                foreach (UIElement obj in transQuoatationGridForm.Children)
                {
                    if (transQuoatationGridForm.Children.IndexOf(obj) == 2)
                    {
                        obj.Visibility = Visibility.Visible;
                    }
                    else
                        obj.Visibility = Visibility.Collapsed;
                }
                    transRequestNext.Content = "Save";
                    salesQuoteToMemory();
                    SalesQuoteDocument df = new SalesQuoteDocument();
                    document = df.CreateDocument("SalesQuote", "asdsadsa");
                    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                    saleQuoteViewer.pagePreview.Ddl = ddl;
            }
            else if (saleQuoteViewer.IsVisible)
            {
                transRequestNext.Content = "Next";
                saveSalesQuoteToDb();

                //PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                //renderer.Document = document;
                //renderer.RenderDocument();
                //string filename = @"d:\test\" + MainVM.SelectedSalesQuote.sqNoChar_ + ".pdf";

                //saveSalesQuoteToDb();
                //SaveFileDialog dlg = new SaveFileDialog();
                //dlg.FileName = "" + MainVM.SelectedSalesQuote.sqNoChar_;
                //dlg.DefaultExt = ".pdf";
                //dlg.Filter = "Text documents (.pdf)|*.pdf";
                //if (dlg.ShowDialog() == true)
                //{
                //    filename = dlg.FileName;
                //    renderer.PdfDocument.Save(filename);
                //}
                resetElements();
                OnSaveCloseButtonClicked(e);
            }
        }
        private void selectCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectCustomerClicked(e);
        }

        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }
        private void transReqAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedCustomerSupplier != null)
            {
                if(!MainVM.isEdit)
                    MainVM.SelectedSalesQuote = new SalesQuote() { };
                OnSelectItemClicked(e);
            }
                
            else
                MessageBox.Show("Select Customer First.");
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
            MainVM.SelectedAvailedServices = MainVM.AvailedServicesList.Where(x => x.AvailedServiceID.Equals(MainVM.SelectedRequestedItem.availedServiceID)).FirstOrDefault();
            MainVM.AdditionalFees = MainVM.SelectedAvailedServices.AdditionalFees;
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
            MainVM.SelectedAvailedServices.AdditionalFees.Remove(MainVM.SelectedAdditionalFee);
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
                        MainVM.SelectedAvailedServices.AdditionalFees.Add(new AdditionalFee() { FeeName = otherFeenameTb.Text, FeePrice = (decimal)feeCostTb.Value });
                    }
                    else
                    {

                        MainVM.SelectedAvailedServices.AdditionalFees.Add(new AdditionalFee() { FeeName = feeTypeCb.SelectedValue.ToString(), FeePrice = (decimal)feeCostTb.Value });
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
        

        private void deleteRequestedItemBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.AvailedServices.Remove(MainVM.AvailedServices.Where(x => x.AvailedServiceID.Equals(MainVM.SelectedRequestedItem.lineNo)).FirstOrDefault());
            MainVM.RequestedItems.Remove(MainVM.SelectedRequestedItem);
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

        private void unitPriceTb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            computePrice();
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".docx";
            dlg.Filter = "Word Files (*.doc)|*.docx";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileNameTb.Text = filename;
            }
        }

        private void generatePDFBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement obj in transQuoatationGridForm.Children)
            {
                if (transQuoatationGridForm.Children.IndexOf(obj) == 2)
                {
                    obj.Visibility = Visibility.Visible;
                }
                else
                    obj.Visibility = Visibility.Collapsed;
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

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            closeModals();
        }

        private void convertToInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            OnConvertToInvoice(e);
        }



        private void editSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement obj in transQuoatationGridForm.Children)
            {
                if (transQuoatationGridForm.Children.IndexOf(obj) <= 1)
                    obj.IsEnabled = true;
            }
            viewSalesQuoteBtns.Visibility = Visibility.Collapsed;
            newSalesQuoteBtns.Visibility = Visibility.Visible;

        }

        private void computePrice()
        {
            decimal totalFee = 0;
            decimal totalPrice = 0;

            foreach (RequestedItem item in MainVM.RequestedItems)
            {
                if (item.itemType == 0)
                {
                    var markupPrice = from itm in MainVM.MarkupHist
                                      where itm.ItemID == item.itemID
                                      && itm.DateEffective <= DateTime.Now
                                      select itm;
                    MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID == item.itemID).FirstOrDefault();
                    item.unitPriceMarkUp = item.unitPrice + (item.unitPrice / 100 * (decimal)markupPrice.Last().MarkupPerc);
                    item.totalAmount = (item.unitPriceMarkUp * item.qty) - ((item.unitPriceMarkUp * item.qty) / 100) * (decimal)discountPriceTb.Value;
                }
                else if (item.itemType == 1)
                {
                    
                    MainVM.SelectedAvailedServices = MainVM.AvailedServicesList.Where(x => x.AvailedServiceID.Equals(item.availedServiceID)).FirstOrDefault();
                    MainVM.SelectedProvince = (from prov in MainVM.Provinces
                                               where prov.ProvinceID == MainVM.SelectedAvailedServices.ProvinceID
                                               select prov).FirstOrDefault();
                    MainVM.SelectedRegion = (from rg in MainVM.Regions
                                             where rg.RegionID == MainVM.SelectedProvince.RegionID
                                             select rg).FirstOrDefault();
                    foreach (AdditionalFee af in MainVM.SelectedAvailedServices.AdditionalFees)
                    {
                        if (!(af.FeePrice == 0))
                        {
                            totalFee += af.FeePrice;
                        }
                        
                    }
                    item.unitPriceMarkUp = item.unitPrice + MainVM.SelectedRegion.RatePrice;
                    item.totalAmount = (item.unitPrice + MainVM.SelectedRegion.RatePrice + totalFee) - ((item.unitPrice + MainVM.SelectedRegion.RatePrice + totalFee) / 100) * (decimal)discountPriceTb.Value;
                }
                totalPrice += item.totalAmount;
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
            

            if (MainVM.isEdit)
                stringChars = MainVM.SelectedSalesQuote.sqNoChar_;
            else
            {
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
            }

            var obj = new SalesQuote()
            {
                sqNoChar_ = stringChars,
                custID_ = MainVM.SelectedCustomerSupplier.CompanyID,
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
                warrantyDays_ = warr,
                additionalTerms_ = additionalTermsTb.Text,
                discountPercent_ = (decimal)discountPriceTb.Value
            };
            MainVM.SelectedSalesQuote = obj;

        }

        void saveSalesQuoteToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            string FileName = fileNameTb.Text;

            byte[] DocData;
            FileStream fs;
            BinaryReader br;
            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);
            DocData = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();

            if (dbCon.IsConnect())
            {
                if (!MainVM.isEdit)
                {
                    string query = "INSERT INTO `odc_db`.`sales_quote_t` " + "(`sqNoChar`,`custID`,`quoteSubject`,`priceNote`,`deliveryDate`,`estDelivery`,`validityDays`,`validityDate`,`otherTerms`,`VAT`,`vatIsExcluded`,`paymentIsLanded`,`paymentCurrency`,`status`,`termsDays`,`termsDP`,`discountPercent`,surveyReportDoc)" +
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
                    MainVM.SelectedSalesQuote.discountPercent_ + "','" +
                    DocData + "'" +
                    "); ";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        if (dbCon.IsConnect())
                        {
                            foreach (RequestedItem item in MainVM.RequestedItems)
                            {
                                if (item.itemType == 0)
                                {
                                    
                                    MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID.Equals(item.itemID)).FirstOrDefault();
                                    query = "INSERT INTO `odc_db`.`items_availed_t`(`sqNoChar`,`itemID`,`itemQnty`,`unitPrice`)" +
                                        " VALUES " +
                                        "('" + MainVM.SelectedSalesQuote.sqNoChar_ + "', '" + item.itemID + "','" + item.qty + "', '" + item.unitPrice + "');";
                                    noError = dbCon.insertQuery(query, dbCon.Connection);
                                }
                            }
                            foreach (AvailedService aserv in MainVM.AvailedServicesList)
                            {
                                query = "INSERT INTO `odc_db`.`services_availed_t`(`serviceID`,`provinceID`,`sqNoChar`,`city`,`address`,`totalCost`)" +
                                    " VALUES " +
                                    "('" + aserv.ServiceID + "', '" +
                                    aserv.ProvinceID + "', '" +
                                    MainVM.SelectedSalesQuote.sqNoChar_ + "', '" +
                                    aserv.City + "', '" +
                                    aserv.Address + "', '" +
                                    aserv.TotalCost + "');";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {
                                    query = "SELECT LAST_INSERT_ID();";
                                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                                    DataSet fromDb = new DataSet();
                                    DataTable fromDbTable = new DataTable();
                                    dataAdapter.Fill(fromDb, "t");
                                    fromDbTable = fromDb.Tables["t"];
                                    string aServiceId = "";
                                    foreach (DataRow dr in fromDbTable.Rows)
                                        aServiceId = dr[0].ToString();
                                    foreach (AdditionalFee af in MainVM.SelectedAvailedServices.AdditionalFees)
                                    {
                                        query = "INSERT INTO `odc_db`.`fees_per_transaction_t`(`servicesAvailedID`,`feeName`,`feeValue`)" +
                                        " VALUES " +
                                        "('" + aServiceId + "', '" + af.FeeName + "', '" + af.FeePrice + "');";
                                        noError = dbCon.insertQuery(query, dbCon.Connection);
                                    }

                                }
                            }
                            if (noError)
                            {
                                MessageBox.Show("Successfully added.");

                            }

                            else
                                MessageBox.Show("There is an error occured in saving the record");
                        }
                    }
                }
                else
                {
                    string query = "UPDATE `odc_db`.`sales_quote_t` SET " +
                        "`custID` = '" + MainVM.SelectedSalesQuote.custID_ + "'," +
                        "`quoteSubject` = '" + MainVM.SelectedSalesQuote.quoteSubject_ + "'," +
                        "`priceNote` = '" + MainVM.SelectedSalesQuote.priceNote_ + "'," +
                        "`deliveryDate` = '" + MainVM.SelectedSalesQuote.deliveryDate_.ToString("yyyy-MM-dd") + "'," +
                        "`estDelivery` = '" + MainVM.SelectedSalesQuote.estDelivery_ + "'," +
                        "`validityDays` = '" + MainVM.SelectedSalesQuote.validityDays_ + "'," +
                        "`validityDate` = '" + MainVM.SelectedSalesQuote.validityDate_.ToString("yyyy-MM-dd") + "'," +
                        "`otherTerms` = '" + MainVM.SelectedSalesQuote.otherTerms_ + "'," +
                        "`VAT` = '" + MainVM.SelectedSalesQuote.vat_ + "'," +
                        "`vatIsExcluded` = '" + MainVM.SelectedSalesQuote.vatexcluded_ + "'," +
                        "`paymentIsLanded` = '" + MainVM.SelectedSalesQuote.paymentIsLanded_ + "'," +
                        "`paymentCurrency` = '" + MainVM.SelectedSalesQuote.paymentCurrency_ + "'," +
                        "`status` = '" + MainVM.SelectedSalesQuote.status_ + "'," +
                        "`termsDays` = '" + MainVM.SelectedSalesQuote.termsDays_ + "'," +
                        "`termsDP` = '" + MainVM.SelectedSalesQuote.termsDP_ + "'," +
                        "`discountPercent` = '" + MainVM.SelectedSalesQuote.discountPercent_ + "'," +
                        "`surveyReportDoc` = '" + DocData + "'" +
                        " WHERE `sqNoCHar` = '" + MainVM.SelectedSalesQuote.sqNoChar_ +"'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "DELETE FROM `ITEMS_AVAILED_T` WHERE sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_+"'";
                        dbCon.insertQuery(query, dbCon.Connection);
                        
                        foreach (RequestedItem item in MainVM.RequestedItems)
                        {
                            if (item.itemType == 0)
                            {

                                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID.Equals(item.itemID)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`items_availed_t`(`sqNoChar`,`itemID`,`itemQnty`,`unitPrice`)" +
                                    " VALUES " +
                                    "('" + MainVM.SelectedSalesQuote.sqNoChar_ + "', '" + item.itemID + "','" + item.qty + "', '" + item.unitPrice + "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                            }
                            
                        }
                        foreach (AvailedService aserv in MainVM.AvailedServicesList)
                        {
                            query = "DELETE FROM `FEES_PER_TRANSACTION_T` WHERE servicesAvailedID = " + aserv.AvailedServiceID;
                            dbCon.insertQuery(query, dbCon.Connection);

                            query = "DELETE FROM `services_availed_t` WHERE id = " + aserv.AvailedServiceID;
                            dbCon.insertQuery(query, dbCon.Connection);

                            query = "INSERT INTO `odc_db`.`services_availed_t`(`serviceID`,`provinceID`,`sqNoChar`,`city`,`address`,`totalCost`)" +
                                " VALUES " +
                                "('" + aserv.ServiceID + "', '" +
                                aserv.ProvinceID + "', '" +
                                aserv.SqNoChar + "', '" +
                                aserv.City + "', '" +
                                aserv.Address + "', '" +
                                aserv.TotalCost + "');";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                query = "SELECT LAST_INSERT_ID();";
                                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                                DataSet fromDb = new DataSet();
                                DataTable fromDbTable = new DataTable();
                                dataAdapter.Fill(fromDb, "t");
                                fromDbTable = fromDb.Tables["t"];
                                string aServiceId = "";
                                foreach (DataRow dr in fromDbTable.Rows)
                                    aServiceId = dr[0].ToString();
                                foreach (AdditionalFee af in MainVM.SelectedAvailedServices.AdditionalFees)
                                {
                                    query = "INSERT INTO `odc_db`.`fees_per_transaction_t`(`servicesAvailedID`,`feeName`,`feeValue`)" +
                                    " VALUES " +
                                    "('" + aServiceId + "', '" + af.FeeName + "', '" + af.FeePrice + "');";
                                    noError = dbCon.insertQuery(query, dbCon.Connection);
                                }

                            }
                        }
                    }
                }
            }
        }

        void loadSalesQuoteToUi()
        {
            MainVM.SelectedCustomerSupplier = (from cust in MainVM.Customers
                                               where cust.CompanyID == MainVM.SelectedSalesQuote.custID_
                                               select cust).FirstOrDefault();
            MainVM.RequestedItems.Clear();
            MainVM.AvailedServicesList.Clear();

            var invoiceprod = from ai in MainVM.AvailedItems
                              where ai.SqNoChar.Equals(MainVM.SelectedSalesQuote.sqNoChar_)
                              select ai;
            MainVM.AvailedServicesList = new ObservableCollection<AvailedService>(from aser in MainVM.AvailedServices
                                                                                  where aser.SqNoChar.Equals(MainVM.SelectedSalesQuote.sqNoChar_)
                                                                                  select aser);
            foreach (AvailedItem ai in invoiceprod)
            {
                var markupPrice = from itm in MainVM.MarkupHist
                                  where itm.ItemID == ai.ItemID
                                  && itm.DateEffective <= MainVM.SelectedSalesQuote.dateOfIssue_
                                  select itm;
                decimal totalPric = ai.UnitPrice + (ai.UnitPrice / 100 * markupPrice.Last().MarkupPerc);
                MainVM.RequestedItems.Add(new RequestedItem() { availedItemID = ai.AvailedItemID, itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, unitPrice = ai.UnitPrice, totalAmount = totalPric, qtyEditable = true });
            }

            foreach (AvailedService aserv in MainVM.AvailedServicesList)
            {
                var service = from serv in MainVM.ServicesList
                              where serv.ServiceID == aserv.ServiceID
                              select serv;
                MainVM.RequestedItems.Add(new RequestedItem() { availedServiceID = aserv.AvailedServiceID, itemID = aserv.ServiceID, itemType = 1, qty = 0, totalAmount = aserv.TotalCost, unitPrice = service.Last().ServicePrice, additionalFees = aserv.AdditionalFees });
            }

            if (MainVM.isView)
            {
                foreach (UIElement obj in transQuoatationGridForm.Children)
                {
                    if (transQuoatationGridForm.Children.IndexOf(obj) <= 1)
                        obj.IsEnabled = false;
                }
            }


        }

    }
    
}
