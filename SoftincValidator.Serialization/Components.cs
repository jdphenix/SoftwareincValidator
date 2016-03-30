using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SoftwareincValidator.Model;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization;
using SoftwareincValidator.Validation;

namespace SoftincValidator.Serialization
{
    public static class Components
    {
        private static readonly IContainer _container;

        static Components()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ValidationModule>();
            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<ProxyModule>();
            builder.RegisterModule<SerializationModule>();

            _container = builder.Build();
        }

        /// <summary>
        /// Get an instance of a modification serializer, which allows serialization 
        /// of modification to the file system.
        /// </summary>
        /// <returns>A modification serializer.</returns>
        public static ISoftincModificationSerializer GetSerializer()
        {
            return _container.Resolve<ISoftincModificationSerializer>();
        }

        /// <summary>
        /// Get an instance of a modification loader, which allows loading and 
        /// validation of modifications from the file system.
        /// </summary>
        /// <returns>A modification loader.</returns>
        public static ISoftincModificationLoader GetLoader()
        {
            return _container.Resolve<ISoftincModificationLoader>();
        }
    }
}
