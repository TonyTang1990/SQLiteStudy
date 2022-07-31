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

    /// <summary>
    /// 数据库目录路径
    /// </summary>
    private string DatabaseFolderPath = string.Empty;

    /// <summary>
    /// 数据库连接
    /// </summary>
    private SQLiteConnection mSQLiteConnect;

    /// <summary>
    /// 数据库文件名
    /// </summary>
    private const string DatabaseName = "TestDatabase.db";

    private void Start()
    {
#if UNITY_EDITOR
        DatabaseFolderPath = "Assets/StreamingAssets/";
#else
        DatabaseFolderPath = $"{Application.persistentDataPath}/";
#endif
        // 建立数据库连接
        mSQLiteConnect = new SQLiteConnection($"{DatabaseFolderPath}/{DatabaseName}", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

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
        mSQLiteConnect.CreateTable<Player>();
        PrintPlayerDatas($"创建玩家表成功!");
    }

    /// <summary>
    /// 插入数据按钮点击
    /// </summary>
    public void OnBtnInsertData()
    {
        Debug.Log("OnBtnInsertData()");
        // 填充数据
        mSQLiteConnect.Insert(new Player
        {
            UID = 1,
            FirstName = "Huan",
            LastName = "Tang",
            Age = 29,
        });
        mSQLiteConnect.Insert(new Player
        {
            UID = 3,
            FirstName = "XiaoYun",
            LastName = "Zhou",
            Age = 28,
        });
        mSQLiteConnect.Insert(new Player
        {
            UID = 2,
            FirstName = "Jiang",
            LastName = "Fan",
            Age = 28,
        });
        mSQLiteConnect.Insert(new Player
        {
            UID = 5,
            FirstName = "ZhenLiang",
            LastName = "Li",
            Age = 29,
        });
        mSQLiteConnect.Insert(new Player
        {
            UID = 4,
            FirstName = "XiaoLin",
            LastName = "Kuang",
            Age = 28,
        });
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
            mSQLiteConnect.Delete<Player>(deleteuid);
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
            var valideplayer = mSQLiteConnect.Find<Player>((player) => player.UID == modifyuid);
            if(valideplayer != null)
            {
                var newage = -1;
                if(int.TryParse(IfInput2.text, out newage))
                {
                    var oldage = valideplayer.Age;
                    valideplayer.Age = newage;
                    mSQLiteConnect.Update(valideplayer);
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
            var valideplayer = mSQLiteConnect.Get<Player>(queryuid);
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
        var rownumberaffected = mSQLiteConnect.DeleteAll<Player>();
        PrintPlayerDatas($"删除所有玩家表数据,影响的行数:{rownumberaffected}!");
    }

    /// <summary>
    /// 删除玩家表点击
    /// </summary>
    public void OnBtnDeletePlayerTable()
    {
        Debug.Log("OnBtnDeletePlayerTable()");
        var rownumberaffected = mSQLiteConnect.DropTable<Player>();
        PrintPlayerDatas($"删除玩家表,返回影响的行数:{rownumberaffected}!");
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
    public void PrintPlayerDatas(string extrainfo = "")
    {
        var result = "玩家表数据:\n";
        var playertb = mSQLiteConnect.Table<Player>();
        foreach (var player in playertb)
        {
            result += player.ToString();
            result += "\n";
        }
        Debug.Log(extrainfo + "\n" + result);
    }

    private void OnDestroy()
    {
        mSQLiteConnect.Close();
        Application.logMessageReceived -= this.HandleLog;
    }
}