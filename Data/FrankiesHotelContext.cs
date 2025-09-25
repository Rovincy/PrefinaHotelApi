//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace HotelWebApi.UserModels
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
//        public virtual DbSet<Room> Rooms { get; set; }
//        public virtual DbSet<RoomPrice> RoomPrices { get; set; }
//        public virtual DbSet<RoomType> RoomTypes { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=173.248.129.77,1439; Database=FrankiesHotel; User ID=sa; Password=Pa$$w0rd@21; MultipleActiveResultSets=true");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Booking>(entity =>
//            {
//                entity.Property(e => e.Id).HasColumnName("id");

//                entity.Property(e => e.CustomerName)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.NextOfKin)
//                    .HasMaxLength(255)
//                    .IsFixedLength();

//                entity.Property(e => e.NkNumber)
//                    .HasMaxLength(150)
//                    .HasColumnName("NK_Number")
//                    .IsFixedLength();

//                entity.Property(e => e.PhoneNumber)
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.Property(e => e.Timestamp)
//                    .IsRowVersion()
//                    .IsConcurrencyToken();

//                entity.HasOne(d => d.Room)
//                    .WithMany(p => p.Bookings)
//                    .HasForeignKey(d => d.RoomId)
//                    .HasConstraintName("FK_Bookings_Room");
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
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.Property(e => e.Name)
//                    .HasMaxLength(150)
//                    .IsFixedLength();

//                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
