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

        public virtual DbSet<Login> Logins { get; set; } = null!;

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
            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.LoginId).HasColumnName("Login_ID");

                entity.Property(e => e.LoginEmail)
                    .HasMaxLength(50)
                    .HasColumnName("Login_Email");

                entity.Property(e => e.LoginLevel).HasColumnName("Login_Level");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(16)
                    .HasColumnName("Login_Name");

                entity.Property(e => e.LoginPwd)
                    .HasMaxLength(20)
                    .HasColumnName("Login_Pwd");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
