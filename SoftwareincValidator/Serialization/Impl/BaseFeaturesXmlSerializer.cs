using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;

namespace SoftwareincValidator.Serialization.Impl
{
    class BaseFeaturesXmlSerializer : IXmlSerializer<BaseFeatures>
    {
        public BaseFeatures Deserialize(TextReader reader)
        {
            var doc = XDocument.Load(reader);

            Console.WriteLine("SoftwareType begin results");

            if (doc.Nodes().Count() != 1)
            {
                throw new ModificationLoadException($"Unexpected count of nodes, expected 1 but got {doc.Nodes().Count()}", null);
            }

            return Deserialize(doc);
        }

        public BaseFeatures Deserialize(XmlDocument doc)
        {
            using (var reader = new XmlNodeReader(doc))
            {
                reader.MoveToContent();
                return Deserialize(XDocument.Load(reader));
            }
        }

        public BaseFeatures Deserialize(XDocument doc)
        {
            try
            {
                BaseFeatures sw = new BaseFeatures
                {
                    Features = LoadFeatures(doc.Root),
                    OverridesBaseFeatures = MapBoolean(doc.Root.Attribute("Override")?.Value)
                };

                return sw;
            }
            catch (FormatException ex)
            {
                throw new ModificationLoadException($"Error occured reading software type, doc: {doc}", ex);
            }
        }

        public void Serialize(Stream stream, BaseFeatures component)
        {
            using (var inputStream = Serialize(component))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(inputStream);
                doc.Save(stream);
            }
        }

        public Stream Serialize(BaseFeatures component)
        {
            throw new NotImplementedException();
        }

        private IList<SoftwareTypeFeature> LoadFeatures(XElement element)
        {
            if (element == null)
            {
                throw new ModificationLoadException("Feature provided doesn't have any features defined.", null);
            }

            return element.Elements().Select(x =>
            {
                try
                {
                    return new SoftwareTypeFeature
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
                    };
                }
                catch (FormatException ex)
                {
                    throw new ModificationLoadException($"Error occured reading software type feature, doc: {x}", ex);
                }
            }).ToList();
        }

        private IList<FeatureSoftwareCategory> LoadSoftwareCategories(XElement xElement)
        {
            return xElement.Elements("SoftwareCategory").Select(x =>
            {
                try
                {
                    return new FeatureSoftwareCategory
                    {
                        Category = x.Attribute("Category")?.Value,
                        Unlock = Convert.ToInt32(x.Value)
                    };
                }
                catch (FormatException ex)
                {
                    throw new ModificationLoadException($"Error occured reading software type feature category, doc: {x}", ex);
                }
            }).ToList();
        }

        private IList<FeatureDependency> LoadDependencies(XElement xElement)
        {
            return xElement.Elements("Dependency").Select(x =>
            {
                try
                {
                    var dependency = new FeatureDependency
                    {
                        SoftwareType = x.Attribute("Software")?.Value,
                        Feature = x.Value
                    };
                    return dependency;
                }
                catch (FormatException ex)
                {
                    throw new ModificationLoadException($"Error occured reading software type feature dependency, doc: {x}", ex);
                }
            }).ToList();
        }

        private static string MapBoolean(bool value)
        {
            return value.ToString();
        }

        private static bool MapBoolean(string value)
        {
            return value != null && value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
