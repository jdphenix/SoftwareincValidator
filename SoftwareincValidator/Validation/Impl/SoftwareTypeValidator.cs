using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;

namespace SoftwareincValidator.Validation.Impl
{
    public class SoftwareTypeValidator : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            var results = new List<ValidationResult>();

            if (!results.Any())
            {
                results.Add(new ValidationResult(
                    message: $"[{component.GetType().Name}] Valid.",
                    level: ValidationLevel.Success));
            }

            return results;

        }

        public IEnumerable<ValidationResult> Validate(XDocument component)
        {
            var results = new List<ValidationResult>();
            var root = component.Root;

            if (root.Element("Categories") == null)
            {

            }

            if (!results.Any())
            {
                results.Add(new ValidationResult(
                    message: $"[{component.GetType().Name}] Valid.",
                    level: ValidationLevel.Success));
            }

            return results;
        } 

        public IEnumerable<ValidationResult> Validate(XmlDocument component)
        {
            using (var reader = new XmlNodeReader(component))
            {
                reader.MoveToContent();
                return Validate(XDocument.Load(reader));
            }
        }
    }
}
