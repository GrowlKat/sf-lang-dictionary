using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SF_Lang_Dictionary.Models;

public partial class SfLangContext : DbContext
{
    public SfLangContext()
    {
    }

    public SfLangContext(DbContextOptions<SfLangContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Maintype> Maintypes { get; set; }

    public virtual DbSet<Prefix> Prefixes { get; set; }

    public virtual DbSet<Rootword> Rootwords { get; set; }

    public virtual DbSet<Subtype> Subtypes { get; set; }

    public virtual DbSet<Suffix> Suffixes { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SFConn"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Maintype>(entity =>
        {
            entity.HasKey(e => e.MtpId).HasName("PK__MAINTYPE__611BAFF4BAF54B15");

            entity.ToTable("MAINTYPES");

            entity.HasIndex(e => e.Maintype1, "UQ__MAINTYPE__94737515934264F4").IsUnique();

            entity.Property(e => e.MtpId).HasColumnName("mtp_id");
            entity.Property(e => e.Maintype1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("maintype");
        });

        modelBuilder.Entity<Prefix>(entity =>
        {
            entity.HasKey(e => e.PfxId).HasName("PK__PREFIX__BEC707993B9A2F92");

            entity.ToTable("PREFIXES");

            entity.HasIndex(e => e.StpId, "UQ__PREFIX__28DB520FA5CBDF74").IsUnique();

            entity.HasIndex(e => e.Prefix1, "UQ__PREFIX__DA9292187010B6B5").IsUnique();

            entity.Property(e => e.PfxId).HasColumnName("pfx_id");
            entity.Property(e => e.MtpId).HasColumnName("mtp_id");
            entity.Property(e => e.Prefix1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("prefix");
            entity.Property(e => e.StpId).HasColumnName("stp_id");

            entity.HasOne(d => d.Mtp).WithMany(p => p.Prefixes)
                .HasForeignKey(d => d.MtpId)
                .HasConstraintName("FK__PREFIX__mtp_id__7A672E12");

            entity.HasOne(d => d.Stp).WithOne(p => p.Prefix)
                .HasForeignKey<Prefix>(d => d.StpId)
                .HasConstraintName("FK__PREFIX__stp_id__7B5B524B");
        });

        modelBuilder.Entity<Rootword>(entity =>
        {
            entity.HasKey(e => e.RootId).HasName("PK__ROOTWORD__CD85A0350B3EFB47");

            entity.ToTable("ROOTWORDS");

            entity.Property(e => e.RootId).HasColumnName("root_id");
            entity.Property(e => e.Meaning)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("meaning");
            entity.Property(e => e.Pronunciation)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("pronunciation");
            entity.Property(e => e.Rootword1)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("rootword");
            entity.Property(e => e.Tags)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("tags");
        });

        modelBuilder.Entity<Subtype>(entity =>
        {
            entity.HasKey(e => e.StpId).HasName("PK__SUBTYPE__28DB520EF66363DD");

            entity.ToTable("SUBTYPES");

            entity.Property(e => e.StpId).HasColumnName("stp_id");
            entity.Property(e => e.Subtype1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("subtype");
        });

        modelBuilder.Entity<Suffix>(entity =>
        {
            entity.HasKey(e => e.SfxId).HasName("PK__SUFFIX__3FA9CCF494D93175");

            entity.ToTable("SUFFIXES");

            entity.HasIndex(e => new { e.MtpId, e.SfxId }, "SFX_UNIQUE").IsUnique();

            entity.Property(e => e.SfxId).HasColumnName("sfx_id");
            entity.Property(e => e.MtpId).HasColumnName("mtp_id");
            entity.Property(e => e.StpId).HasColumnName("stp_id");
            entity.Property(e => e.Suffix1)
                .HasMaxLength(10)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("suffix");

            entity.HasOne(d => d.Mtp).WithMany(p => p.Suffixes)
                .HasForeignKey(d => d.MtpId)
                .HasConstraintName("FK__SUFFIX__mtp_id__01142BA1");

            entity.HasOne(d => d.Stp).WithMany(p => p.Suffixes)
                .HasForeignKey(d => d.StpId)
                .HasConstraintName("FK__SUFFIX__stp_id__02084FDA");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordId).HasName("PK_WORD");

            entity.ToTable("WORDS");

            entity.HasIndex(e => e.WordId, "IX_WORD");

            entity.Property(e => e.WordId)
                .ValueGeneratedNever()
                .HasColumnName("word_id");
            entity.Property(e => e.FirstWord).HasColumnName("first_word");
            entity.Property(e => e.Meaning)
                .HasMaxLength(50)
                .HasColumnName("meaning");
            entity.Property(e => e.SecondWord).HasColumnName("second_word");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
