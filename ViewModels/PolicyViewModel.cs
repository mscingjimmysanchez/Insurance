using Insurance.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Insurance.ViewModels
{
    /// <summary>
    /// Policy view model.
    /// </summary>
    public class PolicyViewModel
    {
        /// <summary>
        /// Policy.
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// Policy view model coverages.
        /// </summary>
        public IEnumerable<SelectListItem> Coverages { get; set; }

        /// <summary>
        /// List of selected coverages.
        /// </summary>
        private List<int> _selectedCoverages;

        /// <summary>
        /// List of selected coverages.
        /// </summary>
        public List<int> SelectedCoverages
        {
            get
            {
                if (_selectedCoverages == null)
                    _selectedCoverages = Policy.Coverages.Select(m => m.ID).ToList();
                
                return _selectedCoverages;
            }
            set 
            { 
                _selectedCoverages = value; 
            }
        }

        /// <summary>
        /// Policy view model constructor.
        /// </summary>
        public PolicyViewModel()
        {
            Coverages = new List<SelectListItem>();
            SelectedCoverages = new List<int>();
        }
    }
}