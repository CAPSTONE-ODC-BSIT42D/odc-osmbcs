using System;
using System.Collections.Generic;
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
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System.Data;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for UserControl6.xaml
    /// </summary>
    public partial class ucPurchaseOrderViewer : UserControl
    {
        public ucPurchaseOrderViewer()
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


        private void DisplayReport()
        {

            ucReportViewer.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseOrder.rdlc");
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseOrderTableTable", GetPurchase()));
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetPo", GetPurchaseItem()));

            ucReportViewer.LoadReport(rNames);
            ucReportViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewer.RefreshReport();
        }
           private DataTable GetPurchaseItem()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            string query = "SELECT LAST_INSERT_ID();";
            string result = dbCon.selectScalar(query, dbCon.Connection).ToString();

            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedPurchaseOrder == null)
                cmd.CommandText = "SELECT        ponumchar, shipTo, orderDate, poduedate, asapDueDate, requisitioner, incoterms, currency, importantNotes, preparedBy, approvedBy, refNo, termsDays, termsDP, busStyle, taxNumber, CompanyAddInfo,    companyAddress, companyCity, companyPostalCode, companyEmail, companyTelephone, companyMobile, repTitle, repLname, repFName, repMInitial, repEmail, repMobile, companyName, id, itemName, UNIT,    UNIT_PRICE, itemQnty, total_item FROM            po_view WHERE(ponumchar = '" + result + "') GROUP BY ponumchar";
            else
                cmd.CommandText = "SELECT        ponumchar, shipTo, orderDate, poduedate, asapDueDate, requisitioner, incoterms, currency, importantNotes, preparedBy, approvedBy, refNo, termsDays, termsDP, busStyle, taxNumber, CompanyAddInfo,    companyAddress, companyCity, companyPostalCode, companyEmail, companyTelephone, companyMobile, repTitle, repLname, repFName, repMInitial, repEmail, repMobile, companyName, id, itemName, UNIT,    UNIT_PRICE, itemQnty, total_item FROM            po_view WHERE(ponumchar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "') GROUP BY ponumchar";


            DataSet1.PoItemDataTable dSItem = new DataSet1.PoItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;
        }

        private DataTable GetPurchase()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            if (MainVM.SelectedPurchaseOrder == null)
                MainVM.SelectedPurchaseOrder = (from po in MainVM.PurchaseOrder
                                            orderby po.PONumChar descending
                                            select po).FirstOrDefault();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT        po.PONumChar, po.suppID, po.shipTo, po.orderDate, po.POdueDate, po.asapDueDate, po.shipVia, po.requisitioner, po.incoterms, po.POstatus, po.currency, po.importantNotes, po.preparedBy, po.approvedBy,    po.refNo, po.termsDays, po.termsDP, po.isDeleted, cs.companyID, cs.companyName, cs.busStyle, cs.taxNumber, cs.companyAddInfo, cs.companyAddress, cs.companyCity, cs.companyProvinceID,     cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName, cs.repMInitial, cs.repEmail, cs.repTelephone, cs.repMobile, cs.companyType,      cs.isDeleted AS Expr1 FROM            purchase_order_t po INNER JOIN    cust_supp_t cs ON po.suppID = cs.companyID WHERE(po.PONumChar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "')";

            DataSet1.PODataTableDataTable dSItem = new DataSet1.PODataTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem; 
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();
            }
            else
            {
                ucReportViewer.Reset();
            }
        }
        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
