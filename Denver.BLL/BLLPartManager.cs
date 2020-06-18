﻿using Denver.Common;
using Denver.DAL;
using Denver.PCL;
using System;

namespace Denver.BLL
{
    public class BLLPartManager
    {
        public RetCode AddNewPart(int code, int number, double price, string name, int quantity, string supplier, string description)
        {
            DALPartManager manager = new DALPartManager();
            return manager.AddNewPart(code, number, price, name, quantity, supplier, description);
        }

        public Part[] GetParts()
        {
            throw new NotImplementedException();
        }
    }
}