﻿// <auto-generated />
using System;
using DicomWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DicomWebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230424050520_updatestudytable")]
    partial class updatestudytable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DicomWebAPI.Model.Image", b =>
                {
                    b.Property<string>("ImageInstanceUID")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Columns")
                        .HasColumnType("int");

                    b.Property<int>("Rows")
                        .HasColumnType("int");

                    b.Property<string>("SeriesInstanceUID")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ImageInstanceUID");

                    b.HasIndex("SeriesInstanceUID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("DicomWebAPI.Model.LocalUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LocalUsers");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PatientDOB")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PatientName")
                        .HasColumnType("longtext");

                    b.HasKey("PatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Series", b =>
                {
                    b.Property<string>("SeriesInstanceUID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BodyPart")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Modality")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StudyInstanceUID")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("SeriesInstanceUID");

                    b.HasIndex("StudyInstanceUID");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Study", b =>
                {
                    b.Property<string>("StudyInstanceUID")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StudyDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StudyDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StudyID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("StudyInstanceUID");

                    b.HasIndex("PatientId");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Image", b =>
                {
                    b.HasOne("DicomWebAPI.Model.Series", "Series")
                        .WithMany("Images")
                        .HasForeignKey("SeriesInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Series", b =>
                {
                    b.HasOne("DicomWebAPI.Model.Study", "Study")
                        .WithMany("Series")
                        .HasForeignKey("StudyInstanceUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Study");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Study", b =>
                {
                    b.HasOne("DicomWebAPI.Model.Patient", "Patient")
                        .WithMany("Studies")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Patient", b =>
                {
                    b.Navigation("Studies");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Series", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("DicomWebAPI.Model.Study", b =>
                {
                    b.Navigation("Series");
                });
#pragma warning restore 612, 618
        }
    }
}
