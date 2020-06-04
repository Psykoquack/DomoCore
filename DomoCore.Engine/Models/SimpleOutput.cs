using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{
    public enum SimpleOutputState
    {
        Off,
        On,
        CountingDown,
    }

    public class SimpleOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Input Input { get; set; }
        public int InputId { get; set; }
        public Output Output { get; set; }
        public int OutputId { get; set; }
        public List<SwitchTime> SwitchTimes { get; set; }
        public bool AutoOff { get; set; }
        public int AutoOffTimeSecs { get; set; }
        public SimpleOutputState State { get; set; }
    }
}
