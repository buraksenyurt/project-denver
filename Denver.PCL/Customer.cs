using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.PCL
{
    public class Customer
    {
        private int _id;
        private string _title;
        private string _fullname;
        private string _phone1;
        private string _phone2;
        private string _phone3;
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _postalCode;
        private string _county;
        private string _city;
        private string _country;
        private string _email1;
        private string _email2;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; }
        }

        public string Phone1
        {
            get { return _phone1; }
            set { _phone1 = value; }
        }

        public string Phone2
        {
            get { return _phone2; }
            set { _phone2 = value; }
        }

        public string Phone3
        {
            get { return _phone3; }
            set { _phone3 = value; }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        public string AddressLine3
        {
            get { return _addressLine3; }
            set { _addressLine3 = value; }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        public string County
        {
            get { return _county; }
            set { _county = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string Email1
        {
            get { return _email1; }
            set { _email1 = value; }
        }

        public string Email2
        {
            get { return _email2; }
            set { _email2 = value; }
        }
    }
}
