using System;

namespace Algorithm.Sort.MergeSort;

/// <summary>
/// マージソート
/// </summary>
/// <remarks>
/// https://algoful.com/Archive/Algorithm/MergeSort
/// このサイトのロジックはインデックス操作が、これでもかというくらい最適化されすぎて、処理を追うのが大変。
/// </remarks>
internal class MergeSort1
{
    public static void Execute()
    {
        var targetArray = new int[11] { 11, 300, 10, 51, 126, 1, 53, 14, 12, 55, 6 };
        Console.WriteLine(string.Join(",", targetArray));
        MergeSort(targetArray);
        Console.WriteLine(string.Join(",", targetArray));
    }

    private static void MergeSort<T>(T[] array) where T : IComparable<T>
    {
        // 検査配列の半分の長さの配列を生成する
        var work = new T[(array.Length + 1) / 2];
        // 最初の1回は配列全て、左端インデックス、右端インデックス、一時配列を渡す
        // 配列全体をマージソート
        MergeSort(array, 0, array.Length - 1, work);
    }

    /// <summary>
    /// マージソートを行う
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="work"></param>
    private static void MergeSort<T>(T[] array, int left, int right, T[] work) where T : IComparable<T>
    {
        // ソートを行うデータ数が1つになった場合は処理終了
        if (left == right) return;

        // 中央のインデックスを求める
        var mid = left + (right - left) / 2;

        // 分割した左側をマージソート
        // 1,left:0,mid:5
        // 2,left:0,mid:2
        // 3,left:0,mid:1
        // 4,left:0,mid:0
        // 5,left:3,mid:4
        // 6,left:3,mid:3
        // 7,left:6,mid:8
        // 8,left:6,mid:7
        // 9,left:6,mid:6
        // 10,left:9,mid:9
        MergeSort(array, left, mid, work);

        // 分割した右側をマージソート
        // 1,mid:1,right:1
        // 2,mid:2,right:2
        // 3,mid:3,right:5
        // 4,mid:4,right:4
        // 5,mid:5,right:5
        // 6,mid:6,right:10
        // 7,mid:7,right:7
        // 8,mid:8,right:8
        // 9,mid:9,right:10
        // 10,mid:10,right:10
        MergeSort(array, mid + 1, right, work);

        /* ソート済みの各集合をマージする */
        // 1,left:0,right:1,mid:0
        // 2,left:0,right:2,mid:1
        // 3,left:3,right:4,mid:3
        // 4,left:3,right:5,mid:4
        // 5,left:0,right:5,mid:2
        // 6,left:6,right:7,mid:6
        // 7,left:6,right:8,mid:7
        // 8,left:9,right:10,mid:9
        // 9,left:6,right:10,mid:8
        // 10,left:0,right:10,mid:5
        Merge(array, left, mid, right, work);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ary">マージを行うデータの集合(マージ後のデータの集合を格納)</param>
    /// <param name="left">マージする１つ目の集合範囲の開始点</param>
    /// <param name="mid">マージする１つ目の集合の範囲の終了点</param>
    /// <param name="right">マージする２つ目の集合の範囲の終了点</param>
    /// <param name="work">マージを行うために必要な作業メモリ(マージ先集合として一時退避先として使用)</param>
    public static void Merge<T>(T[] ary, int left, int mid, int right, T[] work) where T : IComparable<T>
    {
        // 0,5,10の場合観測用条件
        if (mid == 5) { _ = 1; }

        // 左部分を作業領域に退避
        for (int i = left; i <= mid; i++)
        {
            work[i - left] = ary[i];
        }
        /* １つ目の集合の開始点をセット */
        int l = left;
        /* ２つ目の集合の開始点をセット */
        int r = mid + 1;

        /* ２つの集合のどちらかが全てマージ済みになるまでループ */
        while (true)
        {
            /* マージ先集合の開始点をセット */
            int k = l + r - (mid + 1);

            // 左部分(作業領域)が並べ終わったらマージ完了(右部分残りはもとの位置のまま)
            // leftがmidを超えたということは、ワーク側のマージが完了したという事なので、処理を終了する。
            if (l > mid) break;

            // 右部分が並べ終わったら左部分(作業領域)をすべて並べて完了
            // rightが終了点を超えたということは、集合側のマージが完了したという事なので、左側の要素を全て集合側に並べる。
            if (r > right)
            {
                while (l <= mid)
                {
                    k = l + r - (mid + 1);
                    ary[k] = work[l++ - left];
                }
                break;
            }

            /* マージ済みデータを抜いた２つの集合の先頭のデータの小さい方をマージ */
            /* マージした集合のインデックスとマージ先集合のインデックスをインクリメント */
            ary[k] = work[l - left].CompareTo(ary[r]) < 0
                // work[l - left] < (ary[r])ならworkに小さい値が入っているので、ワークの値を集合先の左に詰めて、ワーク側のインデックスを進める。
                ? work[l++ - left]
                // work[l - left] > (ary[r])なら集合先に小さい値が入っているので、集合先のインデックスを進めるだけ。rが進めばkも進むのでつじつまがう。
                : ary[r++];
        }
    }
}
