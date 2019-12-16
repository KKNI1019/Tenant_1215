using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;

namespace tenantApp.Data
{
    public class NoticeData
    {
        readonly SQLiteAsyncConnection _database;

        public NoticeData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Notifications>().Wait();
        }

        public Task<List<Notifications>> GetNotiAsync()
        {
            return _database.Table<Notifications>().OrderByDescending(a => a.ID).ToListAsync();
        }

        public Task<int> SaveNotiAsync(Notifications notifcation)
        {
            if (notifcation.ID != 0)
            {
                return _database.UpdateAsync(notifcation);
            }
            else
            {
                return _database.InsertAsync(notifcation);
            }
        }

        public Task<int> DeleteNotiAsync(Notifications notifcation)
        {
            return _database.DeleteAsync(notifcation);
        }
    }
}
