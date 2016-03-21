using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Proxy;

namespace SoftwareincValidator.Serialization
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SoftincFileModificationLoader>().As<ISoftincModificationLoader>();

            builder.RegisterType<SoftincModificationXmlSerializer>().As<ISoftincModificationSerializer>();

            builder.RegisterType<SoftwareTypeXmlSerializer>().As<IXmlSerializer<SoftwareType>>();
        }
    }
}
