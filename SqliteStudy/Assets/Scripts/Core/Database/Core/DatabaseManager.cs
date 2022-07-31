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
    /// ���ݿ������
    /// ��Ҫ��������:
    /// 1. ���ݿ�·������(���ݿ�洢����)
    /// 2. �����ݿ�֧��
    /// 3. ���ݿ����ӣ��رգ�CUID�����Ȳ�����Ȼʹ��SQLite�ķ�װ
    /// </summary>
    public class DatabaseManager : SingletonTemplate<DatabaseManager>
    {
        /// <summary>
        /// ���ݿ��ļ��е�ַ
        /// </summary>
        private readonly static string DatabaseFolderPath = Application.persistentDataPath + "/Database/";

        /// <summary>
        /// �����ӵ����ݿ�ӳ��Map
        /// KeyΪ���ݿ�����ValueΪ��Ӧ�����ݿ�����
        /// </summary>
        private Dictionary<string, SQLiteConnection> ConnectedDatabaseMap;

        public DatabaseManager()
        {
            //������ݿ��ļ�Ŀ¼�Ƿ����
            if(!Directory.Exists(DatabaseFolderPath))
            {
                Directory.CreateDirectory(DatabaseFolderPath);
            }
            ConnectedDatabaseMap = new Dictionary<string, SQLiteConnection>();
        }

        /// <summary>
        /// �����ݿ�����
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
                Debug.Log($"�������ݿ�:{databasename}");
            }
            else
            {
                Debug.Log($"���ݿ�:{databasename}������,�����ظ�����!");
            }
        }
        
        /// <summary>
        /// ��ȡָ�����ݿ�����
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
        /// �ر����ݿ�����
        /// </summary>
        /// <param name="databasename"></param>
        public void closeDatabase(string databasename)
        {
            SQLiteConnection connection;
            if(ConnectedDatabaseMap.TryGetValue(databasename, out connection))
            {
                connection.Close();
                ConnectedDatabaseMap.Remove(databasename);
                Debug.Log($"�ر����ݿ�:{databasename}");
            }
            else
            {
                Debug.LogError($"δ�������ݿ�:{databasename},�ر�ʧ��!");
            }
        }

        /// <summary>
        /// �ر����������ӵ����ݿ�
        /// </summary>
        public void closeAllDatabase()
        {
            foreach (var databasename in ConnectedDatabaseMap.Keys)
            {
                closeDatabase(databasename);
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        public void clear()
        {
            closeAllDatabase();
        }

        /// <summary>
        /// ָ�����ݿ��Ƿ�������
        /// </summary>
        /// <param name="databasename"></param>
        /// <returns></returns>
        private bool isDatabaseConnected(string databasename)
        {
            return ConnectedDatabaseMap.ContainsKey(databasename);
        }

        #region ��������
        /// <summary>
        /// ��ȡָ�����ݿ�����������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databasename">���ݿ���</param>
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