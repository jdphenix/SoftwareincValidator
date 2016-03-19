using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SoftwareincValidator.Proxy.Impl;

namespace SoftwareincValidator.Proxy
{
    public class ProxyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileBackedWriterProvider>().As<IWriterProvider>();
        }
    }
}
