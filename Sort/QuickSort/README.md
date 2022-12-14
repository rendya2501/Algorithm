# クイックソート

## 概要

---

## 特徴

最悪計算量 : $O(n^2)$  
平均計算量 : $O(n log n)$  

・安定ではない。  
不安定なソート(インデックスを破壊する)  

・外部メモリを使わない内部ソート。  
・分割統治法  

---

## 実装

``` C#
    /// <summary>
    /// 大学の時に使っていたJavaのアルゴリズムの本のプログラム
    /// </summary>
    class JavaQuickSort
    {
        public static void Execute()
        {
            var targetArray = new int[9] { 1, 8, 7, 4, 5, 2, 6, 3, 9 };

            Console.WriteLine(string.Join(",", targetArray));

            // クイックソートを行う
            QuickSort(targetArray, 0, targetArray.Length - 1);

            Console.WriteLine(string.Join(",", targetArray));
        }

        private static void QuickSort(int[] a, int left, int right)
        {
            int pl = left;
            int pr = right;
            int pivot = a[(pl + pr) / 2];

            Console.Write($"a[{left}] ～ a[{right}] : {{");
            for (var i = left; i < right; i++)
            {
                Console.Write(a[i]);
            }
            Console.WriteLine($"{a[right]}}}");

            do
            {
                while (a[pl] < pivot) pl++;
                while (a[pr] > pivot) pr--;
                if (pl <= pr) Swap(a, pl++, pr--);
            }
            while (pl <= pr);

            if (left < pr) QuickSort(a, left, pr);
            if (pl < right) QuickSort(a, pl, right);
        }

        private static void Swap(int[] a, int idx1, int idx2)
        {
            int t = a[idx1];
            a[idx1] = a[idx2];
            a[idx2] = t;
        }
    }
```

``` C#

    /// <summary>
    /// https://www.hanachiru-blog.com/entry/2020/03/08/120000
    /// クイックソートで調べて一番上に出てきたサイトを少し改造したやつ。
    /// </summary>
    class QuickSort1
    {
        public static void Execute()
        {
            var targetArray = new int[11] { 11, 300, 10, 51, 126, 1, 53, 14, 12, 55, 6 };
            Console.WriteLine(string.Join(",", targetArray));
            QuickSort(targetArray, 0, targetArray.Length - 1);
            Console.WriteLine(string.Join(",", targetArray));
        }

        /// <summary>
        /// クイックソート
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">対称配列</param>
        /// <param name="left">ソート範囲の最初のインデックス</param>
        /// <param name="right">ソート範囲の最後のインデックス</param>
        private static void QuickSort<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            // 範囲が1つだけなら処理を抜ける
            // これの否定は left < right なので、左が中央を突き破って右にいかない限りは続ける事を意味する。
            if (left >= right) return;

            // ピボット:グループ分けの基準。枢軸(pivot)
            // ピボットを選択(範囲の先頭・真ん中・末尾の中央値を使用)
            T pivot = Median2(array[left], array[(left + right) / 2], array[right]);

            // 左の現在位置 0
            int pl = left;
            // 右の現在位置 10
            int pr = right;

            while (true)
            {
                // 左の要素と中央値を比較して、左の要素が小さければ、左を1つ右に進める。
                // 中央値より大きい値を見つけるため。見つからなければ見つけるまで続ける。
                // 中央値より大きい値は右側のグループに飛ばさないといけない。
                while (array[pl].CompareTo(pivot) < 0) pl++;
                // 右の要素と中央値を比較して、右の要素が大きければ、右を1つ左に進める。
                // 中央値より小さい値を見つけるため。見つからなければ見つかるまで続ける。
                // 中央値より小さい値は左型のグループに飛ばさないといけない。
                while (array[pr].CompareTo(pivot) > 0) pr--;

                // 左と右のインデックスが同じか、左右が交差したら終了
                if (pl >= pr) break;
                // 左右のインデックスが交差することはあるのか？
                // →あった。中央値が6で 6,1,10,51,…と並んでいる時、
                // 左からは6より大きい数がarray[2]の10で右からは6より小さい数がarray[1]で1の時、見事に左右が突き抜ける。
                // この状態で値を交換しても意味がないし、処理を続ける意味もないので、
                // breakしてとっとと次の中央値を決めてソートしていくのが手っ取り早いからこの条件文があるわけか。

                // 現在の左の位置と右の位置の数字を入れ替える
                Swap(ref array[pl], ref array[pr]);

                // 交換を行った要素の次の要素にインデックスを進める
                // 左は1つ右に。右は1つ左にインデックスを進める。
                pl++;
                pr--;
            }
            // 小さいグループ内で同じロジックを繰り替えす。
            // 繰り返して行くと、小さいグループ内で更に小さいグループと大きいグループにわかれるので、それも同じように繰り返す。
            QuickSort(array, left, pl - 1);
            // 大きいグループ内で同じロジックを繰り替えす。
            // 繰り返していくと、大きいグループ内で更に大きいグループと小さいグループにわかれるので、それも同じように繰り返す。
            QuickSort(array, pr + 1, right);
        }

        /// <summary>
        /// 3値から中央値を求める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        /// <remarks>
        /// x.CompareTo(y)はx > y なら1以上の整数値が返却される。
        /// </remarks>
        private static T Median<T>(T x, T y, T z) where T : IComparable<T>
        {
            // 初期値で見れば、x:11,y:1,z:6と入ってくる。
            // xがyより大きい場合、xとyを入れ替える。 x=1,y=11
            if (x.CompareTo(y) > 0) Swap(ref x, ref y);
            // xがzより大きい場合、xとzを入れ替える。 x=1,z=6
            if (x.CompareTo(z) > 0) Swap(ref x, ref z);
            // yがzより大きい場合、yとzを入れ替える。 y=6,z=11
            if (y.CompareTo(z) > 0) Swap(ref y, ref z);
            // 初期値で見れば、6が中央値となる。
            return y;
        }

        /// <summary>
        /// 3値の中から中央値を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        /// <remarks>
        /// 上の方式より効率がいい。
        /// 上は最悪の場合、6回の演算を行うが、こちらは最悪でも3回で済む。
        /// x:11,y:1,z:6と入ってくる。
        /// </remarks>
        private static T Median2<T>(T x, T y, T z) where T : IComparable<T> =>
            x.CompareTo(y) < 0
                ? x.CompareTo(z) < 0
                    ? (y.CompareTo(z) < 0 ? y : z)
                    : x
                : y.CompareTo(z) < 0
                    ? (x.CompareTo(z) < 0 ? x : z)
                    : y;

        // 元の形式
        // private static T Median<T>(T x, T y, T z) where T : IComparable<T>
        // {
        //    // (11 < 6) = 1 < 0 なのでelse
        //    if (x.CompareTo(y) < 0)
        //    {
        //        // xがzより小さい場合、
        //        return x.CompareTo(z) < 0 ? (y.CompareTo(z) < 0 ? y : z) : x;
        //    }
        //    else
        //    {
        //     (1 < 6) = -1 < 0なのでtrue。
        //     (11 < 6) = 1 < 0なのでfalse,zの6が中央値として返却される。
        //        return y.CompareTo(z) < 0 ? (x.CompareTo(z) < 0 ? x : z) : y;
        //    }
        //}

        private static void Swap<T>(ref T x, ref T y)
        {
            var tmp = x;
            x = y;
            y = tmp;
        }
    }
```

