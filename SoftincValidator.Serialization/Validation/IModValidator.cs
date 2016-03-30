using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation
{
    public interface IModValidator
    {
        IEnumerable<ValidationResult> Validate(XmlDocument doc);
        IEnumerable<ValidationResult> Validate(XDocument doc);
        IEnumerable<ValidationResult> Validate(CompanyType component);
        IEnumerable<ValidationResult> Validate(SoftwareType component);
        IEnumerable<ValidationResult> Validate(Scenario component);
        IEnumerable<ValidationResult> Validate(PersonalityGraph component);
        IEnumerable<ValidationResult> Validate(BaseFeatures component);
        IEnumerable<ValidationResult> Validate(CompanyTypes component);
        IEnumerable<ValidationResult> Validate(ISoftincModification modification);
    }
}
