using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.Configuration
{
    public static class DbConfig
    {
        public static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\project-denver\Database\DenverDb.mdf;Integrated Security = True; Connect Timeout = 30";
        public static string LogDbConnectionString = "data source=localhost:database=LogDB;user id=logger;password=AlQ390!#f;";
    }
}
