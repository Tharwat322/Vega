﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vega.Persistence;

namespace Vega.Migrations
{
    [DbContext(typeof(VegaDbContext))]
    [Migration("20200817134210_SeedFeature")]
    partial class SeedFeature
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Vega.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Vega.Models.Make", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Makes");
                });

            modelBuilder.Entity("Vega.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MakeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("MakeId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Vega.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("bit"); 

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Vega.Models.VehicleFeature", b =>
                {
                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.HasKey("VehicleId", "FeatureId");

                    b.HasIndex("FeatureId");

                    b.ToTable("VehicleFeatures");
                });

            modelBuilder.Entity("Vega.Models.Model", b =>
                {
                    b.HasOne("Vega.Models.Make", "Make")
                        .WithMany("Models")
                        .HasForeignKey("MakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vega.Models.Vehicle", b =>
                {
                    b.HasOne("Vega.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vega.Models.VehicleFeature", b =>
                {
                    b.HasOne("Vega.Models.Feature", "Feature")
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vega.Models.Vehicle", "Vehicle")
                        .WithMany("Features")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
