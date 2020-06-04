using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Models
{
    public enum RiskType
    {
        Low, Medium, MediumHigh, High
    }

    public class Policy
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Validity Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidityStart { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Risk Type")]
        public RiskType RiskType { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<Coverage> Coverages { get; set; }
    }
}