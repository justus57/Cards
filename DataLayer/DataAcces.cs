using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataAccess //: IDataAccess
    {
        private readonly CARDEntities1 _db ;
     

        /// <summary>
        /// Initializes a new instance of the DataRepository class
        /// </summary>
        public DataAccess(string conn)
        {
            _db = new CARDEntities1(conn);
        }

        /// <summary>
        /// Fetches the entire class set as an IQueryable
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <returns>An IQueryable of the class</returns>
        public IQueryable<T> Fetch<T>() where T : class
        {
            return _db.CreateObjectSet<T>().AsQueryable();
        }

        /// <summary>
        /// Used to query a result set from the class set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing all the filter criteria</param>
        /// <returns>An IQueryable of the class</returns>
        public IQueryable<T> Search<T>(Func<T, bool> predicate) where T : class
        {
            return _db.CreateObjectSet<T>().Where(predicate).AsQueryable();
        }


        /// <summary>
        /// Returns the single object matching the criteria specified
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing all the filter criteria</param>
        /// <returns>A single oject of the class</returns>
        public T Single<T>(Func<T, bool> predicate) where T : class
        {
            return _db.CreateObjectSet<T>().SingleOrDefault(predicate);
        }


        public IQueryable<T> SearchXp<T>(Expression<Func<T, bool>> predicate) where T : class
        {

            return _db.CreateObjectSet<T>().Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Returns the first oject matching the criteria specified
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing all the filter criteria</param>
        /// <returns>The first oject of the class matching the criteria</returns>
        public T First<T>(Func<T, bool> predicate) where T : class
        {
            return _db.CreateObjectSet<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Saves a single object to the database
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="entity">The object to be saved to the database</param>
        public void Save<T>(T entity) where T : class
        {
            EntityKey key = _db.CreateEntityKey(typeof(T).Name, entity);
            object o = null;
            _db.TryGetObjectByKey(key, out o);
            if (o == null)
                _db.CreateObjectSet<T>().AddObject(entity);
            else
                _db.ApplyCurrentValues(typeof(T).Name, entity);
            SaveChanges();
        }

        /// <summary>
        /// Saves multiple new objects in the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="entities">A list of objects to be saved</param>
        public void Save<T>(List<T> entities) where T : class
        {
            foreach (T entity in entities)
            {
                EntityKey key = _db.CreateEntityKey(typeof(T).Name, entity);
                object o = null;
                _db.TryGetObjectByKey(key, out o);
                if (o == null)
                    _db.CreateObjectSet<T>().AddObject(entity);
                else
                    _db.ApplyCurrentValues(typeof(T).Name, entity);
            }
            SaveChanges();
        }

        /// <summary>
        /// Gets the maximum object from the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field</param>
        /// <returns>The maximum oject of the class matching the criteria</returns>
        public T Max<T>(Func<T, bool> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum object from the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field</param>
        /// <returns>The minimum oject of the class matching the criteria</returns>
        public T Min<T>(Func<T, bool> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if any object matching exists in the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field(s)</param>
        /// <returns>True if an object is found, false otherwise</returns>
        public bool Exists<T>(Func<T, bool> predicate) where T : class
        {
            return _db.CreateObjectSet<T>().Any(predicate);
        }

        /// <summary>
        /// Get the count of items in the set matching the criteria
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field(s)</param>
        /// <returns>Total number of items matching the criteria</returns>
        public int Count<T>(Func<T, bool> predicate) where T : class
        {
            return _db.CreateObjectSet<T>().Count(predicate);
        }

        /// <summary>
        /// Get the sum of items in the set matching the criteria
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field(s)</param>
        /// <param name="field">The filed for which the sum will be computed</param>
        /// <returns>Sum of items matching the criteria</returns>
        public decimal Sum<T>(Func<T, bool> predicate, Func<T, decimal> field) where T : class
        {
            return _db.CreateObjectSet<T>().Where(predicate).Sum(field);
        }

        /// <summary>
        /// Deleted an object from the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="entity">The object to be deleted to the database</param>
        public void Delete<T>(T entity) where T : class
        {

            _db.CreateObjectSet<T>().DeleteObject(entity);
        }

        /// <summary>
        /// Deletes all objects that match the criteria from the set
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="predicate">A predicate construct containing criteria field(s)</param>
        public void Delete<T>(Func<T, bool> predicate) where T : class
        {
            foreach (T entity in Search<T>(predicate))
                _db.CreateObjectSet<T>().DeleteObject(entity);
            SaveChanges();
        }

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Saves all changes to the database based on the specified options
        /// </summary>
        /// <param name="options">The database save options to be applied</param>
        public void SaveChanges(SaveOptions options)
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Used to query a result set from the class set by executing a direct SQL query
        /// </summary>
        /// <typeparam name="T">The class type in context</typeparam>
        /// <param name="sQuery">An SQL statement to be executed</param>
        /// <returns>An IQueryable of the class</returns>
        public IQueryable<T> Exec<T>(string sQuery) where T : class
        {
            return _db.ExecuteStoreQuery<T>(sQuery).AsQueryable();
        }

        /// <summary>
        /// Executes the specified non-scalar query
        /// </summary>
        /// <param name="sQuery">An SQL statement to be executed</param>
        public void Exec(string sQuery)
        {
            _db.ExecuteStoreCommand(sQuery, null);
        }

        public void Exec(string sQuery, SqlParameter[] param)
        {
            _db.ExecuteStoreCommand(sQuery, param);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                    _db.Dispose();
            }
        }








    }
}
