using System;
using System.Collections.Generic;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Class which represents a node of a graph. A node can have a parent and 
    /// any number of children. If the node has no parent it is a root node. 
    /// If the node has no children it is a leaf node.
    /// </summary>
    public class Node : Base
    {
        #region Attributes

        /// <summary>
        /// Parent of this node.
        /// </summary>
        private Node mParent;

        /// <summary>
        /// Children of this node.
        /// </summary>
        protected List<Node> mNodes;

        #endregion

        #region Node members

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        protected Node(Root root): base(root)
        {
            mParent = null;
            mNodes = new List<Node>();
        }

        /// <summary>
        /// Attaches a node to this node.
        /// </summary>
        /// <param name="child">Node which is to be attached.</param>
        public virtual void AttachChild(Node child)
        {
            if (child == null)
            {
                throw new Exception("Node.attachChild(): null child.");
            }

            mNodes.Add(child);
            child.Parent = this;
            RootNode.OnGraphChanged(this);
        }

        /// <summary>
        /// Detaches a node from this node.
        /// </summary>
        /// <param name="child">
        /// Child node which is to be detached.
        /// </param>
        public virtual void DettachChild(Node child)
        {
            if (child == null)
            {
                throw new Exception("Node.dettachChild (): no child specified.");
            }
            //Set the node's parent to null and detach the node.
            else
            {
                child.Parent = null;
                if (!mNodes.Remove(child))
                {
                    throw new Exception("Node.dettachChild (): child not found.");
                }

                //Trigger the GraphChanged event because the graph has changed.
                RootNode.OnGraphChanged(this);
            }
        }

        /// <summary>
        /// Detaches all the children of this node.
        /// </summary>
        public virtual void DettachAll()
        {
            foreach (Node node in mNodes)
            {
                node.Parent = null;
            }
            //Remove all the nodes from the child node dictionary.
            mNodes.Clear();
            //Trigger the GraphChanged event because the graph has changed.
            RootNode.OnGraphChanged(this);
        }

        /// <summary>
        /// Returns a list with all the children of this node.
        /// </summary>
        /// <returns>A list with all the children of this node.</returns>
        public List<Node> GetChildren()
        {
            return mNodes;
        }

        /// <summary>
        /// Returns a list with all the descendants of this node (i.e. all the children, 
        /// grandchildren, etc).
        /// </summary>
        /// <returns>A list with all the descendants of this node.</returns>
        public List<Node> GetDescendants()
        {
            //Create a list to store the descendants.
            List<Node> descendants = new List<Node>();
            //Add the descendants to the list.
            GetDescendants(this, descendants);
            //Return the created list.
            return descendants;
        }

        /// <summary>
        /// Fills a list in the second argument with all the descendants of the node in the first 
        /// argument.
        /// </summary>
        /// <param name="node">Node whose descendants are to be retrieved.</param>
        /// <param name="descendants">
        /// List which is to be filled with the descendants of the node in the first argument.
        /// </param>
        protected void GetDescendants(Node node, List<Node> descendants)
        {
            //Return if there are no nodes to add.
            if (mNodes.Count == 0) return;

            foreach (Node descendantNode in node.mNodes)
            {
                descendants.Add(descendantNode);
                GetDescendants(descendantNode, descendants);
            }
        }

        /// <summary>
        /// Returns the root node of the scene graph.
        /// </summary>
        /// <returns>The root node of the scene graph.</returns>
        protected Node GetRootNode()
        {
            //If the node has no parent, it's the root node so we return it.
            if (this.Parent == null) { return this; }
            //If the node is not the root node, we try the node's parent.
            else { return this.Parent.GetRootNode(); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the parent of the node.
        /// </summary>
        public Node Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }

        /// <summary>
        /// Returns the root node of the graph to which this node is attached.
        /// </summary>
        /// <returns>
        /// The root node of the graph to which this node is attached.
        /// </returns>
        public Node RootNode
        {
            get { return GetRootNode(); }
        }

        /// <summary>
        /// Returns a list with all the children of this node.
        /// </summary>
        public List<Node> Children
        {
            get { return mNodes;  }
        }

        /// <summary>
        /// Returns a list with all the descendants of this node (i.e. all the children, 
        /// grandchildren, etc).
        /// </summary>
        public List<Node> Descendants
        {
            get { return GetDescendants(); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Delegate type for the GraphChanged event.
        /// </summary>
        /// <param name="node">Node which whose sub-graph was changed.</param>
        public delegate void GraphChangedHandler(Node node);

        /// <summary>
        /// Event which is triggered when the sub-graph of a node is changed.
        /// </summary>
        public event GraphChangedHandler GraphChanged;

        /// <summary>
        /// Inkokes the GraphChanged event.
        /// </summary>
        /// <param name="node">Node which whose sub-graph was changed.</param>
        public void OnGraphChanged(Node node)
        {
            if (GraphChanged != null)
            {
                GraphChanged(this);
            }
        }

        #endregion
    }
}
