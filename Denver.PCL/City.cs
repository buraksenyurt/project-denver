using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.PCL
{
    public class City
        :BaseEntityClass
    {
        private int id;
        private string name;
        private int storeCount;

        public int Id
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int StoreCount
        {
            get { return storeCount; }
            set {
                if (value <= 0)
                    throw new Exception();
                else 
                storeCount = value; 
            }
        }        
    }
}
