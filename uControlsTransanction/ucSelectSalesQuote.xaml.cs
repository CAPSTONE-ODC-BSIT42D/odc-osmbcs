using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace prototype2.uControlsTransanction
{
    /// <summary>
    /// Interaction logic for ucSelectSalesQuote.xaml
    /// </summary>
    public partial class ucSelectSalesQuote : UserControl
    {
        public ucSelectSalesQuote()
        {
            InitializeComponent();
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.SalesQuotes.Where(x => x.sqNoChar_.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()) && !(x.status_.Equals("ACCEPTED")));
            var observable = new ObservableCollection<SalesQuote>(linqResults);
            selectSalesQuote.ItemsSource = observable;
        }

        private void selectSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.isNewPurchaseOrder)
            {
                MainVM.SelectedCustomerSupplier = (from cust in MainVM.Customers
                                                   where cust.CompanyID == MainVM.SelectedSalesQuote.custID_
                                                   select cust).FirstOrDefault();

                var invoiceprod = from ai in MainVM.AvailedItems
                                  where ai.SqNoChar.Equals(MainVM.SelectedSalesQuote.sqNoChar_)
                                  select ai;
                var invoiceserv = from aser in MainVM.AvailedServices
                                  where aser.SqNoChar.Equals(MainVM.SelectedSalesQuote.sqNoChar_)
                                  select aser;
                foreach (AvailedItem ai in invoiceprod)
                {
                    var markupPrice = from itm in MainVM.MarkupHist
                                      where itm.ItemID == ai.ItemID
                                      && itm.DateEffective <= MainVM.SelectedSalesQuote.dateOfIssue_
                                      select itm;
                    decimal unitPric = ai.TotalCost - (ai.TotalCost / 100 * markupPrice.Last().MarkupPerc);
                    MainVM.RequestedItems.Add(new RequestedItem() { availedItemID = ai.AvailedItemID,itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, totalAmount = ai.TotalCost, unitPrice = unitPric });
                    MainVM.VatableSale += Math.Round(ai.TotalCost, 2);
                }

                foreach (AvailedService aserv in invoiceserv)
                {
                    var service = from serv in MainVM.ServicesList
                                  where serv.ServiceID == aserv.ServiceID
                                  select serv;
                    MainVM.RequestedItems.Add(new RequestedItem() { itemID = aserv.ServiceID, itemType = 0, qty = 0, totalAmount = aserv.TotalCost, unitPrice = service.Last().ServicePrice });
                    MainVM.VatableSale += Math.Round(aserv.TotalCost, 2);
                }
            }
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                if (MainVM.isNewPurchaseOrder)
                {
                    var observable = new ObservableCollection<SalesQuote>(from sq in MainVM.SalesQuotes where sq.status_.Equals("ACCEPTED") select sq);
                    selectSalesQuote.ItemsSource = observable;
                }
                else if (MainVM.isPaymentInvoice)
                {
                    var observable = new ObservableCollection<SalesQuote>(from sq in MainVM.SalesQuotes where sq.status_.Equals("PENDING") select sq);
                    selectSalesQuote.ItemsSource = observable;
                }
                
            }
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
