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

            builder.RegisterGeneric(typeof(ModComponentValidator<>))
                .As(typeof(IModComponentValidator<>));

            builder.RegisterType<SoftwareTypeValidator>().As<IModComponentValidator<SoftwareType>>();
        }
    }
}
