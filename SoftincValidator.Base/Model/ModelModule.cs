using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace SoftwareincValidator.Model
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SoftincModification>().As<ISoftincModification>();
        }
    }
}
