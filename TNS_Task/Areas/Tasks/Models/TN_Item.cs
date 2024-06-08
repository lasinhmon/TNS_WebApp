using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class TN_Item
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public object Obj { get; set; }
        public TN_Item() { }
    }
}
