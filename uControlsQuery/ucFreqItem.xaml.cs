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
            binddatagrid();
        }
        private void binddatagrid()
        {







        }

    private void comboBoxFreqItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
