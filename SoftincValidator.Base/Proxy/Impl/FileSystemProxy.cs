using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy.Impl
{
    internal class FileSystemProxy : IFileSystem
    {
        public char PathDirectorySeparatorChar => Path.DirectorySeparatorChar;
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public string DirectoryGetCurrentDirectory() => Directory.GetCurrentDirectory();
        public string PathCombine(string path1, string path2) => Path.Combine(path1, path2);
        public string PathGetDirectoryName(string path) => Path.GetDirectoryName(path);
        public string PathGetFullPath(string path) => Path.GetFullPath(path);
        public string PathGetFileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);
        public bool FileExists(string path) => File.Exists(path);
        public TextReader FileOpenText(string location) => File.OpenText(location);
    }
}
