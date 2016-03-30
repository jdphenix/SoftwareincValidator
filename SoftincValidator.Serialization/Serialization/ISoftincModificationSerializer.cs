using SoftwareincValidator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Serialization
{
    public interface ISoftincModificationSerializer
    {
		/// <summary>
        /// Serialize the provided Software Inc modification. 
        /// </summary>
        /// <param name="mod">The modification to serialize.</param>
        void Serialize(ISoftincModification mod);

        /// <summary>
        /// Serialize the provided Software Ind modification.
        /// </summary>
        /// <param name="mod">The modification to serialize.</param>
        /// <param name="path">The path to save the serialized modification to. The mod will be saved as 
        /// as new directory under the specified directory.</param>
        void Serialize(ISoftincModification mod, string path);

        /// <summary>
        /// Fired immediately before serialization is started.
        /// </summary>
        event EventHandler<SerializingEventArgs> Serializing;

		/// <summary>
        /// Fired immediately after serialization is completed.
        /// </summary>
        event EventHandler Serialized;
    }
}
