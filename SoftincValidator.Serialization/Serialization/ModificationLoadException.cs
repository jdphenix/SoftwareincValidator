using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Serialization
{
    public class ModificationLoadException : Exception
    {
        public ModificationLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
