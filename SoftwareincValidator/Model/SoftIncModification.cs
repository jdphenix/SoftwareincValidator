using System;
using System.Collections.Generic;
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
        }

        /// <summary>
        ///     The name of the modification, as displayed in the game.
        /// </summary>
        public string Name { get; }

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
        ///     The Personality Graph, if exists, associated with this modification.
        /// </summary>
        public PersonalityGraph Personalities { get; set; }

        /// <summary>
        /// The base features, if present, associated with this modification.
        /// </summary>
        public BaseFeatures BaseFeatures { get; set; }
    }
}