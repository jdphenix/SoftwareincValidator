using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy.Impl
{
    class FileBackedWriterProvider : IWriterProvider
    {
        private static readonly string OutPath = ConfigurationManager.AppSettings["outputPath"];

        public TextWriter GetWriter(string writerParameter)
        {
            var fullOutputPath = Path.Combine(OutPath, writerParameter);
            var fi = new FileInfo(fullOutputPath);
            fi.Directory?.Create();

            var fileStream = new FileStream(fullOutputPath, FileMode.Create);
            var writer = new StreamWriter(fileStream);

            return writer;
        }
    }
}
