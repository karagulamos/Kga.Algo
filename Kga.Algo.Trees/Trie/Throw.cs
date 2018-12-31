using System;

namespace Kga.Algo.Trees.Trie
{
    internal static class Throw
    {
        public static void KeyNotExist(string key) => throw new ArgumentException(
            $"The key '{key}' does not exist. Consider adding it first."
        );

        public static void KeyAlreadyAdded(string key) => throw new ArgumentException(
            $"The key '{key}' has already been added."
        );
    }
}
