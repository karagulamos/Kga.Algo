using Kga.Algo.Trees.Trie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kga.Algo.UnitTests
{
    [TestClass]
    public class TrieMapShould
    {
        private TrieMap<string> _trie;
        private string[] _items;

        [TestInitialize]
        public void Setup()
        {
            _trie = new TrieMap<string>();

            _items = new[]
            {
                "flow", "follow", "fold", "flower", "flows",
                "hold", "have", "holiday"
            };

            foreach (var item in _items)
                _trie.Add(item, item);
        }

        [TestMethod]
        public void Return_Initial_Count()
        {
            Assert.AreEqual(_items.Length, _trie.KeyCount);
            Assert.AreNotEqual(0, _trie.Size);
            Assert.AreNotEqual(0, _trie.HitCount);
        }

        [TestMethod]
        public void Return_Valid_KeyCount_When_KeysAreRemoved()
        {
            var keys = _items.Take(2).ToList();

            keys.ForEach(k => _trie.Remove(k));

            var keyCountBeforeEmptyingTrie = _trie.KeyCount;

            _trie.Clear();

            Assert.AreEqual(_items.Length - keys.Count, keyCountBeforeEmptyingTrie);

            Assert.AreEqual(0, _trie.KeyCount);
            Assert.AreEqual(0, _trie.Size);
            Assert.AreEqual(0, _trie.HitCount);
        }

        [TestMethod]
        public void Return_Correct_KeyCount_When_KeysAreAdded()
        {
            var keys = _items.Take(2).ToArray();

            foreach (var key in keys)
                _trie.Add(key + "-a", key);

            Assert.AreEqual(_items.Length + keys.Length, _trie.KeyCount);
        }

        [TestMethod]
        public void Throw_Exception_When_Adding_ExistingKey()
        {
            Assert.ThrowsException<ArgumentException>(() => _trie.Add(_items[0], _items[0]));
        }

        [TestMethod]
        public void Throw_Exception_When_Indexing_Or_Removing_UnknownKey()
        {
            Assert.ThrowsException<ArgumentException>(() => _trie.Remove("unknown1"));
            Assert.ThrowsException<ArgumentException>(() => _trie["unknown2"] = "");
            Assert.ThrowsException<ArgumentException>(() => _ = _trie["unknown3"]);

        }

        [TestMethod]
        public void Return_ValidKeys_For_PrefixSearch()
        {
            const string prefix = "flow";

            var originalKeys = _items.Where(s => s.StartsWith(prefix)).ToArray();

            var allKeys = _trie.Search(string.Empty).Select(s => s.Key).ToArray();
            var matchedKeys = _trie.Search(prefix).Select(s => s.Key).ToArray();
            var noMatch = _trie.Search("no match").Select(s => s.Key).ToArray();

            Assert.IsTrue(_items.All(k => allKeys.Contains(k)));
            Assert.AreEqual(_items.Length, allKeys.Count());

            Assert.IsTrue(originalKeys.All(k => matchedKeys.Contains(k)));
            Assert.AreEqual(originalKeys.Length, matchedKeys.Count());

            Assert.AreEqual(noMatch.Length, 0);
        }

        [TestMethod]
        public void Check_Trie_ContainsKey()
        {
            const string key = "flow";

            Assert.IsTrue(_trie.ContainsKey(key));
            Assert.IsTrue(!_trie.ContainsKey("unknown"));
        }

        [TestMethod]
        public void Check_Trie_ContainsValue()
        {
            const string key = "hold";

            Assert.IsTrue(_trie.ContainsValue(key));
            Assert.IsTrue(!_trie.ContainsKey("unknown"));
            Assert.AreEqual(_trie[key], key);
        }
    }
}
