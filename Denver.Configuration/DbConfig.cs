using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.Configuration
{
    public static class DbConfig
    {
        public static string ConnectionString = "data source=localhost;database=DenverDb;integrated security=SSPI;";
        public static string LogDbConnectionString = "data source=localhost:database=LogDB;user id=logger;password=AlQ390!#f;";
    }
}
