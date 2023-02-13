using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamePickerWithAngularJS.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
