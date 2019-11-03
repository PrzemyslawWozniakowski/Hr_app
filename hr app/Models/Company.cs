using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_app.Models
{
    public class Company
    {
        public Company(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
