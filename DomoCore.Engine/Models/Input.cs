using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{
    public enum InputState
    {
        Pressed,
        Released
    }

    public class Input
    {
        public int Id { get; set; }
        public int HWValue { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public InputState CurrentState { get; set; }
        public InputState PreviousState { get; set; }
        public bool Changed { get; set; }
    }
}
