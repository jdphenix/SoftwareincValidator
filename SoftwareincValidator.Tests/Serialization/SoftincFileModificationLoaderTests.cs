using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SoftwareincValidator.Dto;
using SoftwareincValidator.Dto.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization;
using SoftwareincValidator.Serialization.Impl;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Tests.Serialization
{
    [TestClass]
    public class SoftincFileModificationLoaderTests
    {
        private IFileSystem _fileSystem;
        private Func<string, IDirectoryInfo> _directoryFactory; 
        private SoftincFileModificationLoader _modificationLoader;
        private IXmlSerializer<Scenario> _scenarioSerializer;
        private IXmlSerializer<PersonalityGraph> _personalityGraphSerializer;
        private IXmlSerializer<CompanyType> _companyTypeSerializer;
        private IXmlSerializer<SoftwareType> _softwareTypeSerializer;
        private IXmlSerializer<BaseFeatures> _baseFeatureSerializer;
        private IXmlSerializer<CompanyTypes> _companyTypesSerializer; 
        private IModValidator _validator;

        [TestInitialize]
        public void Init()
        {
            _fileSystem = Substitute.For<IFileSystem>();
            _fileSystem.PathDirectorySeparatorChar.Returns('\\');
            _fileSystem.PathGetFullPath(null).ReturnsForAnyArgs(x => "C:\\" + (string) x[0]);
            _fileSystem.PathCombine(null, null).ReturnsForAnyArgs(x => Path.Combine((string) x[0], (string) x[1]));

            _directoryFactory = Substitute.For<Func<string, IDirectoryInfo>>();

            _scenarioSerializer = Substitute.For<IXmlSerializer<Scenario>>();

            _personalityGraphSerializer = Substitute.For<IXmlSerializer<PersonalityGraph>>();

            _companyTypeSerializer = Substitute.For<IXmlSerializer<CompanyType>>();

            _softwareTypeSerializer = Substitute.For<IXmlSerializer<SoftwareType>>();

            _baseFeatureSerializer = Substitute.For<IXmlSerializer<BaseFeatures>>();

            _companyTypesSerializer = Substitute.For<IXmlSerializer<CompanyTypes>>();

            _validator = Substitute.For<IModValidator>();

            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                _companyTypeSerializer,
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullFileSystem_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                null, 
                _directoryFactory, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                _companyTypeSerializer, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullDirectoryFactory_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                null, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                _companyTypeSerializer, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullScenarioSerializer_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                null, 
                _personalityGraphSerializer, 
                _companyTypeSerializer, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullPersonalityGraphSerializer_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                _scenarioSerializer, 
                null, 
                _companyTypeSerializer, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullCompanyTypeSerializer_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                null, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullSoftwareTypeSerializer_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                _companyTypeSerializer, 
                null,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullBaseFeaturesValidator_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem,
                _directoryFactory,
                _scenarioSerializer,
                _personalityGraphSerializer,
                _companyTypeSerializer,
                _softwareTypeSerializer,
                null,
                _companyTypesSerializer,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullCompanyTypesValidator_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem,
                _directoryFactory,
                _scenarioSerializer,
                _personalityGraphSerializer,
                _companyTypeSerializer,
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                null,
                _validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullValidator_ThrowsException()
        {
            _modificationLoader = new SoftincFileModificationLoader(
                _fileSystem, 
                _directoryFactory, 
                _scenarioSerializer, 
                _personalityGraphSerializer, 
                _companyTypeSerializer, 
                _softwareTypeSerializer,
                _baseFeatureSerializer,
                _companyTypesSerializer,
                null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Load_PassedNullLocation_ThrowsException()
        {
            var mod = _modificationLoader.Load(null);
        }

        [TestMethod]
        public void Load_PassedBlankLocation_LoadsWorkingDirectoryMod()
        {
            const string workingDirectory = "path\\to\\mod";
            _fileSystem.DirectoryExists($@"C:\{workingDirectory}").Returns(true);
            _fileSystem.DirectoryGetCurrentDirectory().Returns(workingDirectory);

            var mod = _modificationLoader.Load("");

            _fileSystem.Received(1).DirectoryGetCurrentDirectory();
            Assert.AreEqual("mod", mod.Name);
        }
    }
}
