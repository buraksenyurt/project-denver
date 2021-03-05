using Denver.Common;
using Denver.Common.Services;
using Denver.Facade.Common;
using Denver.Facade.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Denver.Service
{
    public class JobExecuter
    {
        public JobExecuter()
        {
            InitializeComponent();
        }

        private IContainer components = null;

        private void InitializeComponent()
        {
        }
        public BatchResultStruct GetOperationResult(RetCode returnCode, DataSet dsReturn)
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

            return new BatchResultStruct
            {
                ReturnCode = retCode,
                DataReturned = dsReturn
            };
        }

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

                    //FinanceDebtAgingDaily //20150926 Hoze Luiz Gonzaled De Salvatore
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
    }
}
