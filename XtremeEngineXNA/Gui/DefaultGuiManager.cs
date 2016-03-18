using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XtremeEngineXNA.Scene;
using XtremeEngineXNA.Gui.Widgets;

namespace XtremeEngineXNA.Gui
{
    /// <summary>
    /// Default implementation of the <see cref="IGuiManager"/> interface.
    /// </summary>
    public class DefaultGuiManager : PluginBase, IUpdateable, IGuiManager
    {
        #region Attributes

        /// <summary>
        /// Widget which represents the screen.
        /// </summary>
        private ScreenWidget mScreen;

        /// <summary>
        /// Whether the manager is enabled.
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Value used to determine the order in which plug-ins are updated.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGuiManager"/> class.
        /// </summary>
        /// <param name="root">Root object to which the plug-in belongs.</param>
        /// <param name="name">Name of the new plug-in.</param>
        public DefaultGuiManager(Root root, string name)
            : base(root, name)
        {
            mScreen = new ScreenWidget(root);
            mEnabled = true;
        }

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public void Update(TimeSpan elapsedTime)
        {
            mScreen.ValidateProperties();
            mScreen.ValidateSize();
            mScreen.ValidateLayout();

            mScreen.UpdateMatrices(Matrix.Identity);
            UpdateNode(elapsedTime, mScreen);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if (value != mEnabled)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                    {
                        EnabledChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>
        /// The update order of the object.
        /// </value>
        public int UpdateOrder
        {
            get { return mUpdateOrder; }
            set
            {
                if (value != mUpdateOrder)
                {
                    mUpdateOrder = value;
                    if (UpdateOrderChanged != null)
                    {
                        UpdateOrderChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the GUI widget which represents the screen.
        /// </summary>
        public ScreenWidget Screen
        {
            get { return mScreen; }
        }

        /// <summary>
        /// Gets the list of nodes which are to be drawn to the screen.
        /// </summary>
        public List<IDrawable> VisibleDrawableNodes
        {
            get
            {
                List<IDrawable> visibleNodes = new List<IDrawable>();
                foreach (Node node in mScreen.Children)
                {
                    GetVisibleDrawableNodes(node, ref visibleNodes);
                }
                return visibleNodes;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Fills a list with all the children of a node which are visible.
        /// </summary>
        /// <param name="node">Node which is to be recursively searched.</param>
        /// <param name="visibleNodes">List to be filled with the drawable nodes.</param>
        private void GetVisibleDrawableNodes(Node node, ref List<IDrawable> visibleNodes)
        {
            // Verify if the node is an IDrawable.
            IDrawable drawable = node as IDrawable;

            // Add the node if it is drawable and visible.
            if (drawable != null && drawable.Visible)
            {
                //Add the node to the list of drawable nodes.
                visibleNodes.Add(drawable);
            }

            //If the node is not a drawable or, if it is drawable and is visible, we check all its
            //children.
            if (drawable == null || drawable.Visible)
            {
                // Search the node's children.
                foreach (Node n in node.Children)
                {
                    GetVisibleDrawableNodes(n, ref visibleNodes);
                }
            }
        }

        /// <summary>
        /// Updates a node and all its children.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        /// <param name="node">Node which is to be updated.</param>
        private void UpdateNode(TimeSpan elapsedTime, Node node)
        {
            // Verify if the node is updateable.
            IUpdateable updateable = node as IUpdateable;
            // Only update the node and it's children if the node is enabled.
            if (updateable != null && updateable.Enabled)
            {
                // Update the node.
                updateable.Update(elapsedTime);

                // Update the children.
                foreach (Node n in node.Children)
                {
                    UpdateNode(elapsedTime, n);
                }
            }
        }

        #endregion
    }
}
