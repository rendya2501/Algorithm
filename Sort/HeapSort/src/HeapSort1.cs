using System;

namespace Algorithm.Sort.HeapSort;

/// <summary>
/// C#版ヒープソート
/// </summary>
/// <remarks>
/// https://algoful.com/Archive/Algorithm/HeapSort
/// 常に最大値(最小値)を取り出すことができるデータ構造があれば、それをソートアルゴリズムに援用できます。
/// その考えのもと、ヒープソート とは 二分ヒープ と呼ばれるデータ構造を用いてソートを行うアルゴリズムです。
/// 二分ヒープと呼ばれるデータ構造は、二分木を使って作られるデータ構造で、
/// 各ノードの値は常に子ノードの値より大きいか等しくなるように配置されます。
/// つまり二分ヒープのルートノードの値は最大値となります。
/// よって、「ヒープへの追加(UpHeap)」と「ヒープからルートの削除(DownHeap)」についての処理ができれば、ヒープソートを実装できます。
/// ヒープソートは 不安定 な 内部 ソートです。
/// 平均計算量・最悪計算量ともに O(n log n) のため安定して高速で動作します。
/// ただし、なんだかんだクイックソートのほうが速いようです。
/// https://daeudaeu.com/heap-sort/
/// 合わせてここの解説も読めば完璧だ。
/// </remarks>
internal class HeapSort1
{
    public static void Execute()
    {
        Console.WriteLine("C#版ヒープソート1");
        var targetArray = new int[10] { 10, 9, 5, 8, 3, 2, 4, 6, 7, 1 };
        Console.WriteLine(string.Join(",", targetArray));
        HeapSort23(targetArray);
        Console.WriteLine(string.Join(",", targetArray));
    }

    /// <summary>
    /// ヒープソートを実行します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <remarks>
    /// 1.データを取り出し、ヒープに追加(up-heap)します。すべてのデータをヒープに追加するまで繰り返します。
    /// 2.ルートデータを取り出します(down-heap)。ソート済データに取り出したデータを追加します。
    ///   全データ繰り返してソートが完了します。
    /// ヒープ構造はデータ自体の並びのみで表現できる(ポインタなどを持つ必要がない)という利点を活かし、
    /// 元のデータ領域をそのまま使うことで内部ソートとして実装できます。
    /// ヒープ構造のデータとして表現すると、n番目の要素の親要素のインデックスは (n - 1) / 2 となります。
    /// 同じく子要素(左)のインデックスは 2n + 1 で求まります。
    /// </remarks>
    private static void HeapSort23<T>(T[] array) where T : IComparable<T>
    {
        int i = 0;
        // 1.データを全てヒープへ追加
        // 次の処理で早速ヒープの先頭を入れ替えるので、入れながらヒープを構築する。
        while (i < array.Length)
        {
            UpHeap(array, i++);
        }
        // 2.ルートデータを取り出しつつ、ヒープを再構成していく
        while (--i > 0)
        {
            // ヒープの最大値を末端へ移動
            // 2分ヒープ木の特性上、先頭が一番大きいので、先頭の要素を一番後ろに持っていくことで昇順ソートとする
            Swap(ref array[0], ref array[i]);
            // ヒープを再構成
            // ルートが最大値ではなくなったので、ヒープを再構成する。
            // ルートを最大値にして子要素を並び変えていく。
            DownHeap(array, i - 1);
        }
    }

