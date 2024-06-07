﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicketingSystem.Infrastructure.DBContext;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240607094223_UPDATE_ACCESSPERMISSION")]
    partial class UPDATE_ACCESSPERMISSION
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.AccessPermission", b =>
                {
                    b.Property<Guid>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("GrantedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Permission")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PermissionId");

                    b.HasIndex("GrantedById");

                    b.HasIndex("UserId");

                    b.ToTable("AccessPermissions");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DepartmentId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<Guid?>("RaisedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("TicketId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("RaisedById");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketAssignment", b =>
                {
                    b.Property<Guid>("TicketAssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TicketAssignmentId");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("TicketAssignments");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketLog", b =>
                {
                    b.Property<Guid>("TicketLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActionUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TicketLogAction")
                        .HasColumnType("int");

                    b.HasKey("TicketLogId");

                    b.HasIndex("ActionUserId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketLogs");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketResponse", b =>
                {
                    b.Property<Guid>("TicketResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("ResponseMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ResponseUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TicketResponseId");

                    b.HasIndex("ResponseUserId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketResponses");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNewUser")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserCreation", b =>
                {
                    b.Property<Guid>("UserCreationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserCreationId");

                    b.HasIndex("CreatorUserId");

                    b.ToTable("UserCreations");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserSession", b =>
                {
                    b.Property<Guid>("UserSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BrowserInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserSessionId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.AccessPermission", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "GrantedBy")
                        .WithMany("GrantedPermissions")
                        .HasForeignKey("GrantedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "User")
                        .WithMany("AccessPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrantedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.Department", "Department")
                        .WithMany("Tickets")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "RaisedBy")
                        .WithMany("RaisedTickets")
                        .HasForeignKey("RaisedById");

                    b.Navigation("Department");

                    b.Navigation("RaisedBy");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketAssignment", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "AssignedUser")
                        .WithMany("TicketAssignments")
                        .HasForeignKey("AssignedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TicketingSystem.Core.Domain.Entities.Ticket", "Ticket")
                        .WithOne("TicketAssignment")
                        .HasForeignKey("TicketingSystem.Core.Domain.Entities.TicketAssignment", "TicketId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketLog", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "ActionUser")
                        .WithMany("TicketLogs")
                        .HasForeignKey("ActionUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TicketingSystem.Core.Domain.Entities.Ticket", "Ticket")
                        .WithMany("Logs")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionUser");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.TicketResponse", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "ResponseUser")
                        .WithMany("TicketResponses")
                        .HasForeignKey("ResponseUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TicketingSystem.Core.Domain.Entities.Ticket", "Ticket")
                        .WithMany("TicketResponses")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResponseUser");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.User", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserCreation", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "CreatorUser")
                        .WithMany("CreatedUsers")
                        .HasForeignKey("CreatorUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "CreatedUser")
                        .WithOne("UserCreation")
                        .HasForeignKey("TicketingSystem.Core.Domain.Entities.UserCreation", "UserCreationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("CreatorUser");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "User")
                        .WithOne("Role")
                        .HasForeignKey("TicketingSystem.Core.Domain.Entities.UserRole", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.UserSession", b =>
                {
                    b.HasOne("TicketingSystem.Core.Domain.Entities.User", "User")
                        .WithOne("UserSession")
                        .HasForeignKey("TicketingSystem.Core.Domain.Entities.UserSession", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.Department", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.Ticket", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("TicketAssignment");

                    b.Navigation("TicketResponses");
                });

            modelBuilder.Entity("TicketingSystem.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("AccessPermissions");

                    b.Navigation("CreatedUsers");

                    b.Navigation("GrantedPermissions");

                    b.Navigation("RaisedTickets");

                    b.Navigation("Role");

                    b.Navigation("TicketAssignments");

                    b.Navigation("TicketLogs");

                    b.Navigation("TicketResponses");

                    b.Navigation("UserCreation");

                    b.Navigation("UserSession");
                });
#pragma warning restore 612, 618
        }
    }
}
