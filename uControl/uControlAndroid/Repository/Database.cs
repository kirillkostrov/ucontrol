using SQLite;
using System.IO;
using System.Linq;
using uControlAndroid.Controls;

namespace uControlAndroid.Repository
{
    public class Database
    {
        private const string _db = "UControl.db";
        private SQLiteConnection _connection = null;

        public Database(string path)
        {
            string _dbpath = Path.Combine(path, _db);

            _connection = new SQLiteConnection(_dbpath);
            _connection.CreateTable<Control>();

        }
        ~Database()
        {
            if (_connection != null)
                _connection.Close();
        }

        public int AddNewItem(Control item)
        {
            var result = _connection.Insert(item);
            return result;
        }

        public int UpdateItem(Control item)
        {
            var result = _connection.Update(item);
            return result;
        }

        public int DeleteItem(Control item)
        {
            var result = _connection.Delete(item);
            return result;
        }

        public IQueryable<Control> GetAllItems()
        {
            var result = _connection.Table<Control>();
            return result.AsQueryable<Control>();
        }
    }
}