    /// <summary>
    /// ヒープへの追加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="n"></param>
    /// <remarks>
    /// 1.ヒープの最下層(配列の場合はヒープデータの直後)へデータを追加(up-heap)します。
    /// 2.追加されたデータの親データと比較し、順序が正しければ処理完了です。
    /// 3.比較結果が正しくなければ、親データと交換して、停止するまで2.を繰り返します。
    /// </remarks>
    private static void UpHeap<T>(T[] array, int n) where T : IComparable<T>
    {
        while (n != 0)
        {
            // ary[n]の親要素のインデックスを求める

            // 1,2→0
            // 3,4→1
            // 5,6→2
            // 7,8→3
            // 9→4

            // 0,1,2,3,4,5,6,7,8,9
            // ? ! !
            //   ?   ! !
            //     ?     ! !
            //       ?       ! !
            //         ?         !

            //         0
            //      1     2
            //    3   4 5   6
            //   7 8 9
            // という構成になるので、親子関係がその通りに出来上がるよねってわけ。
            int parent = (n - 1) / 2;
            // 子供の要素が親の要素よりも大きかったら要素を入れ替える
            // 逆に親が大きいままなら入れ替える必要はないので処理を終了する
            if (array[n].CompareTo(array[parent]) < 0)
            {
                break;
            }

            // 要素の入れ替え
            Swap(ref array[n], ref array[parent]);
            // 子のインデックスを親のインデックスとする→交換後のインデックスを保持
            // 親と子を入れ替えたので、その道中の要素が大丈夫かもう一度判定する必要があるため
            // ループが継続される。
            n = parent;
        }
    }

    /// <summary>
    /// ヒープの取り出し(ルートの削除)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="n"></param>
    /// <remarks>
    /// 1.ルートデータを取り出し再構成(down-heap)します、ヒープ最下層のデータと交換します。
    /// 2.ルートデータを子データと比較し、正しい順序であれば処理完了です。
    /// 3.比較結果が正しくなければ、子データと交換して、停止するまで2.を繰り返します。
    /// </remarks>
    private static void DownHeap<T>(T[] array, int n) where T : IComparable<T>
    {
        if (n == 0)
        {
            return;
        }

        // n=8の場合のトレース
        //                0:1
        //          1:9          2:5
        //     3:8      4:3
        //  7:6   8:7

        // child = 1
        // Swap(ref array[parent = 0], ref array[child = 1]);
        //                0:9
        //          1:1          2:5
        //     3:8      4:3
        //  7:6   8:7
        // parent = 1

        // child = 1 * 2 + 1 = 3
        // Swap(ref array[parent = 1], ref array[child = 3]);
        //                0:9
        //          1:8          2:5
        //     3:1      4:3
        //  7:6   8:7
        // parent = 3

        // child = 3 * 2 + 1 = 7
        // (child < n) && array[child = 7] = 6.CompareTo(array[child + 1 = 8] = 7)
        // Swap(ref array[parent = 3], ref array[child = 8]);
        //                0:9
        //          1:8          2:5
        //     3:7      4:3
        //  7:6   8:1
        // parent = 8

        // child = 8 * 2 + 1 = 17 break;

        // n=7の場合のトレース
        // 0番目と8番目が入れ替わって来るので後は同じ要領で繰り返していくだけ
        //                0:1
        //          1:8          2:5
        //     3:7      4:3
        //  7:6   8:9

        // child = 1
        // Swap(ref array[parent = 0], ref array[child = 1]);
        //                0:8
        //          1:1          2:5
        //     3:7      4:3
        //  7:6   8:9
        // parent = 1

        // 親のインデックス
        int parent = 0;
        while (true)
        {
            // ary[n]の子要素
            // 現在の親に対する子要素のインデックスを求める。
            // 子供に対する親の求め方が(n - 1) / 2なので、その逆をやれば、子供のインデックスが求まるわけ。
            int child = 2 * parent + 1;
            // n以上は既にソート済みのインデックスなので、そこに達したら処理は完了したとみなす。
            if (child > n)
            {
                break;
            }
            // その子供の階層で大きいほうを採用するための条件式
            // 例えばchild = 7の時、同じ階層の要素には8がある。
            // 7と8の要素を比べて8の要素が大きかったら,childインデックスを8にするためインクリメントする
            if ((child < n) && array[child].CompareTo(array[child + 1]) < 0)
            {
                child++;
            }
            // 親の要素が子要素より小さい場合は、親と子を入れ替える
            // 逆に親の要素が子より大きいなら入れ替え処理は必要ないので抜ける
            if (array[parent].CompareTo(array[child]) > 0)
            {
                break;
            }
            // 親と子の位置を入れ替え
            Swap(ref array[parent], ref array[child]);
            // 交換後のインデックスを保持
            // 次の親子との比較を行っていくため
            parent = child;
        }
    }

    private static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
}
