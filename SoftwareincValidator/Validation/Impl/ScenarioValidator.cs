using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Impl
{
    internal class ScenarioValidator : IModComponentValidator<Scenario>
    {
        private static readonly IDictionary<XmlSeverityType, ValidationLevel> SeverityDictionary =
            new Dictionary<XmlSeverityType, ValidationLevel>()
            {
                { XmlSeverityType.Error, ValidationLevel.Error },
                { XmlSeverityType.Warning, ValidationLevel.Warning }
            };
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Scenario));

        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            var results = new List<ValidationResult>();

            using (var memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, component);
                memoryStream.Position = 0;

                var doc = new XmlDocument();
                // TODO: Refactor out filesystem dependency
                doc.Schemas.Add(null, "xsd\\scenario.xsd");
                doc.Load(memoryStream);
                doc.Validate((s, e) =>
                {
                    results.Add(new ValidationResult(
                        $"[{component.GetType().Name}] [{component.Name}] {e.Message}", 
                        SeverityDictionary[e.Severity], ValidationSource.XmlSchema));
                });
            }

            if (!results.Any())
            {
                results.Add(new ValidationResult(
                    message: $"[{component.GetType().Name}] [{component.Name}] Valid.", 
                    level: ValidationLevel.Success));
            }

            return results;
        }

        public IEnumerable<ValidationResult> Validate(XmlDocument component)
        {
            throw new NotImplementedException();
        }
    }
}
