using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using System;

namespace Denver.BLL
{
    public class BLLProjectManagement
    {
        public int GetSuccessfullyAccomplishedTotalProjectsCountForEmployee(Person person)
        {
            DALProjectManagement manager = new DALProjectManagement();
            int result = manager.GetSuccessfullyAccomplishedTotalProjectsCountForEmployee(person);
            return result;
        }

        public ProjectInfo[] GetProjectsForEmployee(Person person)
        {
            DALProjectManagement manager = new DALProjectManagement();
            return manager.GetProjects(person);
        }
    }
}