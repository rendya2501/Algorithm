using Algorithm.Collections.DoublyLinkedList;

namespace Algorithm.PageReplacement;

/// <summary>
/// 双方向リストだけで実装したLRUキャッシュ
/// </summary>
/// <remarks>
/// ディクショナリとLinkedListを組み合わせて実装しないといけないのは、キーとバリューを紐づけつつ、順番を保持できないから。
/// 自前でLinkedListを実装しているのだから、キーバリューを登録できるようにしたらいけるんじゃない？って思って実装したらいけた。
/// </remarks>
internal class LRUCache_OnlyLinkedList
{
    private readonly int _capacity;
    private readonly DoublyKeyValueLinkedList<int, int> _cacheList;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="capacity"></param>
    public LRUCache_OnlyLinkedList(int capacity)
    {
        _capacity = capacity;
        _cacheList = new DoublyKeyValueLinkedList<int, int>();
    }

    /// <summary>
    /// キーが存在すればその値を返し、そうでなければ-1を返します。
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int Get(int key)
    {
        // キーに対するノードを取得する。
        var node = _cacheList.FindKey(key);
        // キーがなければ -1を返す。
        if (node == null)
        {
            return -1;
        }

        // 双方向リストからそのノードを削除して先頭につけなおす。
        _ = _cacheList.Erase(node);
        _ = _cacheList.InsertFirst(node);
        // キーに対する値を返却する。
        return node.Value;
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
        // ディクショナリーから、そのキーに対応するノードを取得する・
        var node = _cacheList[key];
        // キーがある場合
        // キーに対する情報を更新して、キャッシュの先頭にまわす。
        if (node != null)
        {
            // 双方向リストからそのノードを削除して先頭につけなおす。
            _ = _cacheList.Erase(node);
            _ = _cacheList.InsertFirst(node);
        }
        // キーがない場合
        // 要素を追加するが、キャパシティーを超えるようなら最後の要素を削除する。
        else
        {
            if (_cacheList.Count >= _capacity)
            {
                // ディクショナリーから削除した後、双方向リストの最後の要素を削除する。
                _cacheList.EraseLast();
            }
            // 双方向リストの先頭にキーを登録しつつ、そのキーのノードと値のペアをディクショナリーに登録する。
            _ = _cacheList.InsertFirst(key, value);
        }
    }
}
