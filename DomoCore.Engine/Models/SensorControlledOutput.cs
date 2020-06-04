using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{
    public enum SensorCtrlOutputState
    {
        Off,
        On,
        CountingDown,
    }

    public class SensorControlledOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InputId { get; set; }
        public Input Input { get; set; }
        public int OutputId { get; set; }
        public Output Output{ get; set; }
        public bool AutoOff { get; set; }
        public int AutoOffTimeSecs { get; set; }
        public int Counter { get; set; }
        public SensorCtrlOutputState State { get; set; }
    }
}
