/*
 * Description:             GameDatabase.cs
 * Author:                  TONYTANG
 * Create Date:             2022/08/01
 */

using System.Collections;
using System.Collections.Generic;
using TH.Modules.Data;
using UnityEngine;

/// <summary>
/// GameDatabase.cs
/// ��Ϸ���ݿ�
/// </summary>
public class GameDatabase : BaseDatabase
{
    /// <summary>
    /// ��������
    /// </summary>
    public static GameDatabase Singleton
    {
        get
        {
            if(mSingleton != null)
            {
                return mSingleton;
            }
            mSingleton = new GameDatabase();
            return mSingleton;
        }
    }
    private static GameDatabase mSingleton;

    public GameDatabase() : base()
    {

    }
}