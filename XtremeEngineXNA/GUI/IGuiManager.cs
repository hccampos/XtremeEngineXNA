using System.Collections.Generic;
using XtremeEngineXNA.Gui.Widgets;

namespace XtremeEngineXNA.Gui
{
    /// <summary>
    /// Interface which defines the methods that a GUI manager must provide. A GUI manager 
    /// contains a graph and is responsible for managing and updating the GUI nodes in it.
    /// </summary>
    public interface IGuiManager : IPlugin
    {
        /// <summary>
        /// Gets the GUI widget which represents the screen.
        /// </summary>
         ScreenWidget Screen { get; }

        /// <summary>
        /// Gets the list of nodes which are to be drawn to the screen.
        /// </summary>
        List<IDrawable> VisibleDrawableNodes { get; }
    }
}
