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
          
            ucReportViewer.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseOrder.rdlc");
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseOrderTableTable", GetPurchase()));
           
            ucReportViewer.LoadReport(rNames);
            ucReportViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewer.RefreshReport();
        }

        private DataTable GetPurchase()
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
                cmd.CommandText = "SELECT        po.PONumChar, cs.companyName, cs.companyType, cs.companyAddress, cs.companyCity, pi.unitPrice * pi.itemQnty AS amount, pi.unitPrice, pi.itemQnty, cs.repFName, cs.repMInitial, cs.repLName, cs.repEmail,   cs.repTelephone, cs.repMobile, po.orderDate, po.requisitioner, po.POdueDate, po.incoterms, po.termsDays, po.termsDP, po.currency, it.ID, ui.unitName, it.itemDescr, po.preparedBy, po.approvedBy, po.shipVia,    po.shipTo FROM            purchase_order_t po INNER JOIN    cust_supp_t cs ON po.suppID = cs.companyID INNER JOIN  po_items_availed_t pi ON pi.poNumChar = po.PONumChar INNER JOIN  item_t it ON it.ID = pi.itemID INNER JOIN  unit_t ui ON it.unitID = ui.id WHERE(po.ponumchar= '" + result + "')";
            else
                cmd.CommandText = "SELECT        po.PONumChar, cs.companyName, cs.companyType, cs.companyAddress, cs.companyCity, pi.unitPrice * pi.itemQnty AS amount, pi.unitPrice, pi.itemQnty, cs.repFName, cs.repMInitial, cs.repLName, cs.repEmail,   cs.repTelephone, cs.repMobile, po.orderDate, po.requisitioner, po.POdueDate, po.incoterms, po.termsDays, po.termsDP, po.currency, it.ID, ui.unitName, it.itemDescr, po.preparedBy, po.approvedBy, po.shipVia,    po.shipTo FROM            purchase_order_t po INNER JOIN    cust_supp_t cs ON po.suppID = cs.companyID INNER JOIN  po_items_availed_t pi ON pi.poNumChar = po.PONumChar INNER JOIN  item_t it ON it.ID = pi.itemID INNER JOIN  unit_t ui ON it.unitID = ui.id WHERE (po.ponumchar= '" + MainVM.SelectedPurchaseOrder.PONumChar + "')";
            

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
