using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;

namespace tenantApp.Data
{
    public class Th_CommentData
    {
        readonly SQLiteAsyncConnection _database;

        public Th_CommentData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Th_comments>().Wait();
        }

        public Task<List<Th_comments>> GetTh_commentAsync(string selected_th_id)
        {
            return _database.Table<Th_comments>()
                .Where(i => i.Th_id == selected_th_id)
                .ToListAsync();
        }

        public Task<Th_comments> GetSelectedTh_commentAsync(string th_comment_id)
        {
            return _database.Table<Th_comments>()
                            .Where(i => i.Th_comment_id == th_comment_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTh_CommentAsync(Th_comments th_comment)
        {
            if (th_comment.ID != 0)
            {
                return _database.UpdateAsync(th_comment);
            }
            else
            {
                return _database.InsertAsync(th_comment);
            }
        }

        public Task<int> DeleteTh_commentAsync(Th_comments th_comment)
        {
            return _database.DeleteAsync(th_comment);
        }
    }
}
