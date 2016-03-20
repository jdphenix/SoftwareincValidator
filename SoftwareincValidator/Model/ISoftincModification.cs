using SoftwareincValidator.Model.Generated;
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

        /// <summary>
        /// The personality graph, if present, associated with this modification.
        /// </summary>
        PersonalityGraph Personalities { get; }
    }
}