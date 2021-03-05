using Denver.BLL;
using Denver.Common;
using Denver.PCL;
using Denver.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Denver.Facade.Parts
{
    public class AutomicJobFacade
    {
        public RetCode Add(string jobName, byte parameterCount, object[] parameters, string serverIpAddress, ref int jobId)
        {
            throw new NotImplementedException();
        }

        public int DirectExecuteStoredProcedure(string v)
        {
            throw new NotImplementedException();
        }

        public void UpdateFinishDate(int jobId, int returnCode, bool v)
        {
            throw new NotImplementedException();
        }
    }
}
