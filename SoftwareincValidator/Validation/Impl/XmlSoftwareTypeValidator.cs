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
    public class XmlSoftwareTypeValidator : IXmlComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(XDocument component)
        {
            var results = new List<ValidationResult>();
            var root = component.Root;
            var mustBePresent = new[]
            {
                "Name", "Category", "Description", "Random", "OneClient", "OSSpecific", "OneClient", "InHouse", "Features"
            };
            var mayBePresent = new List<string>
            {
                "Delete", "Unlock", "Categories", "NameGenerator", "Popularity", "Retention", "Iterative", "OSLimit", "OSNeed", "Needs"
            };
            mayBePresent.AddRange(mustBePresent);

            ValidateRequired(root, results, mustBePresent);
            ValidateAllowed(root, results, mayBePresent);

            if (root.Element("Categories") == null)
            {
                ValidateWithNoCategories(component, results);
            }
            else
            {
                ValidateCategories(root.Element("Categories").Elements(), results);
            }

            ValidateFeatures(root.Element("Features").Elements(), results);

            return results;
        }

        private void ValidateFeatures(IEnumerable<XElement> component, List<ValidationResult> results)
        {
            // todo: refactor out to common type
            var attrMayBePresent = new[] {"Vital", "Forced", "From", "Research"};
            var mustBePresent = new[] {"Name", "Description", "DevTime", "Innovation", "Usability", "Stability", "CodeArt"};
            var mayBePresent = mustBePresent.ToList();
            mayBePresent.AddRange(new [] {"Category", "Unlock", "Dependency", "SoftwareCategory", "Server", "Dependencies"});

            foreach (var feature in component)
            {
                results.AddRange(feature.Attributes()
                    .Where(attr => attrMayBePresent.All(x => x != attr.Name))
                    .Select(attr => new ValidationResult(
                        $"Feature has unexpected attribute {attr.Name}, expected any of {string.Join(",", attrMayBePresent)}, doc: {feature}"
                )));

                results.AddRange(feature.Elements("Dependency")
                    .Where(dependency => dependency.Attribute("Software") == null)
                    .Select(dependency => new ValidationResult($"Dependency doesn't have a Software attribute defined, doc: {feature}")));

                results.AddRange(feature.Elements("SoftwareCategory")
                    .Where(category => category.Attribute("Category") == null)
                    .Select(category => new ValidationResult($"SoftwareCategory doesn't have a Category attribute defined, doc: {feature}")));

                ValidateRequired(feature, results, mustBePresent);
                ValidateAllowed(feature, results, mayBePresent);
            }
        }

        private static void ValidateCategories(IEnumerable<XElement> component, List<ValidationResult> results)
        {
            var mustBePresent = new[] {"Description", "Popularity", "Retention", "TimeScale", "Iterative"};
            var mayBePresent = mustBePresent.ToList();
            mayBePresent.AddRange(new [] {"Unlock", "NameGenerator"});

            foreach (var category in component)
            {
                if (category.Attribute("Name") == null)
                {
                    results.Add(new ValidationResult(
                        $"Category doesn't have a Name attribute defined, doc: {category}"));    
                }

                ValidateRequired(category, results, mustBePresent);
                ValidateAllowed(category, results, mayBePresent);
            }
        }

        private static void ValidateWithNoCategories(XDocument component, List<ValidationResult> results)
        {
            var mustBePresent = new[] {"Popularity", "Retention", "Iterative"};
            ValidateRequired(component.Root, results, mustBePresent);
        }

        public IEnumerable<ValidationResult> Validate(XmlDocument component)
        {
            using (var reader = new XmlNodeReader(component))
            {
                reader.MoveToContent();
                return Validate(XDocument.Load(reader));
            }
        }

        private static void ValidateAllowed(XElement component, List<ValidationResult> results, IEnumerable<string> allowed)
        {
            // todo: refactor out to common type
            foreach (var tagName in component.Elements().Select(t => t.Name))
            {
                if (allowed.All(n => n != tagName))
                    results.Add(new ValidationResult(
                        $"{tagName} element defined and not allowed, doc: {component.ToString().Substring(0, 100)}"));
            }
        }

        private static void ValidateRequired(XElement component, List<ValidationResult> results, IEnumerable<string> mustBePresent)
        {
            // todo: refactor out to common type
            foreach (var tagName in mustBePresent)
            {
                if (component.Element(tagName) == null)
                    results.Add(new ValidationResult(
                        $"No {tagName} element defined., doc: {component.ToString().Substring(0, 100)}"));
            }
        }
    }
}
