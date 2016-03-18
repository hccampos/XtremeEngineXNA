namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Enum whose values decribe different quality levels for the shadow maps used by the engine.
    /// </summary>
    public enum GuiDrawMode
    {
        /// <summary>
        /// Draws the GUI onto the back buffer.
        /// </summary>
        BACK_BUFFER,
        /// <summary>
        /// Draws the GUI onto a separate texture which can be accessed from the renderer.
        /// </summary>
        SEPARATE_TEXTURE,
        /// <summary>
        /// Draws the GUI to the final frame texture which can be accessed from the renderer.
        /// </summary>
        FINAL_TEXTURE
    };
}
