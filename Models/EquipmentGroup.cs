using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShemaLavanda.Models
{
    public class EquipmentGroup
    {
        public string Name { get; set; }
        public ObservableCollection<Equipment> Items { get; set; }
    }
}
