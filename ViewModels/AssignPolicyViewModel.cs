using Insurance.Models;
using System;
using System.Collections.Generic;

namespace Insurance.ViewModels
{
    public class AssignPolicyViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ValidityStart { get; set; }

        public double Price { get; set; }

        public RiskType RiskType { get; set; }

        public bool Assigned { get; set; }

        public virtual ICollection<Coverage> Coverages { get; set; }
    }
}