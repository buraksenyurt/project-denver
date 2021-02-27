using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Denver.PCL
{
    /// <summary>
    /// base class of parameter classes. All property classes must be
    /// derived from PropertyClass
    /// </summary>
    [Serializable]
    public abstract class BaseEntityClass : ICloneable
    {
        // Don't convert these properties to AutoProperty syntax
        // We've failed once: https://stackoverflow.com/questions/13022198/how-to-remove-k-backingfield-from-json-when-deserialize
        private DateTime createDate = new DateTime(1900, 1, 1);
        private DateTime modifyDate = new DateTime(1900, 1, 1);
        private int createUserId;
        private int modifyUserId;
        private int status;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        public DateTime ModifyDate
        {
            get { return modifyDate; }
            set { modifyDate = value; }
        }
        public int CreateUserId
        {
            get { return createUserId; }
            set { createUserId = value; }
        }
        public int ModifyUserId
        {
            get { return modifyUserId; }
            set { modifyUserId = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        public string ReflectObj()
        {
            Type t;
            PropertyInfo[] pi;
            string reflectString = "";
            t = this.GetType();

            pi = t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (PropertyInfo pinfo in pi)
            {
                if (pinfo.GetValue(this, null) == null)
                {
                    reflectString += "-" + pinfo.Name + ":" + "null";
                }
                else
                {
                    reflectString += "-" + pinfo.Name + ":" + pinfo.GetValue(this, null).ToString();
                }
            }
            return reflectString;
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
