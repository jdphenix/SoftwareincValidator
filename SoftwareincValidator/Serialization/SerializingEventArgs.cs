using SoftwareincValidator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SoftwareincValidator.Serialization
{
    public class SerializingEventArgs : EventArgs
    {
        /// <summary>
        /// The document that is being serialized.
        /// </summary>
        public XmlDocument Document { get; set; }

        /// <summary>
        /// The modification that is being serialized.
        /// </summary>
        public ISoftincModification Modification { get; set; }
    }
}
