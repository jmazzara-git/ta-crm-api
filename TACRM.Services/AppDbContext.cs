using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;
using TACRM.Services.Enums;

namespace TACRM.Services
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		#region DbSets
		public DbSet<User> User { get; set; }
		public DbSet<Product> Product { get; set; }
		public DbSet<ProductType> ProductType { get; set; }
		public DbSet<Provider> Provider { get; set; }
		public DbSet<Contact> Contact { get; set; }
		public DbSet<ContactStatus> ContactStatus { get; set; }
		public DbSet<ContactSource> ContactSource { get; set; }
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
		}
	}
}
