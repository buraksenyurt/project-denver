using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Services;
using Denver.Common;
using Denver.Common.Services;
using Denver.Facade.Common;
using Denver.Facade.Dealer;
using Denver.Facade.Employee;
using Denver.Facade.Parts;

namespace GT.TurkuazServicesBatch.Services
{
    /// <summary>
    /// Summary description for WSBatchJob.
    /// </summary>
    public class WSAutomicJobs
    {
        public WSAutomicJobs()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        #region Get Operation Result
        private BatchResultStruct GetOperationResult(RetCode returnCode, DataSet dsReturn)
        {
            int retCode = 0;
            switch (returnCode)
            {
                case RetCode.Fail:
                    retCode = 1;
                    break;
                case RetCode.Success:
                    retCode = 0;
                    break;
                default:
                    retCode = (int)returnCode;
                    break;
            }

            return new BatchResultStruct { ReturnCode = retCode, DataReturned = dsReturn };
        }
        #endregion Get Operation Result

        #region Execute Job
        /// <summary>
        /// Execute Job
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [WebMethod(Description = "Arka Plan İşlerin Çalıştırılması")]
        public BatchResultStruct ExecuteJob(string jobName, object[] parameters)
        {
            int jobId = 0;
            AutomicJobFacade automicJobFacade = new AutomicJobFacade();
            PartManagerFacade partManagerFacade = new PartManagerFacade();
            BatchResultStruct result = new BatchResultStruct();

            try
            {
                byte parameterCount = 0;
                if (parameters != null)
                {
                    parameterCount = (byte)parameters.Length;
                }

                string serverIpAddress = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                RetCode returnCode = automicJobFacade.Add(jobName, parameterCount, parameters, serverIpAddress, ref jobId);
                if (returnCode != RetCode.Success)
                {
                    result.ReturnCode = (int)RetCode.GNLServicesBatchJobCanNotBeAdded;
                    return result;
                }

                int retCode = 0;

                switch (jobName)
                {
                    //Direct SP call
                    case "EXECUTSP":
                        retCode = automicJobFacade.DirectExecuteStoredProcedure(parameters[0].ToString());
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    //Part Bonus

                    case "METQSBND":
                        result = new PartManagerFacade().CreateDailyByBrandGroup();
                        break;

                    case "METQSBNW":
                        result = new PartManagerFacade().CreateForWaiting();
                        break;

                    case "METQSBNI":
                        result = new PartManagerFacade().CreateInsuranceBonusDaily();
                        break;

                    case "METQSBNO":
                        result = new PartManagerFacade().CreateOriginalAccessoryBonusDaily();
                        break;

                    //File Transfer
                    case "METQSSIS":
                        result = new FileTransferOperationsFacade().ExecuteSSIS(parameters[0].ToString());
                        break;

                    //File Transfer
                    case "METQSSISByID":
                        result = new FileTransferOperationsFacade().ExecuteSSISByID(Convert.ToInt32(parameters[0].ToString()));
                        break;

                    //Catalog Transfer
                    case "METQSCP1":
                        result = new CatalogTransferOperationsFacade().BatchCatalogPriceComparisonSummaryCreate();
                        break;

                    case "METQSPUW":
                        retCode = new CatalogTransferOperationsFacade().BatchCatalogUpdateForWaiting();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "METQSPUA":
                        retCode = new CatalogTransferOperationsFacade().BatchCatalogUpdateForWaitingExecuteAll();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part Selling
                    case "METQSPS1":
                        result = new PartManagerFacade().BatchDraftProcess();
                        break;

                    //Part Order Follow Up
                    case "METQOFU1":
                        result = new CommonFacade().ProcessStatusInquiry();
                        break;

                    //Part Pricing
                    case "METQPPR1":
                        retCode = new PartManagerFacade().UpdatePartPriceBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //End Of Period
                    case "METQEOP1":
                        result = new CommonFacade().ExecuteProcessBatch();
                        break;

                    case "METQEOP4":
                        result = new CommonFacade().ExecuteProcessBatchAll();
                        break;

                    //FinanceCurrencyFixing
                    case "METQFIN2":
                        retCode = new CommonFacade().Fixing();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinancePOGG
                    case "METQFIN3":
                        retCode = new CommonFacade().POGG();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceDebtAgingDaily //ED.20150926 Hoze Luiz Gonzaled De Salvatore
                    case "METQFIN7":
                        retCode = new CommonFacade().DebtAgingDaily();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceOFOBatch
                    case "METQFIN8":
                        retCode = new CommonFacade().MakeOFO();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceBOtoGBBatch
                    case "MDTQFN10":
                        retCode = new CommonFacade().ProcessGBData();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceVDFCreditBatch
                    case "MDTQFN11":
                        retCode = new CommonFacade().ClearBadCredits(int.Parse(parameters[0].ToString()));
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CentralBankExchangeRate
                    case "DNVR_AUTOMIC_JOB_CBER":
                        retCode = new CommonFacade().CentralBankExchangeRateAddAll();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CentralBankExchangeRate
                    case "DNVR_AUTOMIC_JOB_CBER_MUH":
                        retCode = new CommonFacade().AddExchangeRateForAccountingFirms();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //ClearReportingFilesFolder
                    case "METQARC2":
                        result = new CommonFacade().ClearReportingFilesFolder();
                        break;

                    //RewardSystemCalculationVW
                    case "METQDNT1":
                        retCode = new DealerNetworkManagerFacade().RewardSystemCalculationVW();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //AfterSalesSendCollectiveSMS
                    case "DNVR_AUTOMIC_JOB_AS01":
                        retCode = new SalesManagerFacade().SendCollectiveSMSForAppointment();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //RewardSystemCalculationAudi
                    case "METQDNT2":
                        retCode = new DealerNetworkManagerFacade().RewardSystemCalculationForAudi();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceCreditAgingBatch
                    case "DNVR_AUTOMIC_JOB_FN12":
                        retCode = new FinanceManagerFacade().CreditAging();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //ClearDepotFoldersDODImages
                    case "METQARC3":
                        result = new CommonFacade().ClearDepotFoldersImages();
                        break;

                    //FinanceMultiplePrintDocumentsDelete
                    case "DNVR_AUTOMIC_JOB_FN13":
                        retCode = new CommonFacade().DeleteMultiplePrintDocuments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //WSWarrantyRequestOperation
                    case "DNVR_AUTOMIC_JOB_AS02":
                        retCode = new CommonFacade().CalculateAverageCompletionTimeForWR();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CLAWWarrantyService
                    case "DNVR_AUTOMIC_JOB_AS03":
                        new CommonFacade().SendWarrantyRequestToCLAW();
                        result = new BatchResultStruct { ReturnCode = 0, DataReturned = null };
                        break;

                    //WSWarrantyRequestOperation
                    case "DNVR_AUTOMIC_JOB_FN14":
                        retCode = new CommonFacade().AgreementForm();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0271":
                        retCode = new PartManagerFacade().DeletePoolStockPartsThatHasNoPoolStock();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part Planning
                    case "METQBODY":
                        result = new PartManagerFacade().BatchProcessDealerOrderDraftCreation();
                        break;

                    case "MNTQODDS":
                        result = new PartManagerFacade().ExecuteBatchPlanningJobForDistributor();
                        break;

                    case "MNTQODYS":
                        result = new PartManagerFacade().CreateOrderDraftForDealerBatch();
                        break;

                    case "MNTQQUDS":
                        result = new PartManagerFacade().ExecutePlanningJobFromQueueForDistributor();
                        break;

                    case "DNVR_AUTOMIC_JOB_0371":
                        result = new PartManagerFacade().ExecutePlannedUrgentOrderDrafts();
                        break;







                    //FinanceOTSBatch
                    case "DNVR_AUTOMIC_JOB_0292":
                        retCode = new FinanceManagerFacade().MakeOTS();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0304":
                        retCode = new SalesManagerFacade().InformAll();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0306":
                        retCode = new FinanceManagerFacade().SendPreExpneseApprovalRecordsPendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part Direct Selling - Create Incoming Invoice
                    case "PARTDINV":
                        retCode = new PartManagerFacade().CreateInvoiceForDomesticInvoiceDraftFiles();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0311":
                        retCode = new SalesManagerFacade().SendPartMaintenanceAndExaminationSMSForFirms();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part OEM Order - Process Confirmation
                    case "PARTOEMC":
                        retCode = new PartManagerFacade().ProcessConfirmation();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Abnormal Order
                    case "PARTOEMO":
                        retCode = new PartManagerFacade().BatchAbnormalOrderProcess();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;


                    case "DNVR_AUTOMIC_JOB_0372":
                        result = new PartManagerFacade().SentBackOrderInfoMails();
                        break;

                    case "LAWNTF01":
                        retCode = new DealerNetworkManagerFacade().ListEndDateComingLawNotificationsAndSendEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0316":
                        retCode = new CustomerManagerFacade().ListEndDateComingContractsAndSendEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0318":
                        retCode = new SalesManagerFacade().EmailManagersAndChiefsofUninvoicedWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0317":
                        retCode = new SalesManagerFacade().EmailManagersAndChiefsofUndeliveredWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0357":
                        retCode = new SalesManagerFacade().SendEmailForCustomerStandartPackageBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0320":
                        retCode = new FinanceManagerFacade().SendPurchasePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TDTQ0152":
                        retCode = new FinanceManagerFacade().UpdateDueDateBatchForPrivateInterval();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0324":
                        retCode = new FinanceManagerFacade().SendPurchasePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0334":
                        retCode = new CustomerManagerFacade().AddAutomaticContactsForJobs();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0342":
                        retCode = new FinanceManagerFacade().SendBalanceResetPendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0343":
                        //retCode = new MailManagerFacade().SendOFOPendingMail();
                        //result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0345":
                        retCode = new FinanceManagerFacade().SendInvoicePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0346":
                        retCode = new FinanceManagerFacade().SendReturnedInvoicePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0421":
                        retCode = new SalesManagerFacade().OnlineCustomerSurveyInfoMailBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "CUSSCCOW":
                        string changeDateStart = (DateTime.Now.AddDays(-1)).ToString("yyyyMMdd");
                        string changeDateEnd = DateTime.Now.ToString("yyyyMMdd");
                        RetCode retCodeVeh = new OrderManagerFacade().Update(changeDateStart, changeDateEnd);
                        switch (retCodeVeh)
                        {
                            case RetCode.Fail:
                                retCode = 1;
                                break;
                            case RetCode.Success:
                                retCode = 0;
                                break;
                            default:
                                retCode = (int)retCodeVeh;
                                break;
                        }

                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0349":
                        retCode = new FinanceManagerFacade().AddPosBlockage();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "PDLBUYBC":
                        retCode = new PartManagerFacade().BuyBackDealerDraftCreate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0360"://Badge customer initializa and update batch
                        retCode = new CustomerManagerFacade().InitializeUpdateBadgeCustomerBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0364":
                        result = new PartManagerFacade().SendMailForPartCriticalStock();
                        break;

                    case "DNVR_AUTOMIC_JOB_0365":
                        RetCode retCodeDod = FinanceManagerFacade.ExecuteGoldPartnerPriceTableBatch();
                        switch (retCodeDod)
                        {
                            case RetCode.Fail:
                                retCode = 1;
                                break;
                            case RetCode.Success:
                                retCode = 0;
                                break;
                            default:
                                retCode = (int)retCodeDod;
                                break;
                        }

                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0367":
                        retCode = new PartManagerFacade().DomesticSupplyInvoiceImport();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0369":
                        retCode = new FinanceManagerFacade().SendPendingMail(); //Lauren Di Fonseca. 20050709
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0375":
                        result = new PartManagerFacade().SendMailForOrderDealerShipment();
                        break;

                    case "DNVR_AUTOMIC_JOB_0376":
                        //retCode = new EmployeeManagerFacade().SendDepartmentApprovalWaitingMail(); //TODO: Hotfix required
                        //result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0381":
                        retCode = new FinanceManagerFacade().BatchRun();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0383":
                        retCode = new CustomerManagerFacade().UpdateStatusForExpiredSubcontractorRecords();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0384":
                        retCode = new CustomerManagerFacade().ListAndSendMailForPendingDealerRecords();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0387":
                        RetCode retCodeRenew = new CustomerManagerFacade().GetRenewInsuranceCustomerCallList();
                        switch (retCodeRenew)
                        {
                            case RetCode.Fail:
                                retCode = 1;
                                break;
                            case RetCode.Success:
                                retCode = 0;
                                break;
                            default:
                                retCode = (int)retCodeRenew;
                                break;
                        }
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    //case "DNVR_AUTOMIC_JOB_0388":
                    //    result = new DepotManagemerFacade().SendMessageForHazardousGoodsTransaction();
                    //    break;
                    case "DNVR_AUTOMIC_JOB_0389":
                        retCode = new DealerNetworkManagerFacade().BonusCalculationForSeat();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0391":

                        string dealerServiceIsActive = "";
                        #region Get parameter values fo username and password
                        try
                        {
                            ParameterManager prmMngr = new ParameterManager();
                            DataSet ds = prmMngr.GetParameter("DealerServiceSettings");

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    if (row["code"].ToString() == "10")
                                    {
                                        dealerServiceIsActive = row["parameter_value"].ToString();
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        #endregion Get parameter values

                        if (dealerServiceIsActive == "true")
                        {
                            result = new DealerNetworkManagerFacade().UpdateAllFirmsRights();
                        }
                        else
                        {
                            result = new FileTransferOperationsFacade().ExecuteSSIS(parameters[0].ToString());
                        }
                        break;
                    case "DNVR_AUTOMIC_JOB_0392":
                        retCode = new FinanceManagerFacade().SendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;


                    case "DNVR_AUTOMIC_JOB_0402":
                        //retCode = new WSVehicleIFAEntegration().DoIFAOemStatusEnquery();
                        //result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        //break;
                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://192.168.1.5/Admin/NonAuthenticated/default.aspx?check=106");
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://denver.us:45879/Admin/NonAuthenticated/default.aspx?check=95647");
                        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                        response.Close();
                        result = new BatchResultStruct { ReturnCode = 0, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0403":
                        retCode = new FinanceManagerFacade().SendInvoicePendingMail(); 
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0407":
                        result = new CommonFacade().CloseOpenLocatedCagesForImportInvoice(); //Op_20140220                        
                        break;

                    case "DNVR_AUTOMIC_JOB_0409":
                        result = new CommonFacade().SentEODServiceResultInfo(); //Op_20140220                        
                        break;


                    case "DNVR_AUTOMIC_JOB_0415":
                        int daysCount = 0;
                        DateTime limitDate = DateTime.Today.AddDays(-90);
                        if (parameters.Length > 0)
                        {
                            Int32.TryParse(parameters[0].ToString(), out daysCount);

                            if (daysCount > 0)
                            {
                                limitDate = DateTime.Today.AddDays(-1 * daysCount);
                            }
                        }
                        retCode = new FinanceManagerFacade().GetStatusUpdateCandidates(limitDate); //OS_201400416
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0412":
                        retCode = new FinanceManagerFacade().GetWaitingEInvoicesForSendingBatch(); //OS_20140428
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0413":
                        result = new CommonFacade().SendStatusUpdateNotificationMailAndSMS(); //BOY_20140512                        
                        break;

                    case "DNVR_AUTOMIC_JOB_0414":
                        retCode = new CommonFacade().SendTestVehiclesForExpiredMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0416":
                        result = new CommonFacade().SentLogErrorMails();
                        break;

                    case "DNVR_AUTOMIC_JOB_0422":
                        retCode = new FinanceManagerFacade().SendInvoiceListToGoldPartner(); //OS_20140717
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0423":
                        retCode = new CustomerManagerFacade().ListAndSendMailFirmCustomerToBeCleared();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0424":
                        result = new PartManagerFacade().UpdatePartPlanningStatusForModelStockListParts();
                        break;
                    case "DNVR_AUTOMIC_JOB_0425":
                        result = new PartManagerFacade().UpdateAllModelStockListWithSupersession();
                        break;
                    case "DNVR_AUTOMIC_JOB_0426":
                        result = new PartManagerFacade().TransferCatalogSupersessions();
                        break;

                    case "DNVR_AUTOMIC_JOB_0427":
                        retCode = new FinanceManagerFacade().CreateInvoicesFromWaybillsBatch(); //OS_20140919
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0430":
                        retCode = new PartManagerFacade().SendMailForOpenDomesticOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0431":
                        retCode = new CustomerManagerFacade().SearchFirmCanceledRequest();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0433":
                        retCode = new CustomerManagerFacade().ListAndFindSecondHandUnsoldReplacementPartsAndSendEmailToRel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0434":
                        retCode = new CustomerManagerFacade().CustomerVehicleServiceBadge();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0435":
                        result = new CustomerManagerFacade().SendDeclareTransferReminderMail(); //BOY_20141205                      
                        break;
                    case "DNVR_AUTOMIC_JOB_0436":
                        retCode = new OrderManagerFacade().GetWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0438":
                        retCode = new DealerNetworkManagerFacade().BonusCalculationForSeat();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0440":
                        retCode = new FinanceManagerFacade().SendNotificationMailForGuaranteeLetterPayments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0443":
                        retCode = new FinanceManagerFacade().SendNotificationMailForExpiringGuaranteeLetters();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0442":
                        result = new CustomerManagerFacade().SendPermittedMarketingReportMail();//BOY_20141222                        
                        break;
                    case "DNVR_AUTOMIC_JOB_0448":
                        retCode = new SalesManagerFacade().FillMaintenanceDateAndPremiumCustomerForMaintenanceBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0452":
                        retCode = new CommonFacade().SendDepartmentApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0453":
                        retCode = new FinanceManagerFacade().SendFinanceApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0459":
                        retCode = new SalesManagerFacade().DailyMails();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PSMSDONP":
                        result = new PartManagerFacade().SendMailForOrderDealerNoPriceAll();
                        break;
                    case "PSMSPCHS":
                        result = new PartManagerFacade().SendMailForPartCatalogHistory();
                        break;
                    case "PSMSDTSC":
                        result = new PartManagerFacade().SendMailForPartDifferentSPFOnSupersessionChain();
                        break;
                    case "DNVR_AUTOMIC_JOB_465":
                        retCode = new PartManagerFacade().GetOrderStatusFromUSAPartner();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "DNVR_AUTOMIC_JOB_0466":
                        retCode = new EmployeeManagerFacade().EducationEvaluationFormSecondLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0467":
                        retCode = new EmployeeManagerFacade().EducationEvaluationFormThirdLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0468":
                        retCode = new EmployeeManagerFacade().EducationEvaluationFormFourthLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0471":
                        retCode = new EmployeeManagerFacade().SendUpdatePersonnelHrInfoMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX001":
                        retCode = new CustomerManagerFacade().CompleteServiceContractChassisBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX002":
                        retCode = new SalesManagerFacade().DeliveryNotificationMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0474":
                        result = new CustomerManagerFacade().ProcessCampaignOfBPActivation();                    
                        break;
                    case "DNVR_AUTOMIC_JOB_0475":
                        result = new CustomerManagerFacade().ProcessCampaignOfBPActivationAfterStatusChanged();                   
                        break;

                    case "DNVR_AUTOMIC_JOB_0477":
                        result = new PartManagerFacade().TransferUserPriority();
                        break;
                    case "DNVR_AUTOMIC_JOB_0478":
                        result = new PartManagerFacade().CalculateOEMOrderPriorityForTruckInvoices();
                        break;
                    case "DNVR_AUTOMIC_JOB_0464":
                        result =null;
                        break;



                    case "DNVR_AUTOMIC_JOB_0484":
                        retCode = new CustomerManagerFacade().SendEmailForInvalidEmailSurveyPool();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0489":
                        result = new CustomerManagerFacade().PermissionUpdateForUnsubscribedEmailsBatch();                      
                        break;

                    case "DNVR_AUTOMIC_JOB_0490":
                        result = new PartManagerFacade().SendMessageForADRTypeParts();                       
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX003":
                        retCode = new SalesManagerFacade().GetWeatherInformation();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_SPP001":
                        result = new PartManagerFacade().UpdateDealerBaseStockPartsWithoutStockCards();
                        break;
                    case "DNVR_AUTOMIC_JOB_SPS001":
                        retCode = new PartManagerFacade().RemoveAllLocatedOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PSMSUPSM":
                        result = new PartManagerFacade().SendMailForUnPickedSlips();
                        break;
                    case "SPS_UNPICKED_SLIPS_CUTOFF_MAIL":
                        result = new PartManagerFacade().SendMailForUnPickedSlipsForCutOff();
                        break;
                    case "SPS_UNPACKED_SLIPS_CUTOFF_MAIL":
                        result = new PartManagerFacade().SendMailForUnPackedSlipsForCutOff();
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX004":
                        retCode = new SalesManagerFacade().SendTransactionDataToDMS();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0499":
                        retCode = new FinanceManagerFacade().SendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0500":
                        result = new CustomerManagerFacade().EventPostPoolCalculationBatch();                    
                        break;
                    case "DNVR_AUTOMIC_JOB_SPD001":
                        result = new PartManagerFacade().BatchUpdatePartSupersessionRules();
                        break;
                    case "DNVR_AUTOMIC_JOB_SPP002":
                        result = new PartManagerFacade().CalculatePartLTValues();
                        break;
                    case "DNVR_AUTOMIC_JOB_0505":
                        result = new CustomerManagerFacade().SendEventPostStatusMailBatch();                       
                        break;
                    case "DNVR_AUTOMIC_JOB_0507":
                        result = new CustomerManagerFacade().EventPostPoolStatusFTPBatch();                    
                        break;
                    case "DNVR_AUTOMIC_JOB_0508":
                        result = new CustomerManagerFacade().EventPostPoolStatusFOBatch();                     
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX006":
                        retCode = new SalesManagerFacade().AdditionalWarrantyDaysCalculateDaily(DateTime.Today);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0509":
                        result = new CustomerManagerFacade().EmissionSurveyDeleteFromSSHCampaign();            
                        break;
                    case "DNVR_AUTOMIC_JOB_0510":
                        result = new CustomerManagerFacade().CalculateBadgeCustomer();    
                        break;
                    case "DNVR_AUTOMIC_JOB_0511":
                        result = new CustomerManagerFacade().DeleteBadgeCustomer();          
                        break;
                    case "DNVR_AUTOMIC_JOB_0513":
                        result = new CustomerManagerFacade().UpdateCustomerAddressCoordinate();      
                        break;
                    case "DNVR_AUTOMIC_JOB_0516":
                        result = new CustomerManagerFacade().EventRegularPoolCalculationBatch();                      
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX007":
                        retCode = new PartManagerFacade().CreateAndCalculateQualifiedServicePremiumPeriod();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_SPR001":
                        result = new PartManagerFacade().CreateAndFillFOBPriceExtension();
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX011":
                        retCode = new SalesManagerFacade().FleetContractWarningMailDaily(); //1818
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX012":
                        retCode = new SalesManagerFacade().RecallCampaignTargetPercantageMailDaily(); //1890
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0518":
                        result = new CustomerManagerFacade().SendHoldingLeadRequestInformationMail();       
                        break;
                    case "AcceptTurkuazInvoices":
                        retCode = new FinanceManagerFacade().AcceptInvoicesWithBatch(); //Eduardo_20162709
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_0524":
                        retCode = new PartManagerFacade().UpdateWebOrderStatusAndSendMail(); //BabeK_16112016
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DNVR_AUTOMIC_JOB_ASX013":
                        retCode = new SalesManagerFacade().CustomerSurveyBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    
                    case "SALE_GAC_LOG":
                        {
                            RetCode retCodeGAC = CommonFacade.ProcessWebRequests(DateTime.Today.AddDays(-1).Date.AddHours(-2), DateTime.Now); //BSŞ_20140209
                            switch (retCodeGAC)
                            {
                                case RetCode.Success:
                                    retCode = 0;
                                    break;
                                case RetCode.Fail:
                                default:
                                    retCode = 1;
                                    break;
                            }
                        }
                        break;

                    case "SPD_UPDATEPARTSUPERSESSIONWITHDIFF":
                        result = new PartManagerFacade().BatchUpdatePartSupersessionRulesForCatalogDiff();
                        break;
                    case "SPR_SENDMAILSEATACCESSORYSELLING":
                        result = new PartManagerFacade().SendMailSeatAccessorySelling();
                        break;
                    case "SPR_BENCHBONUS":
                        result = new PartManagerFacade().CreateDailyBenchBonus();
                        break;
                    case "SPR_DIRECTSHIPMENTCONVERSION":
                        retCode = new PartManagerFacade().DirectShipmentConversion();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_DIRECTSHIPMENTTRANSFERSENDMAIL":
                        retCode = new PartManagerFacade().DirectShipmentTransferSendMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "SPD_CALCULATEDAILYDEPOTPERFORMANCE":
                        result = new PartManagerFacade().ExecuteDailyPerformanceBatch();
                        break;

                    case "VEH_ORDER_IFA_FOB_BATCH":
                        HttpWebRequest reqFOB = (HttpWebRequest)WebRequest.Create("http://turkuaz.dohas.com.tr/Admin/NonAuthenticated/UITurkuazCheck.aspx?check=107"); ////CE_20170524
                        HttpWebResponse responseFOB = (HttpWebResponse)reqFOB.GetResponse();
                        responseFOB.Close();
                        result = new BatchResultStruct { ReturnCode = 0, DataReturned = null };
                        break;
                    case "CUS_PENDINGEMARKETINGPOOLSINFORMATIONMAIL":
                        result = new CustomerManagerFacade().SendEmailForPendingEmarketingPools();
                        break;
                    case "CUS_FAILEDEMARKETINGEVENTPOSTUPDATE":
                        result = new CustomerManagerFacade().UpdateFailedEMarketingEventPostStatus();
                        break;
                    case "SPR_IncomingOEMOrderRejectionInfoProcess":
                        retCode = new PartManagerFacade().BatchIncomingOEMOrderRejectionInfoProcess();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_NOTIFYFORUNINVOICEDORDERS_FOR_TESTING":
                        retCode = new CommonFacade().SendNotificationMailForUninvoicedTestFirms();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_ADDSPEECHTEXTTOCALLDETAIL":
                        retCode = new CustomerManagerFacade().AddSpeechTextToCallDetail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CM_NOTIFY_MISSINGDATA":
                        retCode = new CommonFacade().SendCountryModelMissingEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_CLAIMSTATUS":
                        retCode = new SalesManagerFacade().CallClaimStatusAndUpdate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_UPDATECONTRACTSTATUS":
                        retCode = new SalesManagerFacade().UpdateContractStatusBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_UPDATECAMPAIGNSTATUS":
                        retCode = new SalesManagerFacade().UpdateCampaingMngStatusBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_ADD_PERMISSION_INSTANT":
                        retCode = new CustomerManagerFacade().AddMultiplePermissionInstantAsyncConsent();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_ADD_DEALER_PERMISSION":
                        retCode = new CustomerManagerFacade().AddMultipleAddDealerPermissionAsync();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_DELETE_DEALER_PERMISSION":
                        retCode = new CustomerManagerFacade().AddMultipleDeleteDealerPermissionAsync();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_UPDATE_API_TYPE":
                        retCode = new CustomerManagerFacade().IYSUpdateApiType();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_UPDATE_FROM_IYS":
                        retCode = new CustomerManagerFacade().UpdatingPermissionsFromIysAsync();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PERMISSION_PULL_FROM_IYS":
                        retCode = new CustomerManagerFacade().PermissionPull();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PERMISSION_UPDATE_FROM_IYS":
                        retCode = new CustomerManagerFacade().PermissionUpdate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_NOTSHOWUP_MAIL":
                        retCode = new SalesManagerFacade().AppointmentNotShowUpNotificationMailDaily();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_ADD_POOL_STOCK_UNUSED_PARTS":
                        retCode = new PartManagerFacade().AddPoolStockUnusedParts();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SEND_MAIL_FOR_NOT_EXIT_ENTREPOT":
                        retCode = new PartManagerFacade().SendMailForNotEntrepotExitInvoice();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "OPEN_CONTACT_NOT_CLOSED_MAIL":
                        retCode = new CustomerManagerFacade().OpenContactNotClosedSendMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_EXP_APP_MAIL":
                        retCode = new FinanceManagerFacade().ExpenseSheetSendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_PYM_APP_MAIL":
                        retCode = new FinanceManagerFacade().PaymentRequestSendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SMS_AUTOPILOT_SALES_CUSTOMER_DATA":
                        retCode = new CustomerManagerFacade().MessageCustomerSalesSendDataFTP();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CALCULATE_INSIGHT_DASHBOARD_REPORT":
                        retCode = new CustomerManagerFacade().CalculateInsightDashboardReport();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    default:
                        retCode = (int)RetCode.GNLBatchJobCodeNotFound;
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                }

                return result;

            }
            catch (Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                //exceptionManager.Error(ex.Message);

                DataSet dsReturn = new DataSet("BatchLog");
                dsReturn.Tables.Add("Exception");
                dsReturn.Tables["Exception"].Columns.AddRange(new DataColumn[] { new DataColumn("ExceptionType", typeof(string)), new DataColumn("Message", typeof(string)), new DataColumn("ExceptionInfo", typeof(string)) });
                dsReturn.Tables["Exception"].Rows.Add(new object[] { ex.GetType().ToString(), ex.Message, ex.ToString() });
                result.ReturnCode = 999999;
                result.DataReturned = dsReturn.Copy();

                return result;

            }
            finally
            {
                automicJobFacade.UpdateFinishDate(jobId, result.ReturnCode, false);
            }

        }
        #endregion Execute Job
    }
}
