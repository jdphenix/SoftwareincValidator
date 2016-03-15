using SoftwareincValidator.Model;
using SoftwareincValidator.Proxy.Impl;
using SoftwareincValidator.Serialization;
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
        static void Main(string[] args)
        {
            ISoftincModificationSerializer ser = new SoftincModificationXmlSerializer(
                new FileBackedWriterProvider()
            );

            RegisterBaseXmlMutations(ser);

            SoftincModification mod = new SoftincModification("Test");

            mod.Scenarios.Add(new Scenario
            {
                Name = "Low Money Test",
                Money = new uint[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new ushort[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            });

            mod.Scenarios.Add(new Scenario
            {
                Name = "High Money Test",
                Money = new uint[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000000" },
                Years = new ushort[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            });

            ser.Serialize(mod);
        }

        private static void RegisterBaseXmlMutations(ISoftincModificationSerializer ser)
        {
            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                // Hackish removing of XML declaration.
                if (e.Document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    e.Document.RemoveChild(e.Document.FirstChild);
                }
            };

            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                Action<XmlNode> AddTextnodeIfEmpty = null;
                AddTextnodeIfEmpty = node =>
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
                };

                AddTextnodeIfEmpty(e.Document);
            };
        }
    }
}
