using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Proxy
{
    public interface IDirectoryInfo
    {
        IEnumerable<IFileInfo> GetFiles();
        bool Exists { get; }
    }
}
