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
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.SalesQuotes.Where(x => x.sqNoChar_.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()) && !(x.status_.Equals("ACCEPTED")));
            var observable = new ObservableCollection<SalesQuote>(linqResults);
            selectSalesQuote.ItemsSource = observable;
        }

        private void selectSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
