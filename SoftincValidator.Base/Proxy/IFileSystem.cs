using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy
{
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        string DirectoryGetCurrentDirectory();

        char PathDirectorySeparatorChar { get; }
        string PathCombine(string path1, string path2);
        string PathGetDirectoryName(string path);
        string PathGetFullPath(string path);
        string PathGetFileNameWithoutExtension(string path);

        bool FileExists(string path);
        TextReader FileOpenText(string location);
    }
}
