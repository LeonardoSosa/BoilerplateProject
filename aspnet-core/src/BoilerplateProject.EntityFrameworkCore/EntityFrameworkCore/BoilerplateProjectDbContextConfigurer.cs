using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BoilerplateProject.EntityFrameworkCore
{
    public static class BoilerplateProjectDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BoilerplateProjectDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BoilerplateProjectDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
