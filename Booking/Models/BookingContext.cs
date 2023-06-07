using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Booking.Models;

public partial class BookingContext : DbContext
{
    public BookingContext()
    {
    }

    public BookingContext(DbContextOptions<BookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityView> ActivityViews { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<TicketTypeRemainingView> TicketTypeRemainingViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("Activity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(2083)
                .HasColumnName("PhotoURL");
            entity.Property(e => e.SalesEnd).HasColumnType("datetime");
            entity.Property(e => e.SalesStart).HasColumnType("datetime");
            entity.Property(e => e.ShowingEnd).HasColumnType("datetime");
            entity.Property(e => e.ShowingStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<ActivityView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ActivityView");

            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Daydiff).HasColumnName("DAYDIFF");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Sales).HasColumnName("SALES");
            entity.Property(e => e.ShowingEnd).HasColumnType("datetime");
            entity.Property(e => e.ShowingStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password).HasMaxLength(512);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salt).HasMaxLength(512);
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.ToTable("Like");

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MemberName)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.Password).HasMaxLength(512);
            entity.Property(e => e.Salt).HasMaxLength(512);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Order_1");

            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Items).HasMaxLength(500);
            entity.Property(e => e.Trading).HasColumnType("datetime");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.ToTable("ShoppingCart");

            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");
            entity.Property(e => e.AddedDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.ToTable("TicketType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TicketTypeRemainingView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TicketTypeRemainingView");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
