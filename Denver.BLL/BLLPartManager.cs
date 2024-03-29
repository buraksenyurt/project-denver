﻿using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using Denver.Tools;
using System;
using System.Data;

namespace Denver.BLL
{
    public class BLLPartManager
    {
        public RetCode AddNewPart(int code, int number, decimal price,int stockCount, string name, int quantity, string supplier, string description)
        {
            DALPartManager manager = new DALPartManager();
            manager.AddNewPart(code, number, price,stockCount, name, quantity, supplier, description,1099);
            bool result=SendInvoiceSummaryToBrasilBank(code, number, price, stockCount, name, quantity, supplier);
            if (result)
                return RetCode.Success;
            else
                return RetCode.Fail;
        }

        private bool SendInvoiceSummaryToBrasilBank(int code, int number, decimal price, int stockCount, string name, int quantity, string supplier)
        {
            bool result = false;
            result=MailUtility.SendRequest(code, number, price, stockCount, name, quantity, supplier);
            return result;
        }

        public DataSet GetParts()
        {
            DALPartManager manager = new DALPartManager();
            return manager.GetParts();
        }
    }
}