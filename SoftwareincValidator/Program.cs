using SoftwareincValidator.Proxy.Impl;
using SoftwareincValidator.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Autofac;
using SoftwareincValidator.Dto;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Validation;

namespace SoftwareincValidator
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static readonly IContainer _container;

        static Program()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ValidationModule>();
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<ProxyModule>();
            builder.RegisterModule<SerializationModule>();

            _container = builder.Build();
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Expected a single argument, a path to a modification folder.");
            }

            var fileSystem = _container.Resolve<IFileSystem>();

            if (!fileSystem.DirectoryExists(args[0]))
            {
                Console.WriteLine($"Provided path {args[0]} doesn't exist.");
            }

            var loader = _container.Resolve<ISoftincModificationLoader>();

            loader.ModComponentValidation += (s, e) =>
            {
                Console.WriteLine(e);
            };
            loader.XmlValidation += (s, e) =>
            {
                Console.WriteLine(e);
            };

            var mod = loader.Load(args[0]);

            if (mod != null)
            {
                Console.WriteLine($"{mod.Name} loaded.");

                Console.WriteLine("Specializations defined by mod: ");
                mod.GetSpecializations().ToList().ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine("Failed to load modification.");
            }
        }
    }
}