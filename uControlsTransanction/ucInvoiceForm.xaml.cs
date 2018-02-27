using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucInvoiceForm.xaml
    /// </summary>
    public partial class ucInvoiceForm : UserControl
    {
        public ucInvoiceForm()
        {
            InitializeComponent();
        }
        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        DateTime dateOfIssue = new DateTime();
        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }
        Document document;
        private void invoiceNext_Click(object sender, RoutedEventArgs e)
        {
            if (newInvoiceForm.IsVisible)
            {
                foreach (var element in newInvoiceFormGrid.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        if (((TextBox)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((TextBox)element))
                                validationError = true;
                        }
                    }

                }
                if (!validationError /*&& MainVM.LoginEmployee_ != null*/)
                {
                    salesInvoiceToMemory();
                    newInvoiceForm.Visibility = Visibility.Collapsed;
                    documentViewer.Visibility = Visibility.Visible;
                    invoiceNext.Content = "Save";
                    InvoiceDocument df = new InvoiceDocument();
                    document = df.CreateDocument(sqNo: MainVM.SelectedSalesInvoice.sqNoChar_, author: "admin");
                    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                    documentViewer.pagePreview.Ddl = ddl;
                    validationError = false;
                }
                else
                {
                    MessageBox.Show("Resolve the error first");
                    validationError = false;
                }
            }
            else if (documentViewer.IsVisible)
            {
                invoiceNext.Content = "Next";
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = document;
                renderer.RenderDocument();
                string filename = @"d:\test\" + MainVM.SelectedSalesInvoice.sqNoChar_ + "-INVOICE.pdf";
                saveDataToDb();
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "" + MainVM.SelectedSalesInvoice.sqNoChar_ + "-INVOICE";
                dlg.DefaultExt = ".pdf";
                dlg.Filter = "PDF documents (.pdf)|*.pdf";
                if (dlg.ShowDialog() == true)
                {
                    filename = dlg.FileName;
                    renderer.PdfDocument.Save(filename);
                }
                OnSaveCloseButtonClicked(e);

            }

        }

        private void invoiceBack_Click(object sender, RoutedEventArgs e)
        {if (newInvoiceForm.IsVisible)
            {
                newInvoiceForm.Visibility = Visibility.Collapsed;
                documentViewer.Visibility = Visibility.Collapsed;
            }
            else if (documentViewer.IsVisible)
            {
                newInvoiceForm.Visibility = Visibility.Visible;
                documentViewer.Visibility = Visibility.Collapsed;
               
            }
        }

        


        void computeInvoice()
        {
            MainVM.VatableSale = 0;
            MainVM.TotalSalesWithOutDp = 0;

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
                MainVM.RequestedItems.Add(new RequestedItem() { itemID = ai.ItemID, itemType = 0, qty = ai.ItemQty, totalAmount = ai.TotalCost, unitPrice = unitPric });
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

            MainVM.TotalSalesNoVat = Math.Round(MainVM.VatableSale, 2);

            MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));
            MainVM.VatAmount = Math.Round(MainVM.VatAmount, 2);

            MainVM.TotalSales = MainVM.VatableSale + MainVM.VatAmount;
            MainVM.TotalSales = Math.Round(MainVM.TotalSales, 2);

            dateOfIssue = DateTime.Now;
            dateToday.Content = dateOfIssue.ToShortDateString();
        }

        void salesInvoiceToMemory()
        {
            string noChar = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbs = "01234567890123456789";

            var random = new Random();


            if (MainVM.SelectedCustomerSupplier.CompanyName.Length > 5)
            {
                noChar = MainVM.SelectedCustomerSupplier.CompanyName.Trim().Substring(0, 5).ToUpper();

            }
            else
                noChar = MainVM.SelectedCustomerSupplier.CompanyName.Trim().ToUpper();
            string stringChars = "";
            for (int i = 0; i < 4; i++)
            {
                if (!(i > 3))
                {
                    stringChars += chars[random.Next(chars.Length)];
                }
            }
            stringChars += "SI";
            stringChars += MainVM.SalesInvoice.Count + 1;
            stringChars += "-";
            foreach (char c in noChar)
            {
                stringChars += c;
            }
            stringChars += "-";
            stringChars += DateTime.Now.ToString("yyyy-MM-dd");




            DateTime dueDate = new DateTime();
            dueDate = dateOfIssue.AddDays(int.Parse(dueDateTb.Value.ToString()));

            MainVM.SelectedSalesInvoice = (new SalesInvoice() { sqNoChar_ = MainVM.SelectedSalesQuote.sqNoChar_, busStyle_ = busStyleTb.Text, tin_ = tinNumTb.Text, custID_ = MainVM.SelectedSalesQuote.custID_, dueDate_ = dueDate, vat_ = MainVM.VatAmount_, withholdingTax_ = MainVM.WithHoldingTax_, purchaseOrderNumber_ = purchaseOrdNumTb.Text, terms_ = (int)dueDateTb.Value });


        }


        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO `odc_db`.`sales_invoice_t`(`custID`,`sqNoChar`,`TIN`,`busStyle`,`dateOfIssue`,`termsDays`,`dueDate`,`purchaseOrderNumber`,`vat`,`sc_pwd_discount`,`withholdingTax`,`notes`)" +
                    " VALUES " +
                    "('" +
                    MainVM.SelectedSalesInvoice.custID_ + "','" +
                    MainVM.SelectedSalesInvoice.sqNoChar_ + "','" +
                    MainVM.SelectedSalesInvoice.tin_ + "','" +
                    MainVM.SelectedSalesInvoice.busStyle_ + "','" +
                    MainVM.SelectedSalesInvoice.dateOfIssue_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesInvoice.terms_ + "','" +
                    MainVM.SelectedSalesInvoice.dueDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesInvoice.purchaseOrderNumber_ + "','" +
                    MainVM.SelectedSalesInvoice.vat_ + "','" +
                    MainVM.SelectedSalesInvoice.sc_pwd_discount_ + "','" +
                    MainVM.SelectedSalesInvoice.withholdingTax_ + "','" +
                    MainVM.SelectedSalesInvoice.notes_ +
                    "'); ";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    query = "UPDATE `sales_quote_t` SET status = '" + "ACCEPTED" + "' WHERE sqNoChar = '" + MainVM.SelectedSalesInvoice.sqNoChar_ + "'";
                    dbCon.insertQuery(query, dbCon.Connection);
                    MessageBox.Show("Invoice is Saved");
                }
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && MainVM.isPaymentInvoice)
            {
                

                computeInvoice();
            }
        }
    }


}
