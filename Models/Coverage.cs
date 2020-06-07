﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Models
{
    public class Coverage
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required]
        public double Percentage { get; set; }

        [Required]
        public int Period { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}