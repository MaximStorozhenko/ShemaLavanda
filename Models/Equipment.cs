using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShemaLavanda.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        public string Code { get; set; }        // BE_3.3
        public string Name { get; set; }
        public string Type { get; set; }        // Нория
        public string Description { get; set; }
        public string RpNumber { get; set; }

        public string PlcTagStart { get; set; }
        public string PlcTagState { get; set; }
    }
}
