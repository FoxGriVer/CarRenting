using DAL.Context;
using Dapper;
using Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logs;
using DAL.Interfaces;

namespace DAL.Repository
{
    /// <summary>
    /// Работа с таблицей 
    /// </summary>
    public class UserRepository: IRepository<User>
    {
        /// <summary>
        /// Класс логирования
        /// </summary>
        private LogTools log;

        /// <summary>
        /// Oracle конекст
        /// </summary>
        public OracleContext context;

        /// <summary>
        /// Получить OracleContext
        /// </summary>
        public UserRepository()
        {
            GetContext();
            log = new LogTools();
        }

        /// <summary>
        /// Получение конетекста
        /// </summary>
        /// <returns></returns>
        public OracleContext GetContext()
        {
            if (context == null)
            {
                context = OracleContext.getContext();
            }

            return context;
        }

        /// <summary>
        /// Получить один элемент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetOneObject(int id)
        {
            log.WriteDebug("UserRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM USERS WHERE USER_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                User user = db.Query<User>(sql).FirstOrDefault();
                return user;
            }

        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="user"></param>
        public void UpdateObject(User user)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "UPDATE USERS SET USER_ID = '" + user.USER_ID + "', FIO = '" + user.FIO + "', LOGIN = '" + user.LOGIN + "', PASSWORD = '" + user.PASSWORD + "'";
                db.Execute(sqlQuery, user);
            }
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="id"></param>
        public void DeleteObject(int id)
        {
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "DELETE * FROM  USERS  WHERE USER_ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });


            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="user"></param>
        public void InsertObject(User user)
        {
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "INSERT INTO USERS (FIO,LOGIN,PASSWORD) VALUES('" + user.FIO + "','" + user.LOGIN + "','" + user.PASSWORD + "')";
                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.USER_ID = (int)userId;
                db.Execute(sqlQuery, user);


            }
        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetManyObjects()
        {
            log.WriteDebug("UserRepository.GetManyObjects--->");
            IEnumerable<User> users;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromUser = "SELECT * FROM USERS";
                    log.WriteDebug("_SQL=" + selectFromUser);
                    users = db.Query<User>(selectFromUser).ToList();
                }
                catch (Exception exc)
                {
                    log.WriteExceprion("ERROR=" + exc.Message, exc);
                    throw exc;
                }
                finally
                {
                    db.Close();
                }

                return users;
            }

        }


    }
}
