using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.PCL
{
    public class Invoice
    {
        private int _id;
        private Customer _customer;
        private Consumer _consumer;
        private decimal _totalPrice;
        private DateTime _invoiceDate;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public Consumer Consumer
        {
            get { return _consumer; }
            set { _consumer = value; }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public DateTime InvoiceDate
        {
            get { return _invoiceDate; }
            set { _invoiceDate = value; }
        }
    }
}
