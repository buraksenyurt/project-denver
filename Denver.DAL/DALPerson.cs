using Denver.Configuration;
using Denver.PCL;
using System;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALPerson
    {
        public DALPerson()
        {
        }

        public Person FindManager(int userId)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("spFindManager"); //TODO spFindManager SP'si yazılacak
            command.Parameters.AddWithValue("@prmUserId", userId);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlDataReader reader=command.ExecuteReader();
            Person person = new Person();
            person.Email = reader["e_mail"].ToString();
            person.Name = reader["name"].ToString();
            person.Name = reader["middle_name"].ToString();
            person.Name = reader["full_name"].ToString();
            sqlConnection.Close();
            return person;
        }

        public Person FindUnitHead(int userId)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("spFindUnitHead"); //TODO spFindUnitHead SP'si yazılacak
            command.Parameters.AddWithValue("@prmUserId", userId);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Person person = new Person();
            person.Email = reader["e_mail"].ToString();
            person.Name = reader["name"].ToString();
            person.Name = reader["middle_name"].ToString();
            person.Name = reader["full_name"].ToString();
            sqlConnection.Close();
            return person;

        }

        public void Add(Person person)
        {
            throw new NotImplementedException();
        }

        public void Delete(int personId)
        {
            throw new NotImplementedException();
        }
    }
}