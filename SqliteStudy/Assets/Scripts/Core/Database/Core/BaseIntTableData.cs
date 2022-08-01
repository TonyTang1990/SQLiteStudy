/*
 * Description:             BaseIntTableData.cs
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
    /// BaseIntTableData.cs
    /// Int做主键的表数据抽象
    /// </summary>
    public abstract class BaseIntTableData : BaseTableData
    {
        /// <summary>
        /// 主键UID
        /// </summary>
        [PrimaryKey]
        public int UID { get; set; }

        /// <summary>
        /// 打印所有表数据
        /// </summary>
        public override string ToString()
        {
            return $"[TableName:{TableName} UID:{UID}]";
        }
    }
}