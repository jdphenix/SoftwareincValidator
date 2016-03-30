using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SoftincValidator.Base.Model.Generated;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization.Impl
{
    internal sealed class SoftincModificationSerializer : ISoftincModificationSerializer
    {
        private readonly IWriterProvider _writerProvider;
        private readonly IXmlSerializer<SoftwareType> _softwareTypeSerializer;

        private static void RegisterBaseXmlMutations(ISoftincModificationSerializer ser)
        {
            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                // Hackish removing of XML declaration.
                if (e.Document?.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    e.Document.RemoveChild(e.Document.FirstChild);
                }
            };

            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                if (e.Document != null)
                {
                    Action<XmlNode> addTextnodeIfEmpty = null;
                    addTextnodeIfEmpty = node =>
                    {
                        if (node.NodeType == XmlNodeType.Text) return;

                        if (node.HasChildNodes)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                addTextnodeIfEmpty(child);
                            }
                        }
                        else
                        {
                            node.AppendChild(node.OwnerDocument.CreateTextNode(string.Empty));
                        }
                    };

                    addTextnodeIfEmpty(e.Document);
                }
            };
        }

        public SoftincModificationSerializer(
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

            RegisterBaseXmlMutations(this);
        }

        public event EventHandler Serialized;
        public event EventHandler<SerializingEventArgs> Serializing;

        public void Serialize(ISoftincModification mod)
        {
            if (mod.Personalities != null)
            {
                SerializePersonalityGraph(mod);
            }

            foreach (var nameGenerator in mod.NameGenerators)
            {
                SerializeNameGenerator(mod, nameGenerator);
            }

            foreach (var softwareType in mod.SoftwareTypes)
            {
                SerializeSoftwareType(mod, softwareType);
            }

            foreach (var companyType in mod.CompanyTypes)
            {
                SerialzeCompanyType(mod, companyType);
            }

            foreach (var scen in mod.Scenarios)
            {
                SerializeScenario(mod, scen);
            }
        }

        private void SerializeNameGenerator(ISoftincModification mod, NameGenerator nameGenerator)
        {
            using (var writer = _writerProvider.GetWriter($@"{mod.Name}\NameGenerators\{nameGenerator.Name}.txt"))
            {
                writer.WriteLine(nameGenerator.GeneratorText);
            }
        }

        private void SerializePersonalityGraph(ISoftincModification mod)
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

        private void SerializeSoftwareType(ISoftincModification mod, SoftwareType softwareType)
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

        private void SerializeScenario(ISoftincModification mod, Scenario scen)
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

        private void SerialzeCompanyType(ISoftincModification mod, CompanyType companyType)
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
