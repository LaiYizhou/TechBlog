###1. 算法：位运算

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

     *（更多详见第 25）*




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



### 8. Animator面板属性

Animator是Unity3D在4.6版本之后添加的，可以用来管理多个Animation

其面板中，有五个属性：

- Controller：这个属性一般显示着当前GameObject的名字，双击会打开Animator面板
- Avatar：（跟人物动画的骨骼有关）
- Apply Root Motion：默认不勾选，不勾选意味着在该物体的Transform可以通过脚本控制
- Update Mode：枚举类型
  - Normal：表示使用Update进行更新
  - Animate Physics：表示使用FixUpdate进行更新（一般用在和物体有交互的情况下）
  - Unscale Time：表示无视timeScale进行更新（一般用在UI动画中）
- Culling Mode：枚举类型
  - Always Animate：当物体不在视野内时，动画也播放
  - Cull Update Transform：当物体不在视野内时，Retarget动画、 IK动画和Transform的脚本控制都无效
  - Cull Completely：当物体不在视野内时，动画完全停止

上述笔记，参考链接如下：

1. （官方手册）：https://docs.unity3d.com/Manual/class-Animator.html
2. https://www.cnblogs.com/hammerc/p/4828774.html
3. https://www.cnblogs.com/bearhb/p/4519458.html




### 9. 长按和点击事件

参考链接为：http://www.jarcas.com/studios/?p=212

直接在GameObject添加如下脚本，然后再绑定对应的响应方法即可：

https://github.com/LaiYizhou/2017Work/blob/master/Scripts/LongPressOrClickEventTrigger.cs



### 10. 正则表达式



### 11. Invoke() 

这是一个Unity3D自带的延迟执行方法，继承了MonoBehavior的脚本都可以调用

- public void [Invoke](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Invoke.html) (string methodName, float time);

  意思是：在time（单位秒）后，执行methodName；

初此之外，还有几个方法：

