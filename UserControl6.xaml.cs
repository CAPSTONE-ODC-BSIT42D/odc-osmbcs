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
    public partial class UserControl6 : UserControl
    {
        public UserControl6()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            Purchase_order.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetPurchase()));
            Purchase_order.LocalReport.ReportEmbeddedResource = "prototype2.Report8.rdlc";
            Purchase_order.RefreshReport();
        }

        private DataTable GetPurchase()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.PONumChar, cs.companyName, cs.companyType, cs.companyAddress, cs.companyCity, pr.locProvince, cs.repFName, cs.repMInitial, cs.repLName, cs.repEmail, cs.repTelephone, cs.repMobile, po.orderDate,  po.requisitioner, po.POdueDate, po.incoterms, po.termsDays, po.termsDP, po.currency, it.itemCode, ia.itemQnty, it.itemUnit, it.itemDescr, it.salesPrice, ia.totalCost, po.preparedBy, po.approvedBy, po.shipVia,  po.shipTo FROM   purchase_order_t po INNER JOIN cust_supp_t cs ON po.suppID = cs.companyID INNER JOIN item_t it ON it.supplierID = cs.companyID INNER JOIN  items_availed_t ia ON it.itemCode = ia.itemCode INNER JOIN provinces_t pr ON cs.companyProvinceID = pr.locProvinceID";

            DataSet1.DataTable5DataTable dSPurchase = new DataSet1.DataTable5DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;

        }
    }
}
