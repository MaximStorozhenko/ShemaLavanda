using ShemaLavanda.ViewModels;
using System.Windows;

namespace ShemaLavanda.Views
{
    public partial class EquipmentListWindow : Window
    {
        public EquipmentListWindow(EquipmentListViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
