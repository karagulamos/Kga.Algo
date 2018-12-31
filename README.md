# Kga.Algo
This project focuses on heavy duty data structures absent in .NET, and have been designed to be efficient and production ready. Contributions are welcome.

Some of the data structures included in this project solve a varying degree of problems as highlighted below:

## Trie Data Structure

Tries are useful in scenarios where we require fast lookups and/or retrieval defined by some **prefix**. Also, retrieving these keys (and/or associated values where required) can be done efficiently in linear time of the nodes and edges in the trie.

Tries boast of being space efficient due to their ability to represent individual characters as nodes in a tree-like structure.

Consider the words **home, homly, hang, hanger**. There are 26 characters in total, but a trie only requires 12 character nodes to represent all of these words. 

Furthermore, tries are much more memory efficient with data containing lots of duplicates as it needs only a uses few characters to represent them. As a result, lookups are very fast and can be done in O(N).

A practical use of a trie is **autocompletion** search due to its ability to generate all keys in the tree in time proportional O(|nodes| + |edges).

Below is a description of Trie data structures in this project found under the **Kga.Algo.Trees.Trie** namespace.

1. **TrieMap** - A compressed tree of keys and their respective values. As the name suggests, it uses a Trie data structure that supports fast key/value lookups and efficient prefix search of all key entries within the collection.

2. **TrieSet** - A compressed tree of keys and uses **TrieMap** under the covers.

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

## TrieSet Example

```csharp
private static void Main()
{
    string[] words = {
        "hello","help","hell","hill","holla",
        "hitler","bit","hate","heat","cold",
        "cope","hole","haven", "hater", "bat",
        "holder", "folded", "fail", "hail", "bella"
    };

    var set = new TrieSet(words);    
    
    const string tiger = "tiger";

    if(!set.Contains(tiger)) // Throws ArgumentException without this
        set.Add(tiger);
        
    set.Remove("hail");

    Console.WriteLine("No. of nodes in tree: " + set.Size);
    Console.WriteLine("No. of words in tree: " + set.KeyCount);
    Console.WriteLine("No. of hits on tree: " + set.HitCount);

    Console.WriteLine();

    foreach (var result in set) // Or foreach(var result in set.Search(string.Empty))
    {
        Console.WriteLine(result);
    }

    Console.ReadLine();
}
```
