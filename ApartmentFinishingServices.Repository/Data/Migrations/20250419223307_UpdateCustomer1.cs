using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentFinishingServices.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Services_ServiceId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Services_ServiceId",
                table: "Requests",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Services_ServiceId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Services_ServiceId",
                table: "Requests",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
