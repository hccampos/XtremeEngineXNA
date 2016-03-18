using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which must be implemented by all the objects that are to be alpha blended. Alpha
    /// blended objects are sorted and drawn after all the other objects have been drawn.
    /// </summary>
    public interface IAlphaBlended : IDrawableObject
    {
    }
}
