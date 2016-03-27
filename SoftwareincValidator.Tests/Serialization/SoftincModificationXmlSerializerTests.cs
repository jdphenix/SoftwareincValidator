using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SoftwareincValidator.Dto;
using SoftwareincValidator.Dto.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SoftwareincValidator.Serialization.Impl;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator.Tests.Serialization
{
    [TestClass]
    public class SoftincModificationXmlSerializerTests
    {
        private SoftincModificationXmlSerializer _ser;
        private IWriterProvider _writerProvider;
        private TextWriter _writer;

        [TestInitialize]
        public void Initialize()
        {
            _writer = Substitute.For<TextWriter>();
            _writerProvider = Substitute.For<IWriterProvider>();
            _writerProvider.GetWriter(null).ReturnsForAnyArgs(_writer);

            _ser = new SoftincModificationXmlSerializer(_writerProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullWriterProvider_ThrowsException()
        {
            _ser = new SoftincModificationXmlSerializer(null);
        }

        [TestMethod]
        public void Serialize_PassedModWithName_PassedNameInFirstPosition()
        {
            const string expected = "kittens";
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns(expected);
            mod.Scenarios.Returns(new List<Scenario> { new Scenario { Name = expected } });

            _ser.Serialize(mod);

            _writerProvider.Received(1).GetWriter($@"{expected}\Scenarios\{expected}.xml");
        }

        [TestMethod]
        public void Serialize_PassedScenarioWithName_PassedNameInFilenamePosition()
        {
            const string expectedModName = "kittens";
            const string expectedScenarioName = "puppies";

            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns(expectedModName);
            mod.Scenarios.Returns(new List<Scenario> { new Scenario { Name = expectedScenarioName } });

            _ser.Serialize(mod);

            _writerProvider.Received(1).GetWriter($@"{expectedModName}\Scenarios\{expectedScenarioName}.xml");
        }

        [TestMethod]
        public void Serialize_PassedNoScenarios_DoesNotWriteScenarios()
        {
            const int expectedReceivedCount = 0;
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns("kittens");
            mod.Scenarios.Returns(new List<Scenario>());

            _ser.Serialize(mod);

            var receivedCount = _writerProvider.ReceivedCalls()
                .Select(x => x.GetArguments()
                    .Select(ar => ar.ToString()
                    .Contains("Scenarios")))
                .Count();
            Assert.AreEqual(expectedReceivedCount, receivedCount);
        }

        [TestMethod]
        public void Serialize_PassedOneScenario_DoesWriteScenario()
        {
            const int expectedReceivedCount = 1;
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns("kittens");
            mod.Scenarios.Returns(new List<Scenario> { new Scenario { Name = "puppies" } });

            _ser.Serialize(mod);

            var receivedCount = _writerProvider.ReceivedCalls()
                .Select(x => x.GetArguments()
                    .Select(ar => ar.ToString()
                    .Contains("Scenarios")))
                .Count();
            Assert.AreEqual(expectedReceivedCount, receivedCount);
        }

        [TestMethod]
        public void Serialize_Passed200Scenarios_DoesWriteScenarios()
        {
            const int expectedReceivedCount = 200;
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns("kittens");
            mod.Scenarios.Returns(new List<Scenario>(Enumerable.Repeat(new Scenario { Name = "puppies" }, 200)));

            _ser.Serialize(mod);

            var receivedCount = _writerProvider.ReceivedCalls()
                .Select(x => x.GetArguments()
                    .Select(ar => ar.ToString()
                    .Contains("Scenarios")))
                .Count();
            Assert.AreEqual(expectedReceivedCount, receivedCount);
        }
    }
}
