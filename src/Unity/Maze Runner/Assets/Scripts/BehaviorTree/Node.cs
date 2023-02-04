using System.Collections.Generic;

namespace BehaviorTree
{
    public enum Status
    {
        Success,
        Failure,
        Running
    }
    public class Node
    {        
        public Node parent;
        protected List<Node> children = new List<Node>();
        
        private Dictionary<string, object> data = new Dictionary<string, object>();
        
        public Node()
        {
            parent = null;
        }
        
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }
        
        private void Attach(Node child)
        {
            children.Add(child);
            child.parent = this;
        }
        
        public virtual Status Evaluate()
        {
            return Status.Failure;
        }

        protected void SetData(string key, object value, bool local = false)
        {
            if (local || parent == null)
            {
                data[key] = value;
            }
            else
            {
                parent.data[key] = value;
            }
        }

        protected object GetData(string key)
        {
            var node = this;
            while (node != null)
            {
                if (node.data.ContainsKey(key))
                {
                    return node.data[key];
                }
                node = node.parent;
            }
            return null;
        }
        
        protected void ClearData(string key)
        {
            var node = this;
            while (node != null)
            {
                if (node.data.ContainsKey(key))
                {
                    node.data.Remove(key);
                    return;
                }
                node = node.parent;
            }
        }
    }
}