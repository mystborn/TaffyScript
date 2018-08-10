using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace TaffyScript.Compiler
{
    public class BinaryTree<TKey, TValue>
    {
        public IComparer<TKey> Comparer { get; }
        public BinaryTreeNode Root { get; private set; }

        public BinaryTree()
        {
            Comparer = Comparer<TKey>.Default;
        }

        public BinaryTree(IComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public void Clear()
        {
            Root = null;
        }

        public bool Insert(TKey key, TValue value)
        {
            if(Root is null)
            {
                Root = new BinaryTreeNode(key, value);
                return true;
            }

            var root = Root;
            var result = new BinaryTreeNode(key, value);
            while(true)
            {
                var cmp = Comparer.Compare(key, root.Key);
                if (cmp < 0)
                {
                    if (root.Left is null)
                        root.Left = result;
                    else
                        root = root.Left;
                }
                else if (cmp > 0)
                {
                    if (root.Right is null)
                        root.Right = result;
                    else
                        root = root.Right;
                }
                else
                    return false;
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> PreOrder()
        {
            var stack = new Stack<BinaryTreeNode>();
            stack.Push(Root);

            while(stack.Count > 0)
            {
                var top = stack.Pop();
                yield return new KeyValuePair<TKey, TValue>(top.Key, top.Value);

                if (top.Right != null)
                    stack.Push(top.Right);
                if (top.Left != null)
                    stack.Push(top.Left);
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder()
        {
            Stack<BinaryTreeNode> stack = new Stack<BinaryTreeNode>();
            BinaryTreeNode current = Root;

            while(current != null)
            {
                while(current.Left != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);

                while(current.Right is null && stack.Count > 0)
                {
                    current = stack.Pop();
                    yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                }

                current = current.Right;
            }
        }

        public class BinaryTreeNode
        {
            public TKey Key { get; }
            public TValue Value { get; }
            public BinaryTreeNode Left { get; set; }
            public BinaryTreeNode Right { get; set; }

            public BinaryTreeNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
