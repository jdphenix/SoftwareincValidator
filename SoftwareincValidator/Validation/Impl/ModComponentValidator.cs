using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SoftwareincValidator.Validation.Impl
{
    internal class ModComponentValidator<T> : IModComponentValidator<T>
    {
        private static readonly IDictionary<XmlSeverityType, ValidationLevel> SeverityDictionary =
            new Dictionary<XmlSeverityType, ValidationLevel>()
            {
                { XmlSeverityType.Error, ValidationLevel.Error },
                { XmlSeverityType.Warning, ValidationLevel.Warning }
            };

        private readonly ISchemaProvider _schemaProvider;

        public ModComponentValidator(ISchemaProvider schemaProvider)
        {

            if (schemaProvider == null)
            {
                throw new ArgumentNullException(nameof(schemaProvider));
            }

            _schemaProvider = schemaProvider;
        }

        public IEnumerable<ValidationResult> Validate(T component)
        {
            var results = new List<ValidationResult>();

            // TODO: Validation for model objects

            if (!results.Any())
            {
                results.Add(new ValidationResult(
                    message: $"[{component.GetType().Name}] TODO: Validation for model objects.", 
                    level: ValidationLevel.Success));
            }

            return results;
        }

        public IEnumerable<ValidationResult> Validate(XmlDocument component)
        {
            var results = new List<ValidationResult>();

            component.Schemas.Add(_schemaProvider.Schema(typeof(T)));
            component.Validate((s, e) =>
            {
                results.Add(new ValidationResult(
                    $"[{component.GetType().Name}] {e.Message}, first lines of document: {component.OuterXml.Substring(0, 100)}",
                    SeverityDictionary[e.Severity],
                    ValidationSource.XmlSchema));
            });
            return results;
        }

        public IEnumerable<ValidationResult> Validate(XDocument component)
        {
            var doc = new XmlDocument();
            doc.LoadXml(component.ToString());
            return Validate(doc);
        }
    }
}
