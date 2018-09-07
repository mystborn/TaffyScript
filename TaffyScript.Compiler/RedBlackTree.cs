using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class RedBlackTree<TKey, TValue>
    {
        public static RedBlackNode Leaf = new RedBlackNode(default, default);

        public IComparer<TKey> Comparer { get; }
        public RedBlackNode Root { get; private set; } = Leaf;
        public int Count { get; private set; }

        public RedBlackTree()
        {
            Comparer = Comparer<TKey>.Default;
            Leaf.Left = Leaf;
            Leaf.Right = Leaf;
            Leaf.Parent = Leaf;
        }

        public RedBlackTree(IComparer<TKey> comparer)
        {
            Comparer = comparer;
            Leaf.Left = Leaf;
            Leaf.Right = Leaf;
            Leaf.Parent = Leaf;
        }

        public void Insert(TKey key, TValue value)
        {
            Insert(new RedBlackNode(key, value));
            Count++;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder()
        {
            var stack = new Stack<RedBlackNode>();
            var current = Root;
            while(current != Leaf)
            {
                while(current.Left != Leaf)
                {
                    stack.Push(current);
                    current = current.Left;
                }
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                while(current.Right == Leaf && stack.Count > 0)
                {
                    current = stack.Pop();
                    yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                }
                current = current.Right;
            }
        }

        private void RotateLeft(RedBlackNode node)
        {
            var right = node.Right;
            node.Right = right.Left;

            if (right.Left != Leaf)
                right.Left.Parent = node;
            right.Parent = node.Parent;

            if (node.Parent == Leaf)
                Root = right;
            else if (node.Parent.Left == node)
                node.Parent.Left = right;
            else
                node.Parent.Right = right;

            right.Left = node;
            node.Parent = right;
        }

        private void RotateRight(RedBlackNode node)
        {
            var left = node.Left;
            node.Left = left.Right;

            if (left.Right != Leaf)
                left.Right.Parent = node;
            left.Parent = node.Parent;

            if (node.Parent == Leaf)
                Root = left;
            else if (node.Parent.Right == node)
                node.Parent.Right = left;
            else
                node.Parent.Left = left;

            left.Right = node;
            node.Parent = left;
        }

        private void Insert(RedBlackNode node)
        {
            RedBlackNode parent = Leaf, temp = Root;

            while(temp != Leaf)
            {
                parent = temp;

                if (Comparer.Compare(node.Key, temp.Key) < 0)
                    temp = temp.Left;
                else
                    temp = temp.Right;
            }

            node.Parent = parent;

            if (parent == Leaf)
                Root = node;
            else if (Comparer.Compare(node.Key, parent.Key) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            node.Left = Leaf;
            node.Right = Leaf;
            node.Color = RedBlackColor.Red;

            InsertFixup(node);
        }

        private void InsertFixup(RedBlackNode node)
        {
            RedBlackNode uncle = Leaf;

            while(node.Parent.Color == RedBlackColor.Red)
            {
                if(node.Parent == node.Parent.Parent.Left)
                {
                    uncle = node.Parent.Parent.Right;

                    if(uncle.Color == RedBlackColor.Red)
                    {
                        node.Parent.Color = RedBlackColor.Black;
                        uncle.Color = RedBlackColor.Black;
                        node.Parent.Parent.Color = RedBlackColor.Red;
                        node = node.Parent.Parent;
                    }
                    else if(node == node.Parent.Right)
                    {
                        node = node.Parent;
                        RotateLeft(node);
                    }
                    else
                    {
                        node.Parent.Color = RedBlackColor.Black;
                        node.Parent.Parent.Color = RedBlackColor.Red;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else
                {
                    uncle = node.Parent.Parent.Left;

                    if(uncle.Color == RedBlackColor.Red)
                    {
                        node.Parent.Color = RedBlackColor.Black;
                        uncle.Color = RedBlackColor.Black;
                        node.Parent.Parent.Color = RedBlackColor.Red;
                        node = node.Parent.Parent;
                    }
                    else if(node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RotateRight(node);
                    }
                    else
                    {
                        node.Parent.Color = RedBlackColor.Black;
                        node.Parent.Parent.Color = RedBlackColor.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }

            Root.Color = RedBlackColor.Black;
        }

        public class RedBlackNode
        {
            public TKey Key { get; }
            public TValue Value { get; }
            public RedBlackNode Parent { get; internal set; }
            public RedBlackNode Left { get; internal set; }
            public RedBlackNode Right { get; internal set; }
            public RedBlackColor Color { get; internal set; }

            public RedBlackNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public enum RedBlackColor
        {
            Black,
            Red
        }
    }
}
