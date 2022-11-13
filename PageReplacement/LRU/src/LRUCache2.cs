using System.Collections.Generic;

namespace Algorithm.PageReplacement;

public class LRUCache2
{
    private int _capacity;
    private Dictionary<int, (LinkedListNode<int> node, int value)> _cache;
    private LinkedList<int> _list;

    public LRUCache2(int capacity)
    {
        _capacity = capacity;
        _cache = new Dictionary<int, (LinkedListNode<int> node, int value)>(capacity);
        _list = new LinkedList<int>();
    }

    public int Get(int key)
    {
        if (!_cache.ContainsKey(key))
            return -1;

        var node = _cache[key];
        _list.Remove(node.node);
        _list.AddFirst(node.node);

        return node.value;
    }

    public void Put(int key, int value)
    {
        if (_cache.ContainsKey(key))
        {
            var node = _cache[key];
            _list.Remove(node.node);
            _list.AddFirst(node.node);

            _cache[key] = (node.node, value);
        }
        else
        {
            if (_cache.Count >= _capacity)
            {
                _cache.Remove(_list.Last.Value);
                _list.RemoveLast();
            }

            // add cache
            _cache.Add(key, (_list.AddFirst(key), value));
        }
    }
}
