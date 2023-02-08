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

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Booking;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("Activity");

            entity.Property(e => e.ActivityId)
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActivityID");
            entity.Property(e => e.ActivityName).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.EndDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Photo).HasMaxLength(50);
            entity.Property(e => e.PhotoUrl)
                .IsUnicode(false)
                .HasColumnName("PhotoURL");
            entity.Property(e => e.StartDateTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CompanyID");
            entity.Property(e => e.CompanyAccount).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CompanyPhone).HasMaxLength(50);
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberId)
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MemberID");
            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.MemberName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TicketID");
            entity.Property(e => e.Activity).HasMaxLength(50);
            entity.Property(e => e.Member).HasMaxLength(50);
            entity.Property(e => e.OrderTime).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
