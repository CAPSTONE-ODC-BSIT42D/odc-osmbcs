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

        public event EventHandler PrintPurchaseOrder;
        protected virtual void OnPrintPurchaseOrderClicked(RoutedEventArgs e)
        {
            var handler = PrintPurchaseOrder;
            if (handler != null)
                handler(this, e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsLoaded && this.IsVisible)
            {
                MainVM.SelectedCustomerSupplier = null;
                foreach (UIElement obj in containerGrid.Children)
                {
                    if (containerGrid.Children.IndexOf(obj) != 0)
                    {
                        obj.Visibility = Visibility.Collapsed;
                    }
                    else
                        obj.Visibility = Visibility.Visible;
                }
                if (MainVM.isNewPurchaseOrder)
                {
                    nextBtn.Visibility = Visibility.Visible;
                    editSalesQuoteBtn.Visibility = Visibility.Collapsed;
                    statusColumn.Visibility = Visibility.Collapsed;
                    selectSupplierBtn.Visibility = Visibility.Visible;
                    selectItemsBtn.Visibility = Visibility.Visible;
                }
                else if (MainVM.isEditPurchaseOrder && MainVM.SelectedPurchaseOrder != null)
                {
                    refreshDataGrid();
                    loadPurchaseOrdertoUI();
                    nextBtn.Visibility = Visibility.Visible;
                    editSalesQuoteBtn.Visibility = Visibility.Collapsed;
                    statusColumn.Visibility = Visibility.Collapsed;
                    selectSupplierBtn.Visibility = Visibility.Visible;
                    selectItemsBtn.Visibility = Visibility.Visible;
                }
                    
                else if (MainVM.isViewPurchaseOrder && MainVM.SelectedPurchaseOrder != null)
                {
                    refreshDataGrid();
                    loadPurchaseOrdertoUI();
                    foreach (UIElement obj in purchaseOrderForm.Children)
                    {
                        if(purchaseOrderForm.Children.IndexOf(obj) != 1)
                            obj.IsEnabled = false;
                    }
                    nextBtn.Visibility = Visibility.Collapsed;
                    editSalesQuoteBtn.Visibility = Visibility.Visible;
                    statusColumn.Visibility = Visibility.Visible;
                    selectSupplierBtn.Visibility = Visibility.Collapsed;
                    selectItemsBtn.Visibility = Visibility.Collapsed;
                }
                    
            }

        }

        private void loadPurchaseOrdertoUI()
        {
            MainVM.SelectedCustomerSupplier = (from supp in MainVM.Suppliers
                                               where supp.CompanyID == MainVM.SelectedPurchaseOrder.suppID
                                               select supp).FirstOrDefault();
            MainVM.RequestedItems.Clear();
            foreach (POAvailedItem ai in MainVM.SelectedPurchaseOrder.AvailedItems)
            {
                if (MainVM.isViewPurchaseOrder)
                    MainVM.RequestedItems.Add(new RequestedItem() { availedItemID = ai.AvailedItemID, itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, totalAmount = ai.ItemQty * ai.UnitPrice, unitPrice = ai.UnitPrice, qtyEditable = true, status = ai.ItemStatus });
                else
                    MainVM.RequestedItems.Add(new RequestedItem() { availedItemID = ai.AvailedItemID, itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, totalAmount = ai.ItemQty * ai.UnitPrice, unitPrice = ai.UnitPrice, qtyEditable = false, status = ai.ItemStatus });
            }

            shipViaCb.SelectedValue = MainVM.SelectedPurchaseOrder.shipVia;
            if (!MainVM.SelectedPurchaseOrder.asapDueDate)
            {
                selectedDateRequiredTb.SelectedDate = MainVM.SelectedPurchaseOrder.POdueDate;
                asapCb.IsChecked = false;
            }
            else
            {
                asapCb.IsChecked = MainVM.SelectedPurchaseOrder.asapDueDate;
            }
            if (MainVM.SelectedPurchaseOrder.termsDp == 50)
                paymentDefaultRd.IsChecked = true;
            else
                paymentCustomRb.IsChecked = true;
            
        }

        void refreshDataGrid()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MainVM.SelectedPurchaseOrder.AvailedItems.Clear();
                string query = "SELECT * FROM po_items_availed_t where poNumCHar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "'";
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
                        UnitPrice = decimal.Parse(dr[4].ToString()),
                        ItemStatus = dr[5].ToString()
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
                        else if (element is DatePicker)
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
                        OnSaveCloseButtonClicked(e);
                        OnPrintPurchaseOrderClicked(e);
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
            MainVM.poNumChar = poNumChar;
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
            if (MainVM.isEditPurchaseOrder)
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
                            "`preparedBy` = " + "'," + //preparedBy
                            "`approvedBy` = " + "'," + //approveBy
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
                    string query = "INSERT purchase_order_t (`PONumChar`,`suppID`,`shipTo`, `POdueDate`,`asapDueDate`,`shipVia`, `requisitioner`, `incoterms`, `POstatus`, `currency`, `importantNotes`, `preparedBy`, `approvedBy`, `refNo`, `termsDays`, `termsDP`) VALUES " +
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

        private void asapCb_Checked(object sender, RoutedEventArgs e)
        {
            selectedDateRequiredTb.IsEnabled = false;
        }

        private void asapCb_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedDateRequiredTb.IsEnabled = true;
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

        private void editSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            itemDg.IsReadOnly = true;
            foreach (UIElement obj in purchaseOrderForm.Children)
            {
                obj.IsEnabled = true;
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void unitPriceTb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(this.IsVisible)
                computePrice();
        }

        private void computePrice()
        {
            foreach (RequestedItem item in MainVM.RequestedItems)
            {
                if (item.itemType == 0)
                {
                    item.totalAmount = (item.unitPrice * item.qty);
                }
                
            }
        }

        private void receiveItemBtn_Click(object sender, RoutedEventArgs e)
        {
            
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "UPDATE po_items_availed_t SET itemStatus = @itemStatus WHERE id = @id";
                cmd.Parameters.AddWithValue("@itemStatus","RECEIVED");

                cmd.Parameters.AddWithValue("@id", MainVM.SelectedRequestedItem.availedItemID);

                cmd.ExecuteNonQuery();
                conn.Close();
                refreshDataGrid();

                var allReceived = MainVM.SelectedPurchaseOrder.AvailedItems.Where(x => x.ItemStatus.Equals("ORDERED"));
                if (allReceived.Count() == 0)
                {
                    conn.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE purchase_order_t SET POStatus = @status WHERE PONumChar = @id";
                    cmd.Parameters.AddWithValue("@status", "RECEIVED");

                    cmd.Parameters.AddWithValue("@id", MainVM.SelectedPurchaseOrder.PONumChar);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    conn.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE purchase_order_t SET POStatus = @status WHERE PONumChar = @id";
                    cmd.Parameters.AddWithValue("@status", "PARTIALLY RECEIVED");

                    cmd.Parameters.AddWithValue("@id", MainVM.SelectedPurchaseOrder.PONumChar);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBox.Show("Successfully updated!");
            }
            refreshDataGrid();
        }
    }
}
