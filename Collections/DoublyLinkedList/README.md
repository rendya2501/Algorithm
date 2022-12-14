# 双方向連結リスト(DoublyLinkedList)

## 概要

>双方向リストとは、基本的なデータ構造の一つである線形リスト（linear linked list）のうち、各要素が自分の「次」と「前」の要素へのリンクを持ち、先頭側から末尾側へも、逆方向にも要素をたどっていくことができるもの。  
[e-Words]  

``` txt
        Node              Node              Node
← Prev Value Next ↔ Prev Value Next ↔ Prev Value Next →
```

---

## 特徴

利点  

- 「片方向連結リスト」と同様に、常に要素数分のメモリだけ確保しておける。  
- あるノードの直後および直前に新しい要素を挿入する場合、一定時間（O(1)）で行える。  
- あるノードの削除を一定時間（O(1)）で行える。  

欠点  

- 「片方向連結リスト」と同様に、リスト中の要素にランダムアクセスできない。  
- 「片方向連結リスト」と比べて、ちょっとだけ余分にメモリが必要。  
- 先頭から順に、あるいは末尾から逆順にしか要素にアクセスできない。  
- 「あるノード前後への挿入・削除が O(1)」といっても、そのノードを探してくる操作自体は O(n)。  

- 要素の検索には時間がかかるものの、 挿入・削除が高速  
  →名簿等、文字通りのリスト管理にはこの双方向連結リストがよく使われる。  
- 単にリストとか連結リストという言葉で双方向連結リストを指す場合もある。  
  - C++ → STL では双方向連結リストが単に list  
  - C# → LinkedList  

---

## ノードの実装

双方向リストを実現するための要素として、ノードを実装する必要がある。  
概要の通り、「前のノード」、「ノードが保持する値」、「次のノード」を表現できるように実装する。  

internal : 同じアセンブリならアクセスできる  
※アセンブリ:exeやdllのこと。同じアセンブリとは同じプロジェクトのこと。  

``` C# : Node
/// <summary>
/// 連結リストのノード
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
```

---

## ダミーノード

- 双方向連結リスト本体の実装では、リストの先頭ノードや末尾ノードを持つ代わりに、ダミー（有効な値を持たない）ノードを持つ実装方法が一般的。  
- リストの先頭および末尾のノードは、それぞれ dummy.Next および dummy.Previous に格納する。  
- 初期状態では、dummy.Next および dummy.Previous には dummy 自身の参照を入れておく。  
- ダミーノードを使えば、 先頭・末尾への要素の挿入・削除を特別扱いする必要がなくなる。  
- ダミーノード自身は、先頭よりも1つ前、末尾よりも1つ後ろに常に位置することになり、リストの終端判定に使う事ができる。  

``` C#
public class LinkedList<T> : IEnumerable<T>
{
    /// <summary>
    /// ダミーノード
    /// </summary>
    private readonly Node dummy;

    /// <summary>
    /// リストの先頭ノード
    /// </summary>
    public Node First => dummy.Next;
    /// <summary>
    /// リストの末尾ノード
    /// </summary>
    public Node Last => dummy.Previous;
    /// <summary>
    /// リストの終端（末尾よりも後ろの番兵に当たるノード）
    /// </summary>
    public Node End => dummy;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LinkedList()
    {
        dummy = new Node(default, null, null);
        dummy.Next = dummy;
        dummy.Previous = dummy;
    }
}
```

---

## ダミーノードの終端判定を利用したアクセス

単純に先頭から値を参照していく  

``` C#
for (Node n = this.First; n != this.End; n = n.Next)
    _ = n.Value;
```

カウントプロパティの実装  
リストに含まれている要素数を求めるのは、 片方向連結リストと同様に、 前から順にノードをたどって数えるしかない。  
※要素数を保持しておく変数を別に用意しておくという手はある  

``` C#
    public int Count
    {
        get
        {
            int i = 0;
            for (Node n = this.First; n != this.End; n = n.Next)
                ++i;
            return i;
        }
    }
```

