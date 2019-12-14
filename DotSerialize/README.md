# 简介

借助于此库，可以将C#的对象序列为二进制文件或者输出为Lua脚本文件

# 使用说明

## 将C#对象序列化二进制

- 将C#对象存储为二进制

    BinarySerializeWriter通过使用BinaryFormatter类可以将C#的对象存储为二进制数据

    ```csharp

    [Serializable]
    public clss StudentData
    {
        public string name;
        public int age;
    }

    StundentData student = new StudentData();
    BinaryFormatter.WriteToBinary("D:/student.bytes",student);

    ```

- 将二进制数据读取为C#对象

    BinarySerializeReader类通过使用BinaryFormatter类，可以存储的二进制文件读取为C#对象

    ```csharp

    StundentData student = BinarySerializeReader.ReadFromBinary<StudentData>("D:/student.bytes")

    ```


## 将C#对象转存为Lua脚本

目前仅支持将C#对象转存为Lua脚本，并不支持根据Lua脚本生成C#的对象。

- 将字典类型的C#数据存储为Lua脚本
    ```csharp

    Dictionary<string, int> dic = new Dictionary<string, int>();
    dic.Add("T", 1);
    dic.Add("D", 2);
    dic.Add("S", 3);
    dic.Add("B", 4);

    LuaSerializeWriter.WriteToLua("D:/lua.txt", dic);

    ```
    生成的Lua脚本
    ```lua
    local lua = {
        T=1,
        D=2,
        S=3,
        B=4,
    }
    return lua
    ```

- 将List类型的C#数据存储为Lua脚本
    ```csharp
    List<int> intList = new List<int>();
    intList.AddRange(new int[] { 1, 2, 3, 4, 4 });

    LuaSerializeWriter.WriteToLua("D:/lua.txt", intList);
    ```
    生成的Lua脚本
    ```lua
    local lua = {
        1,
        2,
        3,
        4,
        4,
    }
    return lua
    ```

- 将自定义C#数据结构存储为Lua脚本
    ```csharp
    public clss StudentData
    {
        public string name;
        public int age;
    }

    ```

注意：
  - 生成的Lua脚本中，数据指定的变量以局部变量形式存储，如上述中的:local lua = ....
  - 变量的名称为输出的文件的名称
  - 目前支持List，尚未支持Array类型的数据
  - 无法根据Lua脚本再重新生成C#对应的数据

# API详解

## BinarySerializeReader

- public static T ReadFromBinary<T>(string filePath) where T:class

    + T ： 指示需要读取的数据使用的类型
    + filePath : 序列化文件的存储位置

## BinarySerializeWriter

- public static void WriteToBinary<T>(string filePath,T data) where T:class

    + data : 需要序列化存储的C#的对象
    + filePath : 序列化文件的存储位置

## LuaSerializeWriter

- public static void WriteToLua(string filePath,object data)

    + filePath : 转换成的Lua脚本所在的位置
    + data : 需要转换的C#的对象
