using Insurance.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Insurance.ViewModels
{
    public class ClientPolicyViewModel
    {
        public int ClientID { get; set; }
        public Client Client { get; set; }
        public List<AssignPolicyViewModel> Policies { get; set; }

        private List<int> _selectedPolicies;
        public List<int> SelectedPolicies
        {
            get
            {
                if (_selectedPolicies == null)
                {
                    _selectedPolicies = (from p in Policies where p.Assigned select p.ID).ToList();
                }
                return _selectedPolicies;
            }
            set { _selectedPolicies = value; }
        }

        public ClientPolicyViewModel()
        {
            Policies = new List<AssignPolicyViewModel>();
        }
    }
}