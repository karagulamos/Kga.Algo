# Kga.Algo
This project focuses on heavy duty data structures absent in .NET and are designed to be efficient and usable in production (contributions are welcome).

Some of the data structures included in this project solve a varying degree of problems highlighted below:

## Trie Data Structure

Tries are useful in scenarios where we require fast lookups or retrieval defined by some **prefix**. Also, retrieving keys (and associated values) that match a specified prefix can be done in near linear time.

Tries boast of being space efficient due to their ability to represent individual characters as nodes in a tree-like structure.

Consider the words **home, homely, hangs, hanger, have, haven**. There are 30 characters in total, but a trie only requires 15 characters to represent all of these words. 

Furthermore, tries are much more memory efficient when dealing with data that contain lots of duplicates as they only require few characters to represent them. As a result, key lookups are very fast and can be done in **O(M)**, where M is the length of the searched key.

A practical use of a trie is **auto completion** (e.g. Google Search) due to its ability to efficiently generate all keys defined by a user supplied prefix in time proportional to **O(|N| + |E|)**, where N = # of character nodes in the tree and E = # of edges or paths between nodes.

Another efficient use of a trie is **spell checking**, which is used in most applications to suggest corrections to users.

Below is a description of trie data structures in this project found under the **Kga.Algo.Trees.Trie** namespace.

1. **TrieMap** - A compressed tree of keys and their respective values. As the name suggests, it uses a trie data structure that supports fast key/value lookups and efficient prefix search of all key entries within the collection.

2. **TrieSet** - A compressed tree of keys that uses **TrieMap** under the covers.

## TrieSet Example (Auto Completion)

```csharp
private static void Main()
{
    string[] words = {
        "home", "homely", "hangs", "hanger", "have", "haven"
    };

    var set = new TrieSet(words);

    Console.WriteLine("No. of nodes in tree: " + set.Size);
    Console.WriteLine("No. of words in tree: " + set.KeyCount);
    Console.WriteLine("No. of hits on tree: " + set.HitCount);

    Console.WriteLine();

    foreach (var result in set.Search("ha")) // Or foreach(var result in set) for every key
    {
        Console.WriteLine(result);
    }

    Console.ReadLine();
}
```
## TrieSet Example (Spelling Checker)

```csharp
private static void Main()
{
    string[] words =
    {
        "your", "luck", "start", "starts", "lucky",
        "day", "it", "is", "day", "today", "you", "are", "brown",
        "friend", "frisby"
    };

    var dictionary = new TrieSet(words);

    const string sentence = "This is yor lucky brwon dy frind.";

    Console.WriteLine("Sentence: {0}\n", sentence);

    foreach (var word in sentence.Split(new[] { ' ', '.' }))
    {
        if (string.IsNullOrEmpty(word) || dictionary.Contains(word))
            continue;

        Console.Write("Suggestions for '{0}': ", word);

        foreach (var suggestion in dictionary.Suggest(word))
        {
            Console.Write(suggestion + " ");
        }

        Console.WriteLine();
    }

    Console.ReadLine();
}
```

## TrieMap Example

```csharp
private static void Main()
{
    var map = new TrieMap<string>
    {
        {"book", "Something you read."},
        {"holiday", "Time for relaxation."},
        {"bowl", "Where you store stuff or eat from."}
    };

    var bible = map["book"] + " Or maybe write one.";

    map.Remove("book");

    map.Add("bible", bible);

    //map["love"] = "404 not found."; // ArgumentException

    Console.WriteLine("No. of nodes in tree: " + map.Size);
    Console.WriteLine("No. of words in tree: " + map.KeyCount);
    Console.WriteLine("No. of hits on tree: " + map.HitCount);

    Console.WriteLine();

    foreach (var result in map.Search("b"))
    {
        Console.WriteLine(result.Key + " - " + result.Value);
    }

    Console.ReadLine();
}
```

