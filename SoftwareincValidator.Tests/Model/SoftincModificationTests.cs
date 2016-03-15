using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareincValidator.Model;

namespace SoftwareincValidator.Model.Tests
{
    [TestClass]
    public class SoftincModificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassedEmptyName_ShouldThrowException()
        {
            new SoftincModification(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassedNullName_ShouldThrowException()
        {
            new SoftincModification(null);
        }

        [TestMethod]
        public void Constructor_PassedValidName_ShouldCreate()
        {
            const string expected = "Tester";

            var model = new SoftincModification(expected);

            Assert.AreEqual(expected, model.Name);
        }
    }
}