インデクサーの実装  

``` C#
    public Node this[int index]
    {
        get
        {
            int i = 0;
            for (Node n = this.First; n != this.End; n = n.Next)
            {
                ++i;
                if (i == index)
                    return n;
            }
            return null;
        }
    }
```

---

## リストへの要素の追加・削除

``` C#
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
    }

    /// <summary>
    /// ノード n の自身を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    /// <returns>削除した要素の次のノード</returns>
    public Node Erase(Node n)
    {
        if (n == this.dummy)
        {
            return this.dummy;
        }
        // 現在のノードの前のノード。そのノードの次のノードに自分自身の次のノードを登録する。
        n.Previous.Next = n.Next;
        // 現在のノードの次のノード。そのノードの前のノードに自分自身の前のノードを登録する。
        n.Next.Previous = n.Previous;
        return n.Next;
    }
```

---

## 先頭・末尾への要素の挿入・削除

ダミーノードを使うことによって、 先頭・末尾への要素の挿入・削除は特別扱いする必要がない。  
先頭や末尾にダミーノードを入れるだけ。  

``` C#
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
```

---

## new LinkedList()した時のノードの状態 (初期状態)

``` txt
DummyNode
prev  val  next
Dummy null Dummy
```

---

## InsertAfterの動作

``` C#
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
    }
```

現在のノード状況が以下のような場合にnをダミーノードとし、InsertAfterされた状況を図解する。

``` txt
DummyNode        | Node1
prev  val  next  | prev  val   next
Node1 null Node1 | Dummy Node1 Dummy
```

①`Node m = new Node(elem, n, n.Next);`  
①を実行する事で前のノードがDummy、次のノードがNode1を指し示すノード:Node2を作成する。  

``` txt
Node2
prev  val   next
Dummy Node2 Node1
```

②`n.Next.Previous = m;`  
nはダミーノード。n.NextはNode1を指し示している。  
n.Next.PreviousはNode1のPreviousを参照することを意味し、Node1.PreviousはDummyNodeを参照しているので、ここを今回作成したNode2を参照するようにする。  

``` txt
DummyNode        | Node1
prev  val  next  | prev  val   next
Node1 null Node1 | Node2 Node1 Dummy
                    ↑
```

③`n.Next = m;`  
nはダミーノード。n.NextはNode1を指し示しているので、参照をNode2とする。  

``` txt
DummyNode        | Node1
prev  val  next  | prev  val   next
Node1 null Node2 | Node2 Node1 Dummy
            ↑
```

ここまでを実行した結果、以下のような状態となる。  

``` txt
DummyNode        | Node2             | Node1
prev  val  next  | prev  val   next  | prev  val   next
Node1 null Node2 | Dummy Node2 Node1 | Node2 Node1 Dummy
```

---

## InsertBeforeの動作

``` C#
    /// <summary>
    /// ノード n の前に新しい要素を追加。
    /// </summary>
    /// <param name="n">現在のノード(要素の挿入位置)</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertBefore(Node n, T elem)
    {
        // ①前のポインタが現在のノードの前のノードを指し示し、次のポインタが現在のノードを指し示すノードを作る
        Node m = new Node(elem, n.Previous, n);
        // ②現在のノードの前のノード。そのノードの次のノードに作成したノードを登録する。
        n.Previous.Next = m;
        // ③現在のノードの前のノードに作成したノードを登録する。
        n.Previous = m;
        // 作成したノードを返却する。
        return m;
    }
```

現在のノード状況が以下のような場合にnをダミーノードとし、InsertBeforeされた状況を図解する。

``` txt
Node1             | DummyNode
prev  val   next  | prev  val  next
Dummy Node1 Dummy | Node1 null Node1
```

①`Node m = new Node(elem, n.Previous, n);`  
①を実行する事で前のノードがNode1、次のノードがDummyを指し示すノード:Node2を作成する。  

