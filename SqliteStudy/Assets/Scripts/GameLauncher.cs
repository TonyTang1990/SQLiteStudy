/*
 * Description:             GameLauncher.cs
 * Author:                  #AUTHOR#
 * Create Date:             #CREATEDATE#
 */

using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameLauncher.cs
/// </summary>
public class GameLauncher : MonoBehaviour
{
    /// <summary>
    /// 输入文本1
    /// </summary>
    public InputField IfInput1;

    /// <summary>
    /// 输入文本2
    /// </summary>
    public InputField IfInput2;

    /// <summary>
    /// 输出文本
    /// </summary>
    public Text TxtOutput;

    private void Start()
    {
        // 建立数据库连接
        GameDatabase.Singleton.LoadDatabase();
        
        Application.logMessageReceived +=  this.HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        TxtOutput.text = logString;
    }

    /// <summary>
    /// 创建玩家表按钮点击
    /// </summary>
    public void OnBtnCreatePlayerTable()
    {
        Debug.Log("OnBtnCreatePlayerTable()");
        // 创建数据库表(Player)
        GameDatabase.Singleton.CreateTable<Player>();
        PrintPlayerDatas($"创建玩家表成功!");
    }

    /// <summary>
    /// 插入数据按钮点击
    /// </summary>
    public void OnBtnInsertData()
    {
        Debug.Log("OnBtnInsertData()");
        // 填充数据
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player(1, "Huan", "Tang", 29));
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player(3, "XiaoYun", "Zhou", 28));
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player(2, "Jiang", "Fan", 28));
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player(5, "ZhenLiang", "Li", 29));
        GameDatabase.Singleton.InsertOrReplaceDataI<Player>(new Player(4, "XiaoLin", "Kuang", 28));
        PrintPlayerDatas($"插入玩家数据成功!");
    }

    /// <summary>
    /// 删除数据按钮点击
    /// </summary>
    public void OnBtnDelteData()
    {
        Debug.Log("OnBtnDelteData()");
        var deleteuid = -1;
        if (int.TryParse(IfInput1.text, out deleteuid))
        {
            GameDatabase.Singleton.DeleteDataByUIDI<Player>(deleteuid);
            PrintPlayerDatas($"删除UID:{IfInput1.text}数据成功!");
        }
        else
        {
            Debug.LogError($"无效的UID:{IfInput1.text},删除数据失败!");
        }
    }

    /// <summary>
    /// 修改数据按钮点击
    /// </summary>
    public void OnBtnModifyData()
    {
        Debug.Log("OnBtnModifyData()");
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
            else
            {
                Debug.LogError($"找不到UID:{modifyuid}的数据,修改数据失败!");
            }
        }
        else
        {
            Debug.LogError($"无效的UID:{IfInput1.text},修改数据失败!");
        }
    }

    /// <summary>
    /// 查询数据按钮点击
    /// </summary>
    public void OnBtnQueryData()
    {
        Debug.Log("OnBtnQueryData()");
        var queryuid = -1;
        if (int.TryParse(IfInput1.text, out queryuid))
        {
            Debug.Log($"");
            var valideplayer = GameDatabase.Singleton.GetDataByUIDI<Player>(queryuid);
            if (valideplayer != null)
            {
                PrintPlayerDatas($"查询UID:{IfInput1.text}数据!\n{valideplayer.ToString()}!");
            }
            else
            {
                Debug.LogError($"找不到UID:{queryuid}的数据,查询数据失败!");
            }
        }
        else
        {
            Debug.LogError($"无效的UID:{IfInput1.text},查询数据失败!");
        }
    }

    /// <summary>
    /// 删除所有玩家表数据点击
    /// </summary>
    public void OnBtnDeleteAllPlayerData()
    {
        Debug.Log("OnBtnDeleteAllPlayerData()");
        var rownumberaffected = GameDatabase.Singleton.DeleteTableAllData<Player>();
        PrintPlayerDatas($"删除所有玩家表数据,影响的行数:{rownumberaffected}!");
    }

    /// <summary>
    /// 删除玩家表点击
    /// </summary>
    public void OnBtnDeletePlayerTable()
    {
        Debug.Log("OnBtnDeletePlayerTable()");
        var rownumberaffected = GameDatabase.Singleton.DeleteTable<Player>();
        PrintPlayerDatas($"删除玩家表,返回影响的行数:{rownumberaffected}!", false);
    }

    /// <summary>
    /// 查询玩家表是否存在
    /// </summary>
    public void OnBtnExistPlayerTable()
    {
        Debug.Log("OnBtnExistPlayerTable()");
        // TOOD: 待实现
    }

    /// <summary>
    /// 查看数据库按钮点击
    /// </summary>
    public void OnBtnBrowseDatabase()
    {
        PrintPlayerDatas("OnBtnBrowseDatabase()");
    }

    /// <summary>
    /// 打印输出玩家表信息
    /// </summary>
    /// <param name="extrainfo">额外信息</param>
    /// <param name="printAllData">是否打印所有数据</param>
    public void PrintPlayerDatas(string extrainfo = "", bool printAllData = true)
    {
        var result = string.Empty;
        if (printAllData)
        {
            result += "玩家表数据:\n";
            var playertb = GameDatabase.Singleton.GetAllData<Player>();
            foreach (var player in playertb)
            {
                result += player.ToString();
                result += "\n";
            }
        }
        Debug.Log(extrainfo + "\n" + result);
    }

    private void OnDestroy()
    {
        GameDatabase.Singleton.CloseDatabase();
        Application.logMessageReceived -= this.HandleLog;
    }
}