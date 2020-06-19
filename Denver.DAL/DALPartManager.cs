using Denver.Common;
using Denver.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALPartManager
    {
        public RetCode AddNewPart(int code, int number, double price,int stokCount, string name, int quantity, string supplier, string description)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("INSERT INTO Parts (code,number,price,stockCount,name,quantity,supplier,description) VALUES ("+code+","+number+","+price+","+stokCount+",'"+name+"',"+quantity+"',"+supplier+"','"+description+"')");
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
            SqlCommand command = new SqlCommand("SELECT * FROM Parts");
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet set = new DataSet();
            adapter.Fill(set);
            return set;
        }
    }
}