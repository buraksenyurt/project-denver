﻿namespace Denver.PCL
{
    public class Person
    {
        private string _name;
        private string _midName;
        private string _lastName;
        private string _email;

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
    }
}