---

## クイックソートの計算量のO(n logn)の頭のnってなによ？

[クイックソートの時間計算量の評価（メモ）](https://qiita.com/warper/items/ae769e105862bfa25310)  

再帰計算なので、漸化式なるものを使って計算量を求める事になるらしい。  
なので、厳密にやろうとするとまず漸化式を勉強しなきゃいけないし、  
勉強してる最中に別のことがわからなくなって更に別の勉強をしてどんどん再帰的になって、本末転倒になりかねない。  

知りたいのは、計算量の求め方ではなく、なぜそうなるのかという理論なのでそこだけでも頑張って汲み取る。  

``` math
サイズnが2^kの配列が2分割されていく状況を考える  \\

n = 2^k  \\

一つの配列要素に注目したときに、その要素の位置を確定させるまでに必要な分割回数は、大雑把にk回。  \\
どの配列の要素に関しても、位置を確定させるために、k回の分割過程を要する。  \\

k = logn  \\

分割回数がわかったので、分割前に行う要素の比較回数を求めます。  \\
要素数mのあるグループを持ってきた際、その中での要素の比較回数は m−1です。  \\

ですので、分割が1回進み、要素数2^{k−1}のグループが2つ存在する状況では、2回目の分割を進めるために、  \\

2(2^{k−1}−1) = 2^k-2 =  n−2 ~ n = O(n) \\

この計算を同様に、3回目、4回目・・・k回目の分割を進める場合についても行うと、どの場合も比較回数はO(n)になることがわかります。  \\
ゆえに、配列が二分割されながらソートが進む場合、その時間計算量は  \\

O(nk) = O(nlogn)  \\
∵(k=logn)  \\
```

[なぜクイックソートの平均繰り返し回数がO(logn)で平均の計算量がO(nlogn)なんですか？？](https://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q12159958462?__ysp=6KiI566X6YePLOOCr%2BOCpOODg%2BOCr%2BOCveODvOODiA%3D%3D)  

``` math
＊＊大雑把に言って＊＊ \\
n個のソート対象を約n/2個の２つの対象に分けます。 \\
その際の比較回数は約n/2回です。  \\
これを再帰的に行います。  \\
再帰の段数（＝平均繰り返し数）  \\
はおおむねfloor(log(2,n))回でありつまりソート完了まで  \\

n/2 + 2(n/4) + 4(n/8) + .... + 2^(floor(log(2,n))-1)n/2^floor(log(2,n))  \\
= n(1/2 + 1/2 + ..(floor(log(2,n))個).. + 1/2)～nlog(2,n)/2～nlogn  \\

回の比較が行われるわけです。
```

### つまり？

うーん。  
まず、配列を2分割する計算量はO(logn)。  
クイックソートは単純にそれだけでは無く、ピボットとか色々確定させないといけないことがあるからそれがO(n)。  
合わせてO(nlogn)。  
という、かなり乱暴な理論。  

これが正確なわけがないのはわかっているけど、理論的にはそういう事だよね。  
単純な2分割だけならlog nは俺でも知ってる。  
それ以外にも色々やってるんだからnくらいつくさ程度でいいような気がする。  
何より飽きた。  
クイックソートの性質もある程度わかったし、専門家ではないので計算量を応えられるだけで十分ではないか？  

### 沼

[計算量オーダーの求め方を総整理！ 〜 どこから log が出て来るか 〜](https://qiita.com/drken/items/872ebc3a2b5caaa4a0d0)  
[ソートを極める！ 〜 なぜソートを学ぶのか 〜](https://qiita.com/drken/items/44c60118ab3703f7727f#6-1-%E3%82%AF%E3%82%A4%E3%83%83%E3%82%AF%E3%82%BD%E3%83%BC%E3%83%88%E3%81%AE%E8%A8%88%E7%AE%97%E9%87%8F)  

はやり調べ始めたら沼に嵌りそう。  
奥が深い世界だった。  
