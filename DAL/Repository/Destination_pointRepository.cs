using DAL.Context;
using Dapper;
using Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.Repository
{
    /// <summary>
    /// Класс для работы с табицей: точки назначения
    /// </summary>
    class Destination_pointRepository
    {

        private OracleContext context;

        public Destination_pointRepository()
        {
            context = OracleContext.getContext();
        }

        public TransportPoint GetOneObject(int id)
        {
            using (IDbConnection db = context.Connection)
            {
                TransportPoint tr_point = db.Query<TransportPoint>("SELECT * FROM DESTINATION_POINT WHERE DESTINATION_POINT_ID = '" + id + "'").FirstOrDefault();

                return tr_point;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tr_point"></param>
        public void UpdateObject(TransportPoint tr_point)
        {
            using (IDbConnection db = context.Connection)
            {
                var sqlQuery = "UPDATE DESTINATION_POINT SET TEXT = '" + tr_point.TEXT + "'' WHERE DESTINATION_POINT_ID = '" + tr_point.DESTINATION_POINT_ID + "'";
                db.Execute(sqlQuery, tr_point);
            }
        }

        public void DeleteObject(int id)
        {
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "DELETE * FROM  DESTINATION_POINT  WHERE DESTINATION_POINT_ID = '" + id + "'";
                db.Execute(sqlQuery, new { id });


            }
        }

        public void InsertObject(TransportPoint tr_point)
        {
            using (IDbConnection db = context.Connection)
            {

                var sqlQuery = "INSERT INTO DESTINATION_POINT (TEXT) VALUES('" + tr_point.TEXT + "')";
                int? tr_pointId = db.Query<int>(sqlQuery, tr_point).FirstOrDefault();
                tr_point.DESTINATION_POINT_ID = (int)tr_pointId;
                db.Execute(sqlQuery, tr_point);


            }
        }

        public IEnumerable<Active> GetManyObjects()
        {
            using (IDbConnection db = context.Connection)
            {
                db.Open();
                IEnumerable<Active> actives = db.Query<Active>("SELECT * FROM DESTINATION_POINT").ToList();
                db.Close();
                return actives;
            }
        }
    }
}
