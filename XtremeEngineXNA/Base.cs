using System;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Base class of all the objects in XtremeEngine.
    /// </summary>
    public abstract class Base : IBase
    {
        /// <summary>
        /// Root object to which this object belongs.
        /// </summary>
        private Root mRoot;

        /// <summary>
        /// Initializes the base class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public Base(Root root)
        {
            if (root == null)
            {
                throw new Exception("Base(): null root object.");
            }
            mRoot = root;
        }

        /// <summary>
        /// Gets the root object.
        /// </summary>
        public Root Root
        {
            get { return mRoot; }
        }
    }
}
