﻿// <auto-generated />
using BossSystem.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BossSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200610171220_SiteConfigurations")]
    partial class SiteConfigurations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BossSystem.Models.Dbos.SiteConfiguration", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("SiteConfigurations");

                    b.HasData(
                        new
                        {
                            Key = "AdminPassword",
                            Value = "TheBoss"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
