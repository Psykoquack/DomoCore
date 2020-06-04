using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{
    public class SwitchTime
    {
        public int ID { get; set; }
        public string DayOfWeek { get; set; }
        public bool Enabled { get; set; }
        public bool SwitchOn { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public ShutterOutput ShutterOutput { get; set; }
        public int ShutterOutputdId { get; set; }
        public SimpleOutput SimpleOutput { get; set; }
        public int SimpleOutputId { get; set; }
    }
}
