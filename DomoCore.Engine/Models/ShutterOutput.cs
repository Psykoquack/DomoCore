using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{

    public enum ShutterOutputState
    {
        Off,
        SetDirection,
        SetPower,
        Run,
        ClearPower,
        ClearDirection
    }

    public enum ShutterDirection
    {
        Up,
        Down
    }

    public class ShutterOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Input Input { get; set; }
        public int InputId { get; set; }
        public Output DirectionOutput { get; set; }
        public int DirectionOutputId { get; set; }
        public Output PowerOutput { get; set; }
        public int PowerOutputId { get; set; }
        public ShutterDirection Direction { get; set; }
        public ShutterOutputState State { get; set; }
        public List<SwitchTime> SwitchTimes { get; set; }
        public int RunTimeSecs { get; set; }
        public int Counter { get; set; }
    }
}
