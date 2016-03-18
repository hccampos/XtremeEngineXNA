using System;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface for a drawable plug-in, that is, a plug-in that can be drawn by XtremeEngine.
    /// </summary>
    public interface IDrawablePlugin: IPlugin, IDrawable
    {
    }
}
