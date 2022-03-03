using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using ERP.Models;
using ERP.Models.POS;
using ERP.Models.VModel;

namespace ERP.Controllers.POS.Gateway
{
    public class StockGateway
    {

        public List<PosVwPurchaseReceipt> GetPurchaseReceipt( string companyInvNo,int cmnId)
        {
            using (ConnectionDatabase database=new ConnectionDatabase())
            {
                return database.PosVwPurchaseReceiptDbSet.Where(w => w.CompanyInvNo == companyInvNo && w.CmnCompanyId==cmnId).ToList();
            }
        }

        public List<PosVwProductWithBatch> GetProductInfoForPurchasReceipt(int cmnId)
        {
            using (ConnectionDatabase database=new ConnectionDatabase())
            {
                return database.PosVwProductWithBatchDbSet.Where(w => w.CmnCompanyId == cmnId).ToList();
            }
        } 

        public VmProductDetails GetProductDetailsForPurchaseReceipt(int cmnId,string productCode)
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                var lst = (from l in database.PosProductBatchDbSet
                    where l.CmnCompanyId == cmnId && (l.PosProduct.Code==productCode || l.BarCode== productCode)
                    orderby l.SellingRate descending
                    select new VmProductDetails
                    {
                        Code = l.PosProduct.Code,
                        PosUomMasterId = l.PosProduct.PosUomGroup.PosUomGroupDetails.FirstOrDefault(f => f.PosUomGroupId == l.PosProduct.PosUomGroupId && f.PosUomMaster.IsBaseUom == true).PosUomMasterId,
                        PosUomGroupId = l.PosProduct.PosUomGroupId,
                        PosProductBatchId = l.Id,
                        PurchaseRate = l.PurchaseRate,
                        Name = l.PosProduct.Name

                    }).FirstOrDefault();
                return lst;
            }
        }

        public VmProductDetailsForStockTransfer GetProductDetailsForStockTransfer(int cmnId, string productCode,int fromLocation,int officeId)
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                var lst = (from l in database.PosProductBatchDbSet
                           where l.CmnCompanyId == cmnId && (l.PosProduct.Code == productCode || l.BarCode==productCode)
                           orderby l.SellingRate descending
                           select new VmProductDetailsForStockTransfer
                           {
                               Code = l.PosProduct.Code,
                               PosProductBatchId = l.Id,
                               PurchaseRate = l.PurchaseRate,
                               Name = l.PosProduct.Name,
                               StockOnHand= 0
                           }).FirstOrDefault();
                if (lst != null)
                {
                    lst.StockOnHand = database.Database.SqlQuery<VmGetPosCurrentStockByLocationOnly>("GetPosCurrentStockByLocationOnly @OfficeId='" + officeId.ToString() + "',@Location=" + fromLocation + ",@BatchId=" + lst.PosProductBatchId + ",@CmnId=" + cmnId).Sum(s => s.StockQty);
                }
                return lst;
            }
        }


        public decimal GetStockByLocationWise(long batchId, int officeId, int fromLocation,int cmnId)
        {
            using (ConnectionDatabase database =new ConnectionDatabase())
            {
                return database.Database.SqlQuery<VmGetPosCurrentStockByLocationOnly>("GetPosCurrentStockByLocationOnly @OfficeId='" + officeId.ToString() + "',@Location=" + fromLocation + ",@BatchId=" + batchId + ",@CmnId=" + cmnId).Sum(s => s.StockQty);
            }
        }

    }
}