using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization
{
    internal class SoftincFileModificationLoader : ISoftincModificationLoader
    {
        private readonly IFileSystem _fileSystem;
        private readonly Func<string, IDirectoryInfo> _directoryFactory;
        private readonly IXmlSerializer<Scenario> _scenarioSerializer;
        private readonly IXmlSerializer<PersonalityGraph> _personalityGraphSerializer;
        private readonly IXmlSerializer<CompanyType> _companyTypeSerializer;
        private readonly IXmlSerializer<SoftwareType> _softwareTypeSerializer;

        public SoftincFileModificationLoader(
            IFileSystem fileSystem, 
            Func<string, IDirectoryInfo> directoryFactory,
            IXmlSerializer<Scenario> scenarioSerializer,
            IXmlSerializer<PersonalityGraph> personalityGraphSerializer, 
            IXmlSerializer<CompanyType> companyTypeSerializer, 
            IXmlSerializer<SoftwareType> softwareTypeSerializer)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (directoryFactory == null)
            {
                throw new ArgumentNullException(nameof(directoryFactory));
            }

            if (scenarioSerializer == null)
            {
                throw new ArgumentNullException(nameof(scenarioSerializer));
            }

            if (personalityGraphSerializer == null)
            {
                throw new ArgumentNullException(nameof(personalityGraphSerializer));
            }

            if (companyTypeSerializer == null)
            {
                throw new ArgumentNullException(nameof(companyTypeSerializer));
            }

            if (softwareTypeSerializer == null)
            {
                throw new ArgumentNullException(nameof(softwareTypeSerializer));
            }

            _fileSystem = fileSystem;
            _directoryFactory = directoryFactory;
            _scenarioSerializer = scenarioSerializer;
            _personalityGraphSerializer = personalityGraphSerializer;
            _companyTypeSerializer = companyTypeSerializer;
            _softwareTypeSerializer = softwareTypeSerializer;
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

        private PersonalityGraph LoadPersonalityGraph(string location)
        {
            var directory = _directoryFactory(location);

            if (!directory.Exists) return null;

            var personalities = directory
                .GetFiles()
                // todo: refactor, magic string
                .SingleOrDefault(f => f.Name.Equals("Personalities.xml"));

            if (personalities == null) return null;

            using (var reader = personalities.OpenText())
            {
                return _personalityGraphSerializer.Deserialize(reader);
            }
        }

        private IEnumerable<SoftwareType> LoadSoftwareTypes(string location)
        {
            // todo: refactor, magic string
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "SoftwareTypes"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles().Where(x => !x.Name.Equals("Base.xml", StringComparison.OrdinalIgnoreCase)))
                {
                    {
                        using (var reader = file.OpenText())
                        {
                            yield return _softwareTypeSerializer.Deserialize(reader);
                        }
                    }
                }
            }
        }

        private IEnumerable<Scenario> LoadScenarios(string location)
        {
            // todo: refactor, magic string
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "Scenarios"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    {
                        using (var reader = file.OpenText())
                        {
                            yield return _scenarioSerializer.Deserialize(reader);
                        }
                    }
                }
            }
        }

        private IEnumerable<CompanyType> LoadCompanyTypes(string location)
        {
            // todo: refactor, magic string
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "CompanyTypes"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    {
                        using (var reader = file.OpenText())
                        {
                            yield return _companyTypeSerializer.Deserialize(reader);
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

            foreach (var softwareType in LoadSoftwareTypes(absolutePath))
            {
                mod.SoftwareTypes.Add(softwareType);
            }

            foreach (var scenario in LoadScenarios(absolutePath))
            {
                mod.Scenarios.Add(scenario);
            }

            foreach (var companyType in LoadCompanyTypes(absolutePath))
            {
                mod.CompanyTypes.Add(companyType);
            }

            mod.Personalities = LoadPersonalityGraph(location);

            return mod;
        }
    }
}
