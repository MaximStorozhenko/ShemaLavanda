using ShemaLavanda.Models;
using ShemaLavanda.Services;
using System.Collections.ObjectModel;

namespace ShemaLavanda.ViewModels
{
    public class EquipmentListViewModel : ViewModelBase
    {
        private readonly EquipmentService service = new();

        public ObservableCollection<EquipmentGroup> Groups { get; }

        private Equipment selectedEquipment = new();
        public Equipment SelectedEquipment
        {
            get => selectedEquipment;
            set => Set(ref selectedEquipment, value);
        }

        public EquipmentListViewModel()
        {
            var list = service.GetAll();

            Groups = new ObservableCollection<EquipmentGroup>(
                list.GroupBy(e => e.Type)
                    .Select(g => new EquipmentGroup
                    {
                        Name = g.Key,
                        Items = new ObservableCollection<Equipment>(g)
                    }));
        }
    }
}
