using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Configuration;
using SoftwareincValidator.Proxy;

namespace SoftwareincValidator.Serialization
{
    internal sealed class SoftincModificationXmlSerializer : ISoftincModificationSerializer
    {
        private readonly IWriterProvider writerProvider;

        public SoftincModificationXmlSerializer(IWriterProvider writerProvider)
        {
            if (writerProvider == null)
            {
                throw new ArgumentNullException(nameof(writerProvider));
            }

            this.writerProvider = writerProvider;
        }

        public event EventHandler Serialized;
        public event EventHandler<SerializingEventArgs> Serializing;

        public void Serialize(ISoftincModification mod)
        {
            foreach (var scen in mod.Scenarios)
            {
                XmlSerializer ser = new XmlSerializer(scen.GetType());
                XmlDocument doc = null;
                XmlWriterSettings writerSettings = GetSoftwareincWriterSettings();

                using (var memoryStream = new MemoryStream())
                {
                    ser.Serialize(memoryStream, scen);
                    memoryStream.Position = 0;
                    doc = new XmlDocument();
                    // TODO: Refactor out filesystem dependency
                    doc.Schemas.Add(null, "xsd\\scenario.xsd");
                    doc.Load(memoryStream);
                    doc.Validate((s, e) => Console.WriteLine($"{e.Severity}: {e.Message}"));
                }

                using (var writer = writerProvider.GetWriter($@"{mod.Name}\Scenarios\{scen.Name}.xml"))
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
