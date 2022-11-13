using System.Collections;
using System.Collections.Generic;

namespace Algorithm.Collections.DoublyLinkedList;

/// <summary>
/// 双方向キーバリュー連結リスト
/// </summary>
/// <remarks>
/// バリューだけじゃなく、キーもいけるでしょと思って作ったらできた
/// </remarks>
public class DoublyKeyValueLinkedList<K, V> : IEnumerable<KeyValuePair<K, V>>
{
    /// <summary>
    /// ダミーノード
    /// </summary>
    private readonly Node dummy;

    #region プロパティ
    /// <summary>
    /// リストの先頭ノード。
    /// </summary>
    public Node First => dummy.Next;
    /// <summary>
    /// リストの末尾ノード。
    /// </summary>
    public Node Last => dummy.Previous;

    /// <summary>
    /// リストの終端（末尾よりも後ろの番兵に当たるノード）。
    /// </summary>
    public Node End => dummy;

    /// <summary>
    /// 要素の個数。
    /// </summary>
    public int Count
    {
        get
        {
            int i = 0;
            for (Node n = First; n != End; n = n.Next)
                ++i;
            return i;
        }
    }

    /// <summary>
    /// インデクサー
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Node this[int index]
    {
        get
        {
            int i = 0;
            for (Node n = First; n != End; n = n.Next)
            {
                ++i;
                if (i == index)
                    return n;
            }
            return null;
        }
    }
    #endregion

    #region コンストラクタ
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DoublyKeyValueLinkedList()
    {
        dummy = new Node(default, default, null, null);
        dummy.Next = dummy;
        dummy.Previous = dummy;
    }
    #endregion

    #region 挿入・削除
    /// <summary>
    /// ノード n の後ろに新しい要素を追加。
    /// </summary>
    /// <param name="n">現在のノード(要素の挿入位置)</param>
    /// <param name="key">新しいキー</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertAfter(Node n, K key, V elem)
    {
        // 前のポインタが現在のノードを指し示し、次のポインタが現在のノードの次のポインタを指し示すノードを作る
        Node m = new Node(key, elem, n, n.Next);
        // 現在のノードの次のノード。そのノードの前のノードに作成したノードを登録する。
        n.Next.Previous = m;
        // 現在のノードの次のノードに作成したノードを登録する。
        n.Next = m;
        // 作成したノードを返却する。
        return m;
    }

    /// <summary>
    /// ノード n の前に新しい要素を追加。
    /// </summary>
    /// <param name="n">現在のノード(要素の挿入位置)</param>
    /// <param name="key">新しいキー</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertBefore(Node n, K key, V elem)
    {
        // 前のポインタが現在のノードの前のノードを指し示し、次のポインタが現在のノードを指し示すノードを作る
        Node m = new Node(key, elem, n.Previous, n);
        // 現在のノードの前のノード。そのノードの次のノードに作成したノードを登録する。
        n.Previous.Next = m;
        // 現在のノードの前のノードに作成したノードを登録する。
        n.Previous = m;
        // 作成したノードを返却する。
        return m;
    }

    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertFirst(K key, V elem)
    {
        // ダミーが常に先端になるので、ダミーの後に要素を追加するという意味になる。
        return InsertAfter(dummy, key, elem);
    }
    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="node">新しいノード</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertFirst(Node node)
    {
        // ダミーが常に先端になるので、ダミーの後に要素を追加するという意味になる。
        return InsertAfter(dummy, node.Key, node.Value);
    }

    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertLast(K key, V elem)
    {
        // ダミーが常に終端になるので、ダミーの前に要素を追加するという意味になる。
        return InsertBefore(dummy, key, elem);
    }
    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="node">新しいノード</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertLast(Node node)
    {
        return InsertBefore(dummy, node.Key, node.Value);
    }

    /// <summary>
    /// ノード n の自身を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    /// <returns>削除した要素の次のノード</returns>
    public Node Erase(Node n)
    {
        if (n == dummy)
        {
            return dummy;
        }
        n.Previous.Next = n.Next;
        n.Next.Previous = n.Previous;
        return n.Next;
    }

    /// <summary>
    /// 先頭の要素を削除。
    /// </summary>
    public void EraseFirst()
    {
        Erase(First);
    }

    /// <summary>
    /// 末尾の要素を削除。
    /// </summary>
    public void EraseLast()
    {
        Erase(Last);
    }

    /// <summary>
    /// 指定した値を含むノード一覧を検索します。
    /// </summary>
    /// <param name="value"></param>
    /// <returns>存在する場合は、指定した値を含む最初の Algorithm.Collections.BidirectionalLinkedList.Node`1。それ以外の場合は null。</returns>
    public IEnumerable<Node> FindValue(V value)
    {
        for (Node n = First; n != End; n = n.Next)
        {
            if (n.Value.Equals(value))
                yield return n;
        }
        yield return null;
    }

    /// <summary>
    /// 指定したキーを含むノードを検索します。
    /// </summary>
    /// <param name="value"></param>
    /// <returns>存在する場合は、指定した値を含む最初の Algorithm.Collections.BidirectionalLinkedList.Node`1。それ以外の場合は null。</returns>
    public Node FindKey(K key)
    {
        for (Node n = First; n != End; n = n.Next)
        {
            if (n.Key.Equals(key))
                return n;
        }
        return null;
    }

    #endregion

    #region IEnumerable<T> メンバ
    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        for (Node n = First; n != End; n = n.Next)
            yield return new KeyValuePair<K, V>(n.Key, n.Value);

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion


    #region Nodeクラス
    /// <summary>
    /// 連結リストのノード。
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 格納しているキー
        /// </summary>
        public K Key { get; set; }

        /// <summary>
        /// 格納している値
        /// </summary>
        public V Value { get; set; }

        /// <summary>
        /// 次のノード
        /// </summary>
        public Node Next { get; internal set; }

        /// <summary>
        /// 前のノード
        /// </summary>
        public Node Previous { get; internal set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        internal Node(K key, V val, Node prev, Node next)
        {
            Key = key;
            Value = val;
            Previous = prev;
            Next = next;
        }
    }
    #endregion
}
