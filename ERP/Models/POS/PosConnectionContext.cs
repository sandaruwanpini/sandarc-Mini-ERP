using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ERP.Controllers.Manager;
using ERP.Models.POS;

namespace ERP.Models
{
    public partial class ConnectionDatabase
    {
        public DbSet<PosProduct> PosProductDbSet { set; get; }
        public DbSet<PosSupplier> PosSupplierDbSet { set; get; }
        public DbSet<PosProductBatch> PosProductBatchDbSet { set; get; }
        public DbSet<PosUomGroup> PosUomGroupDbSet { set; get; }
        public DbSet<PosUomMaster> PosUomMasterDbSet { set; get; }
        public DbSet<PosUomGroupDetail> PosUomGroupDetailDbSet { set; get; }
        public DbSet<PosStockType> PosStockTypeDbSet { set; get; }
        public DbSet<PosStock> PosStockDbSet { set; get; }
        public DbSet<PosStockDetail> PosStockDetailDbSet { set; get; }
        public IQueryable<PosBranch> PosBrancheList
        {
            get
            {
                
                var userOffice = new ErpManager().UserOffice;
                return PosBrancheDbSet.Where(w => userOffice.Contains((int)w.Id));
            }
        }
        public DbSet<PosBranch> PosBrancheDbSet { set; get; }
        public  DbSet<PosVwPurchaseReceipt> PosVwPurchaseReceiptDbSet { set; get; }
        public DbSet<PosVwProductWithBatch> PosVwProductWithBatchDbSet { set; get; }
        public DbSet<PosTenderType> PosTenderTypeDbSet { set; get; }
        public DbSet<PosTender> PosTenderDbSet { set; get; } 
        public DbSet<PosVwUomDetail> PosVwUomDetailsDbSet { set; get; } 
        public DbSet<PosSalesInvoice> PosSalesInvoiceDbSet { set; get; }
        public DbSet<PosRegion> PosRegionDbSet { set; get; }
        public DbSet<PosCityOrNearestZone> PosCityOrNearestZoneDbSet { set; get; }
        public DbSet<PosCustomer> PosCustomerDbSet { set; get; }
        public DbSet<PosSalesInvoiceFreeProduct> PosSalesInvoiceFreeProductDbSet { set; get; }
        public DbSet<PosSalesInvoiceProduct> PosSalesInvoiceProductsDbSet { set; get; }
        public DbSet<PosCustomerClass> PosCustomerClassDbSet { set; get; }
        public DbSet<PosScheme> PosSchemeDbSet { set; get; }
        public DbSet<PosSchemeBranch> PosSchemeBranchDbSet { set; get; }
        public DbSet<PosSchemeCustomerClass> PosSchemeCustomerClassDbSet { set; get; }
        public DbSet<PosSchemeProduct> PosSchemeProductDbSet { set; get; }
        public DbSet<PosSchemeSlab> PosSchemeSlabDbSet { set; get; }
        public DbSet<PosSchemeSlabFreeProduct> PosSchemeSlabFreeProductDbSet { set; get; }
        public DbSet<PosInvoiceGlobalFileNumber> PosInvoiceGlobalFileNumberDbSet { set; get; }
        public DbSet<PosBillingReportText> PosBillingReportTextDbSet { set; get; }
        public DbSet<PosProductCategory> PosProductCategoryDbSet { set; get; }
        public DbSet<PosCustomerDueCollection> PosCustomerDueCollectionDbSet { set; get; }
        public DbSet<PosBillprintTemplateOfBranch> PosBillprintTemplateOfBrancheDbSet { set; get; }
        public DbSet<PosSalesProductWarrantyIssue> PosSalesProductWarrantyIssueDbSet { set; get; }

        // get next sequence
        public long GetSeqPosSchemeSchemeCode()
        {
            var rawQuery = Database.SqlQuery<long>("SELECT NEXT VALUE FOR dbo.SQ_PosScheme_SchemeCode");
            var task = rawQuery.SingleAsync();
            long nextVal = task.Result;
            return nextVal;
        }
    }
}