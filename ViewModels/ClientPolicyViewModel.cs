using Insurance.Models;
using System.Collections.Generic;

namespace Insurance.ViewModels
{
    /// <summary>
    /// Client policy view model.
    /// </summary>
    public class ClientPolicyViewModel
    {
        /// <summary>
        /// Client ID.
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Policy client.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// List of assign policy view models.
        /// </summary>
        public List<AssignPolicyViewModel> Policies { get; set; }

        /// <summary>
        /// Client policy view model.
        /// </summary>
        public ClientPolicyViewModel()
        {
            Policies = new List<AssignPolicyViewModel>();
        }
    }
}