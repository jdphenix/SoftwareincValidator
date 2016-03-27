using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoftwareincValidator.Dto.Generated
{
    public partial class BaseFeatures
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class CompanyTypes
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class CompanyType
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class PersonalityGraph
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class Scenario
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class SoftwareType
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }

    public partial class SoftwareTypeFeature
    {
        [XmlIgnore]
        public ISoftincModification Modification { get; set; }
    }
}
