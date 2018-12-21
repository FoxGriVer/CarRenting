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
    public class JournOfAccountingRepository:IRepository<Journal>
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
        public JournOfAccountingRepository()
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
        public Journal GetOneObject(int id)
        {
            log.WriteDebug("JournOfAccountingRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM JOURNAL_OF_ACCOUNTING WHERE JOURNAL_OF_ACCOUNTING_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                Journal journ = db.Query<Journal>(sql).FirstOrDefault();
                return journ;
            }
        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="journ"></param>
        public void UpdateObject(Journal journ)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "UPDATE JOURNAL_OF_ACCOUNTING SET FIO_DECLARANT = '" + journ.FIO_DECLARANT + "',CAR_ID = '" + journ.CAR_ID + "',DRIVER_ID = '" + journ.DRIVER_ID + "',DESTINATION_POINT_SENDING_ID = '" + journ.DESTINATION_POINT_SENDING_ID + "',DESTINATION_POINT_ARRIVAL_ID = '" + journ.DESTINATION_POINT_ARRIVAL_ID + "',DEPARTURE_TIME = '" + journ.DEPARTURE_TIME + "',ARRIVAL_TIME = '" + journ.ARRIVAL_TIME + "',STATUS_ID = '" + journ.STATUS_ID + "',COMMENTS = '" + journ.COMMENTS + "' WHERE JOURNAL_OF_ACCOUNTING_ID = '" + journ.JOURNAL_OF_ACCOUNTING_ID + "'";
                db.Execute(sqlQuery, journ);
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
                var sqlQuery = "DELETE * FROM  JOURNAL_OF_ACCOUNTING  WHERE JOURNAL_OF_ACCOUNTING_ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });
            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="journ"></param>
        public void InsertObject(Journal journ)
        {
            using (IDbConnection db = context.Connection)
            {
                string startDateTime = "" + journ.DEPARTURE_TIME.Year + "-" + journ.DEPARTURE_TIME.Month + "-" + journ.DEPARTURE_TIME.Day + " " + journ.DEPARTURE_TIME.Hour + ":" + journ.DEPARTURE_TIME.Minute + ":" + journ.DEPARTURE_TIME.Millisecond + "";
                string endDateTime = "" + journ.ARRIVAL_TIME.Year + "-" + journ.ARRIVAL_TIME.Month + "-" + journ.ARRIVAL_TIME.Day + " " + journ.ARRIVAL_TIME.Hour + ":" + journ.ARRIVAL_TIME.Minute + ":" + journ.ARRIVAL_TIME.Millisecond + "";

                var sqlQuery = "INSERT INTO JOURNAL_OF_ACCOUNTING (FIO_DECLARANT,CAR_ID,DRIVER_ID,DESTINATION_POINT_SENDING_ID,DESTINATION_POINT_ARRIVAL_ID,DEPARTURE_TIME,ARRIVAL_TIME,STATUS_ID,COMMENTS) VALUES"
                               + "('" + journ.FIO_DECLARANT + "'," + journ.CAR_ID + "," + journ.DRIVER_ID + "," + journ.DESTINATION_POINT_SENDING_ID + "," + journ.DESTINATION_POINT_ARRIVAL_ID + "," + "to_date('" + startDateTime + "','yyyy-mm-dd hh24:mi:ss')" + "," + "to_date('" + endDateTime + "','yyyy-mm-dd hh24:mi:ss')" + "," + journ.STATUS_ID + ",'" + journ.COMMENTS + "')";
                //int? journId = db.Query<int>(sqlQuery, journ).FirstOrDefault();
                //journ.JOURNAL_OF_ACCOUNTING_ID = (int)journId;
                db.Execute(sqlQuery, journ);
            }
        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Journal> GetManyObjects()
        {
            log.WriteDebug("StatusRepository.GetManyObjects--->");
            IEnumerable<Journal> journs;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromJournal = "SELECT * FROM JOURNAL_OF_ACCOUNTING";
                    log.WriteDebug("_SQL=" + selectFromJournal);
                    journs = db.Query<Journal>(selectFromJournal).ToList();
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

                return journs;
            }

        }

    }
}
