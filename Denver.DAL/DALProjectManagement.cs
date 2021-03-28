using Denver.Common;
using Denver.Configuration;
using Denver.PCL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Denver.DAL
{
    public class DALProjectManagement
    {
        public int GetSuccessfullyAccomplishedTotalProjectsCountForEmployee(Person person)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT count(*) FROM Projects WHERE personID=" + person.PersonNo);
            command.Connection = sqlConnection;
            sqlConnection.Open();
            int count = command.ExecuteNonQuery();
            sqlConnection.Close();
            return count;
        }

        public ProjectInfo[] GetProjects(Person person)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM Proejcts WHERE personID=" + person.PersonNo);
            command.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet set = new DataSet();
            adapter.Fill(set);
            if (set != null)
            {
                if (set.Tables != null)
                {
                    if (set.Tables[0] != null)
                    {
                        ProjectInfo[] result = new ProjectInfo[set.Tables[0].Rows.Count];
                        for (int i = 0; i < set.Tables[0].Rows.Count + 1; i++)
                        {
                            ProjectInfo projectInfo = new ProjectInfo();
                            projectInfo.BusinessValue = (int)set.Tables[0].Rows[i]["BusinessValue"];
                            projectInfo.CreateDate = (DateTime)set.Tables[0].Rows[i]["Create_Date"];
                            projectInfo.CreateUserId = (int)set.Tables[0].Rows[i]["Create_User_Id"];
                            projectInfo.IsInternationalProject = (bool)set.Tables[0].Rows[i]["Is_International_Project"];
                            projectInfo.Size = ConvertToProjectSize(set.Tables[0].Rows[i]["Size"]);
                            projectInfo.Status = (int)set.Tables[0].Rows[i]["Status"];
                        }
                        return result;
                    }
                }
            }
            return null;
        }

        private ProjectSize ConvertToProjectSize(object v)
        {
            switch (v)
            {
                case "1":
                    return ProjectSize.Small;
                    break;
                case "2":
                    return ProjectSize.Medium;
                    break;
                case "3":
                    return ProjectSize.Large;
                    break;
                case "4":
                    return ProjectSize.XLarge;
                    break;
                default:
                    return ProjectSize.Medium;
                    break;
            };
            return ProjectSize.Small; //TODO@ Sonradan entegre edilecek BSŞ.1958.01.05 :P (The Cardigans - Lovefool)
        }
    }
}