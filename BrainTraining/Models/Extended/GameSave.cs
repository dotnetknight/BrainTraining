using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrainTraining.Models
{
    [MetadataType(typeof(GameSavesMetaData))]
    public partial class GameSave { }
    public class GameSavesMetaData
    {
        public string UserId { get; set; }
        public int MemoryPowerRound { get; set; }
        public int MemoryPowerPoints { get; set; }
        public int LeftOrRightRound { get; set; }
        public int LeftOrRightPoints { get; set; }
        public int MultiTaskingRound { get; set; }
        public int MultiTaskingPoints { get; set; }
        public int RememberFacesRound { get; set; }
        public int RememberFacesPoints { get; set; }
    }
}