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

    public class StatusRepository: IRepository<Status>
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
        public StatusRepository()
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
        public Status GetOneObject(int id)
        {
            log.WriteDebug("StatusRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM STATUS WHERE STATUS_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                Status status = db.Query<Status>(sql).FirstOrDefault();
                return status;
            }
        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="status"></param>
        public void UpdateObject(Status status)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "UPDATE STATUS SET STATUS_ID = '" + status.STATUS_ID + "', TEXT = '" + status.TEXT + "'";
                db.Execute(sqlQuery, status);
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

                var sqlQuery = "DELETE * FROM  STATUS  WHERE STATUS_ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });


            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="status"></param>
        public void InsertObject(Status status)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "INSERT INTO STATUS (TEXT) VALUES('" + status.TEXT + "')";
                int? statusId = db.Query<int>(sqlQuery, status).FirstOrDefault();
                status.STATUS_ID = (int)statusId;
                db.Execute(sqlQuery, status);
            }
        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Status> GetManyObjects()
        {
            log.WriteDebug("StatusRepository.GetManyObjects--->");
            IEnumerable<Status> statuses;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromStatus = "SELECT * FROM STATUS";
                    log.WriteDebug("_SQL=" + selectFromStatus);
                    statuses = db.Query<Status>(selectFromStatus).ToList();
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

                return statuses;
            }

        }
    }
}
