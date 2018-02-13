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
            if (!MainVM.isEdit)
            {
                if (MainVM.SelectedPhaseGroup.PhaseItems.Count == 0)
                    MainVM.SelectedPhaseGroup.PhaseItems.Add(new Phase() { PhaseName = phaseItemNameTb.Text, PhaseDesc = phaseDescTb.Text, SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.Count, LastItem = true, FirstItem = true });
                else if (MainVM.SelectedPhaseGroup.PhaseItems.Count > 0)
                {
                    MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 1].LastItem = false;
                    MainVM.SelectedPhaseGroup.PhaseItems.Add(new Phase() { PhaseName = phaseItemNameTb.Text, PhaseDesc = phaseDescTb.Text, SequenceNo = MainVM.SelectedPhaseGroup.PhaseItems.Count+1,LastItem = true, FirstItem = false });
                }  
                
            }
            else
            {
                MainVM.SelectedPhase.PhaseName = phaseItemNameTb.Text;
                MainVM.SelectedPhase.PhaseDesc = phaseDescTb.Text;
            }
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
