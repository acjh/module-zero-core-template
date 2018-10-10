using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AbpCompanyName.AbpProjectName.Authorization.Roles;
using AbpCompanyName.AbpProjectName.Authorization.Users;
using AbpCompanyName.AbpProjectName.MultiTenancy;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Events.Bus.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace AbpCompanyName.AbpProjectName.EntityFrameworkCore
{
    public class Proposal : Entity
    {
        public ICollection<TravelOfferPriceProposalElement> TravelOfferPriceProposalElements { get; set; }
    }

    public class TravelOfferPriceProposalElement : FullAuditedEntity
    {
    }

    public class AbpProjectNameDbContext : AbpZeroDbContext<Tenant, Role, User, AbpProjectNameDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public DbSet<Proposal> Proposals { get; set; }

        public DbSet<TravelOfferPriceProposalElement> TravelOfferPriceProposalElements { get; set; }


        public AbpProjectNameDbContext(DbContextOptions<AbpProjectNameDbContext> options)
            : base(options)
        {
        }

        protected int? CurrentCompanyId => GetCurrentCompanyIdOrNull();

        protected virtual int? GetCurrentCompanyIdOrNull()
        {
            if (CurrentUnitOfWorkProvider != null &&
                CurrentUnitOfWorkProvider.Current != null)
            {
                return CurrentUnitOfWorkProvider.Current
                    .Filters.FirstOrDefault(f => f.FilterName == "CompanyFilter")?
                    .FilterParameters.GetOrDefault("CompanyId") as int?;
            }

            return null;
        }

        protected override void ApplyAbpConcepts(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        {
            HandleImplicitDeleteForSoftDelete(entry);
            base.ApplyAbpConcepts(entry, userId, changeReport);
        }

        protected virtual void HandleImplicitDeleteForSoftDelete(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified && entry.Entity is ISoftDelete)
            {
                var foreignKeys = entry.Metadata.GetForeignKeys();

                foreach (var foreignKey in foreignKeys)
                {
                    foreach (var property in foreignKey.Properties)
                    {
                        var propertyEntry = entry.Property(property.Name);
                        if (propertyEntry.OriginalValue != null && propertyEntry.CurrentValue == null)
                        {
                            entry.State = EntityState.Deleted;
                        }
                    }
                }
            }
        }

        protected override void ApplyAbpConceptsForDeletedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
        {
            base.ApplyAbpConceptsForDeletedEntity(entry, userId, changeReport);
        }
    }
}
