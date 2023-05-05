using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DicomWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: true),
                    Role = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "longtext", nullable: true),
                    PatientDOB = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Studies",
                columns: table => new
                {
                    StudyInstanceUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    StudyDescription = table.Column<string>(type: "longtext", nullable: false),
                    StudyID = table.Column<string>(type: "longtext", nullable: false),
                    StudyDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studies", x => x.StudyInstanceUID);
                    table.ForeignKey(
                        name: "FK_Studies_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesInstanceUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    Modality = table.Column<string>(type: "longtext", nullable: false),
                    BodyPart = table.Column<string>(type: "longtext", nullable: false),
                    StudyInstanceUID = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesInstanceUID);
                    table.ForeignKey(
                        name: "FK_Series_Studies_StudyInstanceUID",
                        column: x => x.StudyInstanceUID,
                        principalTable: "Studies",
                        principalColumn: "StudyInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageInstanceUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    SeriesInstanceUID = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageInstanceUID);
                    table.ForeignKey(
                        name: "FK_Images_Series_SeriesInstanceUID",
                        column: x => x.SeriesInstanceUID,
                        principalTable: "Series",
                        principalColumn: "SeriesInstanceUID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SeriesInstanceUID",
                table: "Images",
                column: "SeriesInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Series_StudyInstanceUID",
                table: "Series",
                column: "StudyInstanceUID");

            migrationBuilder.CreateIndex(
                name: "IX_Studies_PatientId",
                table: "Studies",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Studies");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
