
namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Enum whose values decribe different quality levels for the shadow maps used by the engine.
    /// </summary>
    public enum ShadowMapQuality
    {
        /// <summary>
        /// Low quality shadow maps (usually with a resolution of 1024x1024).
        /// </summary>
        LOW,
        /// <summary>
        /// Normal quality shadow maps (usually with a resolution of 2048x2048).
        /// </summary>
        NORMAL,
        /// <summary>
        /// High quality shadow maps (usually with a resolution of 4096x4096).
        /// </summary>
        HIGH
    };
}
