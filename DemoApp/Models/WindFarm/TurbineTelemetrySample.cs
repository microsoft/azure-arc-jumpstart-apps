using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class TurbineTelemetrySample
    {
        public Int64 ID { get; set; }
        public Double GearboxOilLevel { get; set; }
        public Double GearboxOilTemp { get; set; }
        public Double GeneratorActivePower { get; set; }
        public Double GeneratorSpeed { get; set; }
        public Double GeneratorTorque { get; set; }
        public Double GridFrequency { get; set; }
        public Double GridVoltage { get; set; }
        public Double HydraulicOilPressure { get; set; }
        public Double NacelleAngle { get; set; }
        public Double OverallWindDirection { get; set; }
        public Double WindSpeedStdDev { get; set; }
        public Boolean Precipitation { get; set; }
        public Double TurbineWindDirection { get; set; }
        public Double TurbineSpeedStdDev { get; set; }
        public Double WindSpeedAverage { get; set; }
        public Double WindTempAverage { get; set; }
        public Double PitchAngle { get; set; }
        public Double Vibration { get; set; }
        public Double TurbineSpeedAverage { get; set; }
        public Boolean AlterBlades { get; set; }
    }
}

