using System.Collections.Generic;

namespace SoftwareincValidator.Model
{
    public interface ISoftincModification
    {
        /// <summary>
        /// The name of the modification, as displayed in the game.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// All Scenarios defined in this modification.
        /// </summary>
        IList<Scenario> Scenarios { get; }
    }
}