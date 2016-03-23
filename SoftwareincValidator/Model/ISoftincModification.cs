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
        /// All company types defined in this modification.
        /// </summary>
        IList<CompanyType> CompanyTypes { get; }

        /// <summary>
        /// All company types defined in this modification.
        /// </summary>
        IList<SoftwareType> SoftwareTypes { get; }

        /// <summary>
        /// The personality graph, if present, associated with this modification.
        /// </summary>
        PersonalityGraph Personalities { get; }

        /// <summary>
        /// The base features, if present, associated with this modification.
        /// </summary>
        BaseFeatures BaseFeatures { get; }
    }
}