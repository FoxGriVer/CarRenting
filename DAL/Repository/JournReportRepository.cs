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
    public class JournReportRepository
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
        public JournReportRepository()
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
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JournalReport> GetManyObjects()
        {
            log.WriteDebug("StatusRepository.GetManyObjects--->");
            IEnumerable<JournalReport> journs;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromJournal = "SELECT * FROM VJOURNAL";
                    log.WriteDebug("_SQL=" + selectFromJournal);
                    journs = db.Query<JournalReport>(selectFromJournal).ToList();
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
