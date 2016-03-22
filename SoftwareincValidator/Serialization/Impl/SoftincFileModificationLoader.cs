using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Serialization.Impl
{
    internal class SoftincFileModificationLoader : ISoftincModificationLoader
    {
        private readonly IFileSystem _fileSystem;
        private readonly Func<string, IDirectoryInfo> _directoryFactory;
        private readonly IXmlSerializer<Scenario> _scenarioSerializer;
        private readonly IXmlSerializer<PersonalityGraph> _personalityGraphSerializer;
        private readonly IXmlSerializer<CompanyType> _companyTypeSerializer;
        private readonly IXmlSerializer<SoftwareType> _softwareTypeSerializer;
        private readonly IModValidator _validator;

        public SoftincFileModificationLoader(
            IFileSystem fileSystem, 
            Func<string, IDirectoryInfo> directoryFactory,
            IXmlSerializer<Scenario> scenarioSerializer,
            IXmlSerializer<PersonalityGraph> personalityGraphSerializer, 
            IXmlSerializer<CompanyType> companyTypeSerializer, 
            IXmlSerializer<SoftwareType> softwareTypeSerializer,
            IModValidator validator)
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

            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            _fileSystem = fileSystem;
            _directoryFactory = directoryFactory;
            _scenarioSerializer = scenarioSerializer;
            _personalityGraphSerializer = personalityGraphSerializer;
            _companyTypeSerializer = companyTypeSerializer;
            _softwareTypeSerializer = softwareTypeSerializer;
            _validator = validator;
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
                var doc = new XmlDocument();
                doc.Load(reader);
                _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                var component = _personalityGraphSerializer.Deserialize(doc);
                _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                return component;
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
                            var doc = new XmlDocument();
                            doc.Load(reader);
                            _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                            var component = _softwareTypeSerializer.Deserialize(doc);
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
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
                            var doc = new XmlDocument();
                            doc.Load(reader);
                            _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                            var component = _scenarioSerializer.Deserialize(doc);
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
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
                            var doc = new XmlDocument();
                            doc.Load(reader);
                            _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                            var component = _companyTypeSerializer.Deserialize(doc);
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
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

        public event EventHandler<ValidationResult> XmlValidation;

        public event EventHandler<ValidationResult> ModComponentValidation;

        private void OnXmlValidation(object sender, ValidationResult e) => 
            XmlValidation?.Invoke(sender, e);

        private void OnModComponentValidation(object sender, ValidationResult e) => 
            ModComponentValidation?.Invoke(sender, e);
    }
}
