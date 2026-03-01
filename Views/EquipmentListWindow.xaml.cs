using ShemaLavanda.Models;
using ShemaLavanda.ViewModels;
using System.Windows;

namespace ShemaLavanda.Views
{
    public partial class EquipmentListWindow : Window
    {
        private EquipmentListViewModel vm => (EquipmentListViewModel)DataContext;

        public EquipmentListWindow()
        {
            InitializeComponent();
            DataContext = new EquipmentListViewModel();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Equipment equipment)
            {
                vm.SelectedEquipment = equipment;
            }
        }
    }
}
