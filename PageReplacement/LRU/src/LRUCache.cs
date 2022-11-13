using Algorithm.Collections.DoublyLinkedList;
using System.Collections.Generic;

namespace Algorithm.PageReplacement;

/// <summary>
/// LRUキャッシュ
/// </summary>
/// <remarks>
/// https://leetcode.com/problems/lru-cache/
/// https://leetcode.com/problems/lru-cache/discuss/930202/c-solution-lru-cache
/// https://www.c-sharpcorner.com/article/fast-and-clean-o1-lru-cache-implementation/
/// ディクショナリと双方向リストを組み合わせて実装しないといけないのは、キーとバリューを保持しつつ、順番も保持しないといけないから。
/// キーバリューの保持はディクショナリで、順番の保持は双方向リストがそれぞれ行っている。
/// </remarks>
internal class LRUCache
{
    private readonly int _capacity;
    private readonly Dictionary<int, (DoublyLinkedList<int>.Node node, int value)> _dic;
    private readonly DoublyLinkedList<int> _cacheList;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="capacity"></param>
    public LRUCache(int capacity)
    {
        _capacity = capacity;
        _dic = new Dictionary<int, (DoublyLinkedList<int>.Node node, int value)>(capacity);
        _cacheList = new DoublyLinkedList<int>();
    }

    /// <summary>
    /// キーが存在すればその値を返し、そうでなければ-1を返します。
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int Get(int key)
    {
        // キーがなければ -1を返す。
        if (!_dic.ContainsKey(key))
        {
            return -1;
        }
        // キーに対するノードを取得する。
        var node = _dic[key];
        // 双方向リストからそのノードを削除して先頭につけなおす。
        _cacheList.Erase(node.node);
        _cacheList.InsertFirst(node.node.Value);
        // キーに対する値を返却する。
        return node.value;
    }

    /// <summary>
    /// キーが存在する場合はキーの値を更新する。
    /// そうでない場合は、キーと値のペアをキャッシュに追加する。  
    /// キーの数がこの操作による容量を超える場合、最も最近使用されたキーを退避させる。
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Put(int key, int value)
    {
        // キーがある場合
        // キーに対する情報を更新して、キャッシュの先頭にまわす。
        if (_dic.ContainsKey(key))
        {
            // ディクショナリーから、そのキーに対応するノードを取得する・
            var node = _dic[key];
            // 双方向リストからそのノードを削除して先頭につけなおす。
            _cacheList.Erase(node.node);
            _cacheList.InsertFirst(node.node.Value);
            // ディクショナリーの情報も更新する。
            _dic[key] = (node.node, value);
        }
        // キーがない場合
        // 要素を追加するが、キャパシティーを超えるようなら最後の要素を削除する。
        else
        {
            if (_cacheList.Count >= _capacity)
            {
                // 双方向リストのキーを取得して、ディクショナリーからそのキーを削除する。
                _dic.Remove(_cacheList.Last.Value);
                // ディクショナリーから削除した後、双方向リストの最後の要素を削除する。
                _cacheList.EraseLast();
            }
            // 双方向リストの先頭にキーを登録しつつ、そのキーのノードと値のペアをディクショナリーに登録する。
            _dic.Add(key, (_cacheList.InsertFirst(key), value));
        }
    }
}
