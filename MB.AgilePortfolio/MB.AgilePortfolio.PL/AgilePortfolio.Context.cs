﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MB.AgilePortfolio.PL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PortfolioEntities : DbContext
    {
        public PortfolioEntities()
            : base("name=PortfolioEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblLanguage> tblLanguages { get; set; }
        public virtual DbSet<tblPortfolio> tblPortfolios { get; set; }
        public virtual DbSet<tblPortfolioProject> tblPortfolioProjects { get; set; }
        public virtual DbSet<tblPrivacy> tblPrivacies { get; set; }
        public virtual DbSet<tblProject> tblProjects { get; set; }
        public virtual DbSet<tblProjectLanguage> tblProjectLanguages { get; set; }
        public virtual DbSet<tblScreenshot> tblScreenshots { get; set; }
        public virtual DbSet<tblUserType> tblUserTypes { get; set; }
        public virtual DbSet<tblStatus> tblStatuses { get; set; }
        public virtual DbSet<tblForgotPassword> tblForgotPasswords { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
    
        public virtual ObjectResult<Portfolio_ViewData_Result> Portfolio_ViewData(Nullable<System.Guid> projectId)
        {
            var projectIdParameter = projectId.HasValue ?
                new ObjectParameter("ProjectId", projectId) :
                new ObjectParameter("ProjectId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Portfolio_ViewData_Result>("Portfolio_ViewData", projectIdParameter);
        }
    
        public virtual ObjectResult<ProjectLanguages_ViewData_Result> ProjectLanguages_ViewData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ProjectLanguages_ViewData_Result>("ProjectLanguages_ViewData");
        }
    
        public virtual ObjectResult<Projects_ViewData_Result> Projects_ViewData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Projects_ViewData_Result>("Projects_ViewData");
        }
    
        public virtual ObjectResult<Projects_ViewSummaryData_Result> Projects_ViewSummaryData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Projects_ViewSummaryData_Result>("Projects_ViewSummaryData");
        }
    
        public virtual ObjectResult<ProtfolioProjects_ViewData_Result> ProtfolioProjects_ViewData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ProtfolioProjects_ViewData_Result>("ProtfolioProjects_ViewData");
        }
    
        public virtual ObjectResult<Screenshots_ViewData_Result> Screenshots_ViewData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Screenshots_ViewData_Result>("Screenshots_ViewData");
        }
    
        public virtual ObjectResult<User_ViewData_Result> User_ViewData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<User_ViewData_Result>("User_ViewData");
        }
    }
}
