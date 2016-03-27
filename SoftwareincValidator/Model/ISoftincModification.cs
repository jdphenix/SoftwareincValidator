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
        /// Gets a list of all specializations defined by this modification. Specializations are 
        /// defined by &lt;Category&gt; on features. This is the 
        /// sole way to added Specializations to Software Inc.
        /// </summary>
        /// <returns>A list of specializations.</returns>
        IList<string> GetSpecializations();

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
        /// A list of all deleted company types, kayed by CompanyType.Name.
        /// </summary>
        CompanyTypes DeletedCompanyTypes { get; }

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