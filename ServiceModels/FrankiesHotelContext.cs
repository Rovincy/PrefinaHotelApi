//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//using HotelWebApi.UserModels;

//namespace HotelWebApi.ServiceModels
//{
//    public partial class FrankiesHotelContext : DbContext
//    {
//        public FrankiesHotelContext()
//        {
//        }

//        public FrankiesHotelContext(DbContextOptions<FrankiesHotelContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Booking> Bookings { get; set; }
//        public virtual DbSet<Guest> Guests { get; set; }
//        public virtual DbSet<GuestService> GuestServices { get; set; }
//        public virtual DbSet<HouseKeepingItem> HouseKeepingItems { get; set; }
//        //public virtual DbSet<NightAudit> NightAudits { get; set; }
//        public virtual DbSet<Note> Notes { get; set; }
//        public virtual DbSet<Room> Rooms { get; set; }
//        public virtual DbSet<RoomPrice> RoomPrices { get; set; }
//        public virtual DbSet<RoomType> RoomTypes { get; set; }
//        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
//        public virtual DbSet<ServiceDetail> ServiceDetails { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=173.248.129.77,1439; Database=FrankiesHotel; User ID=sa; Password=Pa$$w0rd@21; MultipleActiveResultSets=true; TrustServerCertificate=True");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Booking>(entity =>
//            {
//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.HasOne(d => d.Guest)
//                    .WithMany(p => p.Bookings)
//                    .HasForeignKey(d => d.GuestId)
//                    .HasConstraintName("FK_Bookings_Guest");

//                entity.HasOne(d => d.Room)
//                    .WithMany(p => p.Bookings)
//                    .HasForeignKey(d => d.RoomId)
//                    .HasConstraintName("FK_Bookings_Room");
//            });

//            modelBuilder.Entity<Guest>(entity =>
//            {
//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Account)
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.Property(e => e.Dob).HasColumnName("DOB");

//                entity.Property(e => e.DocUrl).HasColumnType("text");

//                entity.Property(e => e.Email)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.Firstname)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.Gender)
//                    .HasMaxLength(20)
//                    .IsFixedLength();

//                entity.Property(e => e.Idnumber)
//                    .HasMaxLength(125)
//                    .HasColumnName("IDNumber")
//                    .IsFixedLength();

//                entity.Property(e => e.Idtype)
//                    .HasMaxLength(155)
//                    .HasColumnName("IDType")
//                    .IsFixedLength();

//                entity.Property(e => e.Lastname)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.Nationality)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.PhoneNumber)
//                    .HasMaxLength(255)
//                    .IsFixedLength();
//            });

//            modelBuilder.Entity<GuestService>(entity =>
//            {
//                entity.Property(e => e.Service)
//                    .IsRequired()
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                //entity.Property(e => e.Timestamp).HasColumnName("timestamp");

//                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

//                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

//                entity.HasOne(d => d.Booking)
//                    .WithMany(p => p.GuestServices)
//                    .HasForeignKey(d => d.BookingId)
//                    .HasConstraintName("FK_GuestServices_Bookings");

//                entity.HasOne(d => d.Guest)
//                    .WithMany(p => p.GuestServices)
//                    .HasForeignKey(d => d.GuestId)
//                    .HasConstraintName("FK_GuestServices_Guests");

//                entity.HasOne(d => d.Room)
//                    .WithMany(p => p.GuestServices)
//                    .HasForeignKey(d => d.RoomId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_GuestServices_Rooms");
//            });

//            modelBuilder.Entity<HouseKeepingItem>(entity =>
//            {
//                entity.ToTable("HouseKeepingItem");

//                entity.Property(e => e.Description)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Name)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.HasOne(d => d.ServiceCategory)
//                    .WithMany(p => p.HouseKeepingItems)
//                    .HasForeignKey(d => d.ServiceCategoryId)
//                    .HasConstraintName("FK_HouseKeepingItem_ServiceCategory");
//            });

//            //modelBuilder.Entity<NightAudit>(entity =>
//            //{
//            //    entity.ToTable("NightAudit");

//            //    entity.Property(e => e.Id).HasColumnName("id");

//            //    entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

//            //    entity.Property(e => e.Timestamp).HasColumnName("timestamp");
//            //});

//            modelBuilder.Entity<Note>(entity =>
//            {
//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Notes).HasColumnType("text");

//                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

//                entity.HasOne(d => d.Guest)
//                    .WithMany(p => p.Notes)
//                    .HasForeignKey(d => d.GuestId)
//                    .HasConstraintName("FK_Notes_Guests");
//            });

//            modelBuilder.Entity<Room>(entity =>
//            {
//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Name)
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.HasOne(d => d.Type)
//                    .WithMany(p => p.Rooms)
//                    .HasForeignKey(d => d.TypeId)
//                    .HasConstraintName("FK_RoomsType_Rooms");
//            });

//            modelBuilder.Entity<RoomPrice>(entity =>
//            {
//                entity.ToTable("RoomPrice");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
//            });

//            modelBuilder.Entity<RoomType>(entity =>
//            {
//                entity.ToTable("RoomType");

//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.Description)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.Name)
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
//            });

//            modelBuilder.Entity<ServiceCategory>(entity =>
//            {
//                entity.ToTable("ServiceCategory");

//                entity.Property(e => e.Description).HasColumnType("text");

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(100)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<ServiceDetail>(entity =>
//            {
//                entity.Property(e => e.Description).HasColumnType("text");

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(100)
//                    .IsUnicode(false);

//                entity.Property(e => e.Price)
//                    .HasMaxLength(50)
//                    .IsUnicode(false);

//                entity.HasOne(d => d.ServiceCategory)
//                    .WithMany(p => p.ServiceDetails)
//                    .HasForeignKey(d => d.ServiceCategoryId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_ServiceDetails_ServiceCategory");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
