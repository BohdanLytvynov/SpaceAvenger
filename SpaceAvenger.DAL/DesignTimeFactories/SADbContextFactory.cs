using SpaceAvenger.DAL.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace SpaceAvenger.DAL.DesignTimeFactories
{
    internal class SADbContextFactory : IDesignTimeDbContextFactory<SADataContext>
    {
        public SADataContext CreateDbContext(string[] args)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\blytv\C#\SpaceAvenger\SpaceAvenger\bin\Debug\net9.0-windows\Database\Local.mdf;Integrated Security=True;Connect Timeout=30";

            return new SADataContext(connectionString);
        }
    }
}

