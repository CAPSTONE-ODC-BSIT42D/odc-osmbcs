using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqItem.xaml
    /// </summary>
    public partial class ucFreqItem : UserControl
    {
       
        public ucFreqItem()
        {
            InitializeComponent();
           
        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
       
        }
        private void binddatagrid()
        {







        }

    private void comboBoxFreqItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}
