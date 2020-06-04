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
        public InputState State { get; set; }
    }
}
