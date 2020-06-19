using Denver.BLL;
using Denver.Common;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Denver.Facade.Parts
{
    public class PartManagerFacade
    {
        public RetCode AddToStock(int code,int number,double price,int stockCount,string name,int quantity,string supplier,string description)
        {
            BLLPartManager manager = new BLLPartManager();
            try
            {
                if (code == 0)
                    return RetCode.Fail;
                else if (price == 0)
                    return RetCode.Fail;
                else if (stockCount == 0)
                    return RetCode.Fail;
                else if (name.Length > 100)
                    return RetCode.Fail;
                else if (supplier.Length > 100)
                    return RetCode.Fail;
                else if (quantity == 0)
                    return RetCode.Fail;

                manager.AddNewPart(code, number, price,stockCount, name, quantity, supplier, description);

                if(stockCount<100)
                {
                    Util.SendEmailToUnitHead(Code:code, Count:stockCount);
                }
                else if(stockCount>100 &&stockCount<1000)
                {
                    Util.SendEmailToManager(Code: code, Count: stockCount);
                }
                else if(stockCount>1000)
                {
                    Util.SendEmailToBoss(Code: code, Count: stockCount);
                }

                return RetCode.Success;
            }
            catch
            {
                return RetCode.Fail;
            }
        }

        public DataSet GetParts()
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
