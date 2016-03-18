using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Scene;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Default implementation of the IProximitySensorComponent interface.
    /// </summary>
    internal class DefaultProximitySensorComponent : EntityComponent, IProximitySensorComponent
    {
        /// <summary>
        /// Utility class which is used to keep an entity and the scene node of its spatial 
        /// component together.
        /// </summary>
        private class EntityEntry
        {
            /// <summary>
            /// Entity of the entry.
            /// </summary>
            public Entity Entity;

            /// <summary>
            /// Scene node of the spatial component of the entity.
            /// </summary>
            public SceneNode SceneNode;
        };

        #region Attributes

        /// <summary>
        /// Entities which are sensed by the component.
        /// </summary>
        private List<EntityEntry> mEntities;

        /// <summary>
        /// Square of the distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        private float mThresholdSq;

        /// <summary>
        /// Distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        private float mThreshold;

        /// <summary>
        /// Scene node of the spatial component of the entity.
        /// </summary>
        private SceneNode mSceneNode;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultProximitySensorComponent(Root root, string name)
            : base(root, name)
        {
            mEntities = new List<EntityEntry>();
            mThreshold = 1.0f;
            mThresholdSq = 1.0f;
        }

        /// <summary>
        /// Detects any hits with this entity.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            foreach (EntityEntry entry in mEntities)
            {
                SceneNode node = entry.SceneNode;

                float SqDist = Vector3.DistanceSquared(node.Position, mSceneNode.Position);
                if (SqDist < this.ThresholdSquared)
                {
                    if (HitDetected != null)
                        HitDetected(entry.Entity);
                }
            }
        }

        /// <summary>
        /// Called when another component is added or removed from the entity. This method should
        /// be used by the component to aquire references to other components in the entity.
        /// </summary>
        public override void OnReset()
        {
            base.OnReset();

            ISpatialComponent spatial = this.Owner.GetComponent<ISpatialComponent>();
            mSceneNode = spatial != null ? spatial.SceneNode : null;
        }

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();

            mSceneNode = null;
            mEntities.Clear();
        }

        /// <summary>
        /// Adds an entity to the list of entities that are to be monitored by the component.
        /// </summary>
        /// <param name="entity">Entity which is to be added.</param>
        public void AddSensedEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("DefaultProximitySensorComponent.AddSensedEntity(): null entity.");
            }

            ISpatialComponent spatial = entity.GetComponent<ISpatialComponent>();
            if (spatial == null || spatial.SceneNode == null)
            {
                string msg = "DefaultProximitySensorComponent.AddSensedEntity(): entity has no ";
                msg += "spatial component.";
                throw new ArgumentException(msg);
            }

            EntityEntry entry = new EntityEntry();
            entry.SceneNode = spatial.SceneNode;
            entry.Entity = entity;

            mEntities.Add(entry);
        }

        /// <summary>
        /// Removes an entity from the list of entities that are sensed by the component.
        /// </summary>
        /// <param name="entity">Entity which is to be removed.</param>
        public void RemoveSensedEntity(Entity entity)
        {
            EntityEntry entry = mEntities.Find(c => c.Entity == entity);
            if (entry == null)
            {
                string msg = "DefaultProximitySensorComponent.AddSensedEntity(): entity not ";
                msg += "found.";
                throw new ArgumentException(msg);
            }

            mEntities.Remove(entry);
        }

        /// <summary>
        /// Removes all the entities from the component.
        /// </summary>
        public void RemoveAllSensedEntities()
        {
            mEntities.Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of entities that are sensed by the component.
        /// </summary>
        public List<Entity> SensedEntities
        {
            get
            {
                List<Entity> result = new List<Entity>(mEntities.Count);
                foreach (EntityEntry entry in mEntities)
                {
                    result.Add(entry.Entity);
                }
                return result;
            }
            set
            {
                mEntities.Clear();
                foreach (Entity entity in value)
                {
                    AddSensedEntity(entity);
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        public float Threshold
        {
            get { return mThreshold; }
            set
            {
                mThreshold = value;
                mThresholdSq = value * value;
            }
        }

        /// <summary>
        /// Gets or sets the square of the distance at which the sensor will trigger the 
        /// HitDetected event.
        /// </summary>
        public float ThresholdSquared
        {
            get { return mThresholdSq; }
            set
            {
                mThreshold = (float)Math.Sqrt(value);
                mThresholdSq = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event which occurs when one of the entities added to the component is at a distance
        /// from the sensor which is smaller than the specified threshold.
        /// </summary>
        public event HitDetectedDelegate HitDetected;

        #endregion
    }
}
