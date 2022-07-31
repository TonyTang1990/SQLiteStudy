/*
 * Description:             Player.cs
 * Author:                  TonyTang
 * Create Date:             2020/3/9
 */

using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家信息
/// </summary>
public class Player
{
    [PrimaryKey]
    public int UID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Indexed]
    public int Age { get; set; }

    public override string ToString()
    {
        return $"[Player: UId={UID}, FirstName={FirstName},  LastName={LastName}, Age={Age}]";
    }
}