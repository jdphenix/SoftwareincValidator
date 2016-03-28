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

namespace SoftwareincValidator.Serialization.Impl
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

        private IList<SoftwareTypeCategory> LoadCategories(XElement element)
        {
            return element?.Elements().Select(x =>
            {
                try
                {
                    var category = new SoftwareTypeCategory
                    {
                        Name = x.Attribute("Name")?.Value,
                        Description = x.Element("Description")?.Value,
                        Popularity = Convert.ToDouble(x.Element("Popularity")?.Value),
                        Retention = Convert.ToDouble(x.Element("Retention")?.Value),
                        Iterative = Convert.ToDouble(x.Element("Iterative")?.Value),
                        TimeScale = Convert.ToDouble(x.Element("TimeScale")?.Value),
                        Unlock = Convert.ToInt32(x.Element("Unlock")?.Value),
                        NameGenerator = x.Element("NameGenerator")?.Value,
                    };
                    return category;
                }
                catch (FormatException ex)
                {
                    throw new ModificationLoadException($"Error occured reading software type category, doc: {x}", ex);
                }
            }).ToList();
        }

        public SoftwareType Deserialize(TextReader reader)
        {
            var doc = XDocument.Load(reader);

            Console.WriteLine("SoftwareType begin results");

            if (doc.Nodes().Count() != 1)
            {
                throw new ModificationLoadException($"Unexpected count of nodes, expected 1 but got {doc.Nodes().Count()}", null);
            }

            return Deserialize(doc);
        }

        public SoftwareType Deserialize(XmlDocument doc)
        {
            using (var reader = new XmlNodeReader(doc))
            {
                reader.MoveToContent();
                return Deserialize(XDocument.Load(reader));
            }
        }

        public SoftwareType Deserialize(XDocument doc)
        {
            try
            {
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
            catch (FormatException ex)
            {
                throw new ModificationLoadException($"Error occured reading software type, doc: {doc}", ex);
            }
        }

        public void Serialize(Stream stream, SoftwareType component)
        {
            using (var inputStream = Serialize(component))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(inputStream);
                doc.Save(stream);
            }
        }

        public Stream Serialize(SoftwareType component)
        {
            XElement softwareType = new XElement("SoftwareType",
                new XElement("Name", component.Name), 
                new XElement("Category", component.Category), 
                new XElement("Description", component.Description), 
                new XElement("Random", component.Random), 
                new XElement("Popularity", component.Popularity), 
                new XElement("Retention", component.Retention), 
                new XElement("Iterative", component.Iterative), 
                new XElement("OSSpecific", component.OSSpecific), 
                new XElement("OneClient", component.OneClient), 
                new XElement("InHouse", component.InHouse), 
                new XElement("Unlock", component.Unlock));

            if (component.NameGenerator != null) softwareType.Add(new XElement("NameGenerator", component.NameGenerator));

            if (component.CategoriesDefined)
            {
                softwareType.Add(new XElement("Categories", 
                    SerializeCategories(component.Categories))
                );
            }

            softwareType.Add(new XElement("Features", SerializeFeatures(component.Features)));

            var doc = new XDocument(
                softwareType
            );

            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream, GetSoftwareincWriterSettings());
            doc.WriteTo(writer);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private IEnumerable<XElement> SerializeFeatures(IList<SoftwareTypeFeature> features)
        {
            foreach (var f in features)
            {
                var e = new XElement("Feature",
                            new XAttribute("Forced", f.Forced),
                            new XAttribute("Vital", f.Vital),
                            new XElement("Name", f.Name ?? ""),
                            new XElement("Description", f.Description ?? ""),
                            new XElement("DevTime", f.DevTime),
                            new XElement("Innovation", f.Innovation),
                            new XElement("Usability", f.Usability),
                            new XElement("Stability", f.Stability),
                            new XElement("CodeArt", f.CodeArt));

                if (f.From != null) e.Add(new XAttribute("From", f.From));
                if (f.Research != null) e.Add(new XAttribute("Research", f.Research));

                foreach (var dependency in f.Dependencies)
                {
                    e.Add(new XElement("Dependency", 
                        new XAttribute("Software", dependency.SoftwareType), 
                        dependency.Feature));
                }

                foreach (var categoryRestriction in f.SoftwareCategories)
                {
                    e.Add(new XElement("SoftwareCategory", 
                        new XAttribute("Category", categoryRestriction.Category),
                        categoryRestriction.Unlock));
                }

                yield return e;
            }
        }

        private IEnumerable<XElement> SerializeCategories(IList<SoftwareTypeCategory> categories)
        {
            return categories.Select(cat => new XElement("Category", 
                new XAttribute("Name", cat.Name), 
                new XElement("Description", cat.Description),
                new XElement("Popularity", cat.Popularity), 
                new XElement("Retention", cat.Retention), 
                new XElement("TimeScale", cat.TimeScale), 
                new XElement("Iterative", cat.Iterative), 
                new XElement("NameGenerator", cat.NameGenerator)
            ));
        }

        private static XmlWriterSettings GetSoftwareincWriterSettings()
        {
            return new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Indent = true,
                IndentChars = "\t"
            };
        }
    }
}
