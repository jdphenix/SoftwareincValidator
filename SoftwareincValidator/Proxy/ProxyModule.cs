using System;
using System.Collections.Generic;
using System.IO;
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
            builder.RegisterType<FileSystemProxy>().As<IFileSystem>();
            builder.RegisterGeneric(typeof(XmlSerializerProxy<>))
                .As(typeof(IXmlSerializer<>));

            builder.RegisterType<FileInfoProxy>().As<IFileInfo>();
            builder.RegisterType<FileInfo>().AsSelf();

            builder.RegisterType<DirectoryInfoProxy>().As<IDirectoryInfo>();
            builder.RegisterType<DirectoryInfo>().AsSelf();

        }
    }
}
