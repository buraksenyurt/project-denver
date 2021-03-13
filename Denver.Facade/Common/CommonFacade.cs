using Denver.BLL;
using Denver.Common;
using Denver.Common.Services;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Denver.Facade.Common
{
    public class CommonFacade
    {
        public BatchResultStruct ClearReportingFilesFolder()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecuteProcessBatch()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecuteProcessBatchAll()
        {
            throw new NotImplementedException();
        }

        public WebUser GetCurrentUser()
        {
            var userName = HttpContext.Current.Session["UserName"].ToString();
            BLLCommon bLLCommon = new BLLCommon();
            WebUser user = new WebUser();
            bLLCommon.GetCurrentUser(userName, user);
            return user;
        }

        public int Fixing()
        {
            throw new NotImplementedException();
        }

        public int POGG()
        {
            throw new NotImplementedException();
        }

        public int DebtAgingDaily()
        {
            throw new NotImplementedException();
        }

        public int MakeOFO()
        {
            throw new NotImplementedException();
        }

        public int ProcessGBData()
        {
            throw new NotImplementedException();
        }

        public int ClearBadCredits(int v)
        {
            throw new NotImplementedException();
        }

        public int CentralBankExchangeRateAddAll()
        {
            throw new NotImplementedException();
        }

        public int AddExchangeRateForAccountingFirms()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ProcessStatusInquiry()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ClearDepotFoldersImages()
        {
            throw new NotImplementedException();
        }

        public int DeleteMultiplePrintDocuments()
        {
            throw new NotImplementedException();
        }

        public int CalculateAverageCompletionTimeForWR()
        {
            throw new NotImplementedException();
        }

        public void SendWarrantyRequestToCLAW()
        {
            throw new NotImplementedException();
        }

        public int AgreementForm()
        {
            throw new NotImplementedException();
        }

        public int CreditAging()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CloseOpenLocatedCagesForImportInvoice()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SentEODServiceResultInfo()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SentLogErrorMails()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendStatusUpdateNotificationMailAndSMS()
        {
            throw new NotImplementedException();
        }

        public static RetCode ProcessWebRequests(DateTime dateTime, DateTime now)
        {
            throw new NotImplementedException();
        }
        public int SendCountryModelMissingEmail()
        {
            throw new NotImplementedException();
        }
        public int SendNotificationMailForUninvoicedTestFirms()
        {
            throw new NotImplementedException();
        }
        public int SendDepartmentApprovalWaitingMail()
        {
            throw new NotImplementedException();
        }

        public int SendTestPartsForExpiredMail()
        {
            throw new NotImplementedException();
        }
    }
}
