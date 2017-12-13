###1. 从“位运算”开场

现在有两个变量A和B，A的取值范围是[1, 9]，B的取值范围也是[1, 9]

需要保存该两个变量的值，完全可以用两个int即可，两个int，一共 32x2 位

如果是，为了节约空间而节约空间，为了位运算而位运算，完全可以使用 **8位** 就保存两个变量的值

总之：**使用一个Byte类型的变量就可以完成**

（注：在C#中，有一个值类型是byte，[官方文档](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/byte)）



所以，num是Byte类型（当然，它的最大值是255），用前4位来保存A，后4位来保存B

因此，现在的主要操作函数有四个：

- 取num的前四位
- 取num的后四位
- 修改num的前四位为value
- 修改num的后四位为value

（当然，这个value的范围也必须是四位放得下的，就是[0, 15]）



为了完成这四个操作函数，设定两个辅助变量（也是Byte类型）

```c#
byte LEFT_MASK = 240;
byte RIGHT_MASK = 15;
```

为什么设置这两个辅助变量？因为 240 二进制为 11110000, 而 15二进制为 00001111



所以，基于此：

- 取num的前四位，就是 { **return (num & LEFT_MASK) >> 4 ;**}
- 取num的后四位，就是 { **return (num & RIGHT_MASK);** }
- 修改num的前四位为value，就是 { **num = ( num& LEFT_MASK) ^ (val & RIGHT_MASK);** }
- 修改num的后四位为value，就是 { **num = ( num& RIGHT_MASK) ^ val;** }



### 2. 异或

版本一问题：

N个数字，其中某个数字出现了1次，剩下的都出现了2次，找个这个出现了一次的数字

方法很简单：**依次异或**

```c#
int GetNum(int[] a)
{
  	int res = 0;
 	for(int i = 0; i<a.Length; i++)
      res ^= a[i];
  	return res;
}
```

注：此处问题可改为“ **某个数字出现了奇数次，剩下的都出现了偶数次** ”，此方法依然有效。



版本二问题：

N个数字，其中某两个数字出现了1次，剩下的都出现了2次，找个这两个出现了一次的数字

方法依然是：**依次异或**

这时候，异或到最后得到的结果一定不等于0，换句话说，这时候的res的二进制一定含有1

这时候，再构建一个变量 int rightOne = res & (~res + 1);

```c#
int[] GetTwoNum(int[] a)
{
  	int res = 0;
 	for(int i = 0; i<a.Length; i++)
      res ^= a[i];
  
	int rightOne = res & (~res + 1);
  
  	int r1 = 0;
  	for(int i = 0; i<a.Length; i++)
    {
      	if(a[i] & rightOne == 0)
          r1 ^= a[i];
    }
  
  	int[] resArr = new int[2];
  	resArr[0] = r1;
  	resArr[1] = r1 ^ res;
  	
  	return resArr;
}
```



版本三问题：

N个数字，其中某个数字出现了1次，剩下的都出现了K次，找个这个出现了一次的数字



























