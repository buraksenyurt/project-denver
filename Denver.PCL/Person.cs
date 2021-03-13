using Denver.Common;
using System;

namespace Denver.PCL
{
    public class Person
        :BaseEntityClass
    {
        private string _name;
        private string _midName;
        private string _lastName;
        private string _email;
        private decimal _salary;
        private DateTime _workStartDate;
        private WorkLocation _workLocation;
        public int PersonNo;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string MidName
        {
            get { return _midName; }
            set { _midName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public decimal Salary
        {
            get { return _salary; }
            set { _salary = value; }
        }

        public DateTime WorkStartDate
        {
            get
            {
                return _workStartDate;
            }
            set
            {
                if (value < DateTime.Now)
                    _workStartDate = value;
                else
                    _workStartDate = DateTime.MinValue;
            }
        }

        public WorkLocation Location
        {
            get
            {
                return _workLocation;
            }
            set
            {
                _workLocation = value;
            }
        }
    }
}