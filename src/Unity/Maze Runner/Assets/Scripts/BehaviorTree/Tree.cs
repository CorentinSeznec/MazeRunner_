using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;
        
        protected void Start()
        {
            root = CreateTree();
        }
        
        protected void Update()
        {
            root.Evaluate();
        }
        
        protected abstract Node CreateTree();
    }
}