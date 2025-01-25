using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services
{
	public class TACRMDbContext : DbContext
	{
		public TACRMDbContext(DbContextOptions<TACRMDbContext> options) : base(options) { }

		// DbSets for each entity
		public DbSet<Agency> Agencies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
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
		public DbSet<Notification> Notifications { get; set; } // Add Notifications DbSet

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Agencies
			modelBuilder.Entity<Agency>().HasKey(a => a.AgencyID);

			// Users
			modelBuilder.Entity<User>()
				.HasKey(u => u.UserID);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Agency)
				.WithMany(a => a.Users)
				.HasForeignKey(u => u.AgencyID)
				.OnDelete(DeleteBehavior.SetNull);

			// Subscriptions
			modelBuilder.Entity<Subscription>()
				.HasKey(s => s.SubscriptionID);

			modelBuilder.Entity<Subscription>()
				.HasOne(s => s.User)
				.WithMany(u => u.Subscriptions)
				.HasForeignKey(s => s.UserID);

			// Contacts
			modelBuilder.Entity<Contact>()
				.HasKey(c => c.ContactID);

			modelBuilder.Entity<Contact>()
				.HasOne(c => c.User)
				.WithMany(u => u.Contacts)
				.HasForeignKey(c => c.UserID);

			modelBuilder.Entity<Contact>()
				.HasOne(c => c.ContactSource)
				.WithMany()
				.HasForeignKey(c => c.ContactSourceID)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Contact>()
				.HasOne(c => c.Status)
				.WithMany()
				.HasForeignKey(c => c.StatusID)
				.OnDelete(DeleteBehavior.SetNull);

			// ContactSources
			modelBuilder.Entity<ContactSource>().HasKey(cs => cs.ContactSourceID);

			// ContactStatuses
			modelBuilder.Entity<ContactStatus>().HasKey(cs => cs.StatusID);

			// Products
			modelBuilder.Entity<Product>().HasKey(p => p.ProductID);

			// Providers
			modelBuilder.Entity<Provider>().HasKey(p => p.ProviderID);

			// ContactProductInterest
			modelBuilder.Entity<ContactProductInterest>()
				.HasKey(cpi => cpi.ContactProductInterestID);

			modelBuilder.Entity<ContactProductInterest>()
				.HasOne(cpi => cpi.Contact)
				.WithMany()
				.HasForeignKey(cpi => cpi.ContactID);

			modelBuilder.Entity<ContactProductInterest>()
				.HasOne(cpi => cpi.Product)
				.WithMany()
				.HasForeignKey(cpi => cpi.ProductID);

			// Budgets
			modelBuilder.Entity<Budget>().HasKey(b => b.BudgetID);

			modelBuilder.Entity<Budget>()
				.HasOne(b => b.Contact)
				.WithMany()
				.HasForeignKey(b => b.ContactID);

			// Sales
			modelBuilder.Entity<Sale>().HasKey(s => s.SaleID);

			modelBuilder.Entity<Sale>()
				.HasOne(s => s.User)
				.WithMany(u => u.Sales)
				.HasForeignKey(s => s.UserID);

			modelBuilder.Entity<Sale>()
				.HasOne(s => s.Contact)
				.WithMany()
				.HasForeignKey(s => s.ContactID);

			// SaleProducts
			modelBuilder.Entity<SaleProduct>().HasKey(sp => sp.SaleProductID);

			modelBuilder.Entity<SaleProduct>()
				.HasOne(sp => sp.Sale)
				.WithMany(s => s.SaleProducts)
				.HasForeignKey(sp => sp.SaleID);

			modelBuilder.Entity<SaleProduct>()
				.HasOne(sp => sp.Product)
				.WithMany()
				.HasForeignKey(sp => sp.ProductID);

			modelBuilder.Entity<SaleProduct>()
				.HasOne(sp => sp.Provider)
				.WithMany()
				.HasForeignKey(sp => sp.ProviderID)
				.OnDelete(DeleteBehavior.SetNull);

			// SaleTravelers
			modelBuilder.Entity<SaleTraveler>().HasKey(st => st.SaleTravelerID);

			modelBuilder.Entity<SaleTraveler>()
				.HasOne(st => st.Sale)
				.WithMany(s => s.SaleTravelers)
				.HasForeignKey(st => st.SaleID);

			// Payments
			modelBuilder.Entity<Payment>().HasKey(p => p.PaymentID);

			modelBuilder.Entity<Payment>()
				.HasOne(p => p.SaleProduct)
				.WithMany()
				.HasForeignKey(p => p.SaleProductID);

			// CalendarEvents
			modelBuilder.Entity<CalendarEvent>().HasKey(ce => ce.EventID);

			modelBuilder.Entity<CalendarEvent>()
				.HasOne(ce => ce.User)
				.WithMany(u => u.CalendarEvents)
				.HasForeignKey(ce => ce.UserID);

			// Notifications
			modelBuilder.Entity<Notification>().HasKey(n => n.NotificationID);

			modelBuilder.Entity<Notification>()
				.HasOne(n => n.User)
				.WithMany(u => u.Notifications)
				.HasForeignKey(n => n.UserID)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
