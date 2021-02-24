using Denver.Common;
using Denver.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALPartManager
    {
        public RetCode AddNewPart(int code, int number, decimal price,int stockCount, string name, int quantity, string supplier, string description,int userId)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("INSERT INTO Part (partCode,partNumber,price,stockCount,name,quantity,supplier,description,createDate,createUser) VALUES ("+code+","+number+","+price.ToString()+","+stockCount+",'"+name+"',"+quantity+",'"+supplier+"','"+description+"',GETDATE(),"+userId.ToString()+")");
            command.Connection = sqlConnection;
            sqlConnection.Open();
            int inserted=command.ExecuteNonQuery();
            sqlConnection.Close();
            if (inserted >= 1)
                return RetCode.Success;
            else
                return RetCode.Fail;

        }

        public DataSet GetParts()
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM Part");
            command.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet set = new DataSet();
            adapter.Fill(set);
            return set;
        }
    }
}