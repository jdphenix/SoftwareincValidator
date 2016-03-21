using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization
{
    public class SoftwareTypeXmlSerializer : IXmlSerializer<SoftwareType>
    {
        private static string MapBoolean(bool value)
        {
            return value.ToString();
        }

        private static bool MapBoolean(string value)
        {
            return value != null && value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        private IList<SoftwareTypeFeature> LoadFeatures(XElement element)
        {
            if (element == null)
            {
                throw new ModificationLoadException("Feature provided doesn't have any features defined.", null);
            }

            return element.Elements().Select(x => new SoftwareTypeFeature
            {
                From = x.Attribute("From")?.Value,
                Research = x.Attribute("Research")?.Value,
                Forced = MapBoolean(x.Attribute("Forced")?.Value),
                Vital = MapBoolean(x.Attribute("Vital")?.Value),
                Name = x.Element("Name")?.Value,
                Category = x.Element("Category")?.Value,
                Description = x.Element("Description")?.Value,
                Unlock = Convert.ToInt32(x.Element("Unlock")?.Value),
                DevTime = Convert.ToDouble(x.Element("DevTime")?.Value),
                Innovation = Convert.ToInt32(x.Element("Innovation")?.Value),
                Usability = Convert.ToInt32(x.Element("Usability")?.Value),
                Stability = Convert.ToInt32(x.Element("Stability")?.Value),
                CodeArt = Convert.ToDouble(x.Element("CodeArt")?.Value),
                Server = Convert.ToDouble(x.Element("Server")?.Value),
                Dependencies = LoadDependencies(x),
                SoftwareCategories = LoadSoftwareCategories(x)
            }).ToList();
        }

        private IList<FeatureSoftwareCategory> LoadSoftwareCategories(XElement xElement)
        {
            return xElement.Elements("SoftwareCategory").Select(x => new FeatureSoftwareCategory
            {
                Category = x.Attribute("Category")?.Value,
                Unlock = Convert.ToInt32(x.Value)
            }).ToList();
        }

        private IList<FeatureDependency> LoadDependencies(XElement xElement)
        {
            return xElement.Elements("Dependency").Select(x => new FeatureDependency
            {
                SoftwareType = x.Attribute("Software")?.Value,
                Feature = x.Value
            }).ToList();
        }

        private IList<SoftwareTypeCategory> LoadCategories(XElement element)
        {
            return element?.Elements().Select(x => new SoftwareTypeCategory
            {
                Name = x.Attribute("Name")?.Value,
                Description = x.Element("Description")?.Value,
                Popularity = Convert.ToDouble(x.Element("Popularity")?.Value),
                Retention = Convert.ToDouble(x.Element("Retention")?.Value),
                Iterative = Convert.ToDouble(x.Element("Iterative")?.Value),
                TimeScale = Convert.ToDouble(x.Element("TimeScale")?.Value),
                Unlock = Convert.ToInt32(x.Element("Unlock")?.Value),
                NameGenerator = x.Element("NameGenerator")?.Value,

            }).ToList();
        }

        public SoftwareType Deserialize(TextReader reader)
        {
            var doc = XDocument.Load(reader);

            Console.WriteLine("SoftwareType begin results");
            //foreach (var result in _validator.Validate(doc))
            //{
            //    Console.WriteLine(result);
            //}

            if (doc.Nodes().Count() != 1)
            {
                throw new ModificationLoadException($"Unexpected count of nodes, expected 1 but got {doc.Nodes().Count()}", null);
            }

            SoftwareType sw = new SoftwareType
            {
                Name = doc.Root.Element("Name")?.Value,
                Category = doc.Root.Element("Category")?.Value,
                Description = doc.Root.Element("Description")?.Value,
                Random = Convert.ToDouble(doc.Root.Element("Random")?.Value),
                Popularity = Convert.ToDouble(doc.Root.Element("Popularity")?.Value),
                Retention = Convert.ToDouble(doc.Root.Element("Retention")?.Value),
                Iterative = Convert.ToDouble(doc.Root.Element("Iterative")?.Value),
                OSSpecific = MapBoolean(doc.Root.Element("OSSpecific").Value),
                OneClient = MapBoolean(doc.Root.Element("OneClient").Value),
                InHouse = MapBoolean(doc.Root.Element("InHouse").Value),
                NameGenerator = doc.Root.Element("NameGenerator")?.Value,
                Categories = LoadCategories(doc.Root.Element("Categories")),
                CategoriesDefined = doc.Root.Element("Categories") != null,
                Unlock = Convert.ToInt32(doc.Root.Element("Unlock")?.Value),
                Features = LoadFeatures(doc.Root.Element("Features")),
            };

            return sw;
        }

        public XmlDocument Serialize(SoftwareType component)
        {
            throw new NotImplementedException();
        }

        public XDocument XSerialize(SoftwareType component)
        {
            XElement softwareType = new XElement("SoftwareType", 
                new XElement("Name", component.Name), 
                new XElement("Category", component.Category), 
                new XElement("Description", component.Description));

            var doc = new XDocument();
            doc.Add(softwareType);
            return doc;
        }

        public event EventHandler<ValidationResult> Validation;

        private void OnValidation(ValidationResult result) => Validation?.Invoke(this, result);
    }
}
