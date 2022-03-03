using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ERP.Migrations;

using ERP.Models.POS;
using ERP.Models.Security;

namespace ERP.Models
{
    public partial class ConnectionDatabase : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ConnectionDatabase>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(38, 5));
        }
        public DbSet<SecUser> UserDbSet { set; get; }
        public DbSet<CmnCompany> CompanyDbSet { set; get; }
        public DbSet<SecRole> RoleDbSet { set; get; }
        public DbSet<SecUserRole> UserRoleDbSet { set; get; }
        public DbSet<SecResource> ResourceDbSet { set; get; }
        public DbSet<SecResourcePermission> ResourcePermissionDbSet { set; get; }
        public DbSet<SecRolePermission> RolePermissionDbSet { set; get; }
        public DbSet<SecModule> ModuleDbSet { set; get; }
        public DbSet<SecLoginHistory> LoginHistoryDbSet { set; get; }
        public DbSet<SecUserBranch> UserBranchDbSet { set; get; }


    }
}