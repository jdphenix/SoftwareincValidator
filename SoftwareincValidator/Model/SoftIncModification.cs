using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Model
{
    public sealed class SoftincModification
    {
        /// <summary>
        /// The name of the modification, as displayed in the game.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// All Scenarios defined in this modification.
        /// </summary>
        public IList<Scenario> Scenarios { get; }

        public SoftincModification(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} must be provided.");
            }

            this.Name = name;
            this.Scenarios = new List<Scenario>();
        }
    }
}
