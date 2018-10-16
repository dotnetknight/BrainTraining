using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTraining.Models
{
    public class ColorsModel
    {
        public string ColorToShow { get; set; }
        public string ColorText { get; set; }
        public string ColorVal { get; set; }
        public int RoundMaxScore { get; set; }
        public int RoundInterval { get; set; }
    }
}