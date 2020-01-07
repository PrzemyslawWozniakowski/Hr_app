﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_app.Models
{
    public class CompanyCreateView
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
