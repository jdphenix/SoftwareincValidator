using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;

namespace SoftwareincValidator.Serialization
{
    internal class SoftincFileModificationLoader : ISoftincModificationLoader
    {
        private readonly IFileSystem _fileSystem;
        private readonly Func<string, IDirectoryInfo> _directoryFactory;
        private readonly IXmlSerializer<Scenario> _xmlSerializer; 

        public SoftincFileModificationLoader(
            IFileSystem fileSystem, 
            Func<string, IDirectoryInfo> directoryFactory,
            IXmlSerializer<Scenario> xmlSerializer)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (directoryFactory == null)
            {
                throw new ArgumentNullException(nameof(directoryFactory));
            }

            if (xmlSerializer == null)
            {
                throw new ArgumentNullException(nameof(xmlSerializer));
            }

            _fileSystem = fileSystem;
            _directoryFactory = directoryFactory;
            _xmlSerializer = xmlSerializer;
        }

        private string GetModName(string location)
        {
            if (_fileSystem.DirectoryExists(location))
            {
                return location
                    .Split(_fileSystem.PathDirectorySeparatorChar)
                    .Last();
            }

            throw new ModificationLoadException($"Provided location {location} doesn't appear to be a mod directory.", null);
        }

        private IEnumerable<Scenario> LoadScenarios(string location)
        {
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "Scenarios"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    {
                        using (var reader = file.OpenText())
                        {
                            yield return _xmlSerializer.Deserialize(reader);
                        }
                    }
                }
            }
        }

        public ISoftincModification Load(string location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            if (location == string.Empty)
            {
                location = _fileSystem.DirectoryGetCurrentDirectory();
            }

            var absolutePath = _fileSystem.PathGetFullPath(location);

            var mod = new SoftincModification(GetModName(absolutePath));
            foreach (var scenario in LoadScenarios(absolutePath))
            {
                mod.Scenarios.Add(scenario);
            }

            return mod;
        }
    }
}
