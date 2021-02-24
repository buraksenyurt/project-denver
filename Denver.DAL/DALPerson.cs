using Denver.Common;
using Denver.Configuration;
using Denver.PCL;
using System;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALPerson
    {
        public ExceptionManager exceptionManager = new ExceptionManager();

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
            person.MidName = reader["middle_name"].ToString();
            person.LastName = reader["last_name"].ToString();
            person.WorkStartDate = Convert.ToDateTime(reader["work_start_date"]);
            person.Salary = Convert.ToDecimal(reader["salary"]);
            sqlConnection.Close();

            if (person != null)
                return person;

            return null;
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
            person.MidName = reader["middle_name"].ToString();
            person.LastName = reader["last_name"].ToString();
            person.WorkStartDate = Convert.ToDateTime(reader["work_start_date"]);
            person.Salary = Convert.ToDecimal(reader["salary"]);
            sqlConnection.Close();
            return person;

        }

        public RetCode Add(Person person)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
                SqlCommand command = new SqlCommand("spAddPerson"); //TODO spFindUnitHead SP'si yazılacak
                command.Parameters.AddWithValue("@prmName", person.Name);
                command.Parameters.AddWithValue("@prmMidName", person.MidName);
                command.Parameters.AddWithValue("@prmLastName", person.LastName);
                command.Parameters.AddWithValue("@prmEmail", person.Email);
                command.Parameters.AddWithValue("@prmWorkStartDate", person.WorkStartDate);
                command.Parameters.AddWithValue("@prmSalary", person.Salary);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();
                int inserted=command.ExecuteNonQuery();
                if (inserted == 1)
                    return RetCode.Success;

                sqlConnection.Close();
            }
            catch(Exception excp)
            {
                exceptionManager.Error(excp.Message);
            }
            return RetCode.Fail;
        }

        public RetCode Delete(int personId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
                SqlCommand command = new SqlCommand("spDeletePerson");
                command.Parameters.AddWithValue("@prmPersonId", personId);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();
                int deleted=command.ExecuteNonQuery();
                sqlConnection.Close();
                if (deleted == 1)
                    return RetCode.Success;
            }
            catch (Exception excp)
            {
                exceptionManager.Error(excp.Message);
            }
            return RetCode.Fail;
        }
    }
}