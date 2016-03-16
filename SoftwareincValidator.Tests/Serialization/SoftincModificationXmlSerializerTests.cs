using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Tests.Serialization
{
    [TestClass]
    public class SoftincModificationXmlSerializerTests
    {
        private SoftincModificationXmlSerializer ser;
        private IWriterProvider writerProvider;
        private TextWriter writer;

        [TestInitialize]
        public void Initialize()
        {
            writer = Substitute.For<TextWriter>();
            writerProvider = Substitute.For<IWriterProvider>();
            writerProvider.GetWriter(null).ReturnsForAnyArgs(writer);
            ser = new SoftincModificationXmlSerializer(writerProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullWriterProvider_ThrowsException()
        {
            ser = new SoftincModificationXmlSerializer(null);
        }

        [TestMethod]
        public void Serialize_PassedModWithName_PassedNameInFirstPosition()
        {
            const string expected = "kittens";
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns(expected);
            mod.Scenarios.Returns(new List<Scenario> { new Scenario { Name = expected } });

            ser.Serialize(mod);

            writerProvider.Received(1).GetWriter($@"{expected}\Scenarios\{expected}.xml");
        }

        [TestMethod]
        public void Serialize_PassedScenarioWithName_PassedNameInFilenamePosition()
        {
            const string expectedModName = "kittens";
            const string expectedScenarioName = "puppies";

            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns(expectedModName);
            mod.Scenarios.Returns(new List<Scenario> { new Scenario { Name = expectedScenarioName } });

            ser.Serialize(mod);

            writerProvider.Received(1).GetWriter($@"{expectedModName}\Scenarios\{expectedScenarioName}.xml");
        }

        [TestMethod]
        public void Serialize_PassedNoScenarios_DoesNotWriteScenarios()
        {
            const int expectedReceivedCount = 0;
            var mod = Substitute.For<ISoftincModification>();
            mod.Name.Returns("kittens");
            mod.Scenarios.Returns(new List<Scenario>());

            ser.Serialize(mod);

            var receivedCount = writerProvider.ReceivedCalls()
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

            ser.Serialize(mod);

            var receivedCount = writerProvider.ReceivedCalls()
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

            ser.Serialize(mod);

            var receivedCount = writerProvider.ReceivedCalls()
                .Select(x => x.GetArguments()
                    .Select(ar => ar.ToString()
                    .Contains("Scenarios")))
                .Count();
            Assert.AreEqual(expectedReceivedCount, receivedCount);
        }
    }
}
