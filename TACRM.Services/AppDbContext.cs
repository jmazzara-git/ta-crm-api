using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		#region DbSets
		public DbSet<User> User { get; set; }
		public DbSet<Product> Product { get; set; }
		public DbSet<Provider> Provider { get; set; }
		public DbSet<Contact> Contact { get; set; }
		public DbSet<Budget> Budget { get; set; }
		public DbSet<Sale> Sale { get; set; }
		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User Type Enum
			modelBuilder.HasPostgresEnum<UserTypeEnum>();
			modelBuilder.Entity<User>()
				.Property(e => e.UserType)
				.HasConversion<string>()
				.HasColumnType("user_type");

			// Contact Status Enum
			modelBuilder.HasPostgresEnum<ContactStatusEnum>();
			modelBuilder.Entity<Contact>()
				.Property(e => e.ContactStatus)
				.HasConversion<string>()
				.HasColumnType("contact_status");

			// Product Type Enum
			modelBuilder.HasPostgresEnum<ProductTypeEnum>();
			modelBuilder.Entity<Product>()
				.Property(e => e.ProductType)
				.HasConversion<string>()
				.HasColumnType("product_type");

		}
	}
}
