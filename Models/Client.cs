using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Models
{
    /// <summary>
    /// Client class.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Client ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Client name.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Client policies collection.
        /// </summary>
        public virtual ICollection<Policy> Policies { get; set; }
    }
}