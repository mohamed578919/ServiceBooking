using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceBooking.Migrations
{
    /// <inheritdoc />
    public partial class migV03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "ExperienceYears", "HourlyRate", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Fixing leaks, installing pipes, and other plumbing work", 5, 100m, true, "Plumbing" },
                    { 2, "Woodwork, furniture repairs, and custom designs", 7, 120m, true, "Carpentry" },
                    { 4, "Wiring, lighting, and electrical appliance repair", 6, 150m, true, "Electrical" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
