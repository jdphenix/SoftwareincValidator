using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class XmlSerializerProxy<T> : IXmlSerializer<T>
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof (T));
        public void Serialize(Stream stream, T component) => Serializer.Serialize(stream, component);
        public T Deserialize(TextReader reader) => (T) Serializer.Deserialize(reader);
    }
}
