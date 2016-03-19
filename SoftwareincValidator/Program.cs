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

namespace SoftwareincValidator
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static readonly IContainer _container;

        static Program()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ModelModule>();
            builder.RegisterModule<ProxyModule>();
            builder.RegisterModule<SerializationModule>();

            _container = builder.Build();
        }

        private static void Main(string[] args)
        {
            var ser = _container.Resolve<ISoftincModificationSerializer>();
            var loader = _container.Resolve<ISoftincModificationLoader>();

            RegisterBaseXmlMutations(ser);

            var mod = loader.Load(@"D:\SteamLibrary\steamapps\common\Software Inc\Mods\Mo' Stuff Mod (v0.1.6)");

            ser.Serialize(mod);
        }

        private static void RegisterBaseXmlMutations(ISoftincModificationSerializer ser)
        {
            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                // Hackish removing of XML declaration.
                if (e.Document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    e.Document.RemoveChild(e.Document.FirstChild);
                }
            };

            // TODO: Refactor these out to a.. plugin? 
            ser.Serializing += (s, e) =>
            {
                Action<XmlNode> addTextnodeIfEmpty = null;
                addTextnodeIfEmpty = node =>
                {
                    if (node.NodeType == XmlNodeType.Text) return;

                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            addTextnodeIfEmpty(child);
                        }
                    }
                    else
                    {
                        node.AppendChild(node.OwnerDocument.CreateTextNode(string.Empty));
                    }
                };

                addTextnodeIfEmpty(e.Document);
            };
        }
    }
}
