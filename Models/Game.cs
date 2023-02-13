using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamePickerWithAngularJS.Models
{
    [Table("Games")]
    public class Game
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public string GameName { get; set; }
        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }
        public int PlayingTime { get; set; }
        public string Image { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }

    }
}
