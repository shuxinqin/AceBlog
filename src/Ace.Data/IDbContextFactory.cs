using Chloe;
using Chloe.Infrastructure.Interception;
using Chloe.SQLite;
using Chloe.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Data
{
    public interface IDbContextFactory
    {
        IDbContext CreateContext();
        IDbContext CreateContext(string connString);
    }

    public class DefaultDbContextFactory : IDbContextFactory
    {
        public DefaultDbContextFactory(string dbType, string connString)
        {
            this.DbType = dbType;
            this.ConnString = connString;
        }

        public string DbType { get; private set; }
        public string ConnString { get; private set; }

        public IDbContext CreateContext()
        {
            return this.CreateContext(this.ConnString);
        }
        public IDbContext CreateContext(string connString)
        {
            IDbContext dbContext = null;

            var dbType = this.DbType == null ? "" : this.DbType.ToLower();

            switch (dbType)
            {
                case "sqlite":
                    dbContext = CreateSQLiteContext(connString);
                    break;
                case "sqlserver":
                    dbContext = CreateSqlServerContext(connString);
                    break;
                default:
                    dbContext = CreateSqlServerContext(connString);
                    break;
            }

            IDbCommandInterceptor interceptor = new DbCommandInterceptor();
            dbContext.Session.AddInterceptor(interceptor);

            return dbContext;
        }

        static IDbContext CreateSqlServerContext(string connString)
        {
            MsSqlContext dbContext = new MsSqlContext(connString);
            //dbContext.PagingMode = PagingMode.OFFSET_FETCH;
            return dbContext;
        }
        static IDbContext CreateSQLiteContext(string connString)
        {
            SQLiteContext dbContext = new SQLiteContext(new SQLiteConnectionFactory(connString));
            return dbContext;
        }
    }
}
