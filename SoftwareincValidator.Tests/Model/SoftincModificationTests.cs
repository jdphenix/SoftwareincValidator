using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareincValidator.Model;

namespace SoftwareincValidator.Tests.Model
{
    [TestClass]
    public class SoftincModificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassedEmptyName_ShouldThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new SoftincModification(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassedNullName_ShouldThrowException()
        {
            // ReSharper disable once ObjectCreationAsStatement
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
