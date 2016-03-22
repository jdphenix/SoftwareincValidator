using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model;
using SoftwareincValidator.Validation;

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

        /// <summary>
        /// Occurs when an XML document is loaded and validated.
        /// </summary>
        event EventHandler<ValidationResult> XmlValidation;

        /// <summary>
        /// Occurs when a model representation of a modification component (i.e. Software types, Company
        /// types, etc.) is validated.
        /// </summary>
        event EventHandler<ValidationResult> ModComponentValidation;
    }
}
