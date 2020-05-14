using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.ApplicationLayer.Interfaces;
using System.Configuration;
using UWPLockStep.Domain.Entities.Joins;

using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Ingredients.IngredientDecorators;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Persistance.Services
{
    public class LockStepContextSql : DbContext, ILockStepContextSql
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public DbSet<Factor> Factors { get; set; }
        
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientPolicy> IngredientPolicies  { get; set; }
        public DbSet<FactorIngredient> FactorIngredients { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<ICombinablePolicy<DomainObject>> CombinablePolicies{ get; set; }//Not sure I can do this!
        public DbSet<FluidOrder> FluidOrders { get; set; }
        public DbSet<OrderPolicy> OrderPolicies { get; set; }
        public DbSet<IngredientOrder> IngredientOrders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LockStepPatient> Patients { get; set; }
        //public DbSet<IngredientFactorDecorator> IngredientFactorDecorators { get; set; }
        //public DbSet<FactorDecorator> FactorDecorators{ get; set; }
        public DbSet<IngredientDecorator> IngredientDecorators { get; set; }
        public DbSet<SolutionIngredient> SolutionIngredients { get; set; }

        //public DbSet<FactorBase> FactorBases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlServer(@"Data Source=LAPTOP-G90BMUKL\SQLSERVER2019;Initial Catalog=LockStep;Integrated Security=SSPI");
        //ConfigurationManager.ConnectionStrings["MyAppConnectionString"]?.ConnectionString) // @"Data Source=LAPTOP-G90BMUKL\SQLSERVER2019;Initial Catalog=LockStep;Integrated Security=SSPI")   //could not get it into csproj
        public LockStepContextSql()          
        {
        }

        public LockStepContextSql(
           DbContextOptions<LockStepContextSql> options,
           ICurrentUserService currentUserService,
           IDateTime dateTime)
           : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolicyBase>().ToTable("PolicyBase");
            modelBuilder.Entity<FactorBase>().ToTable("FactorBase");
            modelBuilder.Entity<IngredientBase>().ToTable("IngredientBase");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LockStepContextSql).Assembly);
        }
    }
}
