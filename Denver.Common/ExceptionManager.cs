using Denver.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Denver.Common
{
    public class ExceptionManager
    {
        private string _logTo = "TextFile";
        private SqlConnection _connection;
        private string _logFilePath;       

        public ExceptionManager()
        {
            _logTo = ConfigurationManager.AppSettings["LogDirection"];

            switch (_logTo)
            {
                case "Database":
                    _connection = new SqlConnection(DbConfig.LogDbConnectionString);
                    break;
                case "TextFile":
                    _logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
                    break;
                case "WebService":
                    //TODO http://localhost/web/services/logger.asmx servisine log atmak için örneği oluşturulacak
                    break;
                default:
                    break;
            }

        }

        public void Error(string message)
        {
            WriteMessageTo(message,LogType.Error);
        }

        public void Warn(string message)
        {
            WriteMessageTo(message,LogType.Warn);
        }

        public void Info(string message)
        {
            WriteMessageTo(message, LogType.Info);
        }

        private void WriteMessageTo(string message,LogType logType)
        {
            switch (_logTo)
            {
                case "DB":
                    SqlCommand command = new SqlCommand("Insert into logtable ('Message','log_date','type') Values (@message,@log_date,@type)");
                    command.Parameters.AddWithValue("@message", message.ToString());
                    command.Parameters.AddWithValue("@log_date", DateTime.Now);
                    command.Parameters.AddWithValue("@type", logType.ToString());
                    break;
                case "TextFile":
                    File.AppendAllLines(_logFilePath, new string[] { message });
                    break;
                case "WebService":
                    //TODO http://localhost/web/services/logger.asmx servisine log atmak için gerekli kodlar
                    break;
                default:
                    break;
            }
        }
    }
}
