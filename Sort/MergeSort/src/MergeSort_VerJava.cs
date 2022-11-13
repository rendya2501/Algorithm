using System;

namespace Algorithm.Sort.MergeSort;

/// <summary>
/// Java版マージソート
/// </summary>
internal class MergeSort_VerJava
{
    /// <summary>
    /// 作業用配列
    /// </summary>
    private static int[] buff;

    public static void Execute()
    {
        Console.WriteLine("Java版マージソート");
        var targetArray = new int[11] { 11, 300, 10, 51, 126, 1, 53, 14, 12, 55, 6 };
        Console.WriteLine(string.Join(",", targetArray));
        MergeSort(targetArray);
        Console.WriteLine(string.Join(",", targetArray));
    }

    private static void MergeSort(int[] a)
    {
        int n = a.Length;
        // 作業用配列を生成
        buff = new int[n];
        // 配列全体をマージソート
        MergeSort(a, 0, n - 1);
        // 作業用配列を解放
        buff = null;
    }

    private static void MergeSort(int[] a, int left, int right)
    {
        if (left >= right) return;

        int i;
        int center = (left + right) / 2;
        int p = 0;
        int j = 0;
        int k = left;

        // 前半部をマージソート
        MergeSort(a, left, center);
        // 後半部をマージソート
        MergeSort(a, center + 1, right);

        for (i = left; i <= center; i++)
        {
            buff[p++] = a[i];
        }
        while (i <= right && j < p)
        {
            a[k++] = (buff[j] <= a[i]) ? buff[j++] : a[i++];
        }
        while (j < p)
        {
            a[k++] = buff[j++];
        }
    }
}
