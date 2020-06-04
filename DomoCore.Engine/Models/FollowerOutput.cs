using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Models
{

    public class FollowerOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InputId { get; set; }
        public Input Input { get; set; }
        public int OutputId { get; set; }
        public Output Output { get; set; }
    }
}
