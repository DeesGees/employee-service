using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace TeamApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public ApplicationDbContext(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.Query<T>(sql, param);
        }

        public IEnumerable<T> SelectAll<T>(string sql)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.Query<T>(sql);
        }

        public T LoadSingleData<T>(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.QuerySingleOrDefault<T>(sql, param);
        }

        public int ExecuteQuery(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return connection.Execute(sql, param);        
        }
    }
}
