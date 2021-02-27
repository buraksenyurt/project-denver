using Denver.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Denver.Common
{
    public class ContactTypes
    {
        public const string Phone = "T";
        public const string EMail = "E";
        public const string Fax = "F";
        public const string Other = "D";
        public const string PhoneFax = "TF";
    }
    public class ContactTypesV2
    {
        public const string Phone = "Tel";
        public const string Fax = "Faks";
    }
    public class ContactTypesV2Values
    {
        public const int HomePhone = 0;
        public const int WorkPhone = 1;
        public const int MobilePhone = 2;
    }
    
    public class ContactTypesCorporateValues
    {
        public const int Center = 0;
        public const int Branch = 1;
    }

    public static class ContactSubTypes
    {
        public static string Home { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Home", "Ev"); } }
        public static string Work { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Work", "İş"); } }
        public static string Private { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Private", "Özel"); } }
        public static string CelularPhone { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "CelularPhone", "Cep"); } }
        public static string Center { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Center", "Merkez"); } }
        public static string Branch { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Branch", "Şube"); } }
        public static string Other { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "Other", "Diğer"); } }
        public static string CallCenter { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "CallM", "Çağrı M"); } }
        public static string CenterShort { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "CenterShort", "TELEFON MR"); } }
        public static string BranchShort { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "BranchShort", "TELEFON ŞB"); } }
        public static string HomeForReport { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "HomeForReport", "TELEFON EV"); } }
        public static string WorkForReport { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "WorkForReport", "TELEFON İŞ"); } }
        public static string CelularPhoneForReport { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "ContactSubTypes", "CelularPhoneForReport", "TELEFON CEP"); } }
    }
    
    public static class MobilePhoneTypes
    {
        public const string Work = "İş";
        public const string Private = "Özel";
    }
    public static class DataOperations
    {
        public static string Addition { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "DataOperations", "Addition", "Ekleme"); } }
        public static string Update { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "DataOperations", "Update", "Güncelleme"); } }
        public static string Delete { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "DataOperations", "Delete", "Silme"); } }
        public static string Select { get { return UtilityLocalization.GetLocalizationPropertyString("GT.TurkuazCommon", "DataOperations", "Select", "Çekme"); } }
    }

    public class UtilityLocalization
    {
        private const string cacheKey = "mlConstant";
        public static string GetLocalizationPropertyString(string namespacename, string classname, string propertyname, string propertyDescTr)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["UserLanguage"] == null || Convert.ToInt16(HttpContext.Current.Session["UserLanguage"]) == 1)
            {
                return propertyDescTr;
            }

            List<PCMultilanguageConstants> list = null;
            if (System.Web.HttpRuntime.Cache[cacheKey] == null)
            {
                list = GetLocalizationPropertyString(namespacename, classname, propertyname);
                System.Web.HttpRuntime.Cache.Insert(cacheKey, list);
            }
            list = (List<PCMultilanguageConstants>)System.Web.HttpRuntime.Cache[cacheKey];
            PCMultilanguageConstants pcMultilanguageConstants = list.FirstOrDefault(t => t.Namespace == namespacename && t.Classname == classname && t.Code == propertyname);
            if (pcMultilanguageConstants != null)
            {
                if (Convert.ToInt16(HttpContext.Current.Session["UserLanguage"]) == 2)
                {
                    return pcMultilanguageConstants.DescEn;
                }
            }
            return string.Empty;
        }

        private static List<PCMultilanguageConstants> GetLocalizationPropertyString(string namespacename, string classname, string propertyname)
        {
            SqlConnection sqlConnection = new SqlConnection(DbConfig.ConnectionString);
            SqlCommand command = new SqlCommand("sp_multilanguage_constants_get");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet set = new DataSet();
            adapter.Fill(set);

            IEnumerable<PCMultilanguageConstants> object_List = (from t in set.Tables[0].AsEnumerable()
                       select new PCMultilanguageConstants()
                       {
                           Namespace = t.Field<string>("namespace"),
                           Classname = t.Field<string>("classname"),
                           Code = t.Field<string>("code"),
                           DescEn = t.Field<string>("desc_en"),
                           DescTr = t.Field<string>("desc_tr")
                       });

            return object_List.ToList();
        }
    }

    public class PCMultilanguageConstants
    {
        public string Classname { get; set; }
        public string Namespace { get; set; }
        public string Code { get; set; }
        public string DescTr { get; set; }
        public string DescEn { get; set; }
    }

}
