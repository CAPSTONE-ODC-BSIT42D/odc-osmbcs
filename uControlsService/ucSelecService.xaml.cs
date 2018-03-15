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

namespace prototype2.uControlsService
{
    /// <summary>
    /// Interaction logic for ucSelecService.xaml
    /// </summary>
    public partial class ucSelecService : UserControl
    {
        public ucSelecService()
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

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void selectSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                var obj = new ObservableCollection<AvailedService>(from aserv in MainVM.AvailedServices
                                                                   join sq in MainVM.SalesQuotes on aserv.SqNoChar equals sq.sqNoChar_
                                                                   where sq.status_.Equals("ACCEPTED")
                                                                    && (from ss in MainVM.ServiceSchedules_ select ss.ServiceAvailedID).FirstOrDefault() != aserv.AvailedServiceID
                                                                   select aserv);
                selectServices.ItemsSource = obj;
            }
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

    }
}
