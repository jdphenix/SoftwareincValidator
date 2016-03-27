using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SoftwareincValidator.Dto.Generated;
using SoftwareincValidator.Proxy;
using SoftwareincValidator.Serialization.Impl;

namespace SoftwareincValidator.Serialization
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SoftincFileModificationLoader>().As<ISoftincModificationLoader>();

            builder.RegisterType<SoftincModificationXmlSerializer>().As<ISoftincModificationSerializer>();

            builder.RegisterType<SoftwareTypeXmlSerializer>().As<IXmlSerializer<SoftwareType>>();
            builder.RegisterType<BaseFeaturesXmlSerializer>().As<IXmlSerializer<BaseFeatures>>();
        }
    }
}
