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
            DisplayReport();
        }
        private void DisplayReport()
        {
            ucReportViewer.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseOrder.rdlc");
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("purchaseOrderDataTable", GetPurchase()));
            ucReportViewer.LoadReport(rNames);
            ucReportViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewer.RefreshReport();
        }

        private DataTable GetPurchase()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        " +
                                    "po.PONumChar, cs.companyName, cs.companyType, cs.companyAddress, cs.companyCity, pr.provinceName, cs.repFName, cs.repMInitial, cs.repLName, cs.repEmail, cs.repTelephone, cs.repMobile,   " +
                                    "po.orderDate, po.requisitioner, po.POdueDate, po.incoterms, po.termsDays, po.termsDP, po.currency, it.ID, ia.itemQnty, ui.unitName, it.itemDescr, ia.unitprice, po.preparedBy, po.approvedBy,  po.shipVia, po.shipTo " +
                                "FROM            " +
                                    "purchase_order_t po INNER JOIN cust_supp_t cs ON po.suppID = cs.companyID " +
                                "INNER JOIN " +
                                    "item_t it ON it.supplierID = cs.companyID INNER JOIN  items_availed_t ia ON it.ID = ia.itemID " +
                                "INNER JOIN " +
                                    "provinces_t pr ON cs.companyProvinceID = pr.id INNER JOIN  unit_t ui ON it.unitID = ui.id   " +
                                "WHERE  " +
                                    "ia.id=@B";

            DataSet1.PurchaseOrderDataTableDataTable dSPurchase = new DataSet1.PurchaseOrderDataTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;

        }
    }
}
