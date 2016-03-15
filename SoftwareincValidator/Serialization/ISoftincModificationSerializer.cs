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
        void Serialize(SoftincModification mod);

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
