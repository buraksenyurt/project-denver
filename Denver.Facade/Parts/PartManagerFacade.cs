using Denver.BLL;
using Denver.Common;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.Facade.Parts
{
    public class PartManagerFacade
    {
        public RetCode Add(int code,int number,double price,string name,int quantity,string supplier,string description)
        {
            BLLPartManager manager = new BLLPartManager();
            try
            {
                manager.AddNewPart(code, number, price, name, quantity, supplier, description);
                return RetCode.Success;
            }
            catch
            {
                return RetCode.Fail;
            }
        }

        public Part[] GetParts()
        {
            BLLPartManager manager = new BLLPartManager();
            try
            {
                return manager.GetParts();
            }
            catch
            {
                return null;
            }
        }
    }
}
