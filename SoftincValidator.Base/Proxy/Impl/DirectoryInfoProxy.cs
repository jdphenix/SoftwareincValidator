using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class DirectoryInfoProxy : IDirectoryInfo
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryInfoProxy(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _directoryInfo = new DirectoryInfo(path);
        }

        public IEnumerable<IFileInfo> GetFiles()
        {
            return _directoryInfo
                .EnumerateFiles()
                .Select(file => new FileInfoProxy(file));
        }

        public bool Exists => _directoryInfo.Exists;
    }
}
