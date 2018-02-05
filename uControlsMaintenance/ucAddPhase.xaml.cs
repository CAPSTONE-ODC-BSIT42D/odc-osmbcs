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
    /// Interaction logic for ucAddPhase.xaml
    /// </summary>
    public partial class ucAddPhase : UserControl
    {
        public ucAddPhase()
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

        public event EventHandler AddPhaseItem;
        protected virtual void OnAddPhaseItem(RoutedEventArgs e)
        {
            var handler = AddPhaseItem;
            if (handler != null)
                handler(this, e);
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private bool validationError;

        private void addPhaseItemBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddPhaseItem(e);
        }
    }
}
