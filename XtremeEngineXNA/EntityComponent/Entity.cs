using System;
using System.Collections.Generic;
using System.Reflection;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Delegate for the ComponentAdded and ComponentRemoved events.
    /// </summary>
    /// <param name="entity">Entity to which a component was added or removed.</param>
    /// <param name="component">Component which was added or removed.</param>
    public delegate void ComponentAddedOrRemovedDelegate(Entity entity, IEntityComponent component);

    /// <summary>
    /// Class which represents an entity. All objects in a game made with XtremeEngine are entities.
    /// An entity can be made up of several components that add functionality to it. For instance,
    /// an entity can have a physics component to add physical behavior and a visual component
    /// which adds a model or another object to the XtremeEngine scene manager to render the entity.
    /// </summary>
    public class Entity : Base, IUpdateable
    {
        #region Attributes

        /// <summary>
        /// Name of the entity.
        /// </summary>
        private string mName;

        /// <summary>
        /// List which stores all the components that add functionality to the entity.
        /// </summary>
        private List<IEntityComponent> mComponents;

        /// <summary>
        /// Whether the entity is enabled (i.e. is updated).
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Update order of the entity.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region Entity public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the entity belongs.</param>
        /// <param name="name">Name of the entity.</param>
        public Entity(Root root, string name) : base(root)
        {
            mName = name;
            mComponents = new List<IEntityComponent>();
            mEnabled = true;
            mUpdateOrder = 0;
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <param name="component">Component which is to be added to the entity.</param>
        public void AddComponent(IEntityComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("Entity.AddComponent(): null component.");
            }

            if (mComponents.Find(c => c == component) != null)
            {
                throw new ArgumentException("Entity.AddComponent(): duplicate component.");
            }

            component.Owner = this;
            mComponents.Add(component);

            component.OnAdd();
            ResetComponents();

            if (ComponentAdded != null)
                ComponentAdded(this, component);
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <param name="component">Component which is to be removed from the entity.</param>
        public void RemoveComponent(IEntityComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("Entity.RemoveComponent(): null component.");
            }

            if (mComponents.Remove(component))
            {
                component.Owner = null;
                component.OnRemove();
                ResetComponents();

                if (ComponentRemoved != null)
                    ComponentRemoved(this, component);
            }
            else
            {
                throw new ArgumentException("Entity.RemoveComponent(): component not found.");
            }
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <param name="name">Name of the component which is to be removed from the entity.</param>
        public void RemoveComponent(string name)
        {
            if (name == null || name.Length == 0)
            {
                throw new ArgumentNullException("Entity.RemoveComponent(): null name.");
            }

            IEntityComponent component = mComponents.Find(c => c.Name == name);
            if (component == null)
            {
                throw new ArgumentException("Entity.RemoveComponent(): component not found.");
            }
            else
            {
                component.Owner = null;
                component.OnRemove();
                ResetComponents();

                if (ComponentRemoved != null)
                    ComponentRemoved(this, component);
            }

            mComponents.Remove(component);
        }

        /// <summary>
        /// Removes all the components from the entity.
        /// </summary>
        public void RemoveAllComponents()
        {
            List<IEntityComponent> toRemove = new List<IEntityComponent>(mComponents);
            foreach (IEntityComponent component in toRemove)
            {
                mComponents.Remove(component);
                component.OnRemove();

                if (ComponentRemoved != null)
                    ComponentRemoved(this, component);
            }
        }

        /// <summary>
        /// Gets the first component of the specified type that is found.
        /// </summary>
        /// <returns>
        /// The first component of the specified type that is found. If no component of the
        /// specified type is found, returns null.
        /// </returns>
        public T GetComponent<T>() where T : IEntityComponent
        {
            return (T)mComponents.Find(c => c is T);
        }

        /// <summary>
        /// Gets a list with all the components of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the components which are to be returned.</typeparam>
        /// <returns>A list with all the components of the specified type.</returns>
        public List<T> GetComponents<T>() where T : IEntityComponent
        {
            List<IEntityComponent> list = mComponents.FindAll(c => c is T);
            
            List<T> result = new List<T>();
            foreach (T component in list)
            {
                result.Add(component);
            }

            return result;
        }

        /// <summary>
        /// Gets a component of the specified type. If there are more than one components of the
        /// same type, a name can be provided to select the desired component.
        /// </summary>
        /// <param name="name">Name of the component which is to be returned.</param>
        /// <typeparam name="T">Type of the component which is to be returned.</typeparam>
        /// <returns>
        /// The component of the specified type that has the name passed as a parameter. If no
        /// component matching those criteria is found, returns null.
        /// </returns>
        public T GetComponent<T>(string name) where T : IEntityComponent
        {
            return (T)mComponents.Find(c => (c is T && c.Name == name));
        }

        /// <summary>
        /// Gets the component with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEntityComponent GetComponent(string name)
        {
            return mComponents.Find(c => c.Name == name);
        }

        /// <summary>
        /// Gets the value of the property referenced by the PropertyReference object.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public T GetProperty<T>(PropertyReference property)
        {
            string[] pathElements = property.Reference.Split('.');
            int numElements = pathElements.Length;

            // First we have to get the component in which we want to access a property.
            IEntityComponent component = GetComponent(pathElements[0].Substring(1));
            if (component == null)
                return default(T);

            object value = component;
            for (int i = 1; i < numElements; ++i)
            {
                // If the value is null, we cannot access the next element in the path.
                if (value == null)
                    return default(T);

                // Get the property information.
                string propertyName = pathElements[i];
                PropertyInfo info = value.GetType().GetProperty(propertyName);
                
                // If the property wasn't found or cannot be read, we return the default value
                // of the type parameter.
                if (info == null || !info.CanRead)
                    return default(T);

                // Get the value of the property.
                value = info.GetValue(value, null);
            }

            // Determine if we can cast the value to the specified type.
            if (!(value is T))
                return default(T);

            return (T)value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        public string Name
        {
            get { return mName; }
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Calls the OnReset method on all the components of the entity.
        /// </summary>
        public void ResetComponents()
        {
            foreach (IEntityComponent component in mComponents)
            {
                component.OnReset();
            }
        }

        #endregion

        #region Entity Events

        /// <summary>
        /// Event which occurs when a component is added to the entity.
        /// </summary>
        public event ComponentAddedOrRemovedDelegate ComponentAdded;

        /// <summary>
        /// Event which occurs when a component is removed from the entity.
        /// </summary>
        public event ComponentAddedOrRemovedDelegate ComponentRemoved;

        #endregion

        #region IUpdateable members

        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if (value != mEnabled)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public virtual void Update(TimeSpan elapsedTime)
        {
            foreach (IEntityComponent component in mComponents)
            {
                if (component.Enabled)
                {
                    component.Update(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>The update order of the object.</value>
        public int UpdateOrder
        {
            get { return mUpdateOrder; }
            set
            {
                if (value != mUpdateOrder)
                {
                    mUpdateOrder = value;

                    if (UpdateOrderChanged != null)
                        UpdateOrderChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
