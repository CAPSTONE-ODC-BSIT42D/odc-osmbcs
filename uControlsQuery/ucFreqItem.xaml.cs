using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;
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


//Using namespaces 
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.ObjectModel;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqItem.xaml
    /// </summary>
    public partial class ucFreqItem : UserControl
    {
        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public ucFreqItem()
        {
            InitializeComponent();
          
        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                binddatagrid();

            }
            else
            {
                
            }
          
        }
        private void binddatagrid()
        {
            MainVM.FrequentItems = new ObservableCollection<FreqItem>(from item in MainVM.AvailedItems
                                group item by item.ItemID into freqitem
                                select new FreqItem() { ItemID = freqitem.Key, Qty = (from x in freqitem select x.SqNoChar).Distinct().Count() });
            
        }

        void dailyFilter()
        {
            MainVM.FrequentItems.Clear();
            MainVM.FrequentItems = new ObservableCollection<FreqItem>(from item in MainVM.AvailedItems
                                                                      join sq in MainVM.SalesQuotes on item.SqNoChar equals sq.sqNoChar_
                                                                      where (sq.dateOfIssue_.ToShortDateString() == dailyDatePicker.SelectedDate.Value.ToShortDateString()) && (sq.validityDate_ > DateTime.Now)
                                                                      group item by item.ItemID into freqitem
                                                                      select new FreqItem() { ItemID = freqitem.Key, Qty = (from x in freqitem select x.ItemID).Distinct().Count() });
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void comboBoxFreqItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxFreqItems.SelectedIndex == 0)
            {
                dailyDatePicker.Visibility = Visibility.Visible;
            }
        }

        private void dailyDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dailyFilter();
        }
    }
}
