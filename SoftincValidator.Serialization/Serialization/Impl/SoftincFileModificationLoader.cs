﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using SoftincValidator.Base.Model.Generated;
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
        private readonly IXmlSerializer<BaseFeatures> _baseFeatureSerializer;
        private readonly IXmlSerializer<CompanyTypes> _companyTypesSerializer;
        private readonly IModValidator _validator;

        public SoftincFileModificationLoader(
            IFileSystem fileSystem, 
            Func<string, IDirectoryInfo> directoryFactory,
            IXmlSerializer<Scenario> scenarioSerializer,
            IXmlSerializer<PersonalityGraph> personalityGraphSerializer, 
            IXmlSerializer<CompanyType> companyTypeSerializer, 
            IXmlSerializer<SoftwareType> softwareTypeSerializer,
            IXmlSerializer<BaseFeatures> baseFeatureSerializer,
            IXmlSerializer<CompanyTypes> companyTypesSerializer,
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

            if (baseFeatureSerializer == null)
            {
                throw new ArgumentNullException(nameof(baseFeatureSerializer));
            }

            if (companyTypesSerializer == null)
            {
                throw new ArgumentNullException(nameof(companyTypesSerializer));
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
            _baseFeatureSerializer = baseFeatureSerializer;
            _companyTypesSerializer = companyTypesSerializer;
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

        private PersonalityGraph LoadPersonalityGraph(string location, SoftincModification mod)
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
                component.Modification = mod;
                _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                return component;
            }
        }

        private BaseFeatures LoadBaseFeatures(string absolutePath, SoftincModification mod)
        {
            var directory = _directoryFactory(_fileSystem.PathCombine(absolutePath, "SoftwareTypes"));

            if (!directory.Exists) return null;

            var features = directory
                .GetFiles()
                // todo: refactor, magic string
                .SingleOrDefault(f => f.Name.Equals("Base.xml", StringComparison.OrdinalIgnoreCase));

            if (features == null) return null;

            using (var reader = features.OpenText())
            {
                var doc = new XmlDocument();

                try
                {
                    doc.Load(reader);
                }
                catch (XmlException ex)
                {
                    throw new ModificationLoadException($"XML parsing error occured while parsing {features.Name}, {ex.Message}", ex);
                }

                _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                var component = _baseFeatureSerializer.Deserialize(doc);
                component.Modification = mod;
                _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                return component;
            }
        }

        private IEnumerable<SoftwareType> LoadSoftwareTypes(string location, SoftincModification mod)
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

                            try
                            {
                                doc.Load(reader);
                            }
                            catch (XmlException ex)
                            {
                                throw new ModificationLoadException($"XML parsing error occured while parsing {file.Name}, {ex.Message}", ex);
                            }

                            _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                            var component = _softwareTypeSerializer.Deserialize(doc);
                            component.Modification = mod;
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
                        }
                    }
                }
            }
        }

        private CompanyTypes LoadDeletedCompanyTypes(string location, SoftincModification mod)
        {
            // todo: refactor, magic string
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "CompanyTypes"));

            if (!directory.Exists) return null;

            var deletedCompanyTypes = directory
                .GetFiles()
                // todo: refactor, magic string
                .SingleOrDefault(f => f.Name.Equals("Delete.xml", StringComparison.OrdinalIgnoreCase));

            if (deletedCompanyTypes == null) return null;

            using (var reader = deletedCompanyTypes.OpenText())
            {
                var doc = new XmlDocument();
                doc.Load(reader);
                _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                var component = _companyTypesSerializer.Deserialize(doc);
                component.Modification = mod;
                _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                return component;
            }
        }

        private IEnumerable<Scenario> LoadScenarios(string location, SoftincModification mod)
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
                            component.Modification = mod;
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
                        }
                    }
                }
            }
        }

        private IEnumerable<CompanyType> LoadCompanyTypes(string location, SoftincModification mod)
        {
            // todo: refactor, magic string
            var directory = _directoryFactory(_fileSystem.PathCombine(location, "CompanyTypes"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles().Where(x => !x.Name.Equals("Delete.xml", StringComparison.OrdinalIgnoreCase)))
                {
                    {
                        using (var reader = file.OpenText())
                        {
                            var doc = new XmlDocument();
                            doc.Load(reader);
                            _validator.Validate(doc).ToList().ForEach(x => OnXmlValidation(this, x));
                            var component = _companyTypeSerializer.Deserialize(doc);
                            component.Modification = mod;
                            _validator.Validate(component).ToList().ForEach(x => OnModComponentValidation(this, x));
                            yield return component;
                        }
                    }
                }
            }
        }

        private IEnumerable<NameGenerator> LoadNameGenerators(string absolutePath, SoftincModification mod)
        {
            var directory = _directoryFactory(_fileSystem.PathCombine(absolutePath, "NameGenerators"));

            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    using (var reader = file.OpenText())
                    {
                        yield return new NameGenerator
                        {
                            Name = _fileSystem.PathGetFileNameWithoutExtension(file.Name),
                            GeneratorText = reader.ReadToEnd()
                        };
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

            try
            {
                foreach (var nameGenerator in LoadNameGenerators(absolutePath, mod))
                {
                    mod.NameGenerators.Add(nameGenerator);
                }

                foreach (var softwareType in LoadSoftwareTypes(absolutePath, mod))
                {
                    mod.SoftwareTypes.Add(softwareType);
                }

                foreach (var scenario in LoadScenarios(absolutePath, mod))
                {
                    mod.Scenarios.Add(scenario);
                }

                foreach (var companyType in LoadCompanyTypes(absolutePath, mod))
                {
                    mod.CompanyTypes.Add(companyType);
                }

                mod.Personalities = LoadPersonalityGraph(absolutePath, mod);
                mod.BaseFeatures = LoadBaseFeatures(absolutePath, mod);
                mod.DeletedCompanyTypes = LoadDeletedCompanyTypes(absolutePath, mod);

                _validator.Validate(mod).ToList().ForEach(x => OnModComponentValidation(this, x));
            }
            catch (InvalidOperationException ex)
            {
                OnXmlValidation(this, new ValidationResult(
                    $"Fatal mod loading error: {ex.Message}, {ex.InnerException?.Message}"));
                return null;
            }
            catch (ModificationLoadException ex)
            {
                OnXmlValidation(this, new ValidationResult(

                    $"Fatal mod loading error: {ex.Message}"));
                return null;
            }

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
