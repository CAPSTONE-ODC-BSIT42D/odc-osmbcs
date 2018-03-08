using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucPurchaseOrder.xaml
    /// </summary>
    public partial class ucPurchaseOrder : UserControl
    {
        public ucPurchaseOrder()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SelectCustomer;
        protected virtual void OnSelectCustomerClicked(RoutedEventArgs e)
        {
            MainVM.isNewPurchaseOrder = true;
            var handler = SelectCustomer;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler SelectSalesQuote;
        protected virtual void OnSelectSalesQuote(RoutedEventArgs e)
        {
            MainVM.isNewPurchaseOrder = true;
            var handler = SelectSalesQuote;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsLoaded && this.IsVisible)
            {
                foreach (UIElement obj in containerGrid.Children)
                {
                    if (containerGrid.Children.IndexOf(obj) != 0)
                    {
                        obj.Visibility = Visibility.Collapsed;
                    }
                    else
                        obj.Visibility = Visibility.Visible;
                }
                if (MainVM.isEdit && MainVM.SelectedPurchaseOrder != null)
                {

                    refreshDataGrid();
                    loadPurchaseOrdertoUI();
                }
                    
                if (MainVM.isView) ;
            }

        }

        private void loadPurchaseOrdertoUI()
        {
            MainVM.SelectedCustomerSupplier = (from supp in MainVM.Suppliers
                                               where supp.CompanyID == MainVM.SelectedPurchaseOrder.suppID
                                               select supp).FirstOrDefault();
            
            foreach (POAvailedItem ai in MainVM.SelectedPurchaseOrder.AvailedItems)
            {
                MainVM.RequestedItems.Add(new RequestedItem() { availedItemID = ai.AvailedItemID, itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, totalAmount = ai.ItemQty * ai.UnitPrice, unitPrice = ai.UnitPrice });
            }

        }

        void refreshDataGrid()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MainVM.SelectedSalesInvoice.PaymentHist_.Clear();
                string query = "SELECT * FROM po_item_availed_t where poNumCHar = " + MainVM.SelectedPurchaseOrder.PONumChar;
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.SelectedPurchaseOrder.AvailedItems.Add(new POAvailedItem()
                    {
                        AvailedItemID = int.Parse(dr[0].ToString()),
                        ItemID = int.Parse(dr[2].ToString()),
                        ItemQty = int.Parse(dr[3].ToString()),
                        UnitPrice = decimal.Parse(dr[4].ToString())
                    });
                }
                dbCon.Close();
            }
        }

        private void selectSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectCustomerClicked(e);
        }


        private void selectItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectSalesQuote(e);
        }

        private bool validationError = false;

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseOrderForm.IsVisible)
            {

                nextBtn.Content = "Save";

            }
            else if (purchaseOrderViewer.IsVisible)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    foreach (UIElement element in poOtherDetails.Children)
                    {
                        if (element is TextBox)
                        {
                            if (element.IsVisible)
                            {
                                BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                                if (expression != null)
                                {
                                    if (((TextBox)element).IsEnabled)
                                    {
                                        expression.UpdateSource();
                                        if (Validation.GetHasError((TextBox)element))
                                            validationError = true;
                                    }
                                }
                            }
                        }
                        else if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                        {
                            BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.TextProperty);
                            Validation.ClearInvalid(expression);
                            if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                            {
                                expression.UpdateSource();
                                if (Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element))
                                    validationError = true;
                            }
                        }
                        else if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            if (expression != null)
                            {
                                expression.UpdateSource();
                                if (Validation.GetHasError((ComboBox)element))
                                    validationError = true;
                            }

                        }
                        else if(element is DatePicker)
                        {
                            BindingExpression expression = ((DatePicker)element).GetBindingExpression(DatePicker.SelectedDateProperty);
                            if (expression != null)
                            {
                                expression.UpdateSource();
                                if (Validation.GetHasError((DatePicker)element))
                                    validationError = true;
                            }
                        }
                    }
                    if (!validationError)
                    {
                        saveDataToDb();
                        MainVM.isEdit = false;
                        OnSaveCloseButtonClicked(e);
                    }
                    else
                    {
                        MessageBox.Show("Resolve the error first");
                        validationError = false;
                    }

                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
        }

        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            string poNumChar = "TRANS-" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + (MainVM.PurchaseOrder.Count+1);
            int termsDay = 30;
            decimal termsDp = 50;
            if ((bool)paymentDefaultRd.IsChecked)
            {
                termsDay = 30;
                termsDp = 50;
            }
            else
            {
                //termsDp = downpaymentPercentTb.Value;
            }
            if (MainVM.isEdit)
            {
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `odc_db`.`purchase_order_t` SET  " +
                            "`suppID` = " + MainVM.SelectedCustomerSupplier.CompanyID + "," +
                            "`shipTo` = ''," +
                            "`POdueDate` = '" + selectedDateRequiredTb.SelectedDate.Value.ToString("yyyy-MM-dd") + "'," +
                            "`asapDueDate` = '" + (bool)asapCb.IsChecked + "'," +
                            "`shipVia` = '" + shipViaCb.SelectedValue + "'," +
                            "`requisitioner` = '" + "'," + //requisitionaer
                            "`incoterms` = '" + "'," + //incoterms
                            "`POstatus` = 'PENDING" + "', " +
                            "`currency` = '" + currencyCb.SelectedValue.ToString() + "'," +
                            "`importantNotes` = " + "'," + //importantNotes
                            "`prepareBy` = " + "'," + //preparedBy
                            "`approveBy` = " + "'," + //approveBy
                            "`refNo` = " + "'," + //refNo
                            "`termsDays` = " + termsDay + "," +
                            "`termsDp` = " + termsDp +
                            "' WHERE PONumChar = '"+ MainVM.SelectedPurchaseOrder.PONumChar+"'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "DELETE FROM `po_items_availed_t` WHERE PONumChar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "'";
                        dbCon.insertQuery(query, dbCon.Connection);
                        foreach (RequestedItem item in MainVM.RequestedItems)
                        {
                            if (item.itemType == 0)
                            {

                                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID.Equals(item.itemID)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`po_items_availed_t`(`poNumChar`,`itemID`,`itemQnty`,`unitPrice`)" +
                                    " VALUES " +
                                    "('" + MainVM.SelectedPurchaseOrder.PONumChar + "', '" + item.itemID + "','" + item.qty + "', '" + item.unitPrice + "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                            }
                        }
                        MessageBox.Show("Successfully saved.");

                    }
                }
            }
            else
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT purchase_order_t (`PONumChar`,`suppID`,`shipTo`, `POdueDate`,`asapDueDate`,`shipVia`, `requisitioner`, `incoterms`, `POstatus`, `currency`, `importantNotes`, `preparedBy`, `approveBy`, `refNo`, `termsDays`, `termsDP`) VALUES " +
                            "('" + poNumChar + "'," +
                            MainVM.SelectedCustomerSupplier.CompanyID + "," +
                            "'','" +
                            selectedDateRequiredTb.SelectedDate.Value.ToString("yyyy-MM-dd") + "'," +
                            (bool)asapCb.IsChecked + ",'" +
                            shipViaCb.SelectedValue + "','" +
                            "','" + //requisitionaer
                            "','" + //incoterms
                            "PENDING" + "', '" +
                            currencyCb.SelectedValue.ToString() + "','" +
                            "','" + //importantNotes
                            "','" + //preparedBy
                            "','" + //approveBy
                            "'," + //refNo
                            termsDay + "," +
                            termsDp +
                            ");";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        foreach (RequestedItem item in MainVM.RequestedItems)
                        {
                            if (item.itemType == 0)
                            {

                                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ID.Equals(item.itemID)).FirstOrDefault();
                                query = "INSERT INTO `odc_db`.`po_items_availed_t`(`poNumChar`,`itemID`,`itemQnty`,`unitPrice`)" +
                                    " VALUES " +
                                    "('" + poNumChar + "', '" + item.itemID + "','" + item.qty + "', '" + item.unitPrice + "');";
                                noError = dbCon.insertQuery(query, dbCon.Connection);
                            }
                        }
                        MessageBox.Show("Successfully added.");
                    }
                }
            }
            
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(UIElement obj in containerGrid.Children)
            {
                if (containerGrid.Children.IndexOf(obj) == 0)
                {
                    
                }
                else if(containerGrid.Children.IndexOf(obj) == 1)
                {

                }
            }
        }

        private void paymentCustomRb_Checked(object sender, RoutedEventArgs e)
        {
            downpaymentPercentTb.IsEnabled = true;
        }

        private void paymentCustomRb_Unchecked(object sender, RoutedEventArgs e)
        {
            downpaymentPercentTb.IsEnabled = false;
        }

        private void addNewShipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (newShipViaTb.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = "odc_db";
                if (String.IsNullOrEmpty(newShipViaTb.Text))
                {
                    MessageBox.Show("Please enter a Category");
                }
                else if (!String.IsNullOrEmpty(newShipViaTb.Text))
                {
                    addNewShipBtn.Content = "+";
                    newShipViaTb.Visibility = Visibility.Collapsed;
                    string query = "INSERT INTO `odc_db`.`Ship_Via_t` (`name`) VALUES('" + newShipViaTb.Text + "')";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "SELECT LAST_INSERT_ID();";
                        string result = dbCon.selectScalar(query, dbCon.Connection).ToString();
                        MainVM.ShipVia.Add(new ShipVia() { ShipViaID = int.Parse(result), Name = newShipViaTb.Text });
                        MessageBox.Show("Successfully added");
                        newShipViaTb.Clear();
                        dbCon.Close();
                    }
                }

                
            }
            else
            {

                addNewShipBtn.Content = "Save";
                newShipViaTb.Visibility = Visibility.Visible;
            }
        }
    }
}
