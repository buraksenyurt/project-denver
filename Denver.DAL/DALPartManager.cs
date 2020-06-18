using System;
using System.Data.SqlClient;
using Denver.Common;
using Denver.Configuration;

namespace Denver.DAL
{
    public class DALPartManager
    {
        public RetCode AddNewPart(int code, int number, double price, string name, int quantity, string supplier, string description)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("INSERT INTO Parts (code,number,price,name,quantity,supplier,description) VALUES ("+code+","+number+","+price+",'"+name+"',"+quantity+"',"+supplier+"','"+description+"')");
            sqlConnection.Open();
            int inserted=command.ExecuteNonQuery();
            sqlConnection.Close();
            if (inserted >= 1)
                return RetCode.Success;
            else
                return RetCode.Fail;

        }
    }
}