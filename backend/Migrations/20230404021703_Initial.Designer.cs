﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using backend.entity;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(dbContext))]
    [Migration("20230404021703_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("backend.entity.Action", b =>
                {
                    b.Property<int>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Action1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ActionId");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("backend.entity.ReadingList", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("article")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ArticleId");

                    b.ToTable("ReadingLists");
                });

            modelBuilder.Entity("backend.entity.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TaskTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TaskId");

                    b.HasIndex("ActionId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("backend.entity.User", b =>
                {
                    b.Property<int?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<bool?>("UserAiSwitchedOnPreference")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("UserIsAccountLocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("UserIsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.HasIndex("ActionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("backend.entity.Watchlist", b =>
                {
                    b.Property<int>("WatchListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Stocks")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("WatchListName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("WatchListId");

                    b.HasIndex("UserId");

                    b.ToTable("Watchlists");
                });

            modelBuilder.Entity("backend.entity.Task", b =>
                {
                    b.HasOne("backend.entity.Action", "Action")
                        .WithMany("Tasks")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.entity.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.entity.User", b =>
                {
                    b.HasOne("backend.entity.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");
                });

            modelBuilder.Entity("backend.entity.Watchlist", b =>
                {
                    b.HasOne("backend.entity.User", "User")
                        .WithMany("Watchlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.entity.Action", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("backend.entity.User", b =>
                {
                    b.Navigation("Tasks");

                    b.Navigation("Watchlists");
                });
#pragma warning restore 612, 618
        }
    }
}
