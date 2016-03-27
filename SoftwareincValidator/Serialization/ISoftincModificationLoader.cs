using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Dto;
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
        /// Occurs when an XML document is loaded and validated. Validation failure of this type
        /// typically will cause a modification to not load successfully is Software Inc.
        /// </summary>
        event EventHandler<ValidationResult> XmlValidation;

        /// <summary>
        /// Occurs when a model representation of a modification component (i.e. Software types, Company
        /// types, etc.) is validated. Typically a validation failure of this type will cause 
        /// unexpected behavior in Software Inc, and not failure to load the modification.
        /// </summary>
        event EventHandler<ValidationResult> ModComponentValidation;
    }
}
