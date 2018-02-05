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

namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucAddPhaseItem.xaml
    /// </summary>
    public partial class ucAddPhaseItem : UserControl
    {
        public ucAddPhaseItem()
        {
            InitializeComponent();
        }


        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private bool validationError;

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
