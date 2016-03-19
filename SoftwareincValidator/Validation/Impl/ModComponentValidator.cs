using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;

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

        private readonly IXmlSerializer<T> _serializer;
        private readonly ISchemaProvider _schemaProvider;

        public ModComponentValidator(IXmlSerializer<T> serializer, ISchemaProvider schemaProvider)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (schemaProvider == null)
            {
                throw new ArgumentNullException(nameof(schemaProvider));
            }

            _serializer = serializer;
            _schemaProvider = schemaProvider;
        }

        public IEnumerable<ValidationResult> Validate(T component)
        {
            var results = new List<ValidationResult>();

            using (var memoryStream = new MemoryStream())
            {
                _serializer.Serialize(memoryStream, component);
                memoryStream.Position = 0;

                var doc = new XmlDocument();
                // TODO: Refactor out filesystem dependency
                doc.Schemas.Add(_schemaProvider.Schema(typeof(Scenario)));
                doc.Load(memoryStream);
                doc.Validate((s, e) =>
                {
                    results.Add(new ValidationResult(
                        $"[{component.GetType().Name}] {e.Message}, first lines of document: {doc.OuterXml.Substring(0, 100)}", 
                        SeverityDictionary[e.Severity], 
                        ValidationSource.XmlSchema));
                });
            }

            if (!results.Any())
            {
                results.Add(new ValidationResult(
                    message: $"[{component.GetType().Name}] Valid.", 
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
