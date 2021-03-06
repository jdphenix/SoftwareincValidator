﻿using System;
using System.Collections.Generic;
using System.Linq;
using SoftincValidator.Base.Model.Generated;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Model
{
    public sealed class SoftincModification : ISoftincModification
    {
        public SoftincModification(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} must be provided.");
            }

            Name = name;
            Scenarios = new List<Scenario>();
            CompanyTypes = new List<CompanyType>();
            SoftwareTypes = new List<SoftwareType>();
            NameGenerators = new List<NameGenerator>();
        }

        /// <summary>
        ///     The name of the modification, as displayed in the game.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets a list of all specializations defined by this modification. Specializations are 
        /// defined by &lt;Category&gt; on features. This is the 
        /// sole way to added Specializations to Software Inc.
        /// </summary>
        /// <returns>A list of specializations.</returns>
        public IList<string> GetSpecializations()
        {
            return SoftwareTypes
                .SelectMany(t => t.Features)
                .Select(f => f.Category)
                .Select(c => c ?? "General")
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        ///     All Name Generators defined in this modification.
        /// </summary>
        public IList<NameGenerator> NameGenerators { get; } 

        /// <summary>
        ///     All Scenarios defined in this modification.
        /// </summary>
        public IList<Scenario> Scenarios { get; }

        /// <summary>
        ///     All company types defined in this modification.
        /// </summary>
        public IList<CompanyType> CompanyTypes { get; }

        /// <summary>
        ///     All software types defined in this modification.
        /// </summary>
        public IList<SoftwareType> SoftwareTypes { get; }

        /// <summary>
        ///     A list of all deleted company types, kayed by CompanyType.Name.
        /// </summary>
        public CompanyTypes DeletedCompanyTypes { get; set; }

        /// <summary>
        ///     The Personality Graph, if exists, associated with this modification.
        /// </summary>
        public PersonalityGraph Personalities { get; set; }

        /// <summary>
        /// The base features, if present, associated with this modification.
        /// </summary>
        public BaseFeatures BaseFeatures { get; set; }
    }
}