using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Proxy
{
    public interface IXmlSerializer<T>
    {
        T Deserialize(TextReader reader);
        XmlDocument Serialize(T component);
        XDocument XSerialize(T component);

        event EventHandler<ValidationResult> Validation;
    }
}
