using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class XmlSerializerProxy<T> : IXmlSerializer<T>
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof (T));

        public T Deserialize(TextReader reader) => (T) Serializer.Deserialize(reader);

        public T Deserialize(XmlDocument doc)
        {
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                doc.Save(stream);
                stream.Position = 0;
                return Deserialize(reader);
            }
        }

        public T Deserialize(XDocument doc)
        {
            var xml = new XmlDocument();
            xml.LoadXml(doc.ToString());
            return Deserialize(xml);
        }

        public void Serialize(Stream stream, T component) => Serializer.Serialize(stream, component);

        public Stream Serialize(T component)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, component);
            return stream;
        }
    }
}
