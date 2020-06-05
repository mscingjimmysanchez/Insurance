using Insurance.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Insurance.ViewModels
{
    public class PolicyViewModel
    {
        public Policy Policy { get; set; }
        public IEnumerable<SelectListItem> Coverages { get; set; }

        private List<int> _selectedCoverages;
        public List<int> SelectedCoverages
        {
            get
            {
                if (_selectedCoverages == null)
                {
                    _selectedCoverages = Policy.Coverages.Select(m => m.ID).ToList();
                }
                return _selectedCoverages;
            }
            set { _selectedCoverages = value; }
        }

        public PolicyViewModel()
        {
            Coverages = new List<SelectListItem>();
            SelectedCoverages = new List<int>();
        }
    }
}