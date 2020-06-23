using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denver.Configuration
{
    public class ServicesConfig
    {
        public static string goverment_central_bank_address = "https://www.brazilbank.gov.br/api/budget?authkey=wers234#xq231";
        public static string maps_service = "http://nasa.gov.usa/maps/brasil";
        public static string log_service = "http://localhost/web/services/logger.asmx"; //TODO Log bilgisini alıp bir yerlere yazan XML Web Service yazılacak
    }
}
