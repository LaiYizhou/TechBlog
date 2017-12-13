### 1. 从“位运算”开场

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

【版本一】问题：

N个数字，其中某个数字出现了1次，剩下的都出现了2次，找出这个出现了一次的数字

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



【版本二】问题：

N个数字，其中某两个数字出现了1次，剩下的都出现了2次，找出这两个出现了一次的数字

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



【版本三】问题：

N个数字，其中某个数字出现了1次，剩下的都出现了K次，找出这个出现了一次的数字

详见《[程序员代码面试指南——IT名企算法与数据结构题目最优解](https://github.com/LaiYizhou/2017Work)》 P329



### 3. Unity3D常用的数据结构

##### A. 数组

1. 最常见，例如 `int[] arr = new int[5];`
2. 二维数组， `int[,] arr` 和 `int[][] arr`都可以
3. 当然，作为数组，应该类型一致：如果初始化指定值类型，那么若干个都是值类型；如果初始化指定引用类型，那么若干个都是引用类型
4. 因为是大小是固定的，所以，所有的加、减、查找都是O(n)，只有访问的时候是O(1)，因为靠下标访问

##### B. ArrayList

1. 不用指定大小，也不用类型一致
2. 因为不用类型一致，所以string，char，int，甚至类对象、结构体等都可以添加进去。而所有添加进去的元素都会当做Object类型（也就是引用类型）来处理
3. 所以，一旦是值类型添加进去了，就会把值类型转成引用类型，这个过程，不严谨的说，可以称之为“**装箱（boxing）**”，相反，等到再次需要把该值类型取出来的时候，又需要把引用类型转成值类型，这个过程，不严谨的说，可以称之为“**拆箱（unboxing）**”。
4. 因为添加是通过`Add() `插入到尾部，所以，添加操作是O(1)，剩下的都是O(n)

##### C. List\<T>

1. 上述二者的结合，既解决了“不安全”和“装箱拆箱”的问题，也解决了固定大小的问题
2. 简而言之，可以认为是“**C#中的数组**”
3. 注：关于List\<T>，后续会进一步整理

##### D.其他

1. 其他包括：
   - 双向链表LinkedList\<T>
   - 队列 Queue\<T> 
   - 栈Stack\<T>
   - 哈希表Hashtable
   - 字典Dictionary\<K, T>