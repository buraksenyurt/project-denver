using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using Denver.Tools;
using System;
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
    }
}