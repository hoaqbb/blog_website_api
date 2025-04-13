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
        public virtual DbSet<CommentLike> CommentLikes { get; set; } = null!;
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
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasColumnType("character varying")
                    .HasColumnName("slug");
            });

            modelBuilder.Entity<CommentLike>(entity =>
            {
                entity.ToTable("comment_like");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentLikes)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("comment_like_post_comment_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("comment_like_user_fk");
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
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ShortDescription)
                    .HasColumnType("character varying")
                    .HasColumnName("short_description");

                entity.Property(e => e.Slug)
                    .HasColumnType("character varying")
                    .HasColumnName("slug");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.UpdateAt).HasColumnName("update_at");

                entity.Property(e => e.View)
                    .HasColumnName("view")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("post_user_fk");

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

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("post_comment_post_comment_fk");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("post_comment_post_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("post_comment_user_fk");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.ToTable("post_image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsThumbnail)
                    .HasColumnName("is_thumbnail")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.PublicId)
                    .HasColumnType("character varying")
                    .HasColumnName("public_id");

                entity.Property(e => e.Url)
                    .HasColumnType("character varying")
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("post_image_post_fk");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.ToTable("post_like");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("post_like_post_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("post_like_user_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasColumnType("character varying")
                    .HasColumnName("avatar");

                entity.Property(e => e.CreateAt)
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.FullName).HasColumnName("full_name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Provider)
                    .HasColumnType("character varying")
                    .HasColumnName("provider");

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("character varying")
                    .HasColumnName("refresh_token");

                entity.Property(e => e.Role)
                    .HasColumnType("character varying")
                    .HasColumnName("role")
                    .HasDefaultValueSql("'Member'::character varying");

                entity.Property(e => e.TokenExpiryTime).HasColumnName("token_expiry_time");

                entity.Property(e => e.UpdateAt).HasColumnName("update_at");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
