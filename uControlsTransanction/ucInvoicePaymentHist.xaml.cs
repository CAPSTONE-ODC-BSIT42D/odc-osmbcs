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

namespace prototype2.uControlsTransanction
{
    /// <summary>
    /// Interaction logic for ucInvoicePaymentHist.xaml
    /// </summary>
    public partial class ucInvoicePaymentHist : UserControl
    {
        public ucInvoicePaymentHist()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        private bool validationError = false;

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler ReceivePaymentButtonClicked;
        protected virtual void OnReceivePaymentButtonCliked(RoutedEventArgs e)
        {
            var handler = ReceivePaymentButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && MainVM.SelectedSalesInvoice!=null)
            {
                refreshDataGrid();

            }
        }
        void refreshDataGrid()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM si_payment_t where inoviceNo = " + MainVM.SelectedSalesInvoice.invoiceNo_;
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime paymentDate = new DateTime();
                    DateTime.TryParse(dr["SIpaymentDate"].ToString(), out paymentDate);
                    MainVM.SelectedSalesInvoice.PaymentHist_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString() });
                }
                dbCon.Close();
            }
        }

        private void receivePaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            OnReceivePaymentButtonCliked(e);
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
