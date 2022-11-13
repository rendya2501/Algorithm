using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Sort.QuickSort;

/// <summary>
/// クイックソート
/// </summary>
/// <remarks>
/// 大学の時に使っていたJavaのアルゴリズムの本のプログラム
/// </remarks>
internal class QuickSort_VerJava
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
