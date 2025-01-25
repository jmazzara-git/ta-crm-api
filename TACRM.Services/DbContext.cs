using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services
{
	public class TACRMDbContext(DbContextOptions<TACRMDbContext> options) : DbContext(options)
	{

		// DbSet Properties
		public DbSet<Tenant> Tenants { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<ContactSource> ContactSources { get; set; }
		public DbSet<ContactStatus> ContactStatuses { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Provider> Providers { get; set; }
		public DbSet<ContactProductInterest> ContactProductInterests { get; set; }
		public DbSet<Budget> Budgets { get; set; }
		public DbSet<Sale> Sales { get; set; }
		public DbSet<SaleProduct> SaleProducts { get; set; }
		public DbSet<SaleTraveler> SaleTravelers { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<CalendarEvent> CalendarEvents { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Tenant Configurations
			modelBuilder.Entity<Tenant>(entity =>
			{
				entity.HasKey(e => e.TenantID);
				entity.Property(e => e.TenantName).IsRequired().HasMaxLength(255);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			// User Configurations
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.UserID);
				entity.HasOne(e => e.Tenant)
					  .WithMany()
					  .HasForeignKey(e => e.TenantID)
					  .OnDelete(DeleteBehavior.Cascade);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
				entity.HasIndex(e => e.Email).IsUnique();
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			// Contact Configurations
			modelBuilder.Entity<Contact>(entity =>
			{
				entity.HasKey(e => e.ContactID);
				entity.HasOne(e => e.Tenant)
					  .WithMany()
					  .HasForeignKey(e => e.TenantID)
					  .OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.ContactSource)
					  .WithMany()
					  .HasForeignKey(e => e.ContactSourceID)
					  .OnDelete(DeleteBehavior.SetNull);
				entity.HasOne(e => e.ContactStatus)
					  .WithMany()
					  .HasForeignKey(e => e.StatusID)
					  .OnDelete(DeleteBehavior.SetNull);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			// Add configurations for other entities as needed...

			base.OnModelCreating(modelBuilder);
		}
	}
}
