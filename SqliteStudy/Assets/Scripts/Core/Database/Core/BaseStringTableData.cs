/*
 * Description:             BaseStringTableData.cs
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
    /// BaseStringTableData.cs
    /// string做主键的表数据抽象
    /// </summary>
    public abstract class BaseStringTableData : BaseTableData
    {
        [PrimaryKey]
        public string UID
        {
            get;
            set;
        }

        public BaseStringTableData() : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uid"></param>
        public BaseStringTableData(string uid) : base()
        {
            UID = uid;
        }

        /// <summary>
        /// 打印数据
        /// </summary>
        public override string ToString()
        {
            return $"[TableName:{TableName} UID:{UID}]";
        }
    }
}