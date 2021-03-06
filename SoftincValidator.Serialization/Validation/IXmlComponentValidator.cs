﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SoftwareincValidator.Validation
{
    public interface IXmlComponentValidator<in T>
    {
        /// <summary>
        /// Perform a validation operation on the given component.
        /// </summary>
        /// <param name="component">The component to perform validation upon.</param>
        /// <returns>An enumerable of <see cref="ValidationResult"/>, ultimately determining if validation 
        /// succeeded for the given component.</returns>
        IEnumerable<ValidationResult> Validate(XmlDocument component);

        /// <summary>
        /// Perform a validation operation on the given component.
        /// </summary>
        /// <param name="component">The component to perform validation upon.</param>
        /// <returns>An enumerable of <see cref="ValidationResult"/>, ultimately determining if validation 
        /// succeeded for the given component.</returns>
        IEnumerable<ValidationResult> Validate(XDocument component);
    }
}
