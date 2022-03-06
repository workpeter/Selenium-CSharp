using System.Data.Entity;

namespace Tesco.Framework.Models
{
    public partial class ModelTesco : DbContext
    {
        public ModelTesco()
            : base("name=ModelTesco")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ClubCardStatu> ClubCardStatus { get; set; }
        public virtual DbSet<MarketingCommunication> MarketingCommunications { get; set; }
        public virtual DbSet<PersonalDetail> PersonalDetails { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.ClubCardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressLine3)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.TownOrCity)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Postcode)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.County)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Address)
                .HasForeignKey(e => e.AddressID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubCardStatu>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ClubCardStatu>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.ClubCardStatu)
                .HasForeignKey(e => e.ClubCardStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MarketingCommunication>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<MarketingCommunication>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.MarketingCommunication1)
                .HasForeignKey(e => e.MarketingCommunication)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PersonalDetail>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.PersonalDetail)
                .HasForeignKey(e => e.PersonalDetailsID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(8, 2);
        }
    }
}