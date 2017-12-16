###1. 一个“位运算”

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

详见《[程序员代码面试指南——IT名企算法与数据结构题目最优解](https://github.com/LaiYizhou/2017Work/tree/master/EBooks)》 P329



### 3. Unity3D常用的数据结构

#### A. 数组

1. 最常见，例如 `int[] arr = new int[5];`
2. 二维数组， `int[,] arr` 和 `int[][] arr`都可以
3. 当然，作为数组，应该类型一致：如果初始化指定值类型，那么若干个都是值类型；如果初始化指定引用类型，那么若干个都是引用类型
4. 因为是大小是固定的，所以，所有的加、减、查找都是O(n)，只有访问的时候是O(1)，因为靠下标访问

#### B. ArrayList

1. 不用指定大小，也不用类型一致
2. 因为不用类型一致，所以string，char，int，甚至类对象、结构体等都可以添加进去。而所有添加进去的元素都会当做Object类型（也就是引用类型）来处理
3. 所以，一旦是值类型添加进去了，就会把值类型转成引用类型，这个过程，不严谨的说，可以称之为“**装箱（boxing）**”，相反，等到再次需要把该值类型取出来的时候，又需要把引用类型转成值类型，这个过程，不严谨的说，可以称之为“**拆箱（unboxing）**”。
4. 因为添加是通过`Add() `插入到尾部，所以，添加操作是O(1)，剩下的都是O(n)

#### C. List\<T>

1. 上述二者的结合，既解决了“不安全”和“装箱拆箱”的问题，也解决了固定大小的问题
2. 简而言之，可以认为是“**C#中的数组**”
3. 注：关于List\<T>，后续会进一步整理

#### D. 其他

1. 其他包括：
   - 双向链表LinkedList\<T>
   - 队列 Queue\<T> 
   - 栈Stack\<T>
   - 哈希表Hashtable
   - 字典Dictionary\<K, T>




### 4. FPS计算 

FPS，就是指单位时间的帧数，换句话说，就是：帧数/秒数

在Unity中，有一个`Update()`函数，真好一帧执行一次，所以，只需要在`Update()`	中写一个计数器即可

```c#
public class FPSCounter : MonoBehaviour
{
	private int FPS;
	private int count = 0;
	private float timeCount = 0;
	
	void Start()
    {
    	count = 0;
    	timeCount = 0.0f;
    }
    
    void Update()
    {
    	count++;
    	timeCount += Time.unscaledDeltaTime;
    	
    	//表示每 0.5秒 计算一次FPS
    	if(timeCount > 0.5f)
        {
        	FPS = (int)(count / timeCount);
        	count = 0;
        	timeCount = 0.0f;
        	
        	ShowFPS(FPS);
        }
    }

}
```

其实，还有更简单的

```c#
public class FPSCounter : MonoBehaviour
{
	private int FPS;
	
    void Update()
    {
    	FPS = (int)(1 / Time.unscaledDeltaTime);
        ShowFPS(FPS);
    }

}
```



### 5. Unity3D同时打开多个工程

在Win下，只需要在Unity3D的快捷方式，右击“属性”，然后在“目标”路径栏，后面添加`-projectPath` 就好，具体[如图](https://github.com/LaiYizhou/2017Work/blob/master/Images/20171213171900.png)。 但是，在Unity3D打开的时候，会报一个Error，不过没关系，Clear掉就好，具体[如图](https://github.com/LaiYizhou/2017Work/blob/master/Images/20171213182502.png)。



### 6. 快速排序

其实快排有一个关键的函数，叫`Partition()`, 它返回的是pivot的位置

然后就可以递归调用`QuickSort()`了

先说，QuickSort()

```c#
void QuickSort(ref List<T> list, int left, int right)
{
  	if(left >= right)
      return;
  	
  	//关键就在于这个函数
  	int index = Partition(ref list, left, right);
  	
  	QuickSort(ref list, left, index-1);
  	QuickSort(ref list, index+1, right);
}
```

接下来，就是Partition()，有两种实现方式，说一种比较好理解的：

```c#
int Partition(ref List<T> list, int left, int right)
{
  	int i = left;
  	int j = right;
  	
  	while(i!=j)
    {
      	while(j>i && list[j] >= pivot)
          j--;
      	while(j>i && list[i] <= pivot)
          i++;
      
      	T temp = list[j];
      	list[j] = list[i];
      	list[i] = temp;
    }
  
  	T pivot = list[left];
  
  	list[left] = list[i];
  	list[i]=  pivot;
  
  	return i;
  	
}
```

还有一种`Partition()` 实现方法，详见《[剑指Offer](https://github.com/LaiYizhou/2017Work/tree/master/EBooks)》P63



### 7. 关键字readonly

说的表面一点，`readonly`就是“仅读”，或者说“无法修改”，这一点和`const`十分相像。

与其比较`readonly`和`const`，还不如比较`static readonly`和`const`

**先说前两者的比较**：

官方说法是：`const`的值是在**编译**的时候确定的，而`readonly`的值是在**运行**的时候确定的。

也就是说：不管怎样，`const`毕竟是常量，`readonly`毕竟是变量；其中，一个是常量一个是变量，这是最起码的区别。

所以，因为`const`是常量，所以在内存里没有存储空间，也所以在编译代码时仅仅是一个替换，也正因如此，所以在编译的时候就替换好了，所以确定了值。

而`readonly`，因为是变量，所以可以初始化，甚至可以在同一个类的不同构造函数里初始化不同的值。跟一般变量的初始化没什么区别，只不过初始化之后就不能再改了。

此外，关于写代码的用法上，还需要记住三点：

- 其一，但凡涉及到`new`操作符的，都不能声明为`const`，比如`const Vector3 V = new Vector(0.5f, 0.5f, 0.5f);` 是不行的；
- 其二，但凡涉及到局部变量的，即使想要让它不被修改，也不能声明为`readonly`；
- 其三，如果一个类的对象是`readonly`，比如`readonly A a = new A();`， 只能说明变量a是不允许修改的，这并不代表a包含的属性是不能修改的，比如`a.count = 10;`  还是可以的

**再说后两者：**

`static readonly`和`const` 确实是太像了，除了在上述理论说的那些区别之外

在写代码的时候，**几乎**是可以混用的。（因为都相似到：都可以**直接通过类名来访问**了）