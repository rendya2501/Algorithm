using System;
using Algorithm.Collections.DoublyLinkedList;

// 参考サイトを元に愚直に実装した双方向連結リスト
var doublyLinkedList = new DoublyLinkedList<int>();
doublyLinkedList.InsertFirst(10);
doublyLinkedList.InsertFirst(9);
Console.WriteLine(doublyLinkedList.First.Value);

// KeyValuePairで実装した双方向キーバリュー連結リスト
var doublyKeyValueLinkedList = new DoublyKeyValueLinkedList<int,int>();
doublyLinkedList.InsertFirst(10);
doublyLinkedList.InsertFirst(9);
Console.WriteLine(doublyLinkedList.First.Value);