- [InvokeRepeating](https://docs.unity3d.com/ScriptReference/MonoBehaviour.InvokeRepeating.html)
- [CancelInvoke](https://docs.unity3d.com/ScriptReference/MonoBehaviour.CancelInvoke.html)
- [IsInvoking](https://docs.unity3d.com/ScriptReference/MonoBehaviour.IsInvoking.html)

笔记中显示，`InvokeRepeating()`  和  `Invoke()` 用得最频繁，当然，是需要`CancelInvoke()` 配合



### 12. int变量

1. 用一个int变量表示**时间**
   - 时间戳
2. 用一个int变量表示**概率**
   - 在游戏中碰到，因为概率都比较，如果使用float变量都快趋近于0le
   - int p = 120，可以与团队约定，意思为：万分之一百二





### 13. A*算法



### 14. 小数点和千位符

主要就是：`3.000` 和 `3,000`的区别

通常，前者是：三；后者是：三千

但需要注意的是，有些地区的习惯是：`.` 是千位符，`,`是小数点意义



### 15. CanvasGroup

CanvasGroup，可见官方文档：<https://docs.unity3d.com/Manual/class-CanvasGroup.html>

大概类似于“在PS里面的图层文件夹上修改属性”，一旦修改，所有子物体全部生效

只不过它控制的属性比较少，只有四种

- Alpha：透明度通道
- Interactable：是否响应UI输入事件（Button组件上也有这个属性）
- Block Raycasts：是否响应鼠标射线（因为在游戏中不是每个元素都是UI的）
- Ignore Parent Groups

官网说，常见的用法有三种：

- 第一种，通过Alpha属性来实现淡入淡出效果
- 第二种，通过Interactable属性来控制所有子物体的UI输入
- 第三种，把Block Raycasts设为false，这样射线就可以透过该物体，不会“遮挡”

此外，据说（https://www.cnblogs.com/wangzy-88/p/6270431.html），当不可见一个GameObject的时候，可以`gameObject.setActive(false/true)`， 也可以`canvasGroup.alpha = 1.0f(0.0f);`， **后者性能更好**



UI淡入淡出效果：（DOTween和CanvasGroup配合）

```c#
Sequence uiMoveSequence = DOTween.Sequence();
uiMoveSequence.Append(canvasGroup.DOFade(0.01f, 0.5f) );

uiMoveSequence.AppendCallback(() =>
  {
      this.gameObject.SetActive(false);
      canvasGroup.alpha = 1.0f;
  }
);
```



### 16. 有趣的重载

Random类，官方文档：https://docs.unity3d.com/ScriptReference/Random.html

注：这里指的是**UnityEngine**里面的`Random`类，因为C#库里也有`Random`类

里面有一个方法，是个静态（static）方法：`Range()`

- public static float Range(float min, float max);
- public static int Range(int min, int max);

结论是：

- Random.Range(int a, int b)

  那么，取值范围为：[ a , b） , 即，**前闭后开**

- Random.Range(float a, float b)

  那么，取值范围为：[ a , b ] , 即，**前闭后闭**



对于int参数前闭后开的情况，配合枚举，有一个用处：

```c#
public enum EColor
{
  	RED,
  	GREEN,
  	YELLOW,
  	
  	Count
}
```

这时候，EColor.Count 的自然意思便是：颜色的种数

然后，随机生成一种颜色，`EColor color = (EColor)(Random.Range(0, (int)EColor.Count));` 即可

一来，后续添加颜色直接在Count之前添加即可；二来，EColor.Count正好不包括，不会“越界”



### 17. Enum转换

enum转换，一般也就跟 int 和 string 转换

其中，跟int转换是可以**双向强行转换**的，所以不必过多介绍

比如：`int a = (int)EColor.RED;`  或者 `EColor color = (EColor)2;`

此外，对于从enum转成string，直接`.ToString()`就可以了

关键是：**从string转成enum**

利用Enum的静态方法：`EColor color = (EColor) Enum.Parse( typeof(EColor), "GREEN");`



### 18. Tranform/GameObject.Find()

在Hierarchy面板中，如果checkbox去掉了（也就是false了）

`Tranform.Find("xxx")`  是可以找到的；`GameObject.Find("xxx")` 是找不到的

不过，像这样直接按name查找物体的情况，还是非常少的



### 19. Parse() / TryParse()

先说这两个静态方法，其实也很好说清楚：用来把string转成int

区别在于：

- Parse() 通过返回值来返回转换结果，比如，`int num = int.Parse("12");`，如果转换失败，报异常即可
- TryParse() 通过参数来返回转换结果，比如，`int.TryParse("12", out number);` ，如果转换失败，number则为0，同时返回false

此外，据说，TryParse()的效率比Parse()要好一点点

但是，重要的是：什么时候用TryParse()？什么时候用Parse() ？

有一个“软件工程”的思想是：如果读取某个指定的文件时，建议用Parse()，因为一旦报错，说明数据源头出了问题，可以检查，而且需要检查的是数据源，而不是修改代码为TryParse()；如果是读取用户输入等情况，建议用TryParse()



### 20. GetComponentsInChildren\<T>();

三点说明：

1. 返回的是一个数组，而不是List

   ```c#
   Image[] Images = this.transform.GetComponentsInChildren<Image>();
   ```

   虽然从数组到List，使用LINO语句，就一个后缀的问题，比如

   ```c#
   List<Image> imagesList = this.transform.GetComponentsInChildren<Image>().ToList();
   ```

2. 数组中的查找范围包含“自己”，同时也包含“孩子的孩子”，而且根据返回的数组结果，是**深度优先**

3. 如果一个“孩子”是false，那么，其上的组件将无法获取

   但是！！！ 

   该方法有重载：**传入参数true，即可获取**（当然，默认为false）

   ```c#
   Image[] Images = this.transform.GetComponentsInChildren<Image>(true);
   ```

   ​

此外，顺便说一下：数组的`ToList()`

有时候在读取XML和`string.Split()` 一起使用，需要注意的是：

```c#
string s = "";
string[] sArray = s.Split(',');
List<string> l = sArray.ToList();
```

此时，`l.Count = 1`。



### 21. 打乱数组

目前是这样实现，代码如下：

```c#
private void Shuffle<T>(ref List<T> list)
{
    for (int i = 0; i < list.Count; i++)
    {
      var temp = list[i];
      int randomIndex = Random.Range(0, list.Count);
      list[i] = list[randomIndex];
      list[randomIndex] = temp;
    }
}
```



### 22. 算法题：在无序数列中，找到最小的K个数

目前最优的是O(KlogN)，即：**堆排序**

大概的思想就是：利用一个容量为K的堆，然后遍历原数列，见到一个插入一个，堆会自行调整。

所以，关键在于：如何实现堆？

先说主函数：

```c#
int[] arr = new int[]{3,4,2,1,5};

List<int> Heap = new List<int>();

public List<int> GetMinKNumbers(int k)
{
  	for(int i = 0; i<arr.Length; i++)
      	InsertHeap(arr[i]);
  
  	return Heap;
}
```

堆的实现，两点：

- 当还没满（Heap.Count<=k）的时候，重点是**插入**；
- 满了（Heap.Count>k）之后，重点是**调整**

```c#
void InsertHeap(int num)
{
  	if(Heap.Count<=k)
    {
      	Heap.Add(num);
      	int index = Heap.Count-1;
      	int parent = (index-1) / 2;	
      
      	while(NumIndex!=0)
        {
          	parent = (index-1) / 2;
          	if(arr[parent] < arr[index])
            {
               	int temp = arr[parent];
              	arr[parent] = arr[index];
              	arr[index] = temp;
              
              	index = parent;
            }
          	else
              break;
        }
      
    }
  	else
    {
      	Heap[0] = num;
      	int index = 0;
      	int left = index * 2 + 1;
      	int right = index * 2 - 1;
      	
      	int largest = index;
      
      	while(left < Heap.Count && right < Heap.Count)
        {
          	// 这个函数：看原数组index, left, right哪个最大
          	// 注意：比较的是数，返回的是下标 （函数略）
          	largest = getMax(index, left, right);
          	
          	if(largest!=index)
            {
              	int temp = arr[largest];
            	arr[largest] = arr[index];
            	arr[index] = temp;
            }
          	else
              	break;
          	
          
          	index = largest;
          	int left = index * 2 + 1;
      		int right = index * 2 - 1;
        }
       	
    }
}
```

现在看来，这道题的关键知识点在于：**父子节点的索引关系**

-  子：index；那么，父：**(index-1) / 2;**
-  父：index；那么，左子：**index * 2 + 1**；右子：**index * 2 - 1**;




### 23. 2048游戏

工程源码如下：https://github.com/LaiYizhou/2017Work/blob/master/Scripts/2048Game.rar



### 24. DoTween 

Unity3D的一款插件，商店链接：[https://www.assetstore.unity3d.com/cn/#!/content/27676](https://www.assetstore.unity3d.com/cn/#!/content/27676)

DoTween的官方网站：http://www.demigiant.com/

DoTween的官方文档：http://dotween.demigiant.com/documentation.php

目前发现值得注意之处是：

- DOTween和动画设为一个层级，可能会有些问题；为了避免万一，**建议设为父子层级**





### 25. HashTable 和 Dictionary 

都是**键值对**

有如下区别：

-  HashTable不支持泛型，Dictionary支持泛型
-  因为HashTable不支持泛型，所以，所有对象都当作Object存入，则会引起装箱/拆箱操作（同理于ArrayList，*详见第 3*）
-  HashTable是线程安全的，Dictionary则不是，如果需要，则必须人为使用互斥机制保护

所以，结论是：

- 如果是单线程，推荐Dictionary
- 此外，如果Key属于字符串类型，可以考虑使用HashTable





### 26. HashSet 和 SortedSet

但凡叫set的，一般都是指：**无重复**；（因为set翻译成数学上的“集合”）

C#的里面set，是HashSet和SortedSet。（没有一个容器叫set，set是C++里的）

不严谨得来说：HashSet是**无序不重复**的List；SortedSet是**有序不重复**的List

其实，HashSet之所以叫Hash，那是因为它跟Hash有关，实际上，它是一个只有Key、没有Value的HashTable，所以性能与HashTable相仿。

所以，这才是HashSet和List的最大区别：

- List毕竟类似于数组，所以内存连续；HashSet类似于哈希表，所以内存不连续

所以，即便想要不重复或者排好序的List，更推荐使用List，并进行人为筛选或者排序；

（List里有 `.Contain()` 和  `.Sort()`）

因为，**HashSet无法从指定位置访问元素**，而HashSet更大的用途范围在于：集合运算（如并集、交集等）



### 27. 矩阵乘法

比如，以下两个矩阵相乘：
$$
\begin{bmatrix}
   1 & 0 \\
   -3 & 1 \\
  \end{bmatrix} \times
  \begin{bmatrix}
   1 & 2 & 3 \\
   3 & 4 & 5 \\
  \end{bmatrix}
$$
结论：矩阵的乘法，是基于 **单行乘法**，也就是 **行的合并**（行是左乘）



先说 单行乘法：
$$
\begin{bmatrix}
   1 & 0 \\
  \end{bmatrix} \times
  \begin{bmatrix}
   1 & 2 & 3 \\
   3 & 4 & 5 \\
  \end{bmatrix}
$$
应该视为：**带系数的第一行和带系数的第二行，进行合并**


$$
\begin{bmatrix}
   1 & 0 \\
  \end{bmatrix} \times
  \begin{bmatrix}
   1 & 2 & 3 \\
   3 & 4 & 5 \\
  \end{bmatrix}
  =1 \times
  \begin{bmatrix}
   1 & 2 & 3 \\
  \end{bmatrix}
  + 0 \times
  \begin{bmatrix}
 3 & 4 & 5\\
  \end{bmatrix}
  =
  \begin{bmatrix}
   1 & 2 & 3\\
  \end{bmatrix}
$$


于是，基于此 单行乘法：

- 把第一行运算的结果放在第一行，把第二行运算的结果放在第二行，以此类推
- 列运算同理，带系数的第一列和带系数的第二列，进行合并，**列的合并**（列是右乘）




### 28. 拓展方法

拓展方法，看上去像：带this的参数

拓展方法是基于如下情况：有一个类`A`， 里面有方法`f1()`、`f2()`， 现在还需要一个方法`f3()`，怎么处理？

- 最简单直接的方法便是，直接修改类`A`的代码，加入`f3()`即可
- 其次，写一个子类`AChild`继承`A`，然后再子类中加入`f3()`

但上述情况，有限制条件，比如，无法修改代码或者无法派生子类（比如 seal）

可以考虑第三种解决办法：使用拓展方法

拓展方法：**两个static + 一个this**

```c#
static class B
{
  	public static f3(this A)
    {
       ....
    }
}
```

这样以后，`A.f1()`、`A.f2()`没有问题，同时，`A.f3()` 也没有问题



**举例**：

DoTween插件，就是如此

```c#
public static Tweener DOIntensity(this Light target, float endValue, float duration);
```

意思是`Light` 这个类，增加了一个名叫 `DOIntensity` 的静态方法，所以，直接调用即可

```c#
 Light.DOIntensity(1.0f, 1.5f); 
```



###29. 协程和inactive

简单来说，就是：

- 如果一个GameObject上挂着某个脚本，GameObject是inactive，那么，该脚本里的协程无法调用

  调用时，会报error

  ```
  Coroutine couldn't be started because the the game object 'Image' is inactive!
  ```

- 如果一个GameObject上挂着某个脚本，在StartCoroutine()之后，再setActive(false)

  虽然不会报error，但协程里的代码也继续跑下去

（推荐阅读：http://www.theappguruz.com/blog/how-to-use-coroutines-in-unity）



### 30. 子类和父类

虽然，C#中也称呼“派生类”和“基类”

- 子类转父类，是可以随意转换，而且是自动转换的
- 父类转子类，需要强行转换，还不一定成功；如果，这个父类是被子类赋值过的，那么，才可以转换成功

很好理解，子类中至少不少于父类的方法

比如，`Duck`  可以 `Swim()`， 不代表 `Animal` 可以 `Swim()`

所以， `Duck` 转成 `Animal` 是可以的，反正 `Animal` 里面也没有 `Swim()`，反之则不然。

但是， `Animal a = new Duck()` 之后，再把 `a` 转成 `Duck` 自然是没有问题的。

（ 注：涉及到类型转换，可以考虑`as`运算符）



### 31. 余弦距离

余弦距离就是：
$$
cos\theta = 
\frac{\vec{a} · \vec{b}}
{|\vec{a}| · |\vec{b}|}
$$
比如，有6位用户，各自评论了6本书

|       | Book1 | Book2 | Book3 | Book4 | Book5 | Book6 |
| :---: | :---: | :---: | :---: | :---: | :---: | :---: |
| User1 |   4   |   3   |       |       |   5   |       |
| User2 |   5   |       |   4   |       |   4   |       |
| User3 |   4   |       |   5   |   3   |   4   |       |
| User4 |       |   3   |       |       |       |   5   |
| User5 |       |   4   |       |       |       |   4   |
| User6 |       |       |   2   |   4   |       |   5   |

那么，可以把评分视为向量 u1 = {4, 3, 0, 0, 5, 0}，u2 = {5, 0, 4, 0, 4, 0} ，诸如此类

然后计算u1、u2各向量之间的余弦距离

结论是：**余弦距离越小，用户相似度越高。**



### 32. Unity3D 破解

推荐链接：http://www.ceeger.com/forum/read.php?tid=23396&fid=8



### 33. 关于Unity3D优化推荐链接

1. 《 干货：Unity游戏开发图片纹理压缩方案 》

   https://zhuanlan.zhihu.com/p/25205686

2. 《Optimizing the size of the built iOS player》（官方文档）

    https://docs.unity3d.com/Manual/iphone-playerSizeOptimization.html

3. 《Reducing the file size of your build》（官方文档）

   https://docs.unity3d.com/Manual/ReducingFilesize.html

4. 《 Unity优化百科（UWA博客目录）》

   https://blog.uwa4d.com/archives/Index.html





### 34. 算法题：旋转词

题目：判断string a 和 string b 是否为旋转词

【结论】**只需要 `string s = b+b`，然后判断 s是否包含a（例如C#中，判断`s.IndexOf(a) == -1`）即可**



### 35. LINQ

官方网址：<https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/concepts/linq/>

举个例子：翻转string

```c#
public string Reverse(string s)
{
  	return new string(s.ToCharArray().Reverse().ToArray());
}
```



### 36. XML中的 XDocument

C#官方文档：https://msdn.microsoft.com/zh-cn/library/system.xml.linq.xdocument.aspx



### 37. List\<T>的Sort()

“正常”的List，比如，`List<int> l1 = new List<int>(){12, 9, 13, 45, 33};`

对于排序，只需要 `l1.Sort()` 即可，使用了默认的比较器

对于需要自己实现的比较器，可以配合着 Lambda表达式，简化不少，直接在括号里写出来

比如

```c#
List<Films> filmList = new List<Films>() { …… };

filmList.Sort((f1, f2) => {f1.Name.CompareTo(f2.Name)} );
```

或者

```c#
List<Object> l1 = new List<Object>(){ …… };

l1.Sort((g1, g2) => {
  	……
});
```



### 38. NormalizedPosition

Unity3D的UI里，有一个类 ScrollRect

里面有三个 *NormalizedPosition 属性，表示滑动窗口的相对位置，都是**[0, 1]**的范围

- NormalizedPosition：是一个Vector2，起点在**左下角**
- horizontalNormalizedPosition：是一个float，起点是**左端**
- verticalNormalizedPosition：是一个float，起点是**下端**

比如，在做“游戏长地图推图”模式的时候，可以控制视窗（viewport）滑动的相对位置



### 39. TransformPoint()和InverseTransformPoint()

这两个方法是Transform下的方法，用法解释很简单

- TransformPoint()

  在A下面，有一个相对坐标为（3, 3, 3）的点，求它的世界坐标

  ```c#
  Vector3 worldV = ATransform.TransformPoint(new Vector(3.0f, 3.0f, 3.0f));
  ```

- InverseTransformPoint()

  在世界坐标系中，有一个点是（100, 200, 10），求它相对B物体的相对坐标

  ```c#
  Vector3 localV = BTransform.nverseTransformPoint(new Vector(100.0f, 200.0f, 10.0f));
  ```



补充两点：

- 坐标的转化结果，会受到坐标系的缩放的影响
- 可以用在：A物体下的点，想知道它相对B物体的相对坐标；此时，可以通过A转成世界坐标，再从世界坐标转成B物体下的相对坐标



那么，现在假设一个场景：A物体下有一个子物体B

此时：`ATransform.TransformPoint(BTransform.localPositon)`  一定等于 `BTransform.position`
