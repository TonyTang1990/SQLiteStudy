/*
 * Description:             Player.cs
 * Author:                  TonyTang
 * Create Date:             2020/3/9
 */

using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using TH.Modules.Data;
using UnityEngine;

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