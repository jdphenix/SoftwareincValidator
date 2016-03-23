using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Impl
{
    public class ModValidator : IModValidator
    {
        private IDictionary<string, Func<XmlDocument, IEnumerable<ValidationResult>>> _validations;

        private readonly IModComponentValidator<Scenario> _scenarioValidator;
        private readonly IModComponentValidator<PersonalityGraph> _personalitiesValidator;
        private readonly IModComponentValidator<CompanyType> _companyTypeValidator;
        private readonly IModComponentValidator<SoftwareType> _softwareTypeValidator;
        private readonly IModComponentValidator<BaseFeatures> _baseFeaturesValidator;

        public ModValidator(
            IModComponentValidator<PersonalityGraph> personalitiesValidator,
            IModComponentValidator<Scenario> scenarioValidator,
            IModComponentValidator<CompanyType> companyTypeValidator,
            IModComponentValidator<SoftwareType> softwareTypeValidator, 
            IModComponentValidator<BaseFeatures> baseFeaturesValidator)
        {
            if (personalitiesValidator == null)
            {
                throw new ArgumentNullException(nameof(personalitiesValidator));
            }

            if (scenarioValidator == null)
            {
                throw new ArgumentNullException(nameof(scenarioValidator));
            }

            if (companyTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(companyTypeValidator));
            }

            if (softwareTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(softwareTypeValidator));
            }

            if (baseFeaturesValidator == null)
            {
                throw new ArgumentNullException(nameof(baseFeaturesValidator));
            }

            _personalitiesValidator = personalitiesValidator;
            _scenarioValidator = scenarioValidator;
            _companyTypeValidator = companyTypeValidator;
            _softwareTypeValidator = softwareTypeValidator;
            _baseFeaturesValidator = baseFeaturesValidator;

            _validations = new Dictionary<string, Func<XmlDocument, IEnumerable<ValidationResult>>>
            {
                { "Features", document => _baseFeaturesValidator.Validate(document) },
                { "SoftwareType", document => _softwareTypeValidator.Validate(document) },
                { "CompanyType", document => _companyTypeValidator.Validate(document) },
                { "PersonalityGraph", document => _personalitiesValidator.Validate(document) },
                { "Scenario", document => _scenarioValidator.Validate(document) }
            };
        }

        public IEnumerable<ValidationResult> Validate(XmlDocument doc)
        {
            var results = new List<ValidationResult>();

            if (doc.FirstChild.NodeType != XmlNodeType.Element)
            {
                results.Add(new ValidationResult(
                    $"Unexpected first node type of document, expected element but got {doc.FirstChild}, document: {doc}"));
                return results;
            }

            var type = doc.FirstChild.Name;
            if (!_validations.ContainsKey(type))
            {
                results.Add(new ValidationResult(
                    $"Expected first element, expected one of [{string.Join(",", _validations.Keys)}] but got {type}, document: {doc}"));
                return results;
            }

            return _validations[type](doc);
        }

        public IEnumerable<ValidationResult> Validate(XDocument doc)
        {
            var xml = new XmlDocument();
            xml.LoadXml(doc.ToString());
            return Validate(xml);
        }

        public IEnumerable<ValidationResult> Validate(CompanyType component)
        {
            return _companyTypeValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return _softwareTypeValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            return _scenarioValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(PersonalityGraph component)
        {
            return _personalitiesValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(BaseFeatures component)
        {
            return _baseFeaturesValidator.Validate(component);
        }
    }
}
