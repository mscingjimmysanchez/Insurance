using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Models
{
    /// <summary>
    /// Risk type enumeration.
    /// </summary>
    public enum RiskType
    {
        Low, Medium, MediumHigh, High
    }

    /// <summary>
    /// Policy class.
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// Policy ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Policy name.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Policy description.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string Description { get; set; }

        /// <summary>
        /// Policy validity start.
        /// </summary>
        [Required]
        [Display(Name = "Validity Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidityStart { get; set; }

        /// <summary>
        /// Policy price.
        /// </summary>
        [Required]
        public double Price { get; set; }

        /// <summary>
        /// Policy risk type.
        /// </summary>
        [Required]
        [Display(Name = "Risk Type")]
        public RiskType RiskType { get; set; }

        /// <summary>
        /// Policy clients collection.
        /// </summary>
        public virtual ICollection<Client> Clients { get; set; }

        /// <summary>
        /// Policy coverages collection.
        /// </summary>
        public virtual ICollection<Coverage> Coverages { get; set; }
    }
}