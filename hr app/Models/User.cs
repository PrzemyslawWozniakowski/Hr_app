using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums;
namespace hr_app.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
