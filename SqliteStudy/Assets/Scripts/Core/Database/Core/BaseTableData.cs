/*
 * Description:             BaseTableData.cs
 * Author:                  TONYTANG
 * Create Date:             2022/08/01
 */

using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Modules.Data
{
    /// <summary>
    /// BaseTableData.cs
    /// 数据库表数据基类抽象
    /// </summary>
    public abstract class BaseTableData
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return GetType().Name;
            }
        }

        public BaseTableData()
        {

        }

        /// <summary>
        /// 打印数据
        /// </summary>
        public override string ToString()
        {
            return $"[TableName:{TableName}]";
        }
    }
}