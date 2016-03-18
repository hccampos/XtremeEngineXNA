using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA.Objects;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Interface of the text 3D component. This component creates a 3D text model to represent the
    /// entity.
    /// </summary>
    public interface IText3DComponent : IEntityComponent
    {
        /// <summary>
        /// 3D model of the text. This can be used to set materials and other properties.
        /// </summary>
        Text3D TextModel { get; }

        /// <summary>
        /// Gets or sets the text which is displayed by the component.
        /// </summary>
        string Text { get; set; }
    }
}
