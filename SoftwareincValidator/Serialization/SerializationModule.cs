using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace SoftwareincValidator.Serialization
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SoftincFileModificationLoader>().As<ISoftincModificationLoader>();
            builder.RegisterType<SoftincModificationXmlSerializer>().As<ISoftincModificationSerializer>();
        }
    }
}
