using Denver.BLL;
using Denver.Common;
using Denver.Common.Services;
using Denver.PCL;
using Denver.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Denver.Facade.Parts
{
    public class PartManagerFacade
    {
        public RetCode AddToStock(int code, int number, decimal price, int stockCount, string name, int quantity, string supplier, string description)
        {
            BLLPartManager manager = new BLLPartManager();
            try
            {
                if (code == 0)
                    return RetCode.Fail;
                else if (price == 0)
                    return RetCode.Fail;
                else if (stockCount == 0)
                    return RetCode.Fail;
                else if (name.Length > 100)
                    return RetCode.Fail;
                else if (supplier.Length > 100)
                    return RetCode.Fail;
                else if (quantity == 0)
                    return RetCode.Fail;

                manager.AddNewPart(code, number, price, stockCount, name, quantity, supplier, description);

                int userId = Convert.ToInt32(HttpContext.Current.Session["CurrentUserId"]);
                if (stockCount < 100)
                {
                    MailUtility.SendEmailToUnitHead(userId, code, stockCount);
                }
                else if (stockCount > 100 && stockCount < 1000)
                {
                    MailUtility.SendEmailToManager(userId, code, stockCount);
                }
                else if (stockCount > 1000)
                {
                    MailUtility.SendEmailToBoss(Code: code, Count: stockCount);
                }

                return RetCode.Success;
            }
            catch (Exception excp)
            {

                return RetCode.Fail;
            }
        }

        public DataSet GetParts()
        {
            BLLPartManager manager = new BLLPartManager();
            try
            {
                return manager.GetParts();
            }
            catch
            {
                return null;
            }
        }

        public BatchResultStruct CreateDailyByBrandGroup()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateForWaiting()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateInsuranceBonusDaily()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateOriginalAccessoryBonusDaily()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct BatchDraftProcess()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct BatchProcessDealerOrderDraftCreation()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecuteBatchPlanningJobForDistributor()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateOrderDraftForDealerBatch()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecutePlanningJobFromQueueForDistributor()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecutePlannedUrgentOrderDrafts()
        {
            throw new NotImplementedException();
        }

        public int BuyBackDealerDraftCreate()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForPartCriticalStock()
        {
            throw new NotImplementedException();
        }

        public int DomesticSupplyInvoiceImport()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForOrderDealerShipment()
        {
            throw new NotImplementedException();
        }

        public int CreateInvoiceForDomesticInvoiceDraftFiles()
        {
            throw new NotImplementedException();
        }

        public int ProcessConfirmation()
        {
            throw new NotImplementedException();
        }

        public int BatchAbnormalOrderProcess()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SentBackOrderInfoMails()
        {
            throw new NotImplementedException();
        }

        public int DeletePoolStockPartsThatHasNoPoolStock()
        {
            throw new NotImplementedException();
        }

        public int UpdatePartPriceBatch()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct UpdatePartPlanningStatusForModelStockListParts()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct UpdateAllModelStockListWithSupersession()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct TransferCatalogSupersessions()
        {
            throw new NotImplementedException();
        }

        public int SendMailForOpenDomesticOrders()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForOrderDealerNoPriceAll()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForPartCatalogHistory()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForPartDifferentSPFOnSupersessionChain()
        {
            throw new NotImplementedException();
        }

        public int GetOrderStatusFromUSAPartner()
        {
            throw new NotImplementedException();
        }

        public int UpdateWebOrderStatusAndSendMail()
        {
            throw new NotImplementedException();
        }

        public int BatchIncomingOEMOrderRejectionInfoProcess()
        {
            throw new NotImplementedException();
        }

        public int AddPoolStockUnusedParts()
        {
            throw new NotImplementedException();
        }

        public int SendMailForNotEntrepotExitInvoice()
        {
            throw new NotImplementedException();
        }

        public int DirectShipmentTransferSendMail()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct ExecuteDailyPerformanceBatch()
        {
            throw new NotImplementedException();
        }

        public int DirectShipmentConversion()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateDailyBenchBonus()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct BatchUpdatePartSupersessionRulesForCatalogDiff()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailSeatAccessorySelling()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CreateAndFillFOBPriceExtension()
        {
            throw new NotImplementedException();
        }

        public int CreateAndCalculateQualifiedServicePremiumPeriod()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CalculatePartLTValues()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct BatchUpdatePartSupersessionRules()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForUnPickedSlips()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForUnPackedSlipsForCutOff()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMailForUnPickedSlipsForCutOff()
        {
            throw new NotImplementedException();
        }

        public int RemoveAllLocatedOrders()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct UpdateDealerBaseStockPartsWithoutStockCards()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct SendMessageForADRTypeParts()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct TransferUserPriority()
        {
            throw new NotImplementedException();
        }

        public BatchResultStruct CalculateOEMOrderPriorityForTruckInvoices()
        {
            throw new NotImplementedException();
        }
    }
}
