namespace ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upDateBYchanchal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PosSalesInvoice", "UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId");
            CreateTable(
                "dbo.PosBillingReportTexts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PosBranchId = c.Int(nullable: false),
                        Type = c.String(maxLength: 20),
                        Text = c.String(maxLength: 2000),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifideDate = c.DateTime(),
                        CmnCompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CmnCompanies", t => t.CmnCompanyId, cascadeDelete: false)
                .ForeignKey("dbo.PosBranch", t => t.PosBranchId, cascadeDelete: false)
                .Index(t => t.PosBranchId)
                .Index(t => t.CmnCompanyId);
          
            AddColumn("dbo.CmnCompanies", "VatRegNo", c => c.String(maxLength: 100));
            AddColumn("dbo.PosBranch", "Email", c => c.String(maxLength: 100));
            AddColumn("dbo.PosSalesInvoice", "TotalAmt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PosSalesInvoice", "PosInvoiceType", c => c.Int(nullable: false));
            AddColumn("dbo.PosSalesInvoice", "PosInvoiceGlobalFileNumberId", c => c.Long(nullable: false));
        
            AlterColumn("dbo.PosSalesInvoice", "InvDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PosSalesInvoice", "InvoiceNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.PosScheme", "DateFrom", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PosScheme", "DateTo", c => c.DateTime(nullable: false, storeType: "date"));
            CreateIndex("dbo.PosSalesInvoice", new[] { "InvoiceNumber", "CmnCompanyId" }, unique: true, name: "UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId");
            CreateIndex("dbo.PosSalesInvoice", "PosInvoiceGlobalFileNumberId");
            CreateIndex("dbo.PosSalesInvoiceProducts", "PosProductId");
            CreateIndex("dbo.PosSalesInvoiceProducts", "PosProductBatchId");
            CreateIndex("dbo.PosSalesInvoiceTenders", "PosTenderId");
            AddForeignKey("dbo.PosSalesInvoice", "PosInvoiceGlobalFileNumberId", "dbo.PosInvoiceGlobalFileNumber", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PosSalesInvoiceProducts", "PosProductId", "dbo.PosProducts", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PosSalesInvoiceProducts", "PosProductBatchId", "dbo.PosProductBatch", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PosSalesInvoiceTenders", "PosTenderId", "dbo.PosTenders", "Id", cascadeDelete: false);
            DropColumn("dbo.PosSalesInvoiceFreeProducts", "CreatedBy");
            DropColumn("dbo.PosSalesInvoiceFreeProducts", "CreatedDate");
            DropColumn("dbo.PosSalesInvoiceFreeProducts", "ModifiedBy");
            DropColumn("dbo.PosSalesInvoiceFreeProducts", "ModifideDate");
            DropColumn("dbo.SecUsers", "VisibleExtraOt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SecUsers", "VisibleExtraOt", c => c.Int(nullable: false));
            AddColumn("dbo.PosSalesInvoiceFreeProducts", "ModifideDate", c => c.DateTime());
            AddColumn("dbo.PosSalesInvoiceFreeProducts", "ModifiedBy", c => c.Int());
            AddColumn("dbo.PosSalesInvoiceFreeProducts", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.PosSalesInvoiceFreeProducts", "CreatedBy", c => c.Int());
            DropForeignKey("dbo.PosSalesInvoiceTenders", "PosTenderId", "dbo.PosTenders");
            DropForeignKey("dbo.PosSalesInvoiceProducts", "PosProductBatchId", "dbo.PosProductBatch");
            DropForeignKey("dbo.PosSalesInvoiceProducts", "PosProductId", "dbo.PosProducts");
            DropForeignKey("dbo.PosSalesInvoice", "PosInvoiceGlobalFileNumberId", "dbo.PosInvoiceGlobalFileNumber");
            DropForeignKey("dbo.PosInvoiceGlobalFileNumber", "CmnCompanyId", "dbo.CmnCompanies");
            DropForeignKey("dbo.PosBillingReportTexts", "PosBranchId", "dbo.PosBranch");
            DropForeignKey("dbo.PosBillingReportTexts", "CmnCompanyId", "dbo.CmnCompanies");
            DropIndex("dbo.PosSalesInvoiceTenders", new[] { "PosTenderId" });
            DropIndex("dbo.PosSalesInvoiceProducts", new[] { "PosProductBatchId" });
            DropIndex("dbo.PosSalesInvoiceProducts", new[] { "PosProductId" });
            DropIndex("dbo.PosSalesInvoice", new[] { "PosInvoiceGlobalFileNumberId" });
            DropIndex("dbo.PosSalesInvoice", "UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId");
            DropIndex("dbo.PosInvoiceGlobalFileNumber", new[] { "CmnCompanyId" });
            DropIndex("dbo.PosInvoiceGlobalFileNumber", "UK_PosInvoiceGlobalFileNumber_InvoiceGlobalFileNumber");
            DropIndex("dbo.PosBillingReportTexts", new[] { "CmnCompanyId" });
            DropIndex("dbo.PosBillingReportTexts", new[] { "PosBranchId" });
            AlterColumn("dbo.PosScheme", "DateTo", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PosScheme", "DateFrom", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PosSalesInvoice", "InvoiceNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.PosSalesInvoice", "InvDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.PosVwProductWithBatch", "PosProductId");
            DropColumn("dbo.PosSalesInvoice", "PosInvoiceGlobalFileNumberId");
            DropColumn("dbo.PosSalesInvoice", "PosInvoiceType");
            DropColumn("dbo.PosSalesInvoice", "TotalAmt");
            DropColumn("dbo.PosBranch", "Email");
            DropColumn("dbo.CmnCompanies", "VatRegNo");
            DropTable("dbo.PosInvoiceGlobalFileNumber");
            DropTable("dbo.PosBillingReportTexts");
            CreateIndex("dbo.PosSalesInvoice", new[] { "InvoiceNumber", "CmnCompanyId" }, unique: true, name: "UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId");
        }
    }
}
