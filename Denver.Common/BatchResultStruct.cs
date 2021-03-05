using System.Data;

namespace Denver.Common.Services
{
    public class BatchResultStruct
    {
        public int ReturnCode { get; set; }
        public DataSet DataReturned { get; set; }
    }
}