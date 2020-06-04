using DomoCore.Engine.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Data
{
    public class InMemDbContextFactory : IDisposable
    {
        private DbConnection connection;

        public InMemDbContextFactory()
        {
        }

        private DbContextOptions<DomoCoreInMemDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<DomoCoreInMemDbContext>().UseSqlite(connection).Options;
        }


        public DomoCoreInMemDbContext CreateContext()
        {
            if (connection == null)
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var options = CreateOptions();
                using (var context = new DomoCoreInMemDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

            }

            return new DomoCoreInMemDbContext(CreateOptions());
        }


        public void Dispose()
        {
            connection?.Dispose();
            connection = null;
        }
    }
}
