using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class FileInfoProxy : IFileInfo
    {
        private readonly FileInfo _fileInfo;

        public FileInfoProxy(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            _fileInfo = fileInfo;
        }

        public TextReader OpenText() => _fileInfo.OpenText();
    }
}
