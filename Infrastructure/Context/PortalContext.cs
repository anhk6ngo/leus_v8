namespace LeUs.Infrastructure.Context
{
    public class PortalContext(
        DbContextOptions<PortalContext> options,
        IDateTimeService dateTimeService,
        IServerCurrentUserService userService) : DbContext(options)
    {
        public DbSet<UserBalance>? UserBalance { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var modelEntityTypes = builder.Model.GetEntityTypes();
            foreach (var entity in modelEntityTypes)
            {
                entity.SetTableName($"{entity.GetTableName()?.ToLower()}s");
                foreach (var property in entity.GetProperties()
                             .Where(p => p.Name is "CreatedBy" or "LastModifiedBy"
                                 or "CreateUser" or "UpdateUser"))
                {
                    property.SetColumnType("character varying(255)");
                }
            }

            base.OnModelCreating(builder);
        }
        
        // protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        // {
        //     configurationBuilder.Properties<DateTime>()
        //         .HaveConversion<DateTimeToDateTimeUtc>();
        // }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntityNew>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = await userService.UserId();
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = await userService.UserId();
                        break;
                }
            }

            //await mediator.DispatchDomainEvents(this);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}