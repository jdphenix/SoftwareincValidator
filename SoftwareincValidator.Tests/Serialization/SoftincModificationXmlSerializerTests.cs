using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareincValidator.Serialization;
using SoftwareincValidator.Proxy;
using NSubstitute;

namespace SoftwareincValidator.Tests.Serialization
{
    [TestClass]
    public class SoftincModificationXmlSerializerTests
    {
        private SoftincModificationXmlSerializer ser;
        private IWriterProvider writerProvider;

        [TestInitialize]
        public void Initialize()
        {
            writerProvider = Substitute.For<IWriterProvider>();
            ser = new SoftincModificationXmlSerializer(writerProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PassedNullWriterProvider_ThrowsException()
        {
            ser = new SoftincModificationXmlSerializer(null);
        }
    }
}
