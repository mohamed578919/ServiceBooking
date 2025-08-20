using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceBooking.Migrations
{
    /// <inheritdoc />
    public partial class service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Complaints");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "Services",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Services",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId1",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Complaints",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgainstUserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Complaints",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Complaints",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FiledByUserId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RelatedRequestId",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedServiceId",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(180)",
                maxLength: 180,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProviderId1",
                table: "Services",
                column: "ProviderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_AgainstUserId",
                table: "Complaints",
                column: "AgainstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_FiledByUserId",
                table: "Complaints",
                column: "FiledByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_AgainstUserId",
                table: "Complaints",
                column: "AgainstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_FiledByUserId",
                table: "Complaints",
                column: "FiledByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId1",
                table: "Services",
                column: "ProviderId1",
                principalTable: "Providers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_FiledByUserId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_ProviderId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId1",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProviderId1",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_FiledByUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProviderId1",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AgainstUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "FiledByUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "RelatedRequestId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "RelatedServiceId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Complaints");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");
        }
    }
}
