/*
 * Description:             DatabaseManager.cs
 * Author:                  TANGHUAN
 * Create Date:             2020/03/13
 */

using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;

namespace TH.Modules.Data
{
    /// <summary>
    /// 数据库管理类
    /// 主要功能如下:
    /// 1. 数据库路径处理(数据库存储管理)
    /// 2. 多数据库支持
    /// 3. 数据库连接，关闭，CUID操作等操作依然使用SQLite的封装
    /// </summary>
    public class DatabaseManager : SingletonTemplate<DatabaseManager>
    {
        /// <summary>
        /// 数据库文件夹地址
        /// </summary>
        private readonly static string DatabaseFolderPath = Application.persistentDataPath + "/Database/";

        /// <summary>
        /// 已连接的数据库映射Map
        /// Key为数据库名，Value为对应的数据库连接
        /// </summary>
        private Dictionary<string, SQLiteConnection> ConnectedDatabaseMap;

        public DatabaseManager()
        {
            //检查数据库文件目录是否存在
            if(!Directory.Exists(DatabaseFolderPath))
            {
                Directory.CreateDirectory(DatabaseFolderPath);
            }
            ConnectedDatabaseMap = new Dictionary<string, SQLiteConnection>();
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="databasename"></param>
        /// <param name="openflags"></param>
        /// <param name="storeDateTimeAsTicks"></param>
        public void openDatabase(string databasename, SQLiteOpenFlags openflags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, bool storeDateTimeAsTicks = false)
        {
            if(!isDatabaseConnected(databasename))
            {
                var databasepath = DatabaseFolderPath + databasename;
                var connection = new SQLiteConnection(databasepath, openflags, storeDateTimeAsTicks);
                ConnectedDatabaseMap.Add(databasename, connection);
                Debug.Log($"连接数据库:{databasename}");
            }
            else
            {
                Debug.Log($"数据库:{databasename}已连接,请勿重复连接!");
            }
        }
        
        /// <summary>
        /// 获取指定数据库连接
        /// </summary>
        /// <param name="databasename"></param>
        /// <returns></returns>
        public SQLiteConnection getDatabaseConnection(string databasename)
        {
            SQLiteConnection connection;
            if (ConnectedDatabaseMap.TryGetValue(databasename, out connection))
            {
                return connection;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="databasename"></param>
        public void closeDatabase(string databasename)
        {
            SQLiteConnection connection;
            if(ConnectedDatabaseMap.TryGetValue(databasename, out connection))
            {
                connection.Close();
                ConnectedDatabaseMap.Remove(databasename);
                Debug.Log($"关闭数据库:{databasename}");
            }
            else
            {
                Debug.LogError($"未连接数据库:{databasename},关闭失败!");
            }
        }

        /// <summary>
        /// 关闭所有已连接的数据库
        /// </summary>
        public void closeAllDatabase()
        {
            foreach (var databasename in ConnectedDatabaseMap.Keys)
            {
                closeDatabase(databasename);
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void clear()
        {
            closeAllDatabase();
        }

        /// <summary>
        /// 指定数据库是否已连接
        /// </summary>
        /// <param name="databasename"></param>
        /// <returns></returns>
        private bool isDatabaseConnected(string databasename)
        {
            return ConnectedDatabaseMap.ContainsKey(databasename);
        }

        #region 辅助方法
        /// <summary>
        /// 获取指定数据库表的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databasename">数据库名</param>
        /// <returns></returns>
        public string getTableAllDatasInOneString<T>(string databasename) where T : new()
        {
            SQLiteConnection sqliteconnection = getDatabaseConnection(databasename);
            if (sqliteconnection != null)
            {
                var querytable = sqliteconnection.Table<T>();
                if (querytable != null)
                {
                    var result = string.Empty;
                    foreach (var data in querytable)
                    {
                        result += data.ToString();
                        result += "\n";
                    }
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}