﻿// <auto-generated />
using System;
using BenchmarkCar.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BenchmarkCar.Ui.Api.Migrations
{
    [DbContext(typeof(SqlVehicleContext))]
    [Migration("20240130011859_model and makes")]
    partial class modelandmakes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.BestModelModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("InsertedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("VehicleModelId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("BestModels");
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.ModelBodyModel", b =>
                {
                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Doors")
                        .HasColumnType("int");

                    b.Property<decimal?>("EngineSize")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("InsertedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("Length")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.Property<decimal?>("Width")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ModelId");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("ModelBodies");
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.ModelEngineModel", b =>
                {
                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("HorsePowerHp")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("HorsePowerRpm")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset>("InsertedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("TorqueFtLbs")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TorqueRpm")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Valves")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ModelId");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("EngineModels");
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.VehicleMakeModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("InsertedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("VehiclesMakes");
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.VehicleModelModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("InsertedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("VehicleMakeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.HasIndex("VehicleMakeId");

                    b.ToTable("VehiclesModels");
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.BestModelModel", b =>
                {
                    b.HasOne("BenchmarkCar.Application.Model.Vehicles.VehicleModelModel", null)
                        .WithMany()
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.ModelBodyModel", b =>
                {
                    b.HasOne("BenchmarkCar.Application.Model.Vehicles.VehicleModelModel", null)
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.ModelEngineModel", b =>
                {
                    b.HasOne("BenchmarkCar.Application.Model.Vehicles.VehicleModelModel", null)
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BenchmarkCar.Application.Model.Vehicles.VehicleModelModel", b =>
                {
                    b.HasOne("BenchmarkCar.Application.Model.Vehicles.VehicleMakeModel", "VehicleMake")
                        .WithMany()
                        .HasForeignKey("VehicleMakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleMake");
                });
#pragma warning restore 612, 618
        }
    }
}
