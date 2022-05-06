﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UserContextLib;

#nullable disable

namespace UserContextLib.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UserContextLib.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("Password")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "admin",
                            Password = new byte[] { 74, 127, 25, 134, 139, 148, 52, 183, 7, 133, 43, 77, 211, 133, 254, 244, 224, 182, 170, 197, 135, 106, 148, 219, 178, 164, 61, 214, 20, 198, 28, 83, 142, 187, 248, 182, 100, 137, 201, 73, 113, 200, 86, 117, 36, 191, 143, 43, 6, 231, 85, 169, 57, 77, 225, 146, 125, 38, 10, 229, 148, 105, 230, 205 },
                            PasswordSalt = new byte[] { 214, 10, 156, 1, 138, 132, 199, 136, 85, 8, 120, 27, 40, 154, 139, 111, 52, 196, 1, 24, 54, 228, 162, 42, 97, 80, 215, 24, 253, 105, 57, 196, 219, 198, 225, 225, 47, 255, 149, 15, 83, 123, 106, 245, 104, 186, 95, 53, 243, 192, 136, 117, 116, 152, 98, 94, 253, 48, 0, 32, 255, 76, 184, 58, 97, 248, 148, 235, 241, 234, 42, 140, 81, 141, 217, 42, 6, 8, 105, 196, 68, 22, 53, 169, 189, 232, 216, 102, 0, 35, 143, 54, 113, 132, 104, 247, 158, 8, 208, 170, 227, 169, 179, 220, 193, 70, 100, 63, 189, 124, 155, 34, 145, 220, 240, 231, 116, 61, 234, 147, 150, 188, 165, 213, 140, 80, 169, 131 },
                            Role = "Admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
