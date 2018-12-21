using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Repository;

namespace DAL
{
    public class UnitOfWork: IDisposable
    {
        private OracleContext db = new OracleContext();
        private DriverRepository driverRepository;
        private StatusRepository statusRepository;
        private CarRepository carRepository;
        private DestinationPointRepository destinationPointRepository;
        private JournOfAccountingRepository journOfAccountingRepository;

        public DriverRepository Drivers
        {
            get
            {
                if (driverRepository == null)
                    driverRepository = new DriverRepository();
                return driverRepository;
            }
        }

        public StatusRepository Statuses
        {
            get
            {
                if (statusRepository == null)
                    statusRepository = new StatusRepository();
                return statusRepository;
            }
        }

        public CarRepository Cars
        {
            get
            {
                if (carRepository == null)
                    carRepository = new CarRepository();
                return carRepository;
            }
        }

        public DestinationPointRepository DestinationPoints
        {
            get
            {
                if (destinationPointRepository == null)
                    destinationPointRepository = new DestinationPointRepository();
                return destinationPointRepository;
            }
        }

        public JournOfAccountingRepository JournsOfAccounting
        {
            get
            {
                if (journOfAccountingRepository == null)
                    journOfAccountingRepository = new JournOfAccountingRepository();
                return journOfAccountingRepository;
            }
        }
        

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
