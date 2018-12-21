using DAL.Context;
using Models.DataBaseModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Web;
using System;
using Oracle.ManagedDataAccess.Client;
using DAL.Interfaces;

namespace DAL.Repository
{
    using Utilities.Logs;

    /// <summary>
    /// Класс для работы с табицей: точки назначения
    /// </summary>
    public class DestinationPointRepository: IRepository<TransportPoint>
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
        /// Объект для блокировки
        /// </summary>
        private static object lockObj = new object();

        /// <summary>
        /// Получить OracleContext
        /// </summary>
        public DestinationPointRepository()
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

        public List<TransportPoint> Data
        {
            get
            {
                return this.GetManyObjects().ToList();
            }
        }

        /// <summary>
        /// Получения Storage
        /// </summary>
        public List<TransportPoint> Storage
        {
            get
            {
                var drivers = HttpContext.Current.Session["TransportPoint"];

                if (drivers == null || !(drivers is List<Driver>))
                {
                    drivers = this.Data;
                    HttpContext.Current.Session["TransportPoint"] = drivers;
                }

                return (List<TransportPoint>)drivers;
            }

            set
            {
                HttpContext.Current.Session["TransportPoint"] = value;
            }
        }


        /// <summary>
        /// Очистка Storage
        /// </summary>
        public void Clear()
        {
            this.Storage = null;
        }

        /// <summary>
        /// Получить один элемент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TransportPoint GetOneObject(int id)
        {
            log.WriteDebug("DestinationPointRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM DESTINATION_POINT WHERE DESTINATION_POINT_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                TransportPoint tr_point = db.Query<TransportPoint>(sql).FirstOrDefault();
                return tr_point;
            }
        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="tr_point"></param>
        public void UpdateObject(TransportPoint objectDB)
        {
            lock (lockObj)
            {
                using (IDbConnection db = context.Connection)
                {
                    var sqlQuery = "UPDATE DESTINATION_POINT SET TEXT = '" + objectDB.TEXT
                                                                           + "'' WHERE DESTINATION_POINT_ID = '"
                                                                           + objectDB.DESTINATION_POINT_ID + "'";
                    db.Execute(sqlQuery, objectDB);
                }

                var cars = this.Storage;
                TransportPoint updatingDriver = null;

                foreach (TransportPoint p in cars)
                {
                    if (p.DESTINATION_POINT_ID == objectDB.DESTINATION_POINT_ID)
                    {
                        updatingDriver = p;
                        break;
                    }
                }

                if (updatingDriver == null)
                {
                    throw new Exception("Объект не найден.");
                }

                updatingDriver.TEXT = objectDB.TEXT;

                this.Storage = cars;
            }
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="id"></param>
        public void DeleteObject(int id)
        {

            lock (lockObj)
            {
                using (IDbConnection db = context.Connection)
                {


                    var drivers = this.Storage;
                    TransportPoint driver = null;

                    foreach (TransportPoint p in drivers)
                    {
                        if (p.DESTINATION_POINT_ID == id)
                        {
                            driver = p;
                            break;
                        }
                    }

                    if (driver == null)
                    {
                        throw new Exception("Объект не найден.");
                    }

                    drivers.Remove(driver);
                    this.Storage = drivers;


                    var sqlQuery = "DELETE FROM  DESTINATION_POINT  WHERE DESTINATION_POINT_ID = '" + id + "'";
                    db.Execute(sqlQuery, new { id });
                }
            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="tr_point"></param>
        public void InsertObject(TransportPoint objectDB)
        {
            /*using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "INSERT INTO DESTINATION_POINT (TEXT) VALUES('" + objectDB.TEXT + "')";
                int? tr_pointId = db.Query<int>(sqlQuery, objectDB).FirstOrDefault();
                objectDB.DESTINATION_POINT_ID = (int)tr_pointId;
                db.Execute(sqlQuery, objectDB);
            }
            */


            lock (lockObj)
            {
                OracleConnection conn = new OracleConnection(context.getCnnectionString);
                conn.Open();
                OracleCommand cmd = new OracleCommand("INSERT_POINTS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter returnVal = new OracleParameter("pk", null);
                returnVal.OracleDbType = OracleDbType.Int32;
                returnVal.DbType = DbType.Int32;
                returnVal.Direction = ParameterDirection.ReturnValue;

                OracleParameter p_one = new OracleParameter("vTEXT_in", objectDB.TEXT);
                p_one.OracleDbType = OracleDbType.NVarchar2;
                p_one.DbType = DbType.String;
                p_one.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(returnVal);
                cmd.Parameters.Add(p_one);
                cmd.ExecuteNonQuery();


                int driverId = 0;

                if (returnVal.Value != null)
                {
                    driverId = (int)returnVal.Value;
                }


                objectDB.DESTINATION_POINT_ID = driverId;

                var cars = this.Storage;
                cars.Add(objectDB);
                this.Storage = cars;
            }

        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TransportPoint> GetManyObjects()
        {
            log.WriteDebug("DestinationPointRepository.GetManyObjects--->");
            IEnumerable<TransportPoint> tr_points;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromTransportPoint = "SELECT * FROM DESTINATION_POINT";
                    log.WriteDebug("_SQL=" + selectFromTransportPoint);
                    tr_points = db.Query<TransportPoint>(selectFromTransportPoint).ToList();
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

                return tr_points;
            }
        }
    }
}
