using Algorithm.PageReplacement;


LRUCache lRUCache = new LRUCache(2);
lRUCache.Put(1, 1000); // cache is {1=1}
lRUCache.Put(2, 2000); // cache is {1=1, 2=2}
Console.WriteLine(lRUCache.Get(1));    // return 1
lRUCache.Put(3, 3000); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
Console.WriteLine(lRUCache.Get(2));    // returns -1 (not found)
lRUCache.Put(4, 4000); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
Console.WriteLine(lRUCache.Get(1));    // return -1 (not found)
Console.WriteLine(lRUCache.Get(3));    // return 3
Console.WriteLine(lRUCache.Get(4));    // return 4


LRUCache_OnlyLinkedList cache_OnlyLinkedList = new LRUCache_OnlyLinkedList(2);
cache_OnlyLinkedList.Put(1, 1000); // cache is {1=1000}
cache_OnlyLinkedList.Put(2, 2000); // cache is {2=2000,1=1000}
Console.WriteLine(cache_OnlyLinkedList.Get(1));    // return 1000 cache is {1=1000,2=2000}
cache_OnlyLinkedList.Put(3, 3000); // LRU key was 1, evicts key 1, cache is {3=3000, 1=1000}
Console.WriteLine(cache_OnlyLinkedList.Get(2));    // returns -1 (not found)
cache_OnlyLinkedList.Put(4, 4000); // LRU key was 1, evicts key 1, cache is {4=4000, 3=3000}
Console.WriteLine(cache_OnlyLinkedList.Get(1));    // return -1 (not found)
Console.WriteLine(cache_OnlyLinkedList.Get(3));    // return 3000
Console.WriteLine(cache_OnlyLinkedList.Get(4));    // return 4000


LRUCache_WithoutKey LRUCache_WithoutKey = new LRUCache_WithoutKey(3);
Console.WriteLine(LRUCache_WithoutKey.Reference(0)); // 0
Console.WriteLine(LRUCache_WithoutKey.Reference(1)); // 1,0
Console.WriteLine(LRUCache_WithoutKey.Reference(2)); // 2,1,0
Console.WriteLine(LRUCache_WithoutKey.Reference(3)); // 3,2,1 → out 0
Console.WriteLine(LRUCache_WithoutKey.Reference(0)); // 0,3,2 → out 1
Console.WriteLine(LRUCache_WithoutKey.Reference(3)); // 3,0,2
Console.WriteLine(LRUCache_WithoutKey.Reference(4)); // 4,3,0 → out 2
Console.WriteLine(LRUCache_WithoutKey.Reference(2)); // 2,4,3 → out 0
Console.WriteLine(LRUCache_WithoutKey.Reference(3)); // 3,2,4
Console.WriteLine(LRUCache_WithoutKey.Reference(2)); // 2,3,4
Console.WriteLine(LRUCache_WithoutKey.Reference(1)); // 1,2,3 → out 4
Console.WriteLine(LRUCache_WithoutKey.Reference(3)); // 3,1,2
