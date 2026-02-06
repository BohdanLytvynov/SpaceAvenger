using SpaceAvenger.DAL.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace SpaceAvenger.DAL.DesignTimeFactories
{
    internal class SADbContextFactory : IDesignTimeDbContextFactory<SADataContext>
    {
        public SADataContext CreateDbContext(string[] args)
        {
            string pathToExe = AppContext.BaseDirectory;
            string pathToFolder = pathToExe + "Database";
            
            if(!Directory.Exists(pathToFolder))
                Directory.CreateDirectory(pathToFolder);

            string pathToFile = pathToFolder + Path.DirectorySeparatorChar
                + "Game.db";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Data base file will be located at : \n{pathToFile}");
            Console.ResetColor();

            return new SADataContext($"Data Source={pathToFile}");
        }
    }
}

