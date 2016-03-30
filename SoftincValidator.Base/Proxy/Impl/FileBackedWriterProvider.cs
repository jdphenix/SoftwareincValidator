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
        private string _outPath = ConfigurationManager.AppSettings["outputPath"];
        private readonly string _originalOutPath;

        public FileBackedWriterProvider()
        {
            _originalOutPath = _outPath;
        }

        public void OverrideLocation(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"{nameof(path)} must be provided.");
            }

            _outPath = path;
        }

        public void ResetLocation()
        {
            _outPath = _originalOutPath;
        }

        public TextWriter GetWriter(string writerParameter)
        {
            var fileStream = GetOutputStream(writerParameter);
            var writer = new StreamWriter(fileStream);

            return writer;
        }

        public Stream GetOutputStream(string writerParameter)
        {
            var fullOutputPath = Path.Combine(_outPath, writerParameter);
            var fi = new FileInfo(fullOutputPath);
            fi.Directory?.Create();

            var fileStream = new FileStream(fullOutputPath, FileMode.Create);
            return fileStream;
        }
    }
}