``` txt
Node2
prev  val   next
Node1 Node2 Dummy
```

②`n.Previous.Next = m;`  
nはダミーノード。n.PreviousはNode1を指し示している。  
n.Previous.NextはNode1のNextを参照することを意味し、Node1.NextはDummyNodeを参照しているので、ここを今回作成したNode2を参照するようにする。  

``` txt
Node1             | DummyNode
prev  val   next  | prev  val  next
Dummy Node1 Node2 | Node1 null Node1
             ↑
```

③`n.Previous = m;`  
nはダミーノード。n.PreviousはNode1を指し示しているので、参照をNode2とする。  

``` txt
Node1             | DummyNode
prev  val   next  | prev  val  next
Dummy Node1 Node2 | Node2 null Node1
                     ↑
```

ここまでを実行した結果、以下のような状態となる。  

``` txt
Node1             | Node2             | DummyNode
prev  val   next  | prev  val   next  | prev  val  next
Dummy Node1 Node2 | Node1 Node2 Dummy | Node2 null Node1
```

---

## Eraseの動作

``` C#
    /// <summary>
    /// ノード n の自身を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    /// <returns>削除した要素の次のノード</returns>
    public Node Erase(Node n)
    {
        if (n == this.dummy)
        {
            return this.dummy;
        }
        // 現在のノードの前のノード。そのノードの次のノードに自分自身の次のノードを登録する。
        n.Previous.Next = n.Next;
        // 現在のノードの次のノード。そのノードの前のノードに自分自身の前のノードを登録する。
        n.Next.Previous = n.Previous;
        return n.Next;
    }
```

現在のノード状況が以下のような場合にNode2をEraseした状況を図解する。

``` txt
Node1             | Node2             | DummyNode
prev  val   next  | prev  val   next  | prev  val  next
Dummy Node1 Node2 | Node1 Node2 Dummy | Node2 null Node1
```

①`n.Previous.Next = n.Next;`  
nはNode2。n.PreviousはNode1を指し示している。  
n.Previous.NextはNode1のNextを参照することを意味し、Node1.NextはNode2を参照しているので、ここをNode2の次であるDymmyを参照するようにする。  

``` txt
Node1             | Node2             | DummyNode
prev  val   next  | prev  val   next  | prev  val  next
Dummy Node1 Dummy | Node1 Node2 Dummy | Node2 null Node1
             ↑
```

②`n.Next.Previous = n.Previous;`  
nはNode2。n.NextはDummyを指し示している。  
n.Next.PreviousはDummyのPreviousを参照することを意味し、Dummy.PreviousはNode2を参照しているので、ここをNode2の前であるNode1を参照するようにする。  

``` txt
Node1             | Node2             | DummyNode
prev  val   next  | prev  val   next  | prev  val  next
Dummy Node1 Dummy | Node1 Node2 Dummy | Node1 null Node1
                                         ↑
```

この時点でNode2への参照は全て失われた。  
結果、Node2を削除したものとみなせる。  

``` txt
Node1              | DummyNode
prev  val   next   | prev  val  next
Dummy Node1 Dummy  | Node1 null Node1
```

---

## EraseFirst・EraseLastの動作

``` C#
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
```

このようなノードの状態において考える。  

``` txt
Node1             | Node2             | DummyNode
prev  val   next  | prev  val   next  | prev  val  next
Dummy Node1 Dummy | Node1 Node2 Dummy | Node2 null Node1
```

`First`は`Node First => dummy.Next;`  
`dummy.Next`は`Node1`となる。  

`Last`は`Node Last => dummy.Previous;`  
`dummy.Previous`は`Node2`となる。  

どちらも実質、Node1,Node2をEraseに渡すのと等しく、それ以降の処理はEraseの動作をトレースすればよい。  
結果的に先頭、末尾の参照を削除することになる。  

---

## 参考

[未確認飛行C](https://ufcpp.net/study/algorithm/col_blist.html)  
