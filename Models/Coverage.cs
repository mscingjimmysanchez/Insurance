using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Models
{
    /// <summary>
    /// Coverage class.
    /// </summary>
    public class Coverage
    {
        /// <summary>
        /// Coverage ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Coverage name.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Coverage percentage.
        /// </summary>
        [Required]
        public double Percentage { get; set; }

        /// <summary>
        /// Coverage period.
        /// </summary>
        [Required]
        public int Period { get; set; }

        /// <summary>
        /// Coverage policies collection.
        /// </summary>
        public virtual ICollection<Policy> Policies { get; set; }
    }
}