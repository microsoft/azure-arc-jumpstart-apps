using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Platform
    {
        public int ID { get; set; }
        public string LongPosition { get; set; }
        public string LatPosition { get; set; }
        public DateTime DateLastInspected { get; set; }
    }
}
