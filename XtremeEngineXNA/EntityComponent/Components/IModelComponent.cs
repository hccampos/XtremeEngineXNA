using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Content;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Interface of the model component. A model component loads a 3D model and adds it to the
    /// scene manager to represent the entity.
    /// </summary>
    public interface IModelComponent : IEntityComponent
    {
        /// <summary>
        /// Loads the model from the specified file.
        /// </summary>
        /// <param name="file">File which is to be loaded.</param>
        void LoadModel(string file);

        /// <summary>
        /// Generates the model to represent the entity from a previously loaded XNA model.
        /// </summary>
        /// <param name="xnaModel">Model which is to be added to the component.</param>
        void GenerateFromXNAModel(Model xnaModel);

        /// <summary>
        /// Gets the node which contains the model of the entity.
        /// </summary>
        ModelNode ModelNode { get; }
    }
}
