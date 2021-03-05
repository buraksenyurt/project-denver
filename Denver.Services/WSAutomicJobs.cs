using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Services;
using Denver.Common;
using Denver.Common.Services;
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
                        result = new WSOrderFollowUp().ProcessStatusInquiry();
                        break;

                    //Part Pricing
                    case "METQPPR1":
                        retCode = new WSPartPricing().UpdatePartPriceBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //End Of Period
                    case "METQEOP1":
                        result = new WSEndOfPeriod().ExecuteProcessBatch();
                        break;

                    case "METQEOP4":
                        result = new WSEndOfPeriod().ExecuteProcessBatchAll();
                        break;

                    //FinanceCurrencyFixing
                    case "METQFIN2":
                        retCode = new WSCurrencyFixing().Fixing();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinancePOGG
                    case "METQFIN3":
                        retCode = new WSPOGG().POGG();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceDebtAgingDaily //ED.20150926 Hoze Luiz Gonzaled De Salvatore
                    case "METQFIN7":
                        retCode = new WSDebtAgingDaily().DebtAgingDaily();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceOFOBatch
                    case "METQFIN8":
                        retCode = new WSOFO().MakeOFO();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceBOtoGBBatch
                    case "MDTQFN10":
                        retCode = new WSBOToGB().ProcessGBData();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceVDFCreditBatch
                    case "MDTQFN11":
                        retCode = new WSVDFCredit().VDFCredit(int.Parse(parameters[0].ToString()));
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CentralBankExchangeRate
                    case "TMTQCBER":
                        retCode = new WSExchangeRate().CentralBankExchangeRateAddAll();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CentralBankExchangeRate_MUH
                    case "TMTQCBER_MUH":
                        retCode = new WSExchangeRate().AddExchangeRateForAccountingFirms();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //ClearReportingFilesFolder
                    case "METQARC2":
                        result = new WSGeneralArchiving().ClearReportingFilesFolder();
                        break;

                    //RewardSystemCalculationVW
                    case "METQDNT1":
                        retCode = new WSDealerNetwork().RewardSystemCalculationVW();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //AfterSalesSendCollectiveSMS
                    case "TMTQAS01":
                        retCode = new WSAfterSalesSendCollectiveSMS().SendCollectiveSMSForAppointment();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //RewardSystemCalculationAudi
                    case "METQDNT2":
                        retCode = new WSDealerNetwork().RewardSystemCalculationForAudi();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //FinanceCreditAgingBatch
                    case "TMTQFN12":
                        retCode = new WSCreditAging().CreditAging();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //ClearDepotFoldersDODImages
                    case "METQARC3":
                        result = new WSGeneralArchiving().ClearDepotFoldersDODImages();
                        break;

                    //FinanceMultiplePrintDocumentsDelete
                    case "TMTQFN13":
                        retCode = new WSMultiplePrintDocuments().DeleteMultiplePrintDocuments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //WSWarrantyRequestOperation
                    case "TMTQAS02":
                        retCode = new WSWarrantyRequestOperation().CalculateAverageCompletionTimeForWR();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //CLAWWarrantyService
                    case "TMTQAS03":
                        new WSCLAWWarrantyService().SendWarrantyRequestToCLAW();
                        result = new BatchResultStruct { ReturnCode = 0, DataReturned = null };
                        break;

                    //WSWarrantyRequestOperation
                    case "TMTQFN14":
                        retCode = new WSAgreementForm().AgreementForm();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0271":
                        retCode = new WSPartPoolStock().DeletePoolStockPartsThatHasNoPoolStock();
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

                    case "TMTQ0371":
                        result = new PartManagerFacade().ExecutePlannedUrgentOrderDrafts();
                        break;







                    //FinanceOTSBatch
                    case "TMTQ0292":
                        retCode = new WSOTS().MakeOTS();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0304":
                        retCode = new SalesManagerFacade().AddVehicleInformAll();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0306":
                        retCode = new WSFinanceApprovalRecordsPendingMail().SendPreExpneseApprovalRecordsPendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part Direct Selling - Create Incoming Invoice
                    case "PARTDINV":
                        retCode = new WSPartDirectSellingIncomingInvoice().CreateInvoiceForDomesticInvoiceDraftFiles();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0311":
                        retCode = new SalesManagerFacade().SendVehicleMaintenanceAndExaminationSMSForFirms();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Part OEM Order - Process Confirmation
                    case "PARTOEMC":
                        retCode = new WSPartOEMOrder().ProcessConfirmation();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    //Abnormal Order
                    case "PARTOEMO":
                        retCode = new WSPartOEMOrder().BatchAbnormalOrderProcess();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;


                    case "TMTQ0372":
                        result = new WSPartOEMOrder().SentBackOrderInfoMails();
                        break;

                    case "LAWNTF01":
                        retCode = new WSDealerNetwork().ListEndDateComingLawNotificationsAndSendEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0316":
                        retCode = new CustomerManagerFacade().ListEndDateComingContractsAndSendEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0318":
                        retCode = new SalesManagerFacade().EmailManagersAndChiefsofUninvoicedWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0317":
                        retCode = new SalesManagerFacade().EmailManagersAndChiefsofUndeliveredWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0357":
                        retCode = new SalesManagerFacade().SendEmailForCustomerStandartPackageBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0320":
                        retCode = new FinanceManagerFacade().SendPurchasePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TDTQ0152":
                        retCode = new FinanceManagerFacade().UpdateDueDateBatchForPrivateInterval();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0324":
                        retCode = new FinanceManagerFacade().SendPurchasePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0325":
                        result = new OEMInvoiceTransfer().BatchProcessInvoiceOEMFOBPriceCalculation();
                        break;
                    case "TMTQ0332":
                        result = new WSVehicleImportPaymentTermReport().SendMessageForOEMInvoicePaymentTermReport();
                        break;
                    case "TMTQ0333":
                        result = new OEMInvoiceTransfer().BatchProcessInvoiceOEMFOBPriceDifferenceCalculation();
                        break;
                    case "TMTQ0334":
                        retCode = new CustomerManagerFacade().AddAutomaticContactsForJobs();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0342":
                        retCode = new FinanceManagerFacade().SendBalanceResetPendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0343":
                        retCode = new WSOFOPendingMail().SendOFOPendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0345":
                        retCode = new FinanceManagerFacade().SendInvoicePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0346":
                        retCode = new FinanceManagerFacade().SendReturnedInvoicePendingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0421":
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

                    case "TMTQ0349":
                        retCode = new FinanceManagerFacade().AddPosBlockage();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "PDLBUYBC":
                        retCode = new PartManagerFacade().BuyBackDealerDraftCreate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0360"://Badge customer initializa and update batch
                        retCode = new CustomerManagerFacade().InitializeUpdateBadgeCustomerBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0364":
                        result = new PartManagerFacade().SendMailForPartCriticalStock();
                        break;

                    case "TMTQ0365":
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
                    case "TMTQ0367":
                        retCode = new PartManagerFacade().DomesticSupplyInvoiceImport();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0369":
                        retCode = new WSFinanceMoneyTrackingPendingMail().SendPendingMail(); //SK.20130709
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0375":
                        result = new WSPartSellingMessage().SendMailForOrderDealerShipment();
                        break;

                    case "TMTQ0376":
                        //retCode = new WSApprovalExpiration().SendDepartmentApprovalWaitingMail(); //OS_20130601
                        //result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0377":
                        retCode = new WSApprovalExpiration().ExpirationEndOfDayJobs(DateTime.Today); //OS_20130601
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0378":
                        retCode = new WSApprovalExpiration().SendApprovedRequestsMailJob(); //OS_20130601
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0381":
                        retCode = new WSFinanceIntegrationProcessPosBlockage().BatchRun(); //SK.20130911
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0383":
                        retCode = new CustomerManagerFacade().UpdateStatusForExpiredSubcontractorRecords();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0384":
                        retCode = new CustomerManagerFacade().ListAndSendMailForPendingDealerRecords();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0387":
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
                    case "TMTQ0388":
                        result = new WSDepotEntrance().SendMessageForHazardousGoodsTransaction();
                        break;
                    case "TMTQ0389":
                        retCode = new WSDealerNetwork().BonusCalculationForSeat();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0391":

                        string eLogoActive = "";
                        #region Get parameter values fo username and password
                        try
                        {
                            ParameterCall paramCall = new ParameterCall();
                            DataSet ds = paramCall.GetParameter(EnumParameterGeneral.ELogoServiceSettings);

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    if (row["code"].ToString() == "10")
                                    {
                                        eLogoActive = row["parameter_value"].ToString();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Publish(ex);
                        }
                        #endregion Get parameter values

                        if (eLogoActive == "1")
                        {
                            result = new WSELogoAmenableFirm().UpdateAmenableFirms();
                        }
                        else
                        {
                            result = new FileTransferOperationsFacade().ExecuteSSIS(parameters[0].ToString());
                            if (result.ReturnCode != 0)
                            {
                                return result;
                            }
                            result = new WSCommonFirm().UpdateEInvoiceAmenableFeature();
                        }
                        break;
                    case "TMTQ0392":
                        retCode = new WSFinanceEInvoiceApprovalWaitingMail().SendApprovalWaitingMail(); //SK.20131224
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;


                    case "TMTQ0402":
                        //retCode = new WSVehicleIFAEntegration().DoIFAOemStatusEnquery();
                        //result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        //break;
                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://192.168.75.54/Admin/NonAuthenticated/UITurkuazCheck.aspx?check=106");
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://turkuaz.dohas.com.tr/Admin/NonAuthenticated/UITurkuazCheck.aspx?check=106");
                        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                        response.Close();
                        result = new BatchResultStruct { ReturnCode = 0, DataReturned = null };
                        break;

                    case "TMTQ0403":
                        retCode = new WSEInvoiceFailures().SendInvoicePendingMail(); //OS_20140131
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0407":
                        result = new WSDepotEntrance().CloseOpenLocatedCagesForImportInvoice(); //Op_20140220                        
                        break;

                    case "TMTQ0409":
                        result = new WSBatchManager().SentEODServiceResultInfo(); //Op_20140220                        
                        break;


                    case "TMTQ0415":
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
                        retCode = new WSEInvoiceFailures().GetStatusUpdateCandidates(limitDate); //OS_201400416
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0412":
                        retCode = new WSEInvoiceFailures().GetWaitingEInvoicesForSendingBatch(); //OS_20140428
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0413":
                        result = new WSVehicleDeliveryRecordSendMail().SendStatusUpdateNotificationMailAndSMS(); //BOY_20140512                        
                        break;

                    case "TMTQ0414":
                        retCode = new WSVehicleSendFirmVehicleWarningMail().SendTestVehiclesForExpiredMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0416":
                        result = new WSLogControl().SentLogErrorMails();
                        break;

                    case "TMTQ0422":
                        retCode = new WSFinanceSendVDFInvoices().SendInvoiceListToVDF(); //OS_20140717
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0423":
                        retCode = new CustomerManagerFacade().ListAndSendMailFirmCustomerToBeCleared();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0424":
                        result = new PartManagerFacade().UpdatePartPlanningStatusForModelStockListParts();
                        break;
                    case "TMTQ0425":
                        result = new PartManagerFacade().UpdateAllModelStockListWithSupersession();
                        break;
                    case "TMTQ0426":
                        result = new WSPartDefinition().TransferCatalogSupersessions();
                        break;

                    case "TMTQ0427":
                        retCode = new WSInvoiceBatch().CreateInvoicesFromWaybillsBatch(); //OS_20140919
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0430":
                        retCode = new PartManagerFacade().SendMailForOpenDomesticOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0431":
                        retCode = new WSVehicleSendFirmVehicleWarningMail().SearchFirmVehicleCanceledRequest();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0433":
                        retCode = new CustomerManagerFacade().ListSecondHandUnsoldReplacementVehiclesAndSendEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0434":
                        retCode = new WSCustomerVehicleServiceBadge().CustomerVehicleServiceBadge();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0435":
                        result = new CustomerManagerFacade().SendDeclareTransferReminderMail(); //BOY_20141205                      
                        break;
                    case "TMTQ0436":
                        retCode = new WSRoadsideAssistantStatusIntegration().GetWorkOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0438":
                        retCode = new WSDealerNetwork().BonusCalculationForSeat();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0440":
                        retCode = new WSFinanceGuaranteeLettersNotificationMail().SendNotificationMailForGuaranteeLetterPayments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0443":
                        retCode = new WSFinanceGuaranteeLettersNotificationMail().SendNotificationMailForExpiringGuaranteeLetters();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0442":
                        result = new CustomerManagerFacade().SendPermittedMarketingReportMail();//BOY_20141222                        
                        break;
                    case "TMTQ0448":
                        retCode = new SalesManagerFacade().FillMaintenanceDateAndPremiumCustomerForVehicleMaintenanceBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0452":
                        retCode = new WSFinancePurchasinPricelistPendingMail().SendDepartmentApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0453":
                        retCode = new WSFinancePurchasinPricelistPendingMail().SendFinanceApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0455":
                        retCode = new WSVehicleSendFirmVehicleWarningMail().SendFuelExpenseDetailMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0456":
                        retCode = new WSLawFirmContractTerminationPeriodInform().SendMailContractNotification();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0459":
                        retCode = new SalesManagerFacade().ASXDailyMails();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0457":
                        retCode = new WSVehicleDODHeavyVehicleOperation().DeleteOptionBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PSMSDONP":
                        result = new WSPartSellingMessage().SendMailForOrderDealerNoPriceAll();
                        break;
                    case "PSMSPCHS":
                        result = new WSPartSellingMessage().SendMailForPartCatalogHistory();
                        break;
                    case "PSMSDTSC":
                        result = new WSPartSellingMessage().SendMailForPartDifferentSPFOnSupersessionChain();
                        break;
                    case "TMTQ465":
                        retCode = new WSPartDirectSellingIncomingInvoice().GetOrderStatusFromBrisa();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0466":
                        retCode = new WSPersonnel().EducationEvaluationFormSecondLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0467":
                        retCode = new WSPersonnel().EducationEvaluationFormThirdLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0468":
                        retCode = new WSPersonnel().EducationEvaluationFormFourthLevel();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0469":
                        retCode = new WSVehicleADSalesOption().SendMailForPendingFleetPoolVehicleRecords();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0470":
                        retCode = new WSVehicleADContactAndSalesManagement().SendMailForNotFinishedDataEntryForMorningReports();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0471":
                        retCode = new WSPersonnel().SendUpdatePersonnelHrInfoMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX001":
                        retCode = new WSWorkOrderScaniaServiceContract().CompleteServiceContractChassisBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX002":
                        retCode = new SalesManagerFacade().DeliveryNotificationMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0474":
                        result = new CustomerManagerFacade().ProcessCampaignOfBPActivation();//BOY_20150525                      
                        break;
                    case "TMTQ0475":
                        result = new CustomerManagerFacade().ProcessCampaignOfBPActivationAfterStatusChanged(); //BOY_20150526                      
                        break;
                    case "TMTQ0476":
                        retCode = new WSScaniaCarePortAgreementInvoicesIncomeAccounting().ScaniaCarePortAgreementIncomeAccountingBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "TMTQ0477":
                        result = new WSPartOEMOrder().TransferUserPriority();
                        break;
                    case "TMTQ0478":
                        result = new WSPartOEMOrder().CalculateOEMOrderPriorityForTruckInvoices();
                        break;
                    case "TMTQ0464":
                        result = GetOperationResult(FTurkuazGOContainer.Resolve<GT.Turkuaz.SpareParts.Catalog.FCatalog>().AddPartCatalogHistoryForCatalogUpdate(), null);
                        break;

                    case "TMTQ0482":
                        result = new WSVehicleAutomaticInvoiceApproval().InvoiceAprove();
                        break;

                    case "TMTQ0484":
                        retCode = new CustomerManagerFacade().SendEmailForInvalidEmailSurveyPool();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0487":
                        retCode = new WSVehicleCOCAsbisM1Pattern().MatchNTAVehiclesWithCOCAsbisM1Patterns();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0489":
                        result = new CustomerManagerFacade().PermissionUpdateForUnsubscribedEmailsBatch(); //BOY_20150826                      
                        break;

                    case "TMTQ0490":
                        result = new WSDepotEntrance().SendMessageForADRTypeParts(); //BOY_20150826                      
                        break;
                    case "TMTQASX003":
                        retCode = new SalesManagerFacade().GetWeatherInformation();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQSPP001":
                        result = new PartManagerFacade().UpdateDealerBaseStockPartsWithoutStockCards();
                        break;
                    case "TMTQSPS001":
                        retCode = new PartManagerFacade().RemoveAllLocatedOrders();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "PSMSUPSM":
                        result = new WSPartSellingMessage().SendMailForUnPickedSlips();
                        break;
                    case "SPS_UNPICKED_SLIPS_CUTOFF_MAIL":
                        result = new WSPartSellingMessage().SendMailForUnPickedSlipsForCutOff();
                        break;
                    case "SPS_UNPACKED_SLIPS_CUTOFF_MAIL":
                        result = new WSPartSellingMessage().SendMailForUnPackedSlipsForCutOff();
                        break;
                    case "TMTQ0494":
                        retCode = new WSEInvoiceGetHashCodeForInvoicesFromPortal().GetEInvoiceHashCodeForInvoicesFromPortal();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0496":
                        retCode = new WSVehicleScaniaMulti().ScaniaChassisTechnicalInformationRequest();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX004":
                        retCode = new SalesManagerFacade().SendTransactionDataToDMS();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0498":
                        retCode = new WSVehicleScaniaMulti().WVBankStatementSendMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0499":
                        retCode = new WSFinanceEArchiveServices().SendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0500":
                        result = new CustomerManagerFacade().EventPostPoolCalculationBatch(); //BOY_20151214                      
                        break;
                    case "TMTQSPD001":
                        result = new WSPartDefinition().BatchUpdatePartSupersessionRules();
                        break;
                    case "TMTQASX005":
                        retCode = new WSCLAWWarrantyService().GetClawInformationWR();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQSPP002":
                        result = new PartManagerFacade().CalculatePartLTValues();
                        break;
                    case "TMTQ0502":
                        retCode = new WSVehicleDODOperation().SendMailForFallPriceVehicle();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0504":
                        retCode = new WSFinanceEArchiveServices().SendWaitingEArchive();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_UPDATE_WAYBILL_STATUS_FROM_LOGO": //BSŞ_20200617
                        retCode = new WSFinanceEWaybillServices().UpdateWaybillStates();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_UPDATE_VEHICLE_WAYBILL_STATUS_FROM_LOGO": //BSŞ_20200619
                        retCode = new WSFinanceEWaybillServices().UpdateWaybillStates(isVehicleWaybill: true);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0505":
                        result = new CustomerManagerFacade().SendEventPostStatusMailBatch(); //BOY_20151214                      
                        break;
                    case "TMTQ0507":
                        result = new CustomerManagerFacade().EventPostPoolStatusFTPBatch(); //BOY_20160310                      
                        break;
                    case "TMTQ0508":
                        result = new CustomerManagerFacade().EventPostPoolStatusFOBatch(); //BOY_20160310                      
                        break;
                    case "TMTQASX006":
                        retCode = new SalesManagerFacade().AdditionalWarrantyDaysCalculateDaily(DateTime.Today);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ORDER_IFA_UNSENT_MESSAGES_MAIL":
                        retCode = new WSVehicleIFAEntegration().SendMailForIFAUnsentMessages(); //AC_20160322                      
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0509":
                        result = new CustomerManagerFacade().EmissionSurveyDeleteFromSSHCampaign(); //kubraokumus_20160418            
                        break;
                    case "TMTQ0510":
                        result = new CustomerManagerFacade().CalculateBadgeCustomer(); //kubraokumus_201606003      
                        break;
                    case "TMTQ0511":
                        result = new CustomerManagerFacade().DeleteBadgeCustomer(); //kubraokumus_20160603            
                        break;
                    case "VEH_ORDER_IFA_UNSENT_MESSAGES_RESEND":
                        retCode = new WSVehicleIFAEntegration().ResendIFAUnsentMessages(); //AC_20160322                      
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0513":
                        result = new CustomerManagerFacade().UpdateCustomerAddressCoordinate(); //kubraokumus_20160712        
                        break;
                    case "TMTQ0514":
                        result = new WSVehicleStockEntegration().UpdateStockList(); //tubag 13.07.2016        
                        break;
                    case "TMTQ0515":
                        retCode = new WSVehicleOrderSalesAnalysis().FillVehicleOrderSalesData(); //AC_20160620        
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0516":
                        result = new CustomerManagerFacade().EventRegularPoolCalculationBatch(); //BOY_20160823                      
                        break;
                    case "TMTQASX007":
                        retCode = new WSQualifiedServicePremiumProcess().CreateAndCalculateQualifiedServicePremiumPeriod();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQSPR001":
                        result = new WSPartPricing().CreateAndFillFOBPriceExtension();
                        break;
                    case "TMTQ519":
                        retCode = new WSAutomaticAcceptInvoiceBatch().AcceptTurkuazInvoicesWithBatch(); //HandeC_20162709
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX008":
                        retCode = new WSAfterSalesScaniaC200Entegration().InsertGetVehicleDataResponse();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX009":
                        retCode = new WSAfterSalesScaniaC200Entegration().InsertGetVehicleExceptionResponse();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX010":
                        retCode = new WSAfterSalesScaniaC200Entegration().InsertGetVehicleEvaluationResponse();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX011":
                        retCode = new SalesManagerFacade().FleetContractWarningMailDaily(); //1818
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX012":
                        retCode = new SalesManagerFacade().RecallCampaignTargetPercantageMailDaily(); //1890
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0518":
                        result = new CustomerManagerFacade().SendHoldingLeadRequestInformationMail(); //akif_20160927        
                        break;
                    case "AcceptTurkuazInvoices":
                        retCode = new WSAutomaticAcceptInvoiceBatch().AcceptTurkuazInvoicesWithBatch(); //HandeC_20162709
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0524":
                        retCode = new WSPartSellingWebIntegration().UpdateWebOrderStatusAndSendMail(); //BetülK_16112016
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX013":
                        retCode = new SalesManagerFacade().CustomerSurveySSHBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX015":
                        retCode = new SalesManagerFacade().AppointmentNotShowUpNotificationMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX016":
                        retCode = new SalesManagerFacade().FillDMSDashboard();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_DEPOTINPUTOUTPUTVALUESLOGCREATE":
                        result = new WSPartLedIntegration().SendMailInputOutputValues();
                        break;
                    case "TMTQ0525":
                        result = new WSVehicleImportTaxAssesmentSimulation().Run(); //Tubag 15022017
                        break;
                    case "TMTQ0526":
                        result = new CustomerManagerFacade().SendUnsignedDataPrivacyFormReminderMail(); //BOY_20170223                      
                        break;
                    case "TMTQ0528":
                        result = new CustomerManagerFacade().ProcessDataPrivacyFormBatch(); //BOY_20170314                      
                        break;
                    case "TMTQ0530":
                        retCode = new WSVehicleSendFirmVehicleWarningMail().DetermineVehicleRequestsExceedsDeadline(); //YigitE 14.04.2017
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0534":
                        retCode = new WSVehicleSendFirmVehicleWarningMail().DetermineVehicleRequestsExceedsDeadlinePassOneDay(); //YigitE 14.04.2017
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0539":
                        result = new CustomerManagerFacade().ProcessCampaignOfDMSAfterStatusChanged(); // HakanA
                        break;
                    case "TMTQ0540":
                        result = new WSVehicleCOCAsbisM1Pattern().DeleteOldCocFilesJob(); // HakanA
                        break;
                    case "TMTQ0541":
                        result = new CustomerManagerFacade().ProcessCampaignOfDMSActivation(); // HakanA
                        break;
                    case "TMTQ0543":
                        result = new WSVehicleCOCAsbisM1Pattern().DownloadJatoIncrementalSqlFileAndRun(string.Empty); // HakanA
                        break;
                    case "TMTQ0545":
                        result = new WSVehicleCOCAsbisM1Pattern().DeleteOldCocFilesForCocCargoTracking(10); // HakanA
                        break;
                    case "TMTQ0546":
                        result = new WSVehicleCOCAsbisM1Pattern().UpdateUncompletedCocCargoStatus(); // HakanA
                        break;
                    case "DownloadJatoSqlFiles":
                        result = new WSVehicleCOCAsbisM1Pattern().DownloadJatoSqlFiles(string.Empty); // HakanA
                        break;
                    case "RunJatoSqlScripts":
                        result = new WSVehicleCOCAsbisM1Pattern().RunJatoSqlScripts(); // HakanA
                        break;
                    case "TMTQ0549":
                        retCode = new WSInvoiceBatch().CreateSESSPackagesInvoicesFromDosToDealer();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQ0550":
                        retCode = new WSInvoiceBatch().FillEmptyPrintedNoForInvoice();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "EINVOICE_UPDATE_HASHCODE":
                        retCode = new WSInvoiceBatch().UpdateHashCodes();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TMTQASX014":
                        retCode = new SalesManagerFacade().FillWorkshopCapacityBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_SALE_GAC_LOG":
                        {
                            RetCode retCodeGAC = (FTurkuazGOContainer.Resolve<FOperation>()).ProcessWebRequests(DateTime.Today.AddDays(-1).Date.AddHours(-2), DateTime.Now); //CE_20161209
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
                        result = new WSPartDefinition().BatchUpdatePartSupersessionRulesForCatalogDiff();
                        break;
                    case "SPR_SENDMAILSEATACCESSORYSELLING":
                        result = new PartManagerFacade().SendMailSeatAccessorySelling();
                        break;
                    case "PDKS_CHECKIN_CHECKOUT": //HANDEC 15/01/2016 DHYS:35049277  executes from_pdks_checkin_checkout_info package
                        result = new FileTransferOperationsFacade().ExecuteSSIS(parameters[0].ToString());
                        if (result.ReturnCode != 0)
                        {
                            FLabourPlanning fLabourPlanning = FTurkuazGOContainer.Resolve<FLabourPlanning>();
                            fLabourPlanning.SendMailPDKSSSISMail(DateTime.Now);
                            return result;
                        }
                        break;
                    case "FROM_PDKS_CHECKIN_CHECKOUT_INFO": //BARISAK executes from_pdks_checkin_checkout_info package üstteki metodun ismi farklı olduğu için yeni eklenmiştir.
                        result = new FileTransferOperationsFacade().ExecuteSSIS(parameters[0].ToString());
                        if (result.ReturnCode != 0)
                        {
                            FLabourPlanning fLabourPlanning = FTurkuazGOContainer.Resolve<FLabourPlanning>();
                            fLabourPlanning.SendMailPDKSSSISMail(DateTime.Now);
                            return result;
                        }
                        break;
                    case "ASX_C200POSITIONINSERT":
                        retCode = new WSAfterSalesScaniaC200Entegration().InsertGetVehiclePositionResponse();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
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
                        result = new WSPartDepot().ExecuteDailyPerformanceBatch();
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
                    case "VEH_DODUPDATEINTERNETPUBLISHSTATUS":
                        retCode = new WSVehicleDODOperation().DodUpdateInternetPublishStatus();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "SPD_UPDATEPLANNING_INCLUDE_STATUS":
                        result = new WSPartDefinition().BatchUpdatePartPlanningReturnList();
                        break;
                    case "ASX_PARTINSPECTION_WAITING":
                        retCode = new WSPartInspectionList().SendEmailForWaitingListsBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    //FVT burcuo
                    case "TMTQFVT001":
                        retCode = new SalesManagerFacade().FVTActionControlDateWarningMailDaily();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_UPDATEBADGECUSTOMER"://AkifhanK
                        result = new CustomerManagerFacade().UpdateBadgeCustomer();
                        break;
                    //Part Order Follow List
                    case "METQOFSL":
                        result = new WSOrderFollowStatusList().ProcessStatusListBatch();
                        break;
                    case "CUS_VEHICLECONFIGURATORLEADIMPORT"://AkifhanK
                        result = new CustomerManagerFacade().AddSubcontractorRecordFromVehicleConfiguratorXmlFile();
                        break;
                    case "CUS_CREATEPORSCHEWEBPARTCUSTOMERPOOL"://AkifhanK
                        result = new CustomerManagerFacade().CreatePorscheWebPartCustomerPool();
                        break;
                    case "CUS_COPYCHURNPROJECTDATAFROMDMS"://AkifhanK
                        result = new CustomerManagerFacade().CopyChurnProjectDataFromDMS();
                        break;
                    case "CUS_REMOVECHURNSURVEYFROMSURVEYPOOL"://AkifhanK
                        result = new CustomerManagerFacade().RemoveChurnSurveyFromSurveyPool();
                        break;
                    case "CUS_CLEANSTALESURVEY"://MustafaS
                        result = new CustomerManagerFacade().CleanStaleSurvey();
                        break;
                    case "CUS_ANONYMIZATION"://MustafaS
                        result = new CustomerManagerFacade().AnonymizationBatch();
                        break;
                    case "CUS_CLEANWORKORDERINCASESURVEY":
                        result = new CustomerManagerFacade().CleanWorkOrderInCaseSurvey(); // tsp_cus_survey_delete_by_work_order_in_case
                        break;
                    case "VEH_FLEET_REVISIT_WARNING_MAIL":
                        retCode = new WSVehicleSalesFileVisit().FleetRevisitWarningMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_SUPDOC_BACKUP":
                        retCode = new WSVehicleExcellenceApprovalRequest().SaveExcellenceApprovalRequestDocuments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_COPYHOTCUSTOMERPROJECTDATAFROMDMS"://AkifhanK
                        result = new CustomerManagerFacade().CopyHotCustomerProjectDataFromDMS();
                        break;
                    case "CUS_SEGMENTATIONDATAFROMDMS":
                        result = new CustomerManagerFacade().CopySegmentationDataFromDMS();
                        break;
                    case "TMTQ0353":        //BarışAKAR
                        result = new PartManagerFacade().CreateDailyCampaignBonus();
                        break;

                    case "TMTQ0354":        //BarışAKAR
                        result = new WSVehicleSalesOption().EndOptionRequestForDefaultValueBatch();
                        break;
                    case "TMTQ0355":
                        result = new WSVehicleDeliveryRecordSendMail().SendDeliveryCardNegativeBalanceMail();
                        break;

                    case "ASX_CANCELEXPIREDAPPOINTMENTS":
                        retCode = new SalesManagerFacade().CancelExpiredAppointments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "CUS_CLOSECUSTOMERWEBDATASPECIALPROJECTRECORDS":
                        result = new CustomerManagerFacade().CloseCustomerWebDataSpecialProjectRecords();
                        break;
                    case "ASX_GPLUS_ALLIANZ_INSURANCE_PAYMENT": // ED.20180329
                        retCode = new SalesManagerFacade().WorkOrderGplusAllianzPaymentIntegrationBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_DirectShipmetEInvoiceMatch":
                        retCode = new PartManagerFacade().InsertIncomingInvocideFromDraftInvoice();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_SendMailForNotSentDirectShipmentInvoices":
                        retCode = new PartManagerFacade().SendMailForNotSentDirectShipmentInvoices();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "IMP_EXPENSE_DOC_SAVE":
                        retCode = new WSVehicleImportExportPaymentDocument().SaveImportExpenseDocumentToDocumentManagement();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_DMS_REPORT1":
                        retCode = new WSVehicleDMSDashboard().ExecuteReportBudgetCompare();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_DMS_REPORT2":
                        retCode = new WSVehicleDMSDashboard().ExecuteReportDMSSaleDaily();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_DMS_REPORT3":
                        retCode = new WSVehicleDMSDashboard().ExecuteReportDMSSaleHourly();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "IMP_DEC_LIST_MAIL":
                        retCode = new WSVehicleImportDeclaration().SendDailyList();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_SalesCompletionForDealerPriceRequirement":
                        retCode = new WSPartPricing().SalesCompletionForDealerPriceRequirement();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_COC_REPORT":
                        retCode = new WSCocDocumentPrepare().DeleteOldCocDocuments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_CalculateAbnormalDemandQuantity":
                        result = new PartManagerFacade().CalculateAbnormalDemandQuantity();
                        break;
                    case "SPR_IncomingOEMOrderRejectionInfoProcess":
                        retCode = new WSPartOEMOrder().BatchIncomingOEMOrderRejectionInfoProcess();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;

                    case "VEH_NOTIFYFORUNINVOICEDORDERS":
                        retCode = new WSVehicleSendMailForUninvoicedTestVehicles().SendNotificationMailForUninvoicedTestVehicles();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_ADDSPEECHTEXTTOCALLDETAIL":
                        retCode = new CustomerManagerFacade().AddSpeechTextToCallDetail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_CM_NOTIFY_MISSINGDATA":
                        retCode = new WSVehicleCountryModel().SendCountryModelMissingEmail();
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
                    case "SPR_SEND_PACKAGE_INFO_TO_NETLOG":
                        retCode = new WSPackaging().SendPackageInfoToNetlog();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_ClaimCreditInvoiceCreateForCreditNotes":
                        retCode = new PartManagerFacade().CreditInvoiceAddForCreditNotes();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_UPDATETESTVEHICLESTATUS":
                        retCode = new WSTestVehicle().UpdateReservationRequestStatusToClosed();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_SEND_PPSO_NOT_MATCHED_MAIL":
                        retCode = new SalesManagerFacade().SendPPSONotMatchedPackageItemsMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_MULTIPLE_SALESFILE":
                        retCode = new WSVehicleSales().SendSalesFilesForSameCustomer();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_GET_PPSO_SYSTEM_INFO":
                        retCode = new SalesManagerFacade().GetSystemInfo();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_SENDMAILFORPARTHAVEBANROLE":
                        retCode = new WSDepotEntrance().SendMailForPartHaveBandrole();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_GET_PPSO_BLACKLIST":
                        retCode = new SalesManagerFacade().GetVinBlacklist();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    //OptionMail
                    case "TMTQVH18":
                        retCode = new WSVehicleADSalesOption().OptionMailSendBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_SendMailForRejectedClaims":
                        retCode = new PartManagerFacade().SendMailForRejectedClaims();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_COC_IMPORT_XML":
                        retCode = new WSVehicleCreateAllCOCDocuments().ImportCocXmlFiles();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_COC_IMPORT_TEMPLATE_XML":
                        retCode = new WSVehicleCreateAllCOCDocuments().ImportCocTypeTemplateXmlFiles();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_OEMORDERCONFIRMATIONTRANSFER":
                        result = new WSPartOEMOrder().TransferOemOrderConfirmation();
                        break;
                    case "IMP_ImportStagingInvoiceAll":
                        result = new WSPartImportExport().ImportStagingInvoiceAll();
                        break;
                    case "VEH_DOD_DELETE_EXPIRED_OPTIONS":
                        retCode = new WSVehicleDODOperation().DeleteExpiredOptions();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ECALL_FILL_SIMCARD_DATA":
                        retCode = new WSVehicleECall().FillVehicleSimCardData(0);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ECALL_GET_SIM_DETAIL":
                        retCode = new WSVehicleECall().GetSimDetails(0);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ECALL_ACTIVATE_SIMCARD":
                        retCode = new WSVehicleECall().ActivateSimCards(0);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_DELETE_OLD_PROPOSALFORMS_FILE":
                        retCode = new WSVehicleSales().DeleteOldProposalFormsFilesJob();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_TRANSFERPERSONNELPREMIUMINFO":
                        result = new PartManagerFacade().TransferPersonnelPremiumInfo();
                        break;
                    case "ASX_FILL_REPLACEMENT_WEBPARTS":
                        retCode = new SalesManagerFacade().FillReplacementWebParts();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ECALL_CHANGE_ADDON":
                        retCode = new WSVehicleECall().ChangeAddOns(0);
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_SURVEYDAILYBATCH":
                        retCode = new CustomerManagerFacade().SurveyDailyBatch();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_SupplierPaymentDocumentTypeEInvoiceMatch":
                        retCode = new WSPartImportExport().InsertEInvoicedFromDraftInvoice();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ECALL_SEND_TACCODE_EMAIL":
                        retCode = new WSVehicleECall().TacDeviceBrandModelRelationForInfoEmail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_ADD_PERMISSION_FIRST_DATA":
                        retCode = new CustomerManagerFacade().AddMultiplePermissionAsync();
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
                    case "UNSPED_JOB_TRACKING_INFO_GET":
                        retCode = new WSPartEntegrationUnsped().GetUnspedJobTrackingInfo();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_ADD_POOL_STOCK_UNUSED_PARTS":
                        retCode = new PartManagerFacade().AddPoolStockUnusedParts();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SEND_MAIL_FOR_NOT_EXIT_ENTREPOT":
                        retCode = new WSPartEntegrationUnsped().SendMailForNotEntrepotExitInvoice();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "KM_AND_PRICE_CONTACT_MAIL_SENDING":
                        retCode = new CustomerManagerFacade().KmFeeInformationMissingContactClosedSendMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "OPEN_CONTACT_NOT_CLOSED_MAIL":
                        retCode = new CustomerManagerFacade().OpenContactNotClosedSendMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPR_UNSPED_ENTREPOT_ENTRY_INFO_GET":
                        retCode = new WSPartEntegrationUnsped().AddUnspedEntrepotEntryInfoFromIntegration();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "APPOINTMENT_REMINDER_MAIL":
                        retCode = new CustomerManagerFacade().AppointmentReminderMailing();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "UNREALIZED_APPOINTMENT_CANCELLATION":
                        retCode = new CustomerManagerFacade().LeadUnrealizedAppointmentCancellation();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "DAILY_CONSULTANT_AUTO_APPOINTMENT_CREATE":
                        retCode = new CustomerManagerFacade().DailyCounsultantAutoAppointmentCreate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "INVOICE_SHIPPING_AND_CUSTOMS_PROCESS_REPORT":
                        retCode = new WSPartEntegrationUnsped().SendMailForInvoiceShippingandCustomsProcessReport();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CUS_IYS_UPDATE_CONSENT_ERRORS":
                        retCode = new CustomerManagerFacade().UpdateConsentErrors();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "TRIAL_PART_STATUS_UPDATE_EXPIRED_VALIDITY_DATE":
                        retCode = new PartManagerFacade().UpdateTrialPartStatusExpiredValidityDate();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPS_TRIAL_PART_ORDER_MAIL":
                        result = new WSPartSellingMessage().InformTrialPartOrderbyMail();
                        break;
                    case "ASX_PUSH_NOTIFICATION_UPCOMING_APPOINTMENTS":
                        retCode = new SalesManagerFacade().SendPushNotificationForUpComingAppointments();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "CAM_VEH_SALES_FILE_ACTIVE_CAMPAIGN":
                        retCode = new CustomerManagerFacade().VehicleSalesFileActiveCampaignList();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "EMPTY_MSISDN":
                        retCode = new WSVehicleEmptyMsisdnMails().SendEmptyMsisdsnMails();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SALES_TYPE_CHANGED":
                        retCode = new WSVehicleSalesTypeChangedMail().SendChangedSalesTypeMails();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "ASX_CHECK_CUSTOMER_SURVEYS":
                        retCode = new SalesManagerFacade().CheckCustomerSurveys();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "SPS_LABELED_PART_MAIL":
                        retCode = new PartManagerFacade().InformLabeledPartsInStockbyMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "VEH_ORDER_CUSTOMER_CHANGE":
                        retCode = new WSVehicleSales().SendEmailForOrderRelatedCustomerChange();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_EXP_APP_MAIL":
                        retCode = new WSFinanceDocumentSendApprovalWaitingMail().ExpenseSheetSendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "FIN_PYM_APP_MAIL":
                        retCode = new WSFinanceDocumentSendApprovalWaitingMail().PaymentRequestSendApprovalWaitingMail();
                        result = new BatchResultStruct { ReturnCode = retCode, DataReturned = null };
                        break;
                    case "EUROMESSAGE_AUTOPILOT_SALES_CUSTOMER_DATA":
                        retCode = new CustomerManagerFacade().EuroMessageCustomerSalesSendDataFTP();
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
