using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucInvoice.xaml
    /// </summary>
    public partial class ucInvoice : UserControl
    {
        public ucInvoice()
        {
            InitializeComponent();
        }

        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

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
                    if(element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        Validation.ClearInvalid(expression);
                        if (((TextBox)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }
                    }
                    if (element is Xceed.Wpf.Toolkit.IntegerUpDown)
                    {
                        BindingExpression expression = ((Xceed.Wpf.Toolkit.IntegerUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.IntegerUpDown.ValueProperty);
                        Validation.ClearInvalid(expression);
                        if (((Xceed.Wpf.Toolkit.IntegerUpDown)element).IsEnabled)
                        {

                            expression.UpdateSource();
                            validationError = Validation.GetHasError((Xceed.Wpf.Toolkit.IntegerUpDown)element);
                        }
                    }
                }
                if (!validationError && MainVM.LoginEmployee_ != null)
                {
                    
                    salesInvoiceToMemory();
                    newInvoiceForm.Visibility = Visibility.Collapsed;
                    documentViewer.Visibility = Visibility.Visible;
                    invoiceNext.Content = "Save";
                    InvoiceDocument df = new InvoiceDocument();
                    document = df.CreateDocument(sqNo: MainVM.SelectedSalesInvoice.sqNoChar_, author: "admin");
                    string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                    documentViewer.pagePreview.Ddl = ddl;
                }
                else
                {
                    MessageBox.Show("");
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

        private void invoiceBack_Click(object sender, RoutedEventArgs e)
        {
            if (newInvoiceForm.IsVisible)
            {
                newInvoiceForm.Visibility = Visibility.Collapsed;
                documentViewer.Visibility = Visibility.Visible;
                OnSaveCloseButtonClicked(e);
            }
            else if (documentViewer.IsVisible)
            {

            }
        }

        void computeInvoice()
        {
            
            MainVM.RequestedItems.Clear();
            foreach (AddedItem item in MainVM.SelectedSalesQuote.AddedItems)
            {
                MainVM.SelectedProduct = MainVM.ProductList.Where(x => x.ItemCode.Equals(item.ItemCode)).First();
                MainVM.InvoiceItems.Add(new InvoiceItem()
                {
                    lineNo = (MainVM.RequestedItems.Count + 1).ToString(),
                    itemCode = item.ItemCode,
                    desc = MainVM.SelectedProduct.ItemDesc,
                    itemName = MainVM.SelectedProduct.ItemName,
                    qty = item.ItemQty,
                    unitPrice = Math.Round(item.TotalCost - (item.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01))/item.ItemQty, 3),
                    totalAmount = Math.Round(item.TotalCost - (item.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01)),3),
                    itemType = 0
                });
                MainVM.VatableSale += Math.Round(item.TotalCost - (item.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01)), 3);
                MainVM.TotalSalesWithOutDp += Math.Round(item.TotalCost, 3);

            }
            foreach (AddedService service in MainVM.SelectedSalesQuote.AddedServices)
            {
                MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(service.ServiceID)).First();
                MainVM.SelectedProvince = MainVM.Provinces.Where(x => x.ProvinceID == service.ProvinceID).First();
                MainVM.InvoiceItems.Add(new InvoiceItem()
                {
                    lineNo = (MainVM.RequestedItems.Count + 1).ToString(),
                    itemCode = service.TableNoChar,
                    desc = MainVM.SelectedService.ServiceDesc,
                    itemName = MainVM.SelectedService.ServiceName,
                    qty = 1,
                    unitPrice = Math.Round(service.TotalCost - (service.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01)), 3),
                    totalAmount = Math.Round(service.TotalCost - (service.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01)), 3),
                    itemType = 1,
                    additionalFees = service.AdditionalFees
                });
                MainVM.VatableSale += Math.Round(service.TotalCost - (service.TotalCost * (MainVM.SelectedSalesQuote.termsDP_ * (decimal)0.01)), 3);
                MainVM.TotalSalesWithOutDp += Math.Round(service.TotalCost, 3);
            }
            MainVM.TotalSalesNoVat = MainVM.VatableSale;
            MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));
            MainVM.VatAmount = Math.Round(MainVM.VatAmount, 3);
            MainVM.TotalSales = MainVM.VatableSale + (MainVM.VatableSale * ((decimal)0.12));
            MainVM.TotalSales = Math.Round(MainVM.TotalSales, 3);
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

            DateTime dateOfIssue = new DateTime();
            dateOfIssue = DateTime.Now;

            DateTime dueDate = new DateTime();
            dueDate = dateOfIssue.AddDays(int.Parse(dueDateTb.Value.ToString()));

            MainVM.SelectedSalesInvoice = (new SalesInvoice() { sqNoChar_ = MainVM.SelectedSalesQuote.sqNoChar_, busStyle_ = busStyleTb.Text, tin_ = tinNumTb.Text, custID_ = MainVM.SelectedSalesQuote.custID_, empID_ = int.Parse(MainVM.LoginEmployee_.EmpID), dueDate_ = dueDate, vat_ = MainVM.VatAmount_, withholdingTax_ = MainVM.WithHoldingTax_, purchaseOrderNumber_ = purchaseOrdNumTb.Text, terms_ = (int)dueDateTb.Value});

       
        }


        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            bool noError = true;
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO `odc_db`.`sales_invoice_t`(`custID`,`empID`,`sqNoChar`,`TIN`,`busStyle`,`dateOfIssue`,`termDays`,`dueDate`,`purchaseOrderNumber`,`paymentStatus`,`vat`,`sc_pwd_dsicount`,`withholdingTax`,`notes`)" +
                    " VALUES " +
                    "('" +
                    MainVM.SelectedSalesInvoice.custID_ + "','" +
                    MainVM.SelectedSalesInvoice.empID_ + "','" +
                    MainVM.SelectedSalesInvoice.sqNoChar_ + "','" +
                    MainVM.SelectedSalesInvoice.tin_ + "','" +
                    MainVM.SelectedSalesInvoice.busStyle_ + "','" +
                    MainVM.SelectedSalesInvoice.dateOfIssue_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesInvoice.terms_ + "','" +
                    MainVM.SelectedSalesInvoice.dueDate_.ToString("yyyy-MM-dd") + "','" +
                    MainVM.SelectedSalesInvoice.purchaseOrderNumber_ + "','" +
                    MainVM.SelectedSalesInvoice.paymentStatus_ + "','" +
                    MainVM.SelectedSalesInvoice.vat_ + "','" +
                    MainVM.SelectedSalesInvoice.sc_pwd_discount_ + "','" +
                    MainVM.SelectedSalesInvoice.withholdingTax_ + "','" +
                    MainVM.SelectedSalesInvoice.notes_ +
                    "'); ";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    query = "SELECT invoiceNo FROM sales_invoice_t ORDER BY invoiceNo DESC LIMIT 1;";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    MainVM.SalesQuotes.Clear();
                    foreach (DataRow dr in fromDbTable.Rows)
                    {
                        query = "INSERT INTO `odc_db`.`payment_hist_t` " +
                        "(`custBalance`,`invoiceNo`) " +
                        "VALUES " +
                        "('" +
                        MainVM.TotalSales + "','" +
                        dr["invoiceNo"].ToString() + "')";
                        dbCon.insertQuery(query, dbCon.Connection);
                        query = "INSERT INTO `odc_db`.`payment_hist_t` " +
                        "(`custBalance`,`invoiceNo`) " +
                        "VALUES " +
                        "('" +
                        (MainVM.TotalSales - MainVM.TotalSalesWithOutDp) + "','" +
                        dr["invoiceNo"].ToString() + "')";
                        dbCon.insertQuery(query, dbCon.Connection);
                    }
                    MessageBox.Show("Inovice is Saved");
                }
            }
        }
        

        private void UserControlInvoice_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                computeInvoice();
            }
        }
    }
}
