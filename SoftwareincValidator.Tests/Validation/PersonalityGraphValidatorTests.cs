using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Proxy.Impl;
using SoftwareincValidator.Validation;
using SoftwareincValidator.Validation.Impl;

namespace SoftwareincValidator.Tests.Validation
{
    [TestClass]
    public class PersonalityGraphValidatorTests
    {
        private ModComponentValidator<PersonalityGraph> _validator;
        private IXmlSerializer<PersonalityGraph> _serializer;
        private ISchemaProvider _schemaProvider;

        [TestInitialize]
        public void Init()
        {
            // Concrete dependencies are passed in because these are tests of the xsd schema.
            _serializer = new XmlSerializerProxy<PersonalityGraph>(new SchemaProvider(new FileSystemProxy()));
            _schemaProvider = new SchemaProvider(new FileSystemProxy());

            _validator = new ModComponentValidator<PersonalityGraph>(_serializer, _schemaProvider);
        }

        [TestMethod]
        public void Validate_PassedMissingName_FailsValidation()
        {
            const string expectedMessage = "The element 'Personality' has invalid child element 'Aptitude'. List of possible elements expected: 'Name'.";
            const int expectedCount = 1;
            var personalities = new PersonalityGraph
            {
                Incompatibilities = new string[][] { },
                Personalities = new[]
                {
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Relationships = new PersonalityGraphPersonalityRelationships
                        {
                            Relation = new []
                            {
                                new PersonalityGraphPersonalityRelationshipsRelation
                                {
                                    Name = "Test Personality 2", Value = 1
                                }
                            }
                        }
                    },
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Name = "Test Personality 2",
                        Relationships = new PersonalityGraphPersonalityRelationships()
                    },
                }
            };

            var actual = _validator.Validate(personalities);

            Assert.AreEqual(expectedCount, actual.Count());
            Assert.IsTrue(actual.Single().Message.Contains(expectedMessage));
        }

        [TestMethod]
        public void Validate_PassedEmptyIncompatibilities_PassesValidation()
        {
            const string expectedMessage = "[PersonalityGraph] Valid.";
            const int expectedCount = 1;
            var personalities = new PersonalityGraph
            {
                Incompatibilities = new string[][] {},
                Personalities = new[]
                {
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Name = "Test Personality 1",
                        Relationships = new PersonalityGraphPersonalityRelationships
                        {
                            Relation = new []
                            {
                                new PersonalityGraphPersonalityRelationshipsRelation
                                {
                                    Name = "Test Personality 2", Value = 1
                                }
                            }
                        }
                    },
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Name = "Test Personality 2",
                        Relationships = new PersonalityGraphPersonalityRelationships()
                    },
                }
            };

            var actual = _validator.Validate(personalities);

            Assert.AreEqual(expectedCount, actual.Count());
            Assert.AreEqual(expectedMessage, actual.Single().Message);
        }

        [TestMethod]
        public void Validate_PassedValidPersonalities_PassesValidation()
        {
            const string expectedMessage = "[PersonalityGraph] Valid.";
            const int expectedCount = 1;
            var personalities = new PersonalityGraph
            {
                Incompatibilities = new []
                {
                    new [] { "Test Personality 1", "Test Personality 2" }
                }, 
                Personalities = new []
                {
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Name = "Test Personality 1",
                        Relationships = new PersonalityGraphPersonalityRelationships
                        {
                            Relation = new []
                            {
                                new PersonalityGraphPersonalityRelationshipsRelation
                                {
                                    Name = "Test Personality 2", Value = 1
                                }
                            }
                        }
                    },
                    new PersonalityGraphPersonality
                    {
                        Aptitude = 0.2,
                        Diligence = 0.3,
                        Leadership = -0.5,
                        Name = "Test Personality 2", 
                        Relationships = new PersonalityGraphPersonalityRelationships()
                    },
                }
            };

            var actual = _validator.Validate(personalities);

            Assert.AreEqual(expectedCount, actual.Count());
            Assert.AreEqual(expectedMessage, actual.Single().Message);
        }
    }
}
