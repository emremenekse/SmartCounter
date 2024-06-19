﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReportService.Data;

#nullable disable

namespace ReportService.Migrations
{
    [DbContext(typeof(ReportContext))]
    partial class ReportContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReportService.Entities.ReportRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("MeasurementTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RequestTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ReportRequests");
                });

            modelBuilder.Entity("ReportService.Entities.ReportResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("GeneratedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReportRequestId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("ReportResults");
                });
#pragma warning restore 612, 618
        }
    }
}
