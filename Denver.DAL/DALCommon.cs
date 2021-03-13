using Denver.Common;
using Denver.Configuration;
using Denver.PCL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALCommon
    {
        public RetCode GetCurrentUser(string userName, WebUser user)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE UserName ='" + userName + "AND IsActive=1");
                command.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user.FullName = reader["FullName"].ToString();
                    user.Token = reader["Token"].ToString();
                    user.RegistrationNumber = reader["Reg_Number"].ToString();
                    user.LoginTime = DateTime.Now;
                }
                sqlConnection.Close();
                return RetCode.Success;
            }
            catch (Exception ex)
            {
                return RetCode.Fail;
            }
        }
    }
}