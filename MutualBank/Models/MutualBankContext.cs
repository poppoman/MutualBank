﻿using System;
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

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Case> Cases { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Point> Points { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

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
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Area");

                entity.Property(e => e.AreaCity)
                    .HasMaxLength(10)
                    .HasColumnName("Area_City");

                entity.Property(e => e.AreaId).HasColumnName("Area_ID");

                entity.Property(e => e.AreaTown)
                    .HasMaxLength(10)
                    .HasColumnName("Area_Town");
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Case");

                entity.Property(e => e.CaseAddDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_AddDate");

                entity.Property(e => e.CaseClosedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_ClosedDate");

                entity.Property(e => e.CaseExpireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_ExpireDate");

                entity.Property(e => e.CaseId).HasColumnName("Case_ID");

                entity.Property(e => e.CaseIntroduction)
                    .HasMaxLength(200)
                    .HasColumnName("Case_Introduction");

                entity.Property(e => e.CaseNeedHelp).HasColumnName("Case_NeedHelp");

                entity.Property(e => e.CasePhoto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Case_Photo");

                entity.Property(e => e.CaseReleaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_ReleaseDate");

                entity.Property(e => e.CaseSerArea).HasColumnName("Case_SerArea");

                entity.Property(e => e.CaseSerDate)
                    .HasMaxLength(20)
                    .HasColumnName("Case_SerDate");

                entity.Property(e => e.CaseSkilId).HasColumnName("Case_SkilID");

                entity.Property(e => e.CaseTitle)
                    .HasMaxLength(50)
                    .HasColumnName("Case_Title");

                entity.Property(e => e.CaseUserId).HasColumnName("Case_UserID");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Login");

                entity.Property(e => e.LoginEmail)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Login_Email");

                entity.Property(e => e.LoginId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Login_ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(16)
                    .HasColumnName("Login_Name");

                entity.Property(e => e.LoginPwd)
                    .HasMaxLength(20)
                    .HasColumnName("Login_Pwd");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MsgId);

                entity.ToTable("Message");

                entity.Property(e => e.MsgId)
                    .ValueGeneratedNever()
                    .HasColumnName("Msg_ID");

                entity.Property(e => e.MsgAddDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Msg_AddDate");

                entity.Property(e => e.MsgCaseId).HasColumnName("Msg_CaseID");

                entity.Property(e => e.MsgContent)
                    .HasMaxLength(200)
                    .HasColumnName("Msg_Content");

                entity.Property(e => e.MsgUserId).HasColumnName("Msg_UserID");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.PointAddDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Point_AddDate");

                entity.Property(e => e.PointCaseId).HasColumnName("Point_CaseID");

                entity.Property(e => e.PointId).HasColumnName("Point_ID");

                entity.Property(e => e.PointNeedHelp).HasColumnName("Point_NeedHelp");

                entity.Property(e => e.PointQuantity).HasColumnName("Point_Quantity");

                entity.Property(e => e.PointUserId).HasColumnName("Point_UserID");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Skill");

                entity.Property(e => e.SkillId).HasColumnName("Skill_ID");

                entity.Property(e => e.SkillName)
                    .HasMaxLength(50)
                    .HasColumnName("Skill_Name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.UserAreaId).HasColumnName("User_AreaID");

                entity.Property(e => e.UserBirthday)
                    .HasColumnType("datetime")
                    .HasColumnName("User_Birthday");

                entity.Property(e => e.UserCv)
                    .HasMaxLength(50)
                    .HasColumnName("User_CV");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("User_Email");

                entity.Property(e => e.UserFaculty)
                    .HasMaxLength(20)
                    .HasColumnName("User_Faculty");

                entity.Property(e => e.UserFname)
                    .HasMaxLength(5)
                    .HasColumnName("User_FName");

                entity.Property(e => e.UserHphoto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("User_HPhoto");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.UserLname)
                    .HasMaxLength(5)
                    .HasColumnName("User_LName");

                entity.Property(e => e.UserNname)
                    .HasMaxLength(50)
                    .HasColumnName("User_NName");

                entity.Property(e => e.UserPoint).HasColumnName("User_Point");

                entity.Property(e => e.UserResume)
                    .HasMaxLength(50)
                    .HasColumnName("User_Resume");

                entity.Property(e => e.UserSchool)
                    .HasMaxLength(20)
                    .HasColumnName("User_School");

                entity.Property(e => e.UserSex).HasColumnName("User_Sex");

                entity.Property(e => e.UserSkillId).HasColumnName("User_SkillID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
