using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentFinishingServices.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedRequests",
                table: "Workers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedRequests",
                table: "Workers");
        }
    }
}
