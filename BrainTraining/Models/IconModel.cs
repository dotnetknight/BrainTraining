using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTraining.Models
{
    public class IconModel
    {
        public List<string> Icons { get; set; }
        public int IconsCount { get; set; }
        public int PTZ { get; set; }
        public int Round { get; set; }
    }
}