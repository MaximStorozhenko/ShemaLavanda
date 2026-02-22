using ShemaLavanda.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShemaLavanda.ViewModels
{
    public class EquipmentListViewModel
    {
        public ObservableCollection<EquipmentItem> Items { get; } = new();
    }
}
