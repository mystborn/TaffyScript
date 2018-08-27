using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class RedBlackTree<TKey, TValue>
    {
        public IComparer<TKey> Comparer { get; }
        public RedBlackNode Root { get; private set; }

        public RedBlackTree()
        {
            Comparer = Comparer<TKey>.Default;
        }

        public RedBlackTree(IComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public void Add(TKey key, TValue value)
        {
            if (Root is null)
            {
                Root = new RedBlackNode(key, value);
                return;
            }

            Root = Insert(Root, new RedBlackNode(key, value));
        }

        private RedBlackNode Insert(RedBlackNode root, RedBlackNode n)
        {
            InsertRecurse(root, n);

            InsertRepair(n);

            root = n;
            while (root.Parent != null)
                root = root.Parent;
            return root;
        }

        private void InsertRecurse(RedBlackNode root, RedBlackNode n)
        {
            if(root != null && Comparer.Compare(n.Key, root.Key) < 0)
            {
                if (root.Left != null)
                {
                    InsertRecurse(root.Left, n);
                    return;
                }
                else
                    root.Left = n;
            }
            else if(root != null)
            {
                if (root.Right != null)
                {
                    InsertRecurse(root.Right, n);
                    return;
                }
                else
                    root.Right = n;
            }

            n.Parent = root;
            n.Left = null;
            n.Right = null;
            n.Red = true;
        }

        private void InsertRepair(RedBlackNode n)
        {
            if (n.Parent == null)
                InsertCase1(n);
            else if (!n.Parent.Red)
                InsertCase2(n);
            else if (n.Uncle.Red)
                InsertCase3(n);
            else
                InsertCase4(n);
                
        }

        private void InsertCase1(RedBlackNode node)
        {
            if (node.Parent is null)
                node.Red = false;
        }

        private void InsertCase2(RedBlackNode node)
        {
            return;
        }

        private void InsertCase3(RedBlackNode node)
        {
            var grandparent = node.Parent.Parent;
            grandparent.Left.Red = false;
            grandparent.Right.Red = false;
            grandparent.Red = true;
            InsertRepair(grandparent);
        }

        private void InsertCase4(RedBlackNode node)
        {
            var parent = node.Parent;
            var grandparent = parent.Parent;
            if(node == grandparent.Left.Right)
            {
                RotateLeft(parent);
                node = node.Left;
            } else if(node == grandparent.Right.Left)
            {
                RotateRight(parent);
                node = node.Right;
            }

            InsertCase4Step2(node);
        }

        private void InsertCase4Step2(RedBlackNode node)
        {
            var parent = node.Parent;
            var grandparent = parent.Parent;

            if (node == parent.Left)
                RotateRight(grandparent);
            else
                RotateLeft(grandparent);

            parent.Red = false;
            grandparent.Red = true;
        }

        private void RotateLeft(RedBlackNode node)
        {
            var right = node.Right;
            var parent = node.Parent;

            node.Right = right.Left;
            right.Left = node;
            node.Parent = node;

            if (node.Right != null)
                node.Right.Parent = node;

            if(parent != null)
            {
                if (node == parent.Left)
                    parent.Left = right;
                else if (node == parent.Right)
                    parent.Right = right;
                right.Parent = parent;
            }
        }

        private void RotateRight(RedBlackNode node)
        {
            var left = node.Left;
            var parent = node.Parent;
            node.Left = left.Right;
            left.Right = node;
            node.Parent = left;

            if (node.Left != null)
                node.Left.Parent = node;

            if(parent != null)
            {
                if (node == parent.Left)
                    parent.Left = left;
                else if (node == parent.Right)
                    parent.Right = left;
                left.Parent = parent;
            }
        }

        public class RedBlackNode
        {
            public TKey Key { get; }
            public TValue Value { get; }
            public RedBlackNode Parent { get; internal set; }
            public RedBlackNode Left { get; internal set; }
            public RedBlackNode Right { get; internal set; }
            public RedBlackNode Uncle
            {
                get
                {
                    var grandparent = Parent?.Parent;
                    if (grandparent is null)
                        return null;
                    if (grandparent.Left == Parent)
                        return grandparent.Right;
                    else
                        return grandparent.Left;
                }
            }
            public bool Red { get; internal set; } = true;

            public RedBlackNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
