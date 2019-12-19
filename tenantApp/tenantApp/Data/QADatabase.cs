using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;

namespace tenantApp.Data
{
    public class QADatabase
    {
        readonly SQLiteAsyncConnection _database;

        public QADatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<QAList>().Wait();
        }

        public Task<List<QAList>> GetNotiAsync()
        {
            return _database.Table<QAList>().OrderByDescending(a => a.ID).ToListAsync();
        }

        public Task<int> SaveNotiAsync(QAList notifcation)
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

        public Task<int> DeleteNotiAsync(QAList notifcation)
        {
            return _database.DeleteAsync(notifcation);
        }

        public Task<QAList> GetDelQAAsync(string question)
        {
            return _database.Table<QAList>()
                            .Where(i => i.question == question)
                            .FirstOrDefaultAsync();
        }
    }
}
