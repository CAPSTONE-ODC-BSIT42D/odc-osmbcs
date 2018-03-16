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
            if (MainVM.SelectedPhase == null)
            {
                if (MainVM.SelectedService.Phases.Count == 0)
                    MainVM.SelectedService.Phases.Add(new Phase() { PhaseName = phaseItemNameTb.Text, PhaseDesc = phaseDescTb.Text, SequenceNo = MainVM.SelectedService.Phases.Count, LastItem = true, FirstItem = true });
                else if (MainVM.SelectedService.Phases.Count > 0)
                {
                    MainVM.SelectedService.Phases[MainVM.SelectedService.Phases.Count - 1].LastItem = false;
                    MainVM.SelectedService.Phases.Add(new Phase() { PhaseName = phaseItemNameTb.Text, PhaseDesc = phaseDescTb.Text, SequenceNo = MainVM.SelectedService.Phases.Count+1,LastItem = true, FirstItem = false });
                }  
                
            }
            else
            {
                MainVM.SelectedPhase.PhaseName = phaseItemNameTb.Text;
                MainVM.SelectedPhase.PhaseDesc = phaseDescTb.Text;
                MainVM.SelectedPhase.IsModified = true;
            }
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.IsVisible && MainVM.SelectedPhase != null)
            {
                loadDataToUI();
            }
        }

        private void loadDataToUI()
        {
            phaseItemNameTb.Text = MainVM.SelectedPhase.PhaseName;
            phaseDescTb.Text = MainVM.SelectedPhase.PhaseDesc;
        }
    }
}
