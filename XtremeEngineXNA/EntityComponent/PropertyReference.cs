using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Class which represents a reference to a property of a component in an entity. For instance,
    /// lets say we wanted to access the Position property of the Spatial component. We could would
    /// create a new PropertyReference with the reference of @Spatial.Position.
    /// </summary>
    public class PropertyReference
    {
        private string mReference;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">Path to the property.</param>
        public PropertyReference(string reference)
        {
            mReference = reference;
        }

        /// <summary>
        /// Gets the path to the property.
        /// </summary>
        public string Reference
        {
            get { return mReference; }
        }
    }
}
