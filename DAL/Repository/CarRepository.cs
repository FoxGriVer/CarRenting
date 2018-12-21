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
    /// Работа с таблицей автомашин 
    /// </summary>
    public class CarRepository: IRepository<Car>
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
        public CarRepository()
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

        public List<Car> Data
        {
            get
            {
                return this.GetManyObjects().ToList();
            }
        }

        /// <summary>
        /// Получения Storage
        /// </summary>
        public List<Car> Storage
        {
            get
            {
                var drivers = HttpContext.Current.Session["Cars"];

                if (drivers == null || !(drivers is List<Driver>))
                {
                    drivers = this.Data;
                    HttpContext.Current.Session["Cars"] = drivers;
                }

                return (List<Car>)drivers;
            }

            set
            {
                HttpContext.Current.Session["Cars"] = value;
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
        public Car GetOneObject(int id)
        {
            log.WriteDebug("CarRepository.GetOneObject--->");
            using (IDbConnection db = GetContext().Connection)
            {
                string sql = "SELECT * FROM CARS WHERE CAR_ID = '" + id + "'";
                log.WriteDebug("_SQL=" + sql);
                Car car = db.Query<Car>(sql).FirstOrDefault();
                return car;
            }

        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="car"></param>
        public void UpdateObject(Car objectDB)
        {

            lock (lockObj)
            {
                using (IDbConnection db = context.Connection)
                {
                    var sqlQuery = "UPDATE CARS SET MARK = '" + objectDB.MARK + "', CAR_TYPE = '" + objectDB.CAR_TYPE + "', MODEL = '" + objectDB.MODEL + "', GOVERNMENT_NUMBER = '" + objectDB.GOVERNMENT_NUMBER + "' WHERE CAR_ID = '" + objectDB.CAR_ID + "'";
                    db.Execute(sqlQuery, objectDB);
                }

                var cars = this.Storage;
                Car updatingDriver = null;

                foreach (Car p in cars)
                {
                    if (p.CAR_ID == objectDB.CAR_ID)
                    {
                        updatingDriver = p;
                        break;
                    }
                }

                if (updatingDriver == null)
                {
                    throw new Exception("Объект не найден.");
                }

                updatingDriver.CAR_TYPE = objectDB.CAR_TYPE;
                updatingDriver.GOVERNMENT_NUMBER = objectDB.GOVERNMENT_NUMBER;
                updatingDriver.MARK = objectDB.MARK;
                updatingDriver.MODEL = objectDB.MODEL;

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
                    Car driver = null;

                    foreach (Car p in drivers)
                    {
                        if (p.CAR_ID == id)
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


                    var sqlQuery = "DELETE FROM  CARS  WHERE CAR_ID = '" + id + "'";
                    db.Execute(sqlQuery, new { id });
                }
            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="car"></param>
        public void InsertObject(Car car)
        {
            /*   using (IDbConnection db = context.Connection)
               {
                   var sqlQuery = "INSERT INTO CARS (MARK,MODEL,GOVERNMENT_NUMBER,CAR_TYPE) VALUES('" + car.MARK + "','" + car.MODEL + "','" + car.GOVERNMENT_NUMBER + "','" + car.CAR_TYPE + "')";
                   int? carId = db.Query<int>(sqlQuery, car).FirstOrDefault();
                   car.CAR_ID = (int)carId;
                   db.Execute(sqlQuery, car);
               }*/

            lock (lockObj)
            {
                OracleConnection conn = new OracleConnection(context.getCnnectionString);
                conn.Open();
                OracleCommand cmd = new OracleCommand("INSERT_CARS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter returnVal = new OracleParameter("pk", null);
                returnVal.OracleDbType = OracleDbType.Int32;
                returnVal.DbType = DbType.Int32;
                returnVal.Direction = ParameterDirection.ReturnValue;

                OracleParameter p_one = new OracleParameter("vMARK_in", car.MARK);
                p_one.OracleDbType = OracleDbType.NVarchar2;
                p_one.DbType = DbType.String;
                p_one.Direction = ParameterDirection.Input;

                OracleParameter p_two = new OracleParameter("vMODEL_in", car.MODEL);
                p_two.OracleDbType = OracleDbType.NVarchar2;
                p_two.DbType = DbType.String;
                p_two.Direction = ParameterDirection.Input;


                OracleParameter p_3 = new OracleParameter("vGOVERNMENT_NUMBER_in", car.GOVERNMENT_NUMBER);
                p_3.OracleDbType = OracleDbType.NVarchar2;
                p_3.DbType = DbType.String;
                p_3.Direction = ParameterDirection.Input;


                OracleParameter p_4 = new OracleParameter("vCAR_TYPE_in", car.CAR_TYPE);
                p_4.OracleDbType = OracleDbType.NVarchar2;
                p_4.DbType = DbType.String;
                p_4.Direction = ParameterDirection.Input;



                cmd.Parameters.Add(returnVal);
                cmd.Parameters.Add(p_one);
                cmd.Parameters.Add(p_two);

                cmd.Parameters.Add(p_3);
                cmd.Parameters.Add(p_4);

                cmd.ExecuteNonQuery();


                int driverId = 0;

                if (returnVal.Value != null)
                {
                    driverId = (int)returnVal.Value;
                }


                car.CAR_ID = driverId;

                var cars = this.Storage;
                cars.Add(car);
                this.Storage = cars;
            }


        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Car> GetManyObjects()
        {
            log.WriteDebug("CarRepository.GetManyObjects--->");
            IEnumerable<Car> cars;
            using (IDbConnection db = GetContext().Connection)
            {
                db.Open();
                try
                {
                    var selectFromCar = "SELECT * FROM CARS";
                    log.WriteDebug("_SQL=" + selectFromCar);
                    cars = db.Query<Car>(selectFromCar).ToList();
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

                return cars;
            }
        }
    }
}
