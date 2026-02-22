using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShemaLavanda.Models
{
    public class EquipmentItem
    {
        public string Id { get; }
        public string Type { get; }
        public string Name { get; }

        public EquipmentItem(string id, string type)
        {
            Id = id;
            Type = type;//id.Split('_')[0]; // CV, NR, SL...
            Name = id.Replace('_', ' ');
        }
    }
}
