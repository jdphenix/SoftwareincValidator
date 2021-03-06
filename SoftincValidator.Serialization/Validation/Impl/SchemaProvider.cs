﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization;

namespace SoftwareincValidator.Validation.Impl
{
    internal class SchemaProvider : ISchemaProvider
    {
        private readonly IDictionary<Type, XmlSchema> _keyedSchemata;

        private readonly IFileSystem _fileSystem;

        public SchemaProvider(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            _fileSystem = fileSystem;

            _keyedSchemata = new Dictionary<Type, XmlSchema>
            {
                { typeof(Scenario), Load("xsd\\scenario.xsd")},
                { typeof(PersonalityGraph), Load("xsd\\personalities.xsd") },
                { typeof(CompanyType), Load("xsd\\company-type.xsd") },
                { typeof(CompanyTypes), Load("xsd\\company-type-delete.xsd") }
            };
        }

        private XmlSchema Load(string location)
        {
            try
            {
                using (var reader = _fileSystem.FileOpenText(location))
                {
                    return XmlSchema.Read(
                        reader,
                        (s, e) =>
                        {
                            if (e.Severity == XmlSeverityType.Error)
                            {
                                throw new ModificationLoadException(e.Message, null);
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                throw new ModificationLoadException(ex.Message, ex);
            }
        }

        public XmlSchema Schema(Type type)
        {
            try
            {
                return _keyedSchemata[type];
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidOperationException($"Schema of {type} not loaded. This is a programmer error. If {type} has a custom validator not using a schema, ensure you use an instance of that implementation.");
            }
        } 
    }
}
