using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Denver.BLL
{
    public class BLLHumanResource
    {
        public decimal CalculateRevenue(Person person, Experience experience)
        {
            decimal result = 0;
            double factor = 0;
            BLLProjectManagement projectManagement = new BLLProjectManagement();
            int lastYearsTotalAccomplishedProjectsCount = projectManagement.GetSuccessfullyAccomplishedTotalProjectsCountForEmployee(person);
            if (lastYearsTotalAccomplishedProjectsCount > 0)
            {
                ProjectInfo[] projectInfos = projectManagement.GetProjectsForEmployee(person);
                for (int i = 0; i < projectInfos.Length; i++)
                {
                    if (projectInfos[i].BusinessValue >= 100)
                        factor += 0.1;
                    if (projectInfos[i].Size == ProjectSize.Small || projectInfos[i].Size == ProjectSize.Medium)
                        factor += 0.25;
                    if (projectInfos[i].Size == ProjectSize.Large)
                        factor += 0.50;
                    if (projectInfos[i].WorkersCount <= 3)
                        factor += 0.01;
                    if (projectInfos[i].WorkersCount > 3 && projectInfos[i].WorkersCount <= 9)
                        factor += 0.03;
                    if (projectInfos[i].WorkersCount > 9)
                        factor += 0.06;
                    if (person.WorkStartDate.Year > 2005)
                        factor += 0.005;
                    if (projectInfos[i].IsInternationalProject == true)
                        factor += 0.35;
                    if (person.Location == WorkLocation.Office)
                        factor += 0.15;
                    else if (person.Location == WorkLocation.Remote)
                        factor += 0.05;
                }

                result = person.Salary + (person.Salary * Convert.ToDecimal(factor));
            }

            return result;
        }

        public DataSet LoadAllPersons(int start, int end)
        {
            DALPerson dalPerson = new DALPerson();
            return dalPerson.LoadAllPersons(start,end);
        }

        public Dictionary<string, string> LoadWorkLocations()
        {
            string[] names = Enum.GetNames(typeof(WorkLocation));
            Dictionary<string, string> result = new Dictionary<string, string>();
            int i = 0;
            foreach (var name in names)
            {
                result.Add(i.ToString(),name);
                i++;
            }

            return result;
        }
    }
}
