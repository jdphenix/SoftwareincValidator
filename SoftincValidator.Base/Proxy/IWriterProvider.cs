using System.IO;

namespace SoftwareincValidator.Proxy
{
    public interface IWriterProvider
    {
        /// <summary>
        /// Overrides the location created by this writer provider, in lieu of the specified value in configuration.
        /// </summary>
        /// <param name="path">The path to override with.</param>
        void OverrideLocation(string path);

        /// <summary>
        /// Resets the location to the value specified by configuration.
        /// </summary>
        void ResetLocation();

        /// <summary>
        /// Get a new TextWriter.
        /// </summary>
        /// <param name="writerParameter">A string parameter whose use is implementation specific.</param>
        /// <returns>A new TextWriter.</returns>
        TextWriter GetWriter(string writerParameter);

        /// <summary>
        /// Get a new Stream.
        /// </summary>
        /// <param name="writerParameter">A string parameter whose use is implementation specific.</param>
        /// <returns>A new Stream.</returns>
        Stream GetOutputStream(string writerParameter);
    }
}
