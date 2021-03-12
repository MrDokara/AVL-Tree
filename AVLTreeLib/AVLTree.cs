using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AVLTreeLib
{
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private Node<TKey, TValue> Root { get; set; }

        public void Add(TKey key, TValue value)
        {
            Root = AddNode(Root, new Node<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return ContainsNode(Root, key);
        }

        public void RemoveKey(TKey key)
        {
            Root = RemoveNode(Root, key);
        }

        private Node<TKey, TValue> AddNode(Node<TKey, TValue> parent, Node<TKey, TValue> node)
        {
            if (parent == null) return node;
            if (parent.Key.CompareTo(node.Key) == 0) throw new ArgumentException("Dublicate key");

            if (parent.Key.CompareTo(node.Key) > 0)
                parent.Left = AddNode(parent.Left, node);
            else parent.Right = AddNode(parent.Right, node);
            return Balance(parent);
        }

        private Node<TKey, TValue> RemoveNode(Node<TKey, TValue> node, TKey key)
        {
            if (node == null) return null;
            if (key.CompareTo(node.Key) < 0) node.Left = RemoveNode(node.Left, key);
            else if (key.CompareTo(node.Key) > 0) node.Right = RemoveNode(node.Right, key);
            else
            {
                var left = node.Left;
                var right = node.Right;
                if (left == null) return right;
                var max = MaxNode(left);
                max.Left = RemoveMax(left);
                max.Right = right;
                return Balance(max);
            }

            return Balance(node);
        }

        private bool ContainsNode(Node<TKey, TValue> node, TKey key)
        {
            if (key.CompareTo(node.Key) == 0) return true;
            if (key.CompareTo(node.Key) > 0) return (node.Right != null && ContainsNode(node.Right, key));
            return (node.Left != null && ContainsNode(node.Left, key));
        }

        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            node.HeightUpdate();
            if (node.BFactor == 2)
            {
                if ((node.Right?.BFactor ?? 0) < 0)
                    node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            if (node.BFactor == -2)
            {
                if ((node.Left?.BFactor ?? 0) > 0)
                    node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            return node;
        }

        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
        {
            var leftOld = node.Left;
            node.Left = leftOld.Right;
            leftOld.Right = node;
            node.HeightUpdate();
            leftOld.HeightUpdate();
            return leftOld;
        }

        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            var rightOld = node.Right;
            node.Right = rightOld.Left;
            rightOld.Left = node;
            node.HeightUpdate();
            rightOld.HeightUpdate();
            return rightOld;
        }

        private Node<TKey, TValue> MaxNode(Node<TKey, TValue> node)
        {
            return (node.Right != null ? MaxNode(node.Right) : node);
        }

        private Node<TKey, TValue> RemoveMax(Node<TKey, TValue> node)
        {
            if (node.Right == null) return node.Left;
            node.Right = RemoveMax(node.Right);
            return Balance(node);
        }

        public string Print()
        {
            int length = MaxNode(Root).Key.ToString().Length;

            int maxHeight = Root.Height;
            int maxWidth = (int)Math.Pow(2, maxHeight + 1);

            var tree = new Node<TKey, TValue>[maxHeight][];
            tree[0] = new Node<TKey, TValue>[1];
            tree[0][0] = Root;

            var sb = new StringBuilder();

            int width = (maxWidth / tree[0].Length) / 2;

            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = 0; x < tree[y].Length; x++)
                {
                    for (int i = 0; i < width; i++) for (int j = 0; j < length; j++) sb.Append(" ");

                    if (tree[y][x] == null) for (int j = 0; j < length; j++) sb.Append(" ");
                    else
                    {
                        for (int j = 0; j < (length - tree[y][x].Key.ToString().Length) / 2 + ((length - tree[y][x].Key.ToString().Length) % 2 != 0 ? 1 : 0); j++) sb.Append(" ");
                        sb.Append(tree[y][x].Key);
                        for (int j = 0; j < (length - tree[y][x].Key.ToString().Length) / 2; j++) sb.Append(" ");
                    }
                    
                    for (int i = 0; i < width-1; i++) for (int j = 0; j < length; j++) sb.Append(" ");
                }
                sb.Append("\n");
                if (y < maxHeight - 1)
                {
                    tree[y + 1] = new Node<TKey, TValue>[tree[y].Length * 2];

                    for (int x = 0; x < tree[y].Length; x++)
                    {
                        if (tree[y][x] != null)
                        {
                            if (tree[y][x].Left != null) tree[y + 1][x * 2] = tree[y][x].Left;
                            if (tree[y][x].Right != null) tree[y + 1][x * 2 + 1] = tree[y][x].Right;
                        }
                    }
                }
                width = width / 2;
            }

            return sb.ToString();
        }
    }

    public class Node<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Node<TKey, TValue> Left { get; set; }
        public Node<TKey, TValue> Right { get; set; }

        public int Height { get; set; }

        public int BFactor
        {
            get { return (Right?.Height ?? 0) - (Left?.Height ?? 0); }
        }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
        }

        public void HeightUpdate()
        {
            Height = Math.Max(Left?.Height ?? 0, Right?.Height ?? 0) + 1;
        }
    }
}
