### 1. Wave Function Collapse

中文翻译为“波函数坍缩算法”，可以程序自动生成无限大地图

https://github.com/mxgmn/WaveFunctionCollapse

```c#
int? num1 = null;
```



### 2. OpenGL

https://learnopengl.com/Introduction



### 3. 算法：树问题 模板



### 4. 算法：求一个数二进制的1的最低位

比如

5（0110） 1的最低位是 2

```c#
 x &（-x） //0110 & 1010
```



### 5. 算法

- Segment Tree
  - 解决 某个区间内的某值（比如和、最大值、最小值）
  - 因为每个节点都用一个start 和 end 来代表一个区间
  - 然后 提供三个方法 Build、Update、Query 都根据递归来操作
- Fenwick Tree / Binary Indexed Tree
  - 解决 范围求和 的问题，当然完全可以用前n项和 然后Si-Sj，但是 一旦数据是更新的，用这个更好
  - 因为不管是更新Update() 还是求前n项和 Query() 都是 O(logN)
- Union Find
  - 解决 “通过两两同事关系 判断是否在同一个单位里” 的问题



### 6. 算法

大小写字母转换

可以 **直接异或32**

```c#
char ch = 'a';
ch = ch ^ 32;
```



### 7. C++ 常量指针和指针常量



### 8.  C++ 数组指针和指针数组



### 9. Lua和C#

Lua和C#之间传参、返回时，尽可能不要传递以下类型

- 严重类： Vector3/Quaternion等Unity值类型，数组
- 次严重类：bool string 各种object
- 建议传递：int float double

来源：https://blog.uwa4d.com/archives/USparkle_Lua.html



### 10. 卡特兰数

http://lanqi.org/interests/10939/



### 11. BinarySearch()

```c#
List<int> list = new List<int>() {0, 2, 4, 5, 6, 10};
int index = list.BinarySearch(4);
```

**前提是：有序**

- 如果找到了，返回index（从0开始计数）
- 如果没找到，就返回下一个比它大的index的反码

所以

```c#
if (index < 0) 
	index = ~index;
```

返回值小于0，那么 ~index 就是下一个比它大的下标

比如，5的反码是-6 （因为 5的反码 + 1 是 -5，所以 5的反码 就是 -6）



### 12. Convert.ToString

string  valueString  =  Convert.ToString( value,  radix); 

- value  整数值

- radix  为 2, 8,10, 16，分别代表进制

```c#
int i=8;    
string str_value = Convert.ToString(8, 2);  
//str_value	结果为1000   
```



### 13. Showing particles in Screen Space - Overlay canvas

https://www.universityofgames.net/showing-particles-in-screen-space-overlay-canvas-unity-engine/



### 14.题外话

1. Unity引擎渲染技术学习极简路线图

   https://zhuanlan.zhihu.com/p/33432743

2. 《王者荣耀》和 Unity3D

   http://youxiputao.com/articles/11842

   https://www.twblogs.net/a/5db3a6a1bd9eee310ee6a388