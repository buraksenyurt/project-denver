using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using System;
using System.Data;

namespace Denver.BLL
{
    public class BLLPartManager
    {
        public RetCode AddNewPart(int code, int number, double price,int stockCount, string name, int quantity, string supplier, string description)
        {
            DALPartManager manager = new DALPartManager();
            manager.AddNewPart(code, number, price,stockCount, name, quantity, supplier, description);
            bool result=SendInvoiceSummaryToBrasilBank(code, number, price, stockCount, name, quantity, supplier);
            if (result)
                return RetCode.Success;
            else
                return RetCode.Fail;
        }

        private bool SendInvoiceSummaryToBrasilBank(int code, int number, double price, int stockCount, string name, int quantity, string supplier)
        {
            bool result = false;
            result=Util.SendRequest(code, number, price, stockCount, name, quantity, supplier);
            return result;
        }

        public DataSet GetParts()
        {
            DALPartManager manager = new DALPartManager();
            return manager.GetParts();
        }
    }
}