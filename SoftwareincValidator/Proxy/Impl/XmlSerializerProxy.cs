using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class XmlSerializerProxy<T> : IXmlSerializer<T>, IModComponentValidator<T>
    {
        // todo: refactor methods in this type to emit validation events, SoftwareTypeXmlSerializer will model what I want here
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof (T));

        private readonly ISchemaProvider _schemaProvider;
        private readonly IModComponentValidator<T> _validator;

        public XmlSerializerProxy(ISchemaProvider schemaProvider)
        {
            if (schemaProvider == null)
            {
                throw new ArgumentNullException(nameof(schemaProvider));
            }

            _schemaProvider = schemaProvider;
            _validator = new ModComponentValidator(_schemaProvider, this);
        }

        public XmlDocument Serialize(T component)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, component);
                stream.Position = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);
                return doc;
            }
        } 

        public XDocument XSerialize(T component)
        {
            throw new NotImplementedException();
        }

        public T Deserialize(TextReader reader) => (T) Serializer.Deserialize(reader);

        public IEnumerable<ValidationResult> Validate(T component)
        {
            return _validator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(XmlDocument component)
        {
            return _validator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(XDocument component)
        {
            return _validator.Validate(component);
        }

        public event EventHandler<ValidationResult> Validation;

        private void OnValidation(ValidationResult result) => Validation?.Invoke(this, result);

        internal class ModComponentValidator : IModComponentValidator<T>
        {
            private static readonly IDictionary<XmlSeverityType, ValidationLevel> SeverityDictionary =
                new Dictionary<XmlSeverityType, ValidationLevel>()
                {
                { XmlSeverityType.Error, ValidationLevel.Error },
                { XmlSeverityType.Warning, ValidationLevel.Warning }
                };

            private readonly ISchemaProvider _schemaProvider;
            private readonly IXmlSerializer<T> _serializer;

            public ModComponentValidator(ISchemaProvider schemaProvider, IXmlSerializer<T> serializer)
            {
                if (schemaProvider == null)
                {
                    throw new ArgumentNullException(nameof(schemaProvider));
                }

                _schemaProvider = schemaProvider;
                _serializer = serializer;
            }

            public IEnumerable<ValidationResult> Validate(T component)
            {
                var results = new List<ValidationResult>();

                var doc = _serializer.Serialize(component);
                doc.Schemas.Add(_schemaProvider.Schema(typeof(T)));
                doc.Validate((s, e) =>
                {
                    results.Add(new ValidationResult(
                        $"[{component.GetType().Name}] {e.Message}, first lines of document: {doc.OuterXml.Substring(0, 100)}",
                        SeverityDictionary[e.Severity],
                        ValidationSource.XmlSchema));
                });

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

            public IEnumerable<ValidationResult> Validate(XDocument component)
            {
                throw new NotImplementedException();
            }
        }
    }
}
