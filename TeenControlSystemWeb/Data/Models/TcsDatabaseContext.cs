using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class TcsDatabaseContext : DbContext
    {
        public TcsDatabaseContext()
        {
        }

        public TcsDatabaseContext(DbContextOptions<TcsDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DefaultAvatar> DefaultAvatars { get; set; } = null!;
        public virtual DbSet<Point> Points { get; set; } = null!;
        public virtual DbSet<Sensor> Sensors { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TcsDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Points)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Points__SessionI__2B3F6F97");
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.HasIndex(e => e.Order, "Sensors_Order_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Mac, "UQ__Sensors__C7977BD428267B12")
                    .IsUnique();

                entity.Property(e => e.Mac)
                    .HasMaxLength(48)
                    .IsUnicode(false);

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Sensors)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK__Sensors__Session__300424B4");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(512);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sessions__OwnerI__286302EC");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.PasswordMd5Hash)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.HasOne(d => d.DefaultAvatar)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DefaultAvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__DefaultAv__49C3F6B7");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK__Users__SessionId__34C8D9D1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
