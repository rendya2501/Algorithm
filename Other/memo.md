# メモ

## ユークリッド互除法

``` C#
    public int Gcd(int a, int b)
    {
        int gcd(int x, int y) => y == 0 ? x : gcd(y, x % y);
        return a > b ? gcd(a, b) : gcd(b, a);
    }
```

---

## 連結リスト(LinkedList)

[Javaで実装したLinkedListのサンプルプログラムをご紹介](https://rainbow-engine.com/java-linkedlist-sample/)  

- 自分自身の値  
- 自分自身のアドレス  
- 次のアドレス  

---

## 最近見つかったシンプルなソート法

計算量 : `O(n^2)`  

``` js
var array = new int[2,4,5,6,4,6,8,2,8,9,4];
for(var i = 0 ; array.length; i++){
    for(var j = 0 ; array.length; j++){
        if (array[j] < array[i]){
            swap(array[i],array[j]);
        }
    }
}
```
