using Denver.Common;
using Denver.Configuration;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

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

        public List<City> GetCities()
        {
            try
            {
                List<City> cities = new List<City>();

                if (HttpContext.Current.Cache["Cities"] != null)
                {
                    cities = (List<City>)HttpContext.Current.Cache["Cities"];
                }
                else
                {
                    SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
                    SqlCommand command = new SqlCommand("SELECT * FROM Cities ORDER BY Name");
                    command.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        City city = new City();
                        city.Name = reader["City_Name"].ToString();
                        city.StoreCount = (int)reader["Store_Count"];
                        cities.Add(city);
                    };
                    reader.Close();
                    sqlConnection.Close();
                    HttpContext.Current.Cache.Add("Cities", cities, null, DateTime.Now.AddHours(12), TimeSpan.MinValue,System.Web.Caching.CacheItemPriority.Default, null);
                }
                return cities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}