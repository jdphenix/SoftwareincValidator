using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace SoftwareincValidator.Validation
{
    public interface ISchemaProvider
    {
        XmlSchema Schema(Type type);
    }
}
