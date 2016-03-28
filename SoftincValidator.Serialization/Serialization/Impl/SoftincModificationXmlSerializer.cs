using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization.Impl
{
    internal sealed class SoftincModificationXmlSerializer : ISoftincModificationSerializer
    {
        private readonly IWriterProvider _writerProvider;
        private readonly IXmlSerializer<SoftwareType> _softwareTypeSerializer; 

        public SoftincModificationXmlSerializer(
            IWriterProvider writerProvider,
            IXmlSerializer<SoftwareType> softwareTypeSerializer)
        {
            if (writerProvider == null)
            {
                throw new ArgumentNullException(nameof(writerProvider));
            }

            if (softwareTypeSerializer == null)
            {
                throw new ArgumentNullException(nameof(softwareTypeSerializer));
            }

            _writerProvider = writerProvider;
            _softwareTypeSerializer = softwareTypeSerializer;
        }

        public event EventHandler Serialized;
        public event EventHandler<SerializingEventArgs> Serializing;

        public void Serialize(ISoftincModification mod)
        {
            if (mod.Personalities != null)
            {
                XmlDocument doc;

                using (var memoryStream = new MemoryStream())
                {
                    var ser = new XmlSerializer(mod.Personalities.GetType());
                    ser.Serialize(memoryStream, mod.Personalities);
                    memoryStream.Position = 0;
                    // todo: refactor concrete instantiation
                    doc = new XmlDocument();
                    // TODO: Refactor out filesystem dependency
                    // todo: refactor magic string
                    doc.Schemas.Add(null, "xsd\\personalities.xsd");
                    doc.Load(memoryStream);
                }

                // todo: refactor magic string
                using (var writer = _writerProvider.GetWriter($@"{mod.Name}\Personalities.xml"))
                // todo: refactor out, writer provider? 
                using (var xmlWriter = XmlWriter.Create(writer, GetSoftwareincWriterSettings()))
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

            foreach (var softwareType in mod.SoftwareTypes)
            {
                using (var stream = _softwareTypeSerializer.Serialize(softwareType))
                using (var output = _writerProvider.GetOutputStream($@"{mod.Name}\SoftwareTypes\{softwareType.Name}.xml"))
                {
                    // todo: emit the document
                    OnSerializing(new SerializingEventArgs
                    {
                        Document = null,
                        Modification = mod
                    });

                    stream.CopyTo(output);

                    OnSerialized(new EventArgs());
                }
            }

            foreach (var companyType in mod.CompanyTypes)
            {
                var ser = new XmlSerializer(companyType.GetType());
                var writerSettings = GetSoftwareincWriterSettings();
                XmlDocument doc;

                using (var memoryStream = new MemoryStream())
                {
                    ser.Serialize(memoryStream, companyType);
                    memoryStream.Position = 0;
                    // todo: refactor concrete instantiation
                    doc = new XmlDocument();
                    // TODO: Refactor out filesystem dependency
                    // todo: refactor magic string
                    doc.Schemas.Add(null, "xsd\\company-type.xsd");
                    doc.Load(memoryStream);
                }

                // todo: refactor magic string
                using (var writer = _writerProvider.GetWriter($@"{mod.Name}\CompanyTypes\{companyType.Specialization}.xml"))
                // todo: refactor out, writer provider? 
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

            foreach (var scen in mod.Scenarios)
            {
                var ser = new XmlSerializer(scen.GetType());
                var writerSettings = GetSoftwareincWriterSettings();
                XmlDocument doc;

                using (var memoryStream = new MemoryStream())
                {
                    ser.Serialize(memoryStream, scen);
                    memoryStream.Position = 0;
                    // todo: refactor concrete instantiation
                    doc = new XmlDocument();
                    // TODO: Refactor out filesystem dependency
                    // todo: refactor magic string
                    doc.Schemas.Add(null, "xsd\\scenario.xsd");
                    doc.Load(memoryStream);
                }

                // todo: refactor magic string
                using (var writer = _writerProvider.GetWriter($@"{mod.Name}\Scenarios\{scen.Name}.xml"))
                    // todo: refactor out, writer provider? 
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
