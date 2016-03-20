using System.IO;

namespace SoftwareincValidator.Proxy
{
    public interface IFileInfo
    {
        string Name { get; }
        TextReader OpenText();
    }
}