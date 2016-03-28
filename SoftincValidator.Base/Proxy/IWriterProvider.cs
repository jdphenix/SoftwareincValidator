using System.IO;

namespace SoftwareincValidator.Proxy
{
    public interface IWriterProvider
    {
        /// <summary>
        /// Get a new TextWriter.
        /// </summary>
        /// <param name="writerParameter">A string parameter whose use is implementation specific.</param>
        /// <returns>A new TextWeiter.</returns>
        TextWriter GetWriter(string writerParameter);

        /// <summary>
        /// Get a new Stream.
        /// </summary>
        /// <param name="writerParameter">A string parameter whose use is implementation specific.</param>
        /// <returns>A new Stream.</returns>
        Stream GetOutputStream(string writerParameter);
    }
}
