using System.Text;

namespace Kga.Algo.Trees.Trie
{
    public class TrieResult<TValue>
    {
        public TrieResult(StringBuilder key, TValue value)
        {
            _builder = key;
            Value = value;
        }

        public string Key => _builder.ToString();
        public TValue Value { get; }

        private readonly StringBuilder _builder;
    }
}