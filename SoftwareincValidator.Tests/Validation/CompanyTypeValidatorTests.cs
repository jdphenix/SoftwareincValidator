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
    public class CompanyTypeValidatorTests
    {
        private ModComponentValidator<CompanyType> _validator;
        private ISchemaProvider _schemaProvider;

        [TestInitialize]
        public void Init()
        {
            // Concrete dependencies are passed in because these are tests of the xsd schema.
            _schemaProvider = new SchemaProvider(new FileSystemProxy());

            _validator = new ModComponentValidator<CompanyType>(_schemaProvider);
        }

        [TestMethod]
        public void Validate_PassedValidCompanyType_PassedValidation()
        {
            const string expectedMessage = "[CompanyType] Valid.";
            var companyType = new CompanyType
            {
                Min = 1,
                Max = 2,
                Specialization = "Test Spec",
                PerYear = 0.2,
                Types = new CompanyTypeTypes
                {
                    Type = new CompanyTypeTypesType
                    {
                        Category = "Computer",
                        Software = "Test Spec",
                        Value = 1
                    }
                }
            };

            var actual = _validator.Validate(companyType);

            Assert.AreEqual(expectedMessage, actual.Single().Message);
        }
    }
}
