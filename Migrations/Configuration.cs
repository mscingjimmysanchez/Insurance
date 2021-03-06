﻿using Insurance.DAL;
using Insurance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Insurance.Migrations
{
    /// <summary>
    /// Configuration class.
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Insurance.DAL.InsuranceContext>
    {
        /// <summary>
        /// Configuration class constructor.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Feeds the database.
        /// </summary>
        /// <param name="context">Insurance context.</param>
        protected override void Seed(Insurance.DAL.InsuranceContext context)
        {
            var clients = new List<Client>
            {
                new Client { Name = "Alexander Carson" }
            };

            clients.ForEach(c => context.Clients.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            var coverages = new List<Coverage>
            {
                new Coverage {Name = "Earthquake", Percentage = 80, Period = 3 },
                new Coverage {Name = "Fire", Percentage = 60, Period = 4 },
                new Coverage {Name = "Theft", Percentage = 40, Period = 6 },
                new Coverage {Name = "Loss", Percentage = 20, Period = 12 }
            };

            coverages.ForEach(c => context.Coverages.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            var policies = new List<Policy>
            {
                new Policy {
                    Name = "Life",
                    Description = "Life Insurance",
                    ValidityStart = DateTime.Now,
                    Price = 60000,
                    RiskType = RiskType.Low,
                    Coverages = new List<Coverage>(),
                    Clients = clients
                }
            };

            policies.ForEach(c => context.Policies.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            AddOrUpdateCoverage(context, "Life", "Earthquake");
            AddOrUpdateCoverage(context, "Life", "Fire");

            context.SaveChanges();
        }

        /// <summary>
        /// Add or update a coverage to a policy.
        /// </summary>
        /// <param name="context">Insurance context.</param>
        /// <param name="policyName">Policy name.</param>
        /// <param name="coverageName">Coverage name.</param>
        void AddOrUpdateCoverage(InsuranceContext context, string policyName, string coverageName)
        {
            var pol = context.Policies.SingleOrDefault(p => p.Name == policyName);
            var cov = pol.Coverages.SingleOrDefault(c => c.Name == coverageName);
            
            if (cov == null)
                pol.Coverages.Add(context.Coverages.Single(c => c.Name == coverageName));
        }
    }
}