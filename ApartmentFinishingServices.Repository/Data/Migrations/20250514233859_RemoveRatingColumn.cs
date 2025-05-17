using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentFinishingServices.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRatingColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Workers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
