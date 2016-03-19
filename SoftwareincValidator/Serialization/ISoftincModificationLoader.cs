using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model;

namespace SoftwareincValidator.Serialization
{
    public interface ISoftincModificationLoader
    {
        /// <summary>
        /// Instantiates and loads a modification from the given location.
        /// </summary>
        /// <param name="location">The path or location to the modification.</param>
        /// <returns>A loaded modification.</returns>
        ISoftincModification Load(string location);
    }
}
