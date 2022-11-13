using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Sort.HeapSort;

/// <summary>
/// Java版ヒープソート
/// </summary>
internal class HeapSort_VerJava
{
    public static void Execute()
    {
        Console.WriteLine("Java版ヒープソート");
        var targetArray = new int[10] { 10, 9, 5, 8, 3, 2, 4, 6, 7, 1 };
        Console.WriteLine(string.Join(",", targetArray));
        HeapSort(targetArray, 10);
        Console.WriteLine(string.Join(",", targetArray));
    }

    /// <summary>
    /// ヒープソート
    /// </summary>
    /// <param name="a"></param>
    /// <param name="n"></param>
    private static void HeapSort(int[] a, int n)
    {
        // a[i]~a[n-1]をヒープ化
        for (int i = (n - 1) / 2; i >= 0; i--)
        {
            DownHeap(a, i, n - 1);
        }
        for (int i = n - 1; i > 0; i--)
        {
            // 最大要素と未ソート部末尾要素を交換
            Swap(a, 0, i);
            // a[0]~a[i-1]をヒープ化
            DownHeap(a, 0, i - 1);
        }
    }

    /// <summary>
    /// a[left]~a[right]をヒープ化
    /// </summary>
    /// <param name="a"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private static void DownHeap(int[] a, int left, int right)
    {
        // ルート
        int temp = a[left];
        // 大きいほうの子
        int child;
        // 親
        int parent;

        for (parent = left; parent < (right + 1) / 2; parent = child)
        {
            // 左の子
            int cl = parent * 2 + 1;
            // 右の子
            int cr = cl + 1;
            // 大きいほう
            child = (cr <= right && a[cr] > a[cl]) ? cr : cl;
            if (temp >= a[child])
            {
                break;
            }
            a[parent] = a[child];
        }
        a[parent] = temp;
    }

    private static void Swap(int[] a, int idx1, int idx2)
    {
        int t = a[idx1];
        a[idx1] = a[idx2];
        a[idx2] = t;
    }
}
