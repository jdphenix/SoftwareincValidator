using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SoftwareincValidator
{
    class Program
    {
        static void AddTextnodeIfEmpty(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Text) return;

            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    AddTextnodeIfEmpty(child);
                }
            }
            else
            {
                node.AppendChild(node.OwnerDocument.CreateTextNode(string.Empty));
            }
        }

        static void Main(string[] args)
        {
            Scenario scen = new Scenario
            {
                Name = "Test",
                Money = new uint[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new ushort[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true, 
                Trees = ScenarioTrees.FALSE,
                TreesSpecified = true, 
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true, 
                Events = new string[0]
            };

            XmlSerializer ser = new XmlSerializer(scen.GetType());
            XmlDocument doc = null;
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Indent = true, 
                IndentChars = "\t"

            };

            using (var memoryStream = new MemoryStream())
            using (var xmlReader = XmlReader.Create(memoryStream, readerSettings))
            {
                ser.Serialize(memoryStream, scen);
                memoryStream.Position = 0;
                doc = new XmlDocument();
                doc.Load(memoryStream);

                // Hackish removing of XML declaration.
                if (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    doc.RemoveChild(doc.FirstChild);
                }

                // Hackish ensuring there's no self-closing tags.
                AddTextnodeIfEmpty(doc);
            }

            using (var writer = new StreamWriter($@"b:\out\Scenarios\{scen.Name}.xml"))
            using (var xmlWriter = XmlWriter.Create(writer, writerSettings))
            {
                doc.Save(xmlWriter);
            }
        }
    }
}
