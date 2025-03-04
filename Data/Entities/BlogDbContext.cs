using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace blog_website_api.Data.Entities
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostComment> PostComments { get; set; } = null!;
        public virtual DbSet<PostImage> PostImages { get; set; } = null!;
        public virtual DbSet<PostLike> PostLikes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:PostgresDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("status", new[] { "archive", "ban", "public" });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasColumnType("character varying")
                    .HasColumnName("slug");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Slug)
                    .HasColumnType("character varying")
                    .HasColumnName("slug");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("update_at");

                entity.Property(e => e.View)
                    .HasColumnName("view")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("fk_author_post");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_post_category");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.ToTable("post_comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.ToTable("post_image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.PublicId)
                    .HasColumnType("character varying")
                    .HasColumnName("public_id");

                entity.Property(e => e.Url)
                    .HasColumnType("character varying")
                    .HasColumnName("url");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.ToTable("post_like");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasColumnType("character varying")
                    .HasColumnName("avatar");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.FullName).HasColumnName("full_name");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("character varying")
                    .HasColumnName("refresh_token");

                entity.Property(e => e.TokenExpiryTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("token_expiry_time");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("update_at");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
