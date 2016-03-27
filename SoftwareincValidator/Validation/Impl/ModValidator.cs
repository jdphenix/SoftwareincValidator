using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Dto;
using SoftwareincValidator.Dto.Generated;

namespace SoftwareincValidator.Validation.Impl
{
    public class ModValidator : IModValidator
    {
        private IDictionary<string, Func<XmlDocument, IEnumerable<ValidationResult>>> _validations;

        private readonly IXmlComponentValidator<Scenario> _xmlScenarioValidator;
        private readonly IXmlComponentValidator<PersonalityGraph> _xmlPersonalitiesValidator;
        private readonly IXmlComponentValidator<CompanyType> _xmlCompanyTypeValidator;
        private readonly IXmlComponentValidator<SoftwareType> _xmlSoftwareTypeValidator;
        private readonly IXmlComponentValidator<BaseFeatures> _xmlBaseFeaturesValidator;
        private readonly IXmlComponentValidator<CompanyTypes> _xmlCompanyTypesValidator;
        private readonly IEnumerable<IModComponentValidator<Scenario>>  _modScenarioValidator;
        private readonly IEnumerable<IModComponentValidator<PersonalityGraph>> _modPersonalitiesValidator;
        private readonly IEnumerable<IModComponentValidator<CompanyType>>  _modCompanyTypeValidator;
        private readonly IEnumerable<IModComponentValidator<SoftwareType>>  _modSoftwareTypeValidator;
        private readonly IEnumerable<IModComponentValidator<BaseFeatures>> _modBaseFeaturesValidator;
        private readonly IEnumerable<IModComponentValidator<CompanyTypes>> _modCompanyTypesValidator;
        private readonly IEnumerable<IModComponentValidator<ISoftincModification>> _modificationValidator; 

        public ModValidator(
            IXmlComponentValidator<PersonalityGraph> xmlPersonalitiesValidator,
            IXmlComponentValidator<Scenario> xmlScenarioValidator,
            IXmlComponentValidator<CompanyType> xmlCompanyTypeValidator,
            IXmlComponentValidator<SoftwareType> xmlSoftwareTypeValidator, 
            IXmlComponentValidator<BaseFeatures> xmlBaseFeaturesValidator,
            IXmlComponentValidator<CompanyTypes> xmlCompanyTypesValidator,
            IEnumerable<IModComponentValidator<Scenario>> modScenarioValidator,
            IEnumerable<IModComponentValidator<PersonalityGraph>> modPersonalitiesValidator,
            IEnumerable<IModComponentValidator<CompanyType>> modCompanyTypeValidator,
            IEnumerable<IModComponentValidator<SoftwareType>> modSoftwareTypeValidator,
            IEnumerable<IModComponentValidator<BaseFeatures>> modBaseFeaturesValidator,
            IEnumerable<IModComponentValidator<CompanyTypes>> modCompanyTypesValidator,
            IEnumerable<IModComponentValidator<ISoftincModification>> modificationValidator)
        {
            if (xmlPersonalitiesValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlPersonalitiesValidator));
            }

            if (xmlScenarioValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlScenarioValidator));
            }

            if (xmlCompanyTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlCompanyTypeValidator));
            }

            if (xmlSoftwareTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlSoftwareTypeValidator));
            }

            if (xmlBaseFeaturesValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlBaseFeaturesValidator));
            }

            if (xmlCompanyTypesValidator == null)
            {
                throw new ArgumentNullException(nameof(xmlCompanyTypesValidator));
            }

            if (modPersonalitiesValidator == null)
            {
                throw new ArgumentNullException(nameof(modPersonalitiesValidator));
            }

            if (modScenarioValidator == null)
            {
                throw new ArgumentNullException(nameof(modScenarioValidator));
            }

            if (modCompanyTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(modCompanyTypeValidator));
            }

            if (modSoftwareTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(modSoftwareTypeValidator));
            }

            if (modBaseFeaturesValidator == null)
            {
                throw new ArgumentNullException(nameof(modBaseFeaturesValidator));
            }

            if (modCompanyTypesValidator == null)
            {
                throw new ArgumentNullException(nameof(modCompanyTypesValidator));
            }

            if (modificationValidator == null)
            {
                throw new ArgumentNullException(nameof(modificationValidator));
            }

            _xmlPersonalitiesValidator = xmlPersonalitiesValidator;
            _xmlScenarioValidator = xmlScenarioValidator;
            _xmlCompanyTypeValidator = xmlCompanyTypeValidator;
            _xmlSoftwareTypeValidator = xmlSoftwareTypeValidator;
            _xmlBaseFeaturesValidator = xmlBaseFeaturesValidator;
            _xmlCompanyTypesValidator = xmlCompanyTypesValidator;
            _modPersonalitiesValidator = modPersonalitiesValidator;
            _modScenarioValidator = modScenarioValidator;
            _modCompanyTypeValidator = modCompanyTypeValidator;
            _modSoftwareTypeValidator = modSoftwareTypeValidator;
            _modBaseFeaturesValidator = modBaseFeaturesValidator;
            _modCompanyTypesValidator = modCompanyTypesValidator;
            _modificationValidator = modificationValidator;

            _validations = new Dictionary<string, Func<XmlDocument, IEnumerable<ValidationResult>>>
            {
                { "Features", document => _xmlBaseFeaturesValidator.Validate(document) },
                { "SoftwareType", document => _xmlSoftwareTypeValidator.Validate(document) },
                { "CompanyType", document => _xmlCompanyTypeValidator.Validate(document) },
                { "PersonalityGraph", document => _xmlPersonalitiesValidator.Validate(document) },
                { "Scenario", document => _xmlScenarioValidator.Validate(document) },
                { "CompanyTypes", document => _xmlCompanyTypesValidator.Validate(document) }
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
            return EnumerateResults(_modCompanyTypeValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return EnumerateResults(_modSoftwareTypeValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            return EnumerateResults(_modScenarioValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(PersonalityGraph component)
        {
            return EnumerateResults(_modPersonalitiesValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(BaseFeatures component)
        {
            return EnumerateResults(_modBaseFeaturesValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(CompanyTypes component)
        {
            return EnumerateResults(_modCompanyTypesValidator, component);
        }

        public IEnumerable<ValidationResult> Validate(ISoftincModification modification)
        {
            return EnumerateResults(_modificationValidator, modification);
        }

        private static IEnumerable<ValidationResult> EnumerateResults<T>(
            IEnumerable<IModComponentValidator<T>> validators, T component)
        {
            return validators.SelectMany(validator => validator.Validate(component));
        }
    }
}
