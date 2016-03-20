using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Proxy
{
    public interface IXmlSerializer<T>
    {
        T Deserialize(TextReader reader);
        void Serialize(Stream stream, T component);
    }
}
