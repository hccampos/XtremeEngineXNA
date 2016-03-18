using XtremeEngineXNA.EntityComponent.Components;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Class which creates components.
    /// </summary>
    public class ComponentFactory
    {
        private ComponentFactory() { }

        /// <summary>
        /// Creates a new IModelComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An implementation of the IModelComponent interface.</returns>
        public static IModelComponent createModelComponent(Root root, string name)
        {
            return new DefaultModelComponent(root, name);
        }

        /// <summary>
        /// Creates a new IPhysicsComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An implementation of the IPhysicsComponent interface.</returns>
        public static IPhysicsComponent createPhysicsComponent(Root root, string name)
        {
            return new DefaultPhysicsComponent(root, name);
        }

        /// <summary>
        /// Creates a new IProximitySensorComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An implementation of the IProximitySensorComponent interface.</returns>
        public static IProximitySensorComponent createProximitySensorComponent(Root root, string name)
        {
            return new DefaultProximitySensorComponent(root, name);
        }

        /// <summary>
        /// Creates a new IQuadComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An implementation of the IQuadComponent interface.</returns>
        public static IQuadComponent createQuadComponent(Root root, string name)
        {
            return new DefaultQuadComponent(root, name);
        }

        /// <summary>
        /// Creates a new ISpatialComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An implementation of the ISpatialComponent interface.</returns>
        public static ISpatialComponent createSpatialComponent(Root root, string name)
        {
            return new DefaultSpatialComponent(root, name);
        }

        /// <summary>
        /// Creates a new IText3DComponent.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        /// <returns>An IText3DComponent of the ISpatialComponent interface.</returns>
        public static IText3DComponent createText3DComponent(Root root, string name)
        {
            return new DefaultText3DComponent(root, name);
        }
    }
}
