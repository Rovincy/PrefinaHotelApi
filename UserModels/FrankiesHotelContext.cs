using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelWebApi.UserModels
{
    public partial class FrankiesHotelContext : DbContext
    {
        public FrankiesHotelContext()
        {
        }

        public FrankiesHotelContext(DbContextOptions<FrankiesHotelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<BankDetail> BankDetails { get; set; }
        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<BillingsHistory> BillingsHistories { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyBill> CompanyBills { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<GuestService> GuestServices { get; set; }
        public virtual DbSet<HoursExtension> HoursExtensions { get; set; }
        public virtual DbSet<HouseKeepingItem> HouseKeepingItems { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<NightAudit> NightAudits { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<PaymentNote> PaymentNotes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomPrice> RoomPrices { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<ServiceDetail> ServiceDetails { get; set; }
        public virtual DbSet<TaxTable> TaxTables { get; set; }
        public virtual DbSet<ThankYouMessage> ThankYouMessages { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=173.248.129.77,1439; Database=FrankiesHotel; User ID=sa; Password=Pa$$w0rd@21; MultipleActiveResultSets=true");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.ToTable("AuditTrail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<BankDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BankName).HasMaxLength(100);

                entity.Property(e => e.BranchName).HasMaxLength(100);

                entity.Property(e => e.CompanyName).HasMaxLength(100);
            });

            modelBuilder.Entity<Billing>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActualRoomRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.Credit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("credit");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .HasColumnName("currency");

                entity.Property(e => e.CustomerBookingId).HasColumnName("customerBookingId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.CustomerIdTransferedFrom).HasColumnName("customerIdTransferedFrom");

                entity.Property(e => e.Debit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("debit");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.IsAccomodation).HasColumnName("isAccomodation");

                entity.Property(e => e.IsPayment).HasColumnName("isPayment");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasColumnName("notes");

                entity.Property(e => e.PaymentMethod).HasMaxLength(20);

                entity.Property(e => e.ReceiptNumber)
                    .HasMaxLength(255)
                    .HasColumnName("receiptNumber");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.RoomRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("roomRate");

                entity.Property(e => e.CurrencyRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("currencyRate");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Billings)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Billings_Company");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.BillingCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Billing_Customer");

                entity.HasOne(d => d.CustomerIdTransferedFromNavigation)
                    .WithMany(p => p.BillingCustomerIdTransferedFromNavigations)
                    .HasForeignKey(d => d.CustomerIdTransferedFrom)
                    .HasConstraintName("FK_Billings_InitialCustomer");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Billings)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Billing_Room");
            });

            modelBuilder.Entity<BillingsHistory>(entity =>
            {
                entity.ToTable("BillingsHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActualRoomRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.Credit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("credit");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .HasColumnName("currency");

                entity.Property(e => e.CustomerBookingId).HasColumnName("customerBookingId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.CustomerIdTransferedFrom).HasColumnName("customerIdTransferedFrom");

                entity.Property(e => e.Debit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("debit");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.IsAccomodation).HasColumnName("isAccomodation");

                entity.Property(e => e.IsPayment).HasColumnName("isPayment");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasColumnName("notes");

                entity.Property(e => e.PaymentMethod).HasMaxLength(20);

                entity.Property(e => e.ReceiptNumber)
                    .HasMaxLength(255)
                    .HasColumnName("receiptNumber");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.RoomRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("roomRate");
                entity.Property(e => e.CurrencyRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("currencyRate");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.BillingsHistories)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_BillingsHistory_Company");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.BillingsHistoryCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_BillingHistory_Customer");

                entity.HasOne(d => d.CustomerIdTransferedFromNavigation)
                    .WithMany(p => p.BillingsHistoryCustomerIdTransferedFromNavigations)
                    .HasForeignKey(d => d.CustomerIdTransferedFrom)
                    .HasConstraintName("FK_BillingsHistory_InitialCustomer");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.BillingsHistories)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_BillingHistory_Room");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.CustomerReceiptNumber).HasMaxLength(50);

                entity.Property(e => e.IsNightAudited).HasColumnName("isNightAudited");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Bookings_Company");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_Bookings_Guest");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Bookings_Room");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Credit).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Debit).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.FixRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.TinNumber)
                    .HasMaxLength(50)
                    .HasColumnName("tinNumber");
            });

            modelBuilder.Entity<CompanyBill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BillingId).HasColumnName("billingId");

                entity.Property(e => e.CheckNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.Credit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("credit");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .HasColumnName("currency")
                    .IsFixedLength();

                entity.Property(e => e.CustomerBookingId).HasColumnName("customerBookingId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Debit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("debit");

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .HasColumnName("description")
                    .IsFixedLength();

                entity.Property(e => e.IsPayment).HasColumnName("isPayment");

                entity.Property(e => e.ReceiptNumber)
                    .HasMaxLength(255)
                    .HasColumnName("receiptNumber")
                    .IsFixedLength();

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.RoomRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsBase).HasColumnName("isBase");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("rate");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Account)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.DocUrl).HasColumnType("text");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Idnumber)
                    .HasMaxLength(125)
                    .HasColumnName("IDNumber")
                    .IsFixedLength();

                entity.Property(e => e.Idtype)
                    .HasMaxLength(155)
                    .HasColumnName("IDType")
                    .IsFixedLength();

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Guests_Company");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.NationalityId)
                    .HasConstraintName("FK_Guests_Nationality");
            });

            modelBuilder.Entity<GuestService>(entity =>
            {
                entity.Property(e => e.IsNightAudited).HasColumnName("isNightAudited");

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.GuestServices)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_GuestServices_Bookings");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.GuestServices)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_GuestServices_Guests");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.GuestServices)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuestServices_Rooms");
            });

            modelBuilder.Entity<HoursExtension>(entity =>
            {
                entity.ToTable("HoursExtension");

                entity.Property(e => e.Hours)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsCharged)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tariff)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HouseKeepingItem>(entity =>
            {
                entity.ToTable("HouseKeepingItem");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ServiceCategory)
                    .WithMany(p => p.HouseKeepingItems)
                    .HasForeignKey(d => d.ServiceCategoryId)
                    .HasConstraintName("FK_HouseKeepingItem_ServiceCategory");
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("Nationality");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<NightAudit>(entity =>
            {
                entity.ToTable("NightAudit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Notes).HasColumnType("text");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("FK_Notes_Guests");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("Notice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedAt).HasColumnName("Created_At");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ModifiedAt).HasColumnName("Modified_At");

                entity.Property(e => e.ModifiedBy).HasColumnName("Modified_By");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsCredit).HasColumnName("isCredit");

                entity.Property(e => e.IsDebit).HasColumnName("isDebit");

                entity.Property(e => e.IsPayment).HasColumnName("isPayment");

                entity.Property(e => e.Name).HasMaxLength(225);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.Image);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_RoomsType_Rooms");
            });

            modelBuilder.Entity<RoomPrice>(entity =>
            {
                entity.ToTable("RoomPrice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsFixedLength();
                entity.Property(e => e.Image);

                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<ServiceCategory>(entity =>
            {
                entity.ToTable("ServiceCategory");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ServiceDetail>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ServiceCategory)
                    .WithMany(p => p.ServiceDetails)
                    .HasForeignKey(d => d.ServiceCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceDetails_ServiceCategory");
            });

            modelBuilder.Entity<TaxTable>(entity =>
            {
                entity.ToTable("TaxTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsLevy).HasColumnName("isLevy");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("rate");
            });

            modelBuilder.Entity<ThankYouMessage>(entity =>
            {
                entity.ToTable("ThankYouMessage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
