using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;

namespace tenantApp.Data
{
    
    public class ColumnData
    {
        readonly SQLiteAsyncConnection _database;

        public ColumnData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Columns>().Wait();
        }

        public Task<List<Columns>> GetColumnAsync()
        {
            return _database.Table<Columns>().ToListAsync();
        }

        public Task<Columns> GetDelColumnAsync(string column_id)
        {
            return _database.Table<Columns>()
                            .Where(i => i.column_id == column_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveColumnAsync(Columns column)
        {
            if (column.ID != 0)
            {
                return _database.UpdateAsync(column);
            }
            else
            {
                return _database.InsertAsync(column);
            }
        }

        public Task<int> DeletecolumnAsync(Columns column)
        {
            return _database.DeleteAsync(column);
        }

    }
}
