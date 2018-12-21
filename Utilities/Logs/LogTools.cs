using System;
using System.Text;
using NLog;

/// <summary>
/// Класс логирования
/// </summary>
namespace Utilities.Logs
{
    /// <summary>
    ///  Класс для логирования.
    /// В приложении необходимо установить пакеты: NLog и NLog.Config
    /// </summary>
    public class LogTools
    {
        protected string logName = "debuglogs";
        protected string df_dateonly_full = "dd.MM.yyyy HH:mm:ss,SSS";
        protected Logger log;

        /// <summary>
        ///            Получение базового лога
        /// </summary>
        /// <returns></returns>
        protected Logger getLog()
        {
            if (log == null)
            {
                log = LogManager.GetCurrentClassLogger();
            }
            return log;
        }


        /// <summary>
        /// Получение лога с заданными именем
        /// </summary>
        /// <param name="logNameIn"></param>
        /// <returns></returns>
        protected Logger getLog(String logNameIn)
        {
            log = LogManager.GetLogger(logNameIn);
            return log;
        }


        /// <summary>
        /// Создание debuglogs лога 
        /// </summary>
        public LogTools()
        {
            logName = "debuglogs";
        }

        /// <summary>
        /// Создание лога с базовым контекстом
        /// </summary>
        /// <param name="logNameIn"></param>
        public LogTools(String logNameIn)
        {
            getLog(logNameIn);
            logName = logNameIn;
        }

        /// <summary>
        /// Текущая дата время в формате dd.MM.yyyy HH:mm:ss
        /// </summary>
        /// <returns></returns>
        protected String getDate()
        {
            DateTime currentDate = DateTime.Now;

            return currentDate.ToString(df_dateonly_full);
        }


        /// <summary>
        /// Запись информации в лог + дата + текущий пользователь
        /// </summary>
        /// <param name="str"></param>
        protected void WriteLn(Object str)
        {
            StringBuilder sb = getStr(str);

            getLog().Info(sb.ToString());
        }

        /// <summary>
        /// Скрипт генерации строки записи в лог
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private StringBuilder getStr(Object str)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(getDate());
            //sb.Append(" ");
            sb.Append(str);
            return sb;
        }

        /// <summary>
        /// Запись в лог отладки
        /// </summary>
        /// <param name="str">Текст ошибки</param>
        public void WriteDebug(Object str)
        {
            StringBuilder sb = getStr(str);

            getLog().Info(sb.ToString());
        }

        /// <summary>
        /// Запись ошибки
        /// </summary>
        /// <param name="str">Текст ошибки</param>
        /// <param name="err">Exception</param>
        public void WriteExceprion(Object str, Exception err)
        {
            StringBuilder sb = getStr(str);
            getLog().Error(err, sb.ToString());


            /*
                        logger.Trace("trace message");
                        logger.Debug("debug message");
                        logger.Info("info message");
                        logger.Warn("warn message");
                        logger.Error("error message");
                        logger.Fatal("fatal message");
                        */
        }

    }

}
