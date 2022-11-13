using System.Collections;
using System.Collections.Generic;

namespace Algorithm.Collections.DoublyLinkedList;

/// <summary>
/// 双方向連結リスト
/// </summary>
/// <remarks>
/// https://ufcpp.net/study/algorithm/col_blist.html
/// </remarks>
public class DoublyLinkedList<T> : IEnumerable<T>
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
    public DoublyLinkedList()
    {
        dummy = new Node(default, null, null);
        dummy.Next = dummy;
        dummy.Previous = dummy;
    }
    #endregion

    #region 挿入・削除
    /// <summary>
    /// ノード n の後ろに新しい要素を追加。
    /// </summary>
    /// <param name="n">現在のノード(要素の挿入位置)</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertAfter(Node n, T elem)
    {
        // 前のポインタが現在のノードを指し示し、次のポインタが現在のノードの次のポインタを指し示すノードを作る
        Node m = new Node(elem, n, n.Next);
        // 現在のノードの次のノード。そのノードの前のノードに作成したノードを登録する。
        n.Next.Previous = m;
        // 現在のノードの次のノードに作成したノードを登録する。
        n.Next = m;
        // 作成したノードを返却する。
        return m;

        // 現在のノード状況が以下のような場合にnをダミーノードとし、InsertAfterされた状況を図解する。
        // DummyNode        | Node1
        // prev  val  next  | prev  val   next
        // Node1 null Node1 | Dummy Node1 Dummy
        //
        // ①Node m = new Node(elem, n, n.Next);
        // ①を実行する事で前のノードがDummy、次のノードがNode1を指し示すノード:Node2を作る。
        // Node2
        // prev  val   next
        // Dummy Node2 Node1
        // 
        // ②n.Next.Previous = m;
        // nはダミーノード。n.NextはNode1を指し示している。
        // n.Next.PreviousはNode1のPreviousを参照することを意味し、Node1.PreviousはDummyNodeを参照しているので、ここを今回作成したNode2を参照するようにする。
        // DummyNode        | Node1
        // prev  val  next  | prev  val   next
        // Node1 null Node1 | Node2 Node1 Dummy
        //                     ↑
        // ③n.Next = m;
        // nはダミーノード。n.NextはNode1を指し示しているので、参照をNode2とする。  
        // DummyNode        | Node1
        // prev  val  next  | prev  val   next
        // Node1 null Node2 | Node2 Node1 Dummy
        //             ↑
        //
        // ここまでを実行した結果、以下のような状態となる。
        // DummyNode        | Node2             | Node1
        // prev  val  next  | prev  val   next  | prev  val   next
        // Node1 null Node2 | Dummy Node2 Node1 | Node2 Node1 Dummy
    }

    /// <summary>
    /// ノード n の前に新しい要素を追加。
    /// </summary>
    /// <param name="n">現在のノード(要素の挿入位置)</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertBefore(Node n, T elem)
    {
        // 前のポインタが現在のノードの前のノードを指し示し、次のポインタが現在のノードを指し示すノードを作る
        Node m = new Node(elem, n.Previous, n);
        // 現在のノードの前のノード。そのノードの次のノードに作成したノードを登録する。
        n.Previous.Next = m;
        // 現在のノードの前のノードに作成したノードを登録する。
        n.Previous = m;
        // 作成したノードを返却する。
        return m;

        // 現在のノード状況が以下のような場合にnをダミーノードとし、InsertBeforeされた状況を図解する。
        // Node1             | DummyNode
        // prev  val   next  | prev  val  next
        // Dummy Node1 Dummy | Node1 null Node1
        //
        // ①Node m = new Node(elem, n.Previous, n);
        // ①を実行する事で前のノードがNode1、次のノードがDummyを指し示すノード:Node2を作る。
        // Node2
        // prev  val   next
        // Node1 Node2 Dummy
        //
        // ②n.Previous.Next = m;
        // nはダミーノード。n.PreviousはNode1を指し示している。
        // n.Previous.NextはNode1のNextを参照することを意味し、Node1.NextはDummyNodeを参照しているので、ここを今回作成したNode2を参照するようにする。
        // Node1             | DummyNode
        // prev  val   next  | prev  val  next
        // Dummy Node1 Node2 | Node1 null Node1
        //              ↑
        //
        // ③n.Previous = m;
        // nはダミーノード。n.PreviousはNode1を指し示しているので、参照をNode2とする。  
        // Node1             | DummyNode
        // prev  val   next  | prev  val  next
        // Dummy Node1 Node2 | Node2 null Node1
        //                      ↑
        //
        // ここまでを実行した結果、以下のような状態となる。
        // Node1             | Node2             | DummyNode
        // prev  val   next  | prev  val   next  | prev  val  next
        // Dummy Node1 Node2 | Node1 Node2 Dummy | Node2 null Node1
    }

    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertFirst(T elem)
    {
        // ダミーが常に先端になるので、ダミーの後に要素を追加するという意味になる。
        return InsertAfter(dummy, elem);
    }

    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertLast(T elem)
    {
        // ダミーが常に終端になるので、ダミーの前に要素を追加するという意味になる。
        return InsertBefore(dummy, elem);
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
        // 現在のノードの前のノード。そのノードの次のノードに自分自身の次のノードを登録する。
        n.Previous.Next = n.Next;
        // 現在のノードの次のノード。そのノードの前のノードに自分自身の前のノードを登録する。
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
    /// 指定した値を含む最初のノードを検索します。
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    /// 存在する場合は、指定した値を含む最初の Algorithm.Collections.BidirectionalLinkedList.Node`1。それ以外の場合は null。
    /// </returns>
    public Node Find(T value)
    {
        for (Node n = First; n != End; n = n.Next)
        {
            if (n.Value.Equals(value))
            {
                return n;
            }
        }
        return null;
    }
    #endregion

    #region IEnumerable<T> メンバ
    public IEnumerator<T> GetEnumerator()
    {
        for (Node n = First; n != End; n = n.Next)
            yield return n.Value;
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
        /// 格納している値
        /// </summary>
        public T Value { get; set; }

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
        /// <param name="val"></param>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        internal Node(T val, Node prev, Node next)
        {
            Value = val;
            Previous = prev;
            Next = next;
        }
    }
    #endregion
}
