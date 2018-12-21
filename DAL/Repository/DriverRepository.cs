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
    /// <summary>
    /// Работа с таблицей автомашин 
    /// </summary>
    public class DriverRepository: IRepository<Driver>
    {
        /// <summary>
        /// Oracle конекст
        /// </summary>
        public OracleContext context;

        /// <summary>
        /// Объект для блокировки
        /// </summary>
        private static object lockObj = new object();

        public DriverRepository()
        {
            context = OracleContext.getContext();
        }

        /// <summary>
        /// Получить один элемент
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Driver GetOneObject(int id)
        {
            using (IDbConnection db = context.Connection)
            {                
                Driver driver = db.Query<Driver>("SELECT * FROM DRIVERS WHERE DRIVER_ID = '" + id + "'").FirstOrDefault();
                return driver;
            }
        }

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="car"></param>
        public void UpdateObject(Driver driver1)
        {
            lock (lockObj)
            {
                using (IDbConnection db = context.Connection)
                {
                    var sqlQuery = "UPDATE DRIVERS SET EMAIL = '" + driver1.Email + "', FIO = '" + driver1.FIO + "' WHERE DRIVER_ID = '" + driver1.DRIVER_ID + "'";
                    db.Execute(sqlQuery, driver1);
                }

                var drivers = this.Storage;
                Driver updatingDriver = null;

                foreach (Driver p in drivers)
                {
                    if (p.DRIVER_ID == driver1.DRIVER_ID)
                    {
                        updatingDriver = p;
                        break;
                    }
                }

                if (updatingDriver == null)
                {
                    throw new Exception("Водитель не найден.");
                }

                updatingDriver.FIO = driver1.FIO;
                updatingDriver.Email = driver1.Email;

                this.Storage = drivers;
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
                    Driver driver = null;

                    foreach (Driver p in drivers)
                    {
                        if (p.DRIVER_ID == id)
                        {
                            driver = p;
                            break;
                        }
                    }

                    if (driver == null)
                    {
                        throw new Exception("Водитель не найден.");
                    }

                    drivers.Remove(driver);
                    this.Storage = drivers;


                    var sqlQuery = "DELETE FROM DRIVERS WHERE DRIVER_ID = '" + id + "'";
                    db.Execute(sqlQuery, new { id });
                }
            }
        }

        /// <summary>
        /// Добавить элемент
        /// </summary>
        /// <param name="car"></param>
        public void InsertObject(Driver driver1)
        {
            lock (lockObj)
            {
                OracleConnection conn = new OracleConnection(context.getCnnectionString);
                conn.Open();
                OracleCommand cmd = new OracleCommand("INSERT_DRIVERS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter returnVal = new OracleParameter("pk", null);
                returnVal.OracleDbType = OracleDbType.Int32;
                returnVal.DbType = DbType.Int32;
                returnVal.Direction = ParameterDirection.ReturnValue;

                OracleParameter p_one = new OracleParameter("vFIO_in", driver1.FIO);
                p_one.OracleDbType = OracleDbType.NVarchar2;
                p_one.DbType = DbType.String;

                OracleParameter p_two = new OracleParameter("vEMAIL_in", driver1.Email);
                p_two.OracleDbType = OracleDbType.NVarchar2;
                p_two.DbType = DbType.String;

                p_one.Direction = ParameterDirection.Input;
                p_two.Direction = ParameterDirection.Input;


                cmd.Parameters.Add(returnVal);
                cmd.Parameters.Add(p_one);
                cmd.Parameters.Add(p_two);

                cmd.ExecuteNonQuery();


                int driverId = 0;

                if (returnVal.Value != null)
                {
                    driverId = (int)returnVal.Value;
                }


                driver1.DRIVER_ID = driverId;

                var drivers = this.Storage;
                drivers.Add(driver1);
                this.Storage = drivers;
            }
        }

        /// <summary>
        /// Получить все элементы из таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Driver> GetManyObjects()
        {
            using (IDbConnection db = context.Connection)
            {
                db.Open();
                IEnumerable<Driver> drivers = db.Query<Driver>("SELECT * FROM \"DRIVERS\" ").ToList();
                db.Close();
                return drivers;
            }
        }

        public List<Driver> Data
        {
            get
            {
                return this.GetManyObjects().ToList();
            }
        }

        /// <summary>
        /// Получения Storage
        /// </summary>
        public List<Driver> Storage
        {
            get
            {
                var drivers = HttpContext.Current.Session["Drivers"];

                if (drivers == null || !(drivers is List<Driver>))
                {
                    drivers = this.Data;
                    HttpContext.Current.Session["Drivers"] = drivers;
                }

                return (List<Driver>)drivers;
            }

            set
            {
                HttpContext.Current.Session["Drivers"] = value;
            }
        }

        /// <summary>
        /// Очистка Storage
        /// </summary>
        public void Clear()
        {
            this.Storage = null;
        }
    }
}
