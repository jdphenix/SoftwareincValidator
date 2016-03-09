using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoftwareincValidator
{
    class Program
    {
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

            XmlSerializer ser = new XmlSerializer(typeof(Scenario));
            using (var writer = new StreamWriter(@"scenario.xml"))
            {
                ser.Serialize(writer, scen);
            }
        }
    }
}
