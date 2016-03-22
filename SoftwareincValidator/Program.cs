using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy.Impl;
using SoftwareincValidator.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Autofac;
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
            var loader = _container.Resolve<ISoftincModificationLoader>();

            loader.ModComponentValidation += (s, e) => Console.WriteLine(e);
            loader.XmlValidation += (s, e) => Console.WriteLine(e);

            var mod = loader.Load(@"C:\Users\jdphe\Downloads\Resources");
        }
    }
}
