using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Turbine
    {
        public int ID { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public DateTime DateLastServiced { get; set; }
        public ICollection<TurbineTelemetrySample> TurbineTelemetrySamples { get; set; }
    }
}
