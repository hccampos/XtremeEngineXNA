using System;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which must be implemented by all the classes that which to be updated by 
    /// XtremeEngine.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        void Update(TimeSpan elapsedTime);

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>The update order of the object.</value>
        int UpdateOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
