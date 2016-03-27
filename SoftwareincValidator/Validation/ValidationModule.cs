using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SoftwareincValidator.Model.Generated;
using SoftwareincValidator.Validation.Impl;

namespace SoftwareincValidator.Validation
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchemaProvider>().As<ISchemaProvider>();
            builder.RegisterType<ModValidator>().As<IModValidator>();

            builder.RegisterAssemblyTypes(typeof (IModComponentValidator<>).Assembly)
                .Where(t => t.GetInterface("IModComponentValidator`1") != null)
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(XmlComponentValidator<>))
                .As(typeof(IXmlComponentValidator<>));

            builder.RegisterType<XmlSoftwareTypeValidator>().As<IXmlComponentValidator<SoftwareType>>();
            builder.RegisterType<XmlBaseFeaturesValidator>()
                .As<IXmlComponentValidator<BaseFeatures>>();
        }
    }
}
