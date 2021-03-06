﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kga.Algo.Trees.Trie
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a compressed tree of keys and their respective values.
    /// </summary>
    /// <typeparam name="TValue">The type of values in the <see cref="T:Kga.Algo.Trees.Trie.TrieMap`1" /></typeparam>
    public class TrieMap<TValue> : IEnumerable<TrieResult<TValue>>
    {
        private readonly Node _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        public TrieMap()
        {
            _root = new Node();
        }

        /// <summary>
        /// Gets entries in the <see cref="TrieMap{TValue}"/> that match the first valid sub prefix.
        /// </summary>
        /// <param name="pattern">Pattern to search for in the <see cref="TrieMap{TValue}"/>.</param>
        /// <returns>A collection of <see cref="TrieResult{TValue}"/>.</returns>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception> 
        /// <remarks>Complexity: O(|nodes from prefix| + |descendant paths|)</remarks>
        public IEnumerable<TrieResult<TValue>> Suggest(string pattern)
        {
            var currentNode = _root;
            var prefix = new StringBuilder();

            foreach (var c in pattern)
            {
                if (!currentNode.HasChild(c)) break;

                prefix.Append(c);
                currentNode = currentNode.GetChild(c);
            }

            if (currentNode == _root)
                return Enumerable.Empty<TrieResult<TValue>>();

            return Search(currentNode, prefix);
        }

        /// <summary>
        /// Gets all entries in the <see cref="TrieMap{TValue}"/> whose keys have the supplied prefix.
        /// </summary>
        /// <param name="prefix">Prefix to search for in the <see cref="TrieMap{TValue}"/>.</param>
        /// <returns>A collection of <see cref="TrieResult{TValue}"/>.</returns>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception> 
        /// <remarks>Complexity: O(|nodes from prefix| + |descendant paths|)</remarks>
        public IEnumerable<TrieResult<TValue>> Search(string prefix)
        {
            var theNode = FindTerminalNodeFor(prefix);
            return Search(theNode, new StringBuilder(prefix));
        }

        private static IEnumerable<TrieResult<TValue>> Search(Node node, StringBuilder prefix)
        {
            if (node.IsWordEnd)
                yield return new TrieResult<TValue>(prefix, node.Value);

            foreach (var pair in node.Children)
            {
                var nextNode = pair.Value;

                foreach (var result in Search(nextNode, prefix.Append(pair.Key)))
                    yield return result;

                prefix.Length--;
            }
        }

        /// <summary>
        /// Adds the supplied key and value to the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="ArgumentException">The key exists in the <see cref="TrieMap{TValue}"/>.</exception>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception> 
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public void Add(string key, TValue value)
        {
            if (!AddInternal(key, value))
                Throw.KeyAlreadyAdded(key);
        }

        internal bool AddInternal(string key, TValue value)
        {
            var prevSize = Size;
            var currentNode = _root;

            foreach (var c in key.ToLower())
            {
                if (!currentNode.HasChild(c))
                {
                    currentNode.SetChild(c);
                    prevSize++;
                }

                currentNode = currentNode.GetChild(c);
            }

            if (currentNode.IsWordEnd) return false;

            KeyCount++;
            HitCount += key.Length;
            Size = prevSize;

            currentNode.SetValue(value);
            currentNode.EndWord();

            return true;
        }

        /// <summary>
        /// Deletes entries matching the key from the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="ArgumentException">The key does not exist in the <see cref="TrieMap{TValue}"/>.</exception>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception> 
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public void Remove(string key) => Remove(key, _root);

        private void Remove(string word, Node parent)
        {
            var states = new Stack<NodeState>(word.Length);

            foreach (var key in word)
            {
                if (!parent.HasChild(key)) Throw.KeyNotExist(word);

                states.Push(new NodeState(parent, key));

                parent = parent.GetChild(key);
            }

            if (!parent.IsWordEnd) Throw.KeyNotExist(word);

            parent.EndWord(false);

            do
            {
                var state = states.Pop();

                parent = state.Parent;

                var childNode = parent.GetChild(state.ChildKey);

                if (childNode.HasChildren()) break;

                parent.RemoveChild(state.ChildKey);
                Size--;
            }
            while (states.Count > 0 && !parent.IsWordEnd);

            KeyCount--;
            HitCount -= word.Length;
        }

        /// <summary>
        /// Gets or updates the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or update.</param>
        /// <exception cref="ArgumentException">The key does not exist in the <see cref="TrieMap{TValue}"/>.</exception>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception>
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public TValue this[string key]
        {
            get => FindValidNode(key).Value;
            set => FindValidNode(key).SetValue(value);
        }

        /// <summary>
        /// Determines whether the <see cref="TrieMap{TValue}"/> contains the supplied key.
        /// </summary>
        /// <param name="key">The key to find in the <see cref="TrieMap{TValue}"/>.</param>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception> 
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public bool ContainsKey(string key)
        {
            var foundNode = FindTerminalNodeFor(key);
            return foundNode.IsWordEnd;
        }

        /// <summary>
        /// Determines whether the <see cref="TrieMap{TValue}"/> contains the supplied value.
        /// </summary>
        /// <param name="value">The value to find in the <see cref="TrieMap{TValue}"/>.</param>
        /// <remarks>Complexity: O(|nodes| + |descendant paths|)</remarks>
        public bool ContainsValue(TValue value) => ContainsValue(_root, value);

        private static bool ContainsValue(Node root, TValue value)
        {
            var queue = new Queue<Node>(new[] { root });

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (currentNode.IsWordEnd && currentNode.Value.Equals(value))
                    return true;

                foreach (var nextNode in currentNode.Children.Values)
                    queue.Enqueue(nextNode);
            }

            return false;
        }

        /// <summary>
        /// Returns the actual size of the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        public long Size { get; protected set; }

        /// <summary>
        /// Returns the count of keys represented by the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        public long KeyCount { get; protected set; }

        /// <summary>
        /// Returns the count of characters represented by the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        public long HitCount { get; protected set; }

        /// <summary>
        /// Removes all keys from the <see cref="TrieMap{TValue}"/>.
        /// </summary>
        /// <remarks>Complexity: O(1)</remarks>
        public void Clear()
        {
            _root.Children.Clear();
            KeyCount = HitCount = Size = 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets all <see cref="TrieResult{TValue}"/> entries in the <see cref="TrieMap{TValue}" />.
        /// </summary>
        /// <returns>A collection of <see cref="TrieResult{TValue}"/> in the <see cref="TrieMap{TValue}" />.</returns>
        /// <remarks>Complexity: O(|nodes from root| + |descendant paths|)</remarks>
        public IEnumerator<TrieResult<TValue>> GetEnumerator()
        {
            return Search(string.Empty).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region Helpers

        private Node FindTerminalNodeFor(string prefix)
        {
            var currentNode = _root;

            foreach (var c in prefix)
            {
                if (!currentNode.HasChild(c))
                    return new Node();

                currentNode = currentNode.GetChild(c);
            }

            return currentNode;
        }

        private Node FindValidNode(string word)
        {
            var theNode = FindTerminalNodeFor(word);

            if (!theNode.IsWordEnd) Throw.KeyNotExist(word);

            return theNode;
        }

        internal class NodeState
        {
            public NodeState(Node parentNode, char childKey)
            {
                Parent = parentNode;
                ChildKey = childKey;
            }

            public Node Parent { get; }
            public char ChildKey { get; }
        }

        internal class Node
        {
            public Node()
            {
                Children = new Dictionary<char, Node>(new CharComparer());
                IsWordEnd = false;
            }

            public Dictionary<char, Node> Children { get; }

            public TValue Value { get; private set; }
            public void SetValue(TValue value) => Value = value;

            public bool IsWordEnd { get; private set; }
            public void EndWord(bool flag = true) => IsWordEnd = flag;

            public bool HasChild(char key) => Children.ContainsKey(key);
            public void SetChild(char key) => Children[key] = new Node();
            public Node GetChild(char key) => Children[key];
            public bool HasChildren() => Children.Any();
            public void RemoveChild(char key) => Children.Remove(key);

            private struct CharComparer : IEqualityComparer<char>
            {
                public bool Equals(char c1, char c2) =>
                char.ToLowerInvariant(c1) == char.ToLowerInvariant(c2);

                public int GetHashCode(char c1) =>
                char.ToLowerInvariant(c1).GetHashCode();
            }
        }

        #endregion
    }
}