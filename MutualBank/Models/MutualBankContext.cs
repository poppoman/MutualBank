using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MutualBank.Models
{
    public partial class MutualBankContext : DbContext
    {
        public MutualBankContext()
        {
        }

        public MutualBankContext(DbContextOptions<MutualBankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Point> Points { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=61.216.13.147,18349;Initial Catalog=MutualBank;Persist Security Info=True;User ID=TGM101;Password=TGM@5832");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Point>(entity =>
            {
                entity.Property(e => e.PointId).HasColumnName("Point_ID");

                entity.Property(e => e.PointAddDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Point_AddDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PointCaseId).HasColumnName("Point_CaseID");

                entity.Property(e => e.PointIsDone).HasColumnName("Point_IsDone");

                entity.Property(e => e.PointNeedHelp).HasColumnName("Point_NeedHelp");

                entity.Property(e => e.PointQuantity).HasColumnName("Point_Quantity");

                entity.Property(e => e.PointSpgorder)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Point_SPGOrder");

                entity.Property(e => e.PointUserId).HasColumnName("Point_UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
