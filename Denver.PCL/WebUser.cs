using System;

namespace Denver.PCL
{
    public class WebUser
        : BaseEntityClass
    {
        private string _fullName;
        private string _registirationNumber;
        private string _token;
        private DateTime _loginTime;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string RegistrationNumber
        {
            get { return _registirationNumber; }
            set { _registirationNumber = value; }
        }
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
        public DateTime LoginTime
        {
            get { return _loginTime; }
            set { _loginTime = value; }
        }
    }
}