using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kga.Algo.Trees.Trie
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a compressed tree of characters.
    /// </summary>
    public class TrieSet : IEnumerable<string>
    {
        private readonly TrieMap<object> _trie;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrieSet"/>.
        /// </summary>
        public TrieSet()
        {
            _trie = new TrieMap<object>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kga.Algo.Trees.Trie.TrieSet" /> with a collection of keys.
        /// </summary>
        /// <param name="keys">A collection of supplied keys.</param>
        /// <exception cref="NullReferenceException">At least one of the supplied keys is null.</exception>
        public TrieSet(IEnumerable<string> keys) : this()
        {
            foreach (var word in keys)
            {
                if (!Contains(word)) Add(word);
            }
        }

        /// <summary>
        /// Gets all entries in the <see cref="TrieSet"/> whose keys have the prefix.
        /// </summary>
        /// <param name="prefix">Prefix to search for in the <see cref="TrieSet"/>.</param>
        /// <returns>A collection of keys in the <see cref="TrieSet"/>.</returns>
        /// <exception cref="NullReferenceException">The supplied prefix is null.</exception> 
        /// <remarks>Complexity: O(|nodes from prefix| + |descendant paths|)</remarks>
        public IEnumerable<string> Search(string prefix)
        {
            return _trie.Search(prefix).Select(r => r.Key);
        }

        /// <summary>
        /// Adds the key to the <see cref="TrieSet"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <exception cref="ArgumentException">The key exists in the <see cref="TrieSet"/>.</exception>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception>
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public void Add(string key) => _trie.Add(key, default(object));

        /// <summary>
        /// Deletes entries matching the key from the <see cref="TrieSet"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="ArgumentException">The key does not exist in the <see cref="TrieSet"/>.</exception>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception>
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public void Remove(string key) => _trie.Remove(key);

        /// <summary>
        /// Determines whether the <see cref="TrieSet"/> contains the supplied key.
        /// </summary>
        /// <param name="key">The key to find in the <see cref="TrieSet"/>.</param>
        /// <exception cref="NullReferenceException">The supplied key is null.</exception>
        /// <remarks>Complexity: O(|characters in key|)</remarks>
        public bool Contains(string key) => _trie.ContainsKey(key);

        /// <summary>
        /// Returns the actual size of the <see cref="TrieSet"/>.
        /// </summary>
        public long Size => _trie.Size;

        /// <summary>
        /// Returns the count of keys represented by the <see cref="TrieSet"/>.
        /// </summary>
        public long KeyCount => _trie.KeyCount;

        /// <summary>
        /// Returns the count of characters represented by the <see cref="TrieSet"/>.
        /// </summary>
        public long HitCount => _trie.HitCount;

        /// <summary>
        /// Removes all keys from the <see cref="TrieSet"/>.
        /// </summary>
        /// <remarks>Complexity: O(1)</remarks>
        public void Clear() => _trie.Clear();

        /// <inheritdoc />
        /// <summary>
        /// Gets all key entries in the <see cref="T:Kga.Algo.Trees.Trie.TrieSet" />.
        /// </summary>
        /// <returns>A collection of keys in the <see cref="T:Kga.Algo.Trees.Trie.TrieSet" />.</returns>
        /// <remarks>Complexity: O(|nodes from root| + |descendant paths|)</remarks>
        public IEnumerator<string> GetEnumerator()
        {
            return Search(string.Empty).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
