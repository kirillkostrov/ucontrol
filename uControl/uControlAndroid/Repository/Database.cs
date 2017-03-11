using SQLite;
using System;
using System.IO;
using System.Linq;
using uControlAndroid.Entities;

namespace uControlAndroid.Repository
{
    public class Database: IDisposable
    {
        private const string _db = "UControl.db";
        private SQLiteConnection _connection = null;

        public static void InitDatabase(string path)
        {
            using (var connection = GetConnection(path))
            {
                connection.CreateTable<GamePad>();
                connection.CreateTable<Control>();                
            }
        }

        public Database(string path)
        {
            _connection = GetConnection(path);
        }

        private static SQLiteConnection GetConnection(string path)
        {
            string _dbpath = Path.Combine(path, _db);

            return new SQLiteConnection(_dbpath);
        }

        ~Database()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        public int Add<TEntity>(TEntity item)
            where TEntity : new()
        {
            var result = _connection.Insert(item);
            return result;
        }

        public int Update<TEntity>(TEntity item)
            where TEntity : new()
        {
            var result = _connection.Update(item);
            return result;
        }

        public int Delete<TEntity>(TEntity item)
            where TEntity : new()
        {
            var result = _connection.Delete(item);
            return result;
        }

        public IQueryable<TEntity> GetAllItems<TEntity>() 
            where TEntity: new()
        {
            var result = _connection.Table<TEntity>();
            return result.AsQueryable<TEntity>();
        }
    }
}