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
                cmd.CommandText = "SELECT        po.PONumChar, po.suppID, po.shipTo, po.orderDate, po.POdueDate, po.asapDueDate, po.shipVia, i.itemDescr, u.unitName,u.unitshorthand, po.requisitioner, po.incoterms, po.POstatus, po.currency, po.importantNotes,     po.preparedBy, po.approvedBy, po.refNo, po.termsDays, po.termsDP, po.isDeleted, cs.companyID, cs.companyName, cs.busStyle, cs.taxNumber, cs.companyAddInfo, cs.companyAddress, cs.companyCity,     cs.companyProvinceID, cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName, cs.repMInitial, cs.repEmail, cs.repTelephone,    cs.repMobile, cs.companyType, cs.isDeleted AS Expr1, i.ID, i.itemName, TRUNCATE(IFNULL(pa.unitPrice, 0), 2) AS UNIT_PRICE, pa.itemQnty, TRUNCATE(IFNULL(pa.unitPrice, 0) * IFNULL(pa.itemQnty, 0), 2)   AS total_item FROM purchase_order_t po INNER JOIN  po_items_availed_t pa ON po.PONumChar = pa.poNumChar INNER JOIN  item_t i ON i.ID = pa.itemID INNER JOIN  cust_supp_t cs ON cs.companyID = po.suppID INNER JOIN  provinces_t p ON p.id = cs.companyProvinceID INNER JOIN  unit_t u ON i.unitID = u.id WHERE(po.PONumChar = '" + result + "')";
            else
                cmd.CommandText = "SELECT        po.PONumChar, po.suppID, po.shipTo, po.orderDate, po.POdueDate, po.asapDueDate, po.shipVia, i.itemDescr, u.unitName,u.unitshorthand, po.requisitioner, po.incoterms, po.POstatus, po.currency, po.importantNotes,     po.preparedBy, po.approvedBy, po.refNo, po.termsDays, po.termsDP, po.isDeleted, cs.companyID, cs.companyName, cs.busStyle, cs.taxNumber, cs.companyAddInfo, cs.companyAddress, cs.companyCity,     cs.companyProvinceID, cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName, cs.repMInitial, cs.repEmail, cs.repTelephone,    cs.repMobile, cs.companyType, cs.isDeleted AS Expr1, i.ID, i.itemName, TRUNCATE(IFNULL(pa.unitPrice, 0), 2) AS UNIT_PRICE, pa.itemQnty, TRUNCATE(IFNULL(pa.unitPrice, 0) * IFNULL(pa.itemQnty, 0), 2)   AS total_item FROM purchase_order_t po INNER JOIN  po_items_availed_t pa ON po.PONumChar = pa.poNumChar INNER JOIN  item_t i ON i.ID = pa.itemID INNER JOIN  cust_supp_t cs ON cs.companyID = po.suppID INNER JOIN  provinces_t p ON p.id = cs.companyProvinceID INNER JOIN  unit_t u ON i.unitID = u.id WHERE(po.PONumChar = '" + MainVM.SelectedPurchaseOrder.PONumChar + "')";
            

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
