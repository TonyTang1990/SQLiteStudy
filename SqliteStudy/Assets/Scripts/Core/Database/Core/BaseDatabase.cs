/*
 * Description:             BaseDatabase.cs
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
    /// BaseDatabase.cs
    /// 数据库基类抽象
    /// Note:
    /// 子类自行实现单例模式方便访问
    /// </summary>
    public abstract class BaseDatabase
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected SQLiteConnection mSQLiteConnect;

        /// <summary>
        /// 数据库名
        /// </summary>
        public virtual string DatabaseName
        {
            get
            {
                return $"{GetType().Name}.db";
            }
        }

        protected BaseDatabase()
        {

        }

        /// <summary>
        /// 加载数据库
        /// </summary>
        public void LoadDatabase()
        {
            mSQLiteConnect = DatabaseManager.Singleton.openDatabase(DatabaseName);
        }

        /// <summary>
        /// 数据库是否已连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return mSQLiteConnect != null;
        }

        ///// <summary>
        ///// 指定表是否存在
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public bool IsTableExist<T>() where T : BaseTableData, new()
        //{
        //    if (!IsConnected())
        //    {
        //        Debug.LogError($"数据库:{DatabaseName}未连接，访问类名:{typeof(T).Name}表是否存在失败!");
        //        return false;
        //    }
        //    // TODO: 找到方法判定是否存在
        //    return false;
        //}

        /// <summary>
        /// 创建指定数据库表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool CreateTable<T>() where T : BaseTableData, new()
        {
            if(!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，创建类名:{typeof(T).Name}表失败!");
                return false;
            }
            //if(!IsTableExist<T>())
            //{
            //    Debug.LogError($"数据库:{DatabaseName}，类名:{typeof(T).Name}表已存在，请勿重复创建!");
            //    return false;
            //}
            mSQLiteConnect.CreateTable<T>();
            return true;
        }

        /// <summary>
        /// 插入指定int主键的表数据(如果表不存在则创建表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool InsertDataI<T>(T data) where T : BaseIntTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            mSQLiteConnect.Insert(data);
            return true;
        }

        /// <summary>
        /// 插入指定int主键的表数据(如果表不存在则创建表,如果主键存在则覆盖更新)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool InsertOrReplaceDataI<T>(T data) where T : BaseIntTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            mSQLiteConnect.InsertOrReplace(data);
            return true;
        }

        /// <summary>
        /// 插入指定int主键的表数据(如果表不存在则创建表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool InsertDataS<T>(T data) where T : BaseStringTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            mSQLiteConnect.Insert(data);
            return true;
        }

        /// <summary>
        /// 插入指定string主键的表数据(如果表不存在则创建表,如果主键存在则覆盖更新)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool InsertOrReplaceDataS<T>(T data) where T : BaseStringTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            mSQLiteConnect.InsertOrReplace(data);
            return true;
        }

        /// <summary>
        /// 删除指定UID的表数据(整形主键)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool DeleteDataByUIDI<T>(int uid) where T : BaseIntTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，删除类名:{typeof(T).Name}表UID:{uid}数据失败!");
                return false;
            }
            var deletedNumber = mSQLiteConnect.Delete<T>(uid);
            Debug.Log($"数据库:{DatabaseName}，删除类名:{typeof(T).Name}表UID:{uid}数量:{deletedNumber}");
            return deletedNumber > 0;
        }

        /// <summary>
        /// 删除指定UID的表数据(字符串主键)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool DeleteDataByUIDS<T>(string uid) where T : BaseStringTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，删除类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            var deletedNumber = mSQLiteConnect.Delete<T>(uid);
            Debug.Log($"数据库:{DatabaseName}，删除类名:{typeof(T).Name}表UID:{uid}数量:{deletedNumber}");
            return deletedNumber > 0;
        }

        /// <summary>
        /// 更新指定int主键的表数据(如果表不存在则创建表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool UpdateDataI<T>(T data) where T : BaseIntTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            var updatedNumber = mSQLiteConnect.Update(data);
            Debug.Log($"数据库:{DatabaseName}，更新类名:{typeof(T).Name}表UID:{data.UID}数量:{updatedNumber}");
            return updatedNumber > 0;
        }

        /// <summary>
        /// 更新指定string主键的表数据(如果表不存在则创建表)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool UpdateDataS<T>(T data) where T : BaseStringTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，插入类名:{typeof(T).Name}表数据失败!");
                return false;
            }
            //if (!IsTableExist<T>())
            //{
            //    CreateTable<T>();
            //}
            var updatedNumber = mSQLiteConnect.Update(data);
            Debug.Log($"数据库:{DatabaseName}，更新类名:{typeof(T).Name}表UID:{data.UID}数量:{updatedNumber}");
            return updatedNumber > 0;
        }

        /// <summary>
        /// 获取指定UID的表数据(整形主键)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataByUIDI<T>(int uid) where T : BaseIntTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，获取类名:{typeof(T).Name}表数据失败!");
                return null;
            }
            var data = mSQLiteConnect.Find<T>((databaseTable) => databaseTable.UID == uid);
            if (data == null)
            {
                Debug.LogError($"数据库:{DatabaseName}，获取类名:{typeof(T).Name}的UID:{uid}表数据不存在!");
                return null;
            }
            return data;
        }

        /// <summary>
        /// 获取指定UID的表数据(字符串主键)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataByUIDS<T>(string uid) where T : BaseStringTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，获取类名:{typeof(T).Name}表数据失败!");
                return null;
            }
            var data = mSQLiteConnect.Find<T>((databaseTable) => databaseTable.UID == uid);
            if (data == null)
            {
                Debug.LogError($"数据库:{DatabaseName}，获取类名:{typeof(T).Name}的UID:{uid}表数据不存在!");
                return null;
            }
            return data;
        }


        /// <summary>
        /// 获取指定表所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TableQuery<T> GetAllData<T>() where T : BaseTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，获取类名:{typeof(T).Name}表数据失败!");
                return null;
            }
            var tbData = mSQLiteConnect.Table<T>();
            if (tbData == null)
            {
                Debug.LogError($"数据库:{DatabaseName}，获取类名:{typeof(T).Name}表所有数据不存在!");
                return null;
            }
            return tbData;
        }

        /// <summary>
        /// 删除指定表所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteTableAllData<T>() where T : BaseTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，删除类名:{typeof(T).Name}表所有数据失败!");
                return 0;
            }
            //if (!IsTableExist<T>())
            //{
            //    Debug.LogError($"数据库:{DatabaseName}，删除类名:{typeof(T).Name}表不存在,删除所有数据失败!");
            //    return 0;
            //}
            var deletedNumber = mSQLiteConnect.DeleteAll<T>();
            Debug.Log($"数据库:{DatabaseName}，删除类名:{typeof(T).Name}表所有数据数量:{deletedNumber}");
            return deletedNumber;
        }

        /// <summary>
        /// 删除指定表
        /// </summary>
        /// <returns></returns>
        public int DeleteTable<T>() where T : BaseTableData, new()
        {
            if (!IsConnected())
            {
                Debug.LogError($"数据库:{DatabaseName}未连接，删除类名:{typeof(T).Name}表失败!");
                return 0;
            }
            var deletedNumber = mSQLiteConnect.DropTable<T>();
            Debug.Log($"数据库:{DatabaseName}，删除类名:{typeof(T).Name}表数量:{deletedNumber}");
            return deletedNumber;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void CloseDatabase()
        {
            DatabaseManager.Singleton.closeDatabase(DatabaseName);
            mSQLiteConnect = null;
        }
    }
}