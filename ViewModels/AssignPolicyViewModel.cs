using Insurance.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.ViewModels
{
    /// <summary>
    /// Assign policy view model.
    /// </summary>
    public class AssignPolicyViewModel
    {
        /// <summary>
        /// Policy ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Policy name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Policy description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Policy validity start.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidityStart { get; set; }

        /// <summary>
        /// Policy price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Policy risk type.
        /// </summary>
        public RiskType RiskType { get; set; }

        /// <summary>
        /// Indicates if the policy is assigned.
        /// </summary>
        public bool Assigned { get; set; }

        /// <summary>
        /// Policy coverages collection.
        /// </summary>
        public virtual ICollection<Coverage> Coverages { get; set; }
    }
}