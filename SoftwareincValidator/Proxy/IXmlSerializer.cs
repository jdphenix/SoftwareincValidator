using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SoftwareincValidator.Proxy
{
    public interface IXmlSerializer<T>
    {
        T Deserialize(TextReader reader);
        T Deserialize(XmlDocument doc);
        T Deserialize(XDocument doc);
        void Serialize(Stream stream, T component);
        Stream Serialize(T component);
    }
}
