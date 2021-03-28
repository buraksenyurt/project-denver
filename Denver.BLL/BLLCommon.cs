using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using Denver.Tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Denver.BLL
{
    public class BLLCommon
    {
        public RetCode GetCurrentUser(string userName, WebUser user)
        {
            DALCommon common = new DALCommon();
            common.GetCurrentUser(userName, user);
            return RetCode.Success;
        }

        public List<City> GetCities()
        {
            DALCommon common = new DALCommon();
            List<City> cities=common.GetCities();
            return cities;
        }
    }
}