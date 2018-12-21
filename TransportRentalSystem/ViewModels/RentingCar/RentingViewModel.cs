using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Models.DataBaseModels;
using TransportRentalSystem.ViewModels.JsonShare;

namespace TransportRentalSystem.ViewModels.RentingCar
{
    /// <summary>
    /// Хранит в себе все данные для создания брони
    /// </summary>
    public class RentingViewModel : EventModel
    {
        public int Auto { get; set; }

        public int Driver { get; set; }

        public int PointSending { get; set; }

        public int PointArrival { get; set; }

        public int Status { get; set; }

        public StartTimeViewModel CalendarViewModel { get; set; }

        public List<Car> Autos { get; set; }

        public List<Driver> Drivers { get; set; }

        public List<TransportPoint> TransportPoints { get; set; }

        public List<Status> Statuses { get; set; }

        public RentingViewModel(): base()
        {

        }
    }
}
