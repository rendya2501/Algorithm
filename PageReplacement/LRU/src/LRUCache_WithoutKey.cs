using Algorithm.Collections.DoublyLinkedList;

namespace Algorithm.PageReplacement;

/// <summary>
/// LRUキャッシュ(キーなし)
/// </summary>
/// <remarks>
/// 基本情報や応用情報で出題されるタイプを実現したモノ。
/// キーで検索ではなく、値だけを参照し、あればそれを先頭につけなおし、なければページフォールトを発生させる。
/// </remarks>
internal class LRUCache_WithoutKey
{
    /// <summary>
    /// キャパシティー
    /// </summary>
    private readonly int _capacity;
    /// <summary>
    /// 自作の双方向リスト
    /// </summary>
    private readonly DoublyLinkedList<int> _cacheList;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="capacity"></param>
    public LRUCache_WithoutKey(int capacity)
    {
        this._capacity = capacity;
        _cacheList = new DoublyLinkedList<int>();
    }

    /// <summary>
    /// 値を参照します。
    /// 値があれば、それを先頭に更新します。
    /// 値がなければ、それを先頭に入れますが、キャパシティーを超える場合は最後の要素をページアウトします。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int Reference(int value)
    {
        var node = _cacheList.Find(value);
        // キーがあればそれを更新して、キャッシュの先頭にまわす。
        // →そのノードを削除して、新しいノードを先頭に入れなおす
        if (node != null)
        {
            _cacheList.Erase(node);
        }
        // キーがなければ追加するが、キャパシティーを超えるようなら最後の要素を削除する。
        else
        {
            if (_cacheList.Count >= _capacity)
            {
                _cacheList.EraseLast();
            }
        }
        _cacheList.InsertFirst(value);
        return value;
    }
}
