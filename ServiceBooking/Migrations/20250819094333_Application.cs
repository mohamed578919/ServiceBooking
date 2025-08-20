using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceBooking.Migrations
{
    /// <inheritdoc />
    public partial class Application : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_ProviderId",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_ProviderId",
                table: "Applications",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_ProviderId",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_ProviderId",
                table: "Applications",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
