using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetManyObjects();
        T GetOneObject(int id);
        void UpdateObject(T item);
        void DeleteObject(int id);
        void InsertObject(T item);
    }
}
