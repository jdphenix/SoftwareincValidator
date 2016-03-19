using System.IO;

namespace SoftwareincValidator.Proxy
{
    public interface IFileInfo
    {
        TextReader OpenText();
    }
}