using BoardGamePickerWithAngularJS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGame.Models
{
    public class GameInfo : Game
    {
        public string Year { get; set; }
        public string Category { get; set; }
        public string Designer { get; set; }
        public string Artist { get; set; }
        public string Description { get; set; }

    }
}
