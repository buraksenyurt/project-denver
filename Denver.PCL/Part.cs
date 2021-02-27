using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.PCL
{
    public class Part
        :BaseEntityClass
    {
        private int partCode;
        private string name;
        private string partNumber;
        private double price;
        private int quantity;
        private string description;
        private string supplier;
        private DateTime createDate;

        public int PartCode
        {
            get { return partCode; }
            set { partCode = value; }
        }

        public string PartName
        {
            get { return name; }
            set { name = PartName; }
        }

        public string PartNumber
        {
            get { return partNumber; }
            set { partNumber = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = Price; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }
        public DateTime CreateDate
        {
            get { return createDate; }
            set {
                if (value > DateTime.Now)
                    throw new Exception();
                createDate = value; 
            }
        }
    }
}
