using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Serialization
{
    internal class SoftincFileModificationLoader : ISoftincModificationLoader
    {
        private static XmlSerializer ScenarioSerializer = new XmlSerializer(typeof(Scenario));

        private static string GetModName(string location)
        {
            if (Directory.Exists(location))
            {
                return location
                    .Split(Path.DirectorySeparatorChar)
                    .Last();
            }

            if (File.Exists(location))
            {
                try
                {
                    return Path.GetDirectoryName(location)
                        ?.Split(Path.DirectorySeparatorChar)
                        .Last();
                }
                catch (PathTooLongException ex)
                {
                    throw new ModificationLoadException(ex.Message, ex);
                }
            }

            throw new ModificationLoadException($"Provided location {location} doesn't appear to be a mod directory.", null);
        }

        private static IEnumerable<Scenario> LoadScenarios(string location)
        {
            var directory = new DirectoryInfo(Path.Combine(location, "Scenarios"));
            var files = directory.GetFiles();

            foreach (var file in files)
            {
                {
                    using (TextReader reader = file.OpenText())
                    {
                        yield return (Scenario) ScenarioSerializer.Deserialize(reader);
                    }
                }
            }
        }

        public ISoftincModification Load(string location)
        {
            var absolutePath = Path.GetFullPath(location);

            var mod = new SoftincModification(GetModName(absolutePath));
            foreach (var scenario in LoadScenarios(absolutePath))
            {
                mod.Scenarios.Add(scenario);
            }

            return mod;
        }
    }
}
