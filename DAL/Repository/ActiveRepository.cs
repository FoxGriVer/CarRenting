using DAL.Context;
using DAL.Interfaces;
using Dapper;
using Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logs;

namespace DAL.Repository
{
    /// <summary>
    /// Работа с таблицей 
    /// </summary>



    public class ActiveRepository: IRepository<Active>

    {
        /// <summary>
        /// Класс логирования
        /// </summary>
        private LogTools log;


        /// <summary>
        /// Oracle конекст
        /// </summary>
        private OracleContext context;

        /// <summary>
        /// Получить OracleContext
        /// </summary>
        public ActiveRepository()
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
        public Active GetOneObject(int id)
        {
            log.WriteDebug("ActiveRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM ACTIVE WHERE ACTIVE_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                Active active = db.Query<Active>(sql).FirstOrDefault();
                return active;
            }
        }


        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="active"></param>
        public void UpdateObject(Active active)
        {
            using (IDbConnection db = GetContext().Connection)
            {
                var sqlQuery = "UPDATE ACTIVE SET DRIVER_ID = '" + active.DRIVER_ID + "', CAR_ID = '" + active.CAR_ID + "', STATUS_ID = '" + active.STATUS_ID + "' WHERE ACTIVE_ID = '" + active.ACTIVE_ID + "'";
                db.Execute(sqlQuery, active);
            }
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="id"></param>
        public void DeleteObject(int id)
        {
            using (IDbConnection db = GetContext().Connection)
            {

                var sqlQuery = "DELETE * FROM  ACTIVE  WHERE ACTIVE_ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });


            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="active"></param>
        public void InsertObject(Active active)
        {
            using (IDbConnection db = GetContext().Connection)
            {
                var sqlQuery = "INSERT INTO ACTIVE (CAR_ID,DRIVER_ID,STATUS_ID) VALUES('" + active.CAR_ID + "','" + active.DRIVER_ID + "','" + active.STATUS_ID + "')";
                int? activeId = db.Query<int>(sqlQuery, active).FirstOrDefault();
                active.ACTIVE_ID = (int)activeId;
                db.Execute(sqlQuery, active);

            }
        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Active> GetManyObjects()
        {
            log.WriteDebug("ActiveRepository.GetManyObjects--->");
            IEnumerable<Active> actives;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromActive = "SELECT * FROM ACTIVE";
                    log.WriteDebug("_SQL=" + selectFromActive);
                    actives = db.Query<Active>(selectFromActive).ToList();
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

                return actives;
            }
        }

    }
}
