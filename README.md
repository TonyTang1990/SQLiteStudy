# SQLiteStudy
本Git是基于SQLite4Unity3d的学习使用封装，基于SQLite4Unity3d库的基础上封装实现更多功能:

1. **多数据库创建管理**
2. **数据库路径规划统一管理**
3. **自定义数据库统一访问封装**
4. **自定义数据库表统一访问封装**

## 类设计

- DatabaseManager.cs(**对多数据库创建访问以及路径规划进行封装支持**)
- BaseDatabase.cs(**数据库基类抽象**--实现对数据库操作进行封装)
- BaseTableData.cs(**数据库表数据基类抽象**--实现对数据库表操作进行封装)
- BaseIntTableData.cs(**int做主键的表数据抽象**--实现对int主键表的操作封装)
- BaseStringTableData.cs(**string做主键的表数据抽象**--实现对string主键表的操作封装)

## 实战

### 定义数据库表结构

```CS
/// <summary>
/// 玩家信息表成员结构
/// </summary>
public class Player : BaseIntTableData
{
    /// <summary>
    /// 名
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// 姓
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    [Indexed]
    public int Age { get; set; }

    /// <summary>
    /// 打印数据
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[Player: UId={UID}, FirstName={FirstName},  LastName={LastName}, Age={Age}]";
    }
}
```

### 创建并建立SQLite数据库连接

```CS
GameDatabase.Singleton.LoadDatabase();
```

可以看到本来PersistentAssets目录下没有数据库文件的这一下就创建成功了。
![CreateDatabase](/img/Database/CreateDatabaseUnserPersistent.PNG)

### 创建玩家表

```CS
GameDatabase.Singleton.CreateTable<Player>();
```

![CreatePlayerTable](/img/Database/CreatePlayerTable.png)
因为还没有数据所以看不到数据打印，通过Navicat打开数据库，我们可以看到已经成功创建了Player表:
![PlayerTableView](/img/Database/PlayerTableView.png)

### 玩家表插入玩家数据

```CS
GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player
        {
            UID = 1,
            FirstName = "Huan",
            LastName = "Tang",
            Age = 29,
        });
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player
        {
            UID = 3,
            FirstName = "XiaoYun",
            LastName = "Zhou",
            Age = 28,
        });
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player
        {
            UID = 2,
            FirstName = "Jiang",
            LastName = "Fan",
            Age = 28,
        });
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player
        {
            UID = 5,
            FirstName = "ZhenLiang",
            LastName = "Li",
            Age = 29,
        });
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player
        {
            UID = 4,
            FirstName = "XiaoLin",
            LastName = "Kuang",
            Age = 28,
        });
```

![InsertPlayerTableData](/img/Database/InsertPlayerTableData.png)

### 玩家表删除指定玩家ID数据

```CS
GameDatabase.Singleton.DeleteDataByUIDI<Player>(deleteuid);
```

![DeletePlayerTableData](/img/Database/DeletePlayerTableData.png)

### 玩家表修改指定玩家Age数据

```CS
var modifyuid = -1;
if (int.TryParse(IfInput1.text, out modifyuid))
{
    Debug.Log($"修改UID:{IfInput1.text}数据!");
    var valideplayer = GameDatabase.Singleton.GetDataByUIDI<Player>(modifyuid);
    if(valideplayer != null)
    {
        var newage = -1;
        if(int.TryParse(IfInput2.text, out newage))
        {
            var oldage = valideplayer.Age;
            valideplayer.Age = newage;
            GameDatabase.Singleton.UpdateDataI(valideplayer);
            PrintPlayerDatas($"修改UID:{IfInput1.text}的年纪:{oldage}到:{newage}!");
        }
        else
        {
            Debug.LogError($"无效的年龄输入:{IfInput2.text}，更新玩家UID:{modifyuid}的年纪失败!");
        }
    }
}
```

![ModifyPlayerTableAgeData](/img/Database/ModifyPlayerTableAgeData.png)

### 删除玩家表所有数据

```CS
var rownumberaffected = GameDatabase.Singleton.DeleteTableAllData<Player>();
```

![DeleteAllPlayerTableData](/img/Database/DeleteAllPlayerTableData.png)
可以看到我们成功删除了玩家表里的所有数据，接下来用Navicat验证下数据库里的情况：
![DatabaseAfterDeleteAllPlayerTableData](/img/Database/DatabaseAfterDeleteAllPlayerTableData.png)

### 删除数据库里的玩家表

```CS
var rownumberaffected = GameDatabase.Singleton.DeleteTable<Player>();
```

![DeletePlayerTable](/img/Database/DeletePlayerTable.png)
删除玩家表后通过Navicat查看数据库:
![DatabaseViewAfterDeletePlayerTable](/img/Database/DatabaseViewAfterDeletePlayerTable.png)
通过查看源码，我们可以看到，DraopTable底层实际是通过调用SQL的Drop语句来实现删除表的:

```CS
/// <summary>
/// Executes a "drop table" on the database.  This is non-recoverable.
/// </summary>
public int DropTable<T>()
{
    var map = GetMapping (typeof (T));

    var query = string.Format("drop table if exists \"{0}\"", map.TableName);

    return Execute (query);
}
```

### 关闭数据库连接

```CS
        GameDatabase.Singleton.CloseDatabase();
```

# 博客

[SQLite学习](http://tonytang1990.github.io/2019/05/20/SQLite%E5%AD%A6%E4%B9%A0/)