using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization
{
    internal sealed class SoftincModificationXmlSerializer : ISoftincModificationSerializer
    {
        private readonly IWriterProvider _writerProvider;
        private readonly IModComponentValidator<Scenario> _scenarioValidator;
        private readonly IModComponentValidator<PersonalityGraph> _personalitiesValidator;

        public SoftincModificationXmlSerializer(
            IModComponentValidator<PersonalityGraph> personalitiesValidator,
            IModComponentValidator<Scenario> scenarioValidator, 
            IWriterProvider writerProvider)
        {
            if (personalitiesValidator == null)
            {
                throw new ArgumentNullException(nameof(personalitiesValidator));
            }

            if (scenarioValidator == null)
            {
                throw new ArgumentNullException(nameof(scenarioValidator));
            }

            if (writerProvider == null)
            {
                throw new ArgumentNullException(nameof(writerProvider));
            }

            _personalitiesValidator = personalitiesValidator;
            _scenarioValidator = scenarioValidator;
            _writerProvider = writerProvider;
        }

        public event EventHandler Serialized;
        public event EventHandler<SerializingEventArgs> Serializing;

        public void Serialize(ISoftincModification mod)
        {
            if (mod.Personalities != null)
            {
                _personalitiesValidator.Validate(mod.Personalities).ToList().ForEach(x => Console.WriteLine(x));
            }

            foreach (var scen in mod.Scenarios)
            {
                foreach (var result in _scenarioValidator.Validate(scen))
                {
                    // todo: refactor
                    Console.WriteLine(result);
                }

                var ser = new XmlSerializer(scen.GetType());
                var writerSettings = GetSoftwareincWriterSettings();
                XmlDocument doc;

                using (var memoryStream = new MemoryStream())
                {
                    ser.Serialize(memoryStream, scen);
                    memoryStream.Position = 0;
                    doc = new XmlDocument();
                    // TODO: Refactor out filesystem dependency
                    doc.Schemas.Add(null, "xsd\\scenario.xsd");
                    doc.Load(memoryStream);
                }

                using (var writer = _writerProvider.GetWriter($@"{mod.Name}\Scenarios\{scen.Name}.xml"))
                using (var xmlWriter = XmlWriter.Create(writer, writerSettings))
                {
                    OnSerializing(new SerializingEventArgs
                    {
                        Document = doc,
                        Modification = mod
                    });

                    doc.Save(xmlWriter);

                    OnSerialized(new EventArgs());
                }
            }
        }

        private static XmlWriterSettings GetSoftwareincWriterSettings()
        {
            return new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Indent = true,
                IndentChars = "\t"
            };
        }

        private void OnSerialized(EventArgs e)
        {
            Serialized?.Invoke(this, e);
        }

        private void OnSerializing(SerializingEventArgs e)
        {
            Serializing?.Invoke(this, e);
        }
    }
}
