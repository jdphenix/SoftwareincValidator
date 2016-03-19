﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Validation;
using SoftwareincValidator.Validation.Impl;

namespace SoftwareincValidator.Tests.Validation
{
    [TestClass]
    public class ScenarioValidatorTests
    {
        private static readonly ScenarioValidator Validator = new ScenarioValidator();

        [TestMethod]
        public void Validate_PassedMissingEvents_FailsValidation()
        {
            const int expectedCount = 1;

            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedMissingYears_FailsValidation()
        {
            const int expectedCount = 1;

            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedMissingGoals_FailsValidation()
        {
            const int expectedCount = 1;

            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Years = new[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedMissingMoney_FailsValidation()
        {
            const int expectedCount = 1;

            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedMissingName_FailsValidation()
        {
            const int expectedCount = 1;

            var scenario = new Scenario
            {
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedValidScenario_PassesValidation()
        {
            const int expectedCount = 1;
            const string expectedMessage = "Successful validation of Scenario:Low Money Test";
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                Simulation = ScenarioSimulation.TRUE,
                SimulationSpecified = true,
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(expectedMessage, validationResults.Single().Message);
            Assert.AreEqual(ValidationSource.Undefined, validationResults.Single().Source);
            Assert.AreEqual(ValidationLevel.Success, validationResults.Single().Level);
        }

        [TestMethod]
        public void Validate_PassedOutOfLowRangeYearScenario_FailsValidation()
        {
            const int expectedCount = 1;
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1969, 1978 },
                ForceEnvironment = 0,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedOutOfLowRangeMoneyScenario_FailsValidation()
        {
            const int expectedCount = 1;
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { -1, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                ForceEnvironment = 0,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedOutOfLowRangeEnvironmentScenario_FailsValidation()
        {
            const int expectedCount = 1;
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                ForceEnvironment = -1,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedOutOfHighRangeEnvironmentScenario_FailsValidation()
        {
            const int expectedCount = 1;
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                ForceEnvironment = 4,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(ValidationLevel.Error, validationResults.Single().Level);
            Assert.AreEqual(ValidationSource.XmlSchema, validationResults.Single().Source);
        }

        [TestMethod]
        public void Validate_PassedLowRangeEnvironmentScenario_PassesValidation()
        {
            const int expectedCount = 1;
            const string expectedMessage = "Successful validation of Scenario:Low Money Test";
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] { 5000, 15000, 35000 },
                Goals = new[] { "Money 200000" },
                Years = new[] { 1976, 1978 },
                ForceEnvironment = 0,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(expectedMessage, validationResults.Single().Message);
            Assert.AreEqual(ValidationSource.Undefined, validationResults.Single().Source);
            Assert.AreEqual(ValidationLevel.Success, validationResults.Single().Level);
        }

        [TestMethod]
        public void Validate_PassedHighRangeEnvironmentScenario_PassesValidation()
        {
            const int expectedCount = 1;
            const string expectedMessage = "Successful validation of Scenario:Low Money Test";
            var scenario = new Scenario
            {
                Name = "Low Money Test",
                Money = new[] {5000, 15000, 35000},
                Goals = new[] {"Money 200000"},
                Years = new[] {1976, 1978},
                ForceEnvironment = 3,
                ForceEnvironmentSpecified = true,
                Events = new string[0]
            };

            var actual = Validator.Validate(scenario);

            var validationResults = actual as ValidationResult[] ?? actual.ToArray();

            Assert.AreEqual(expectedCount, validationResults.Length);
            Assert.AreEqual(expectedMessage, validationResults.Single().Message);
            Assert.AreEqual(ValidationSource.Undefined, validationResults.Single().Source);
            Assert.AreEqual(ValidationLevel.Success, validationResults.Single().Level);
        }
    }
}