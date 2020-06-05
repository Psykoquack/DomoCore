using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{
    public enum OutputState
    {
        On,
        Off
    }


    public class Output
    {
        public int Id { get; set; }
        public int HWValue { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public OutputState State { get; set; }
        public bool Changed { get; set; }
    }
}
