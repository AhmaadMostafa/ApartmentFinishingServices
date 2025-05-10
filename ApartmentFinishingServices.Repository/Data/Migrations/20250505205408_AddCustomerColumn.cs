using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentFinishingServices.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestsCount",
                table: "Customers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestsCount",
                table: "Customers");
        }
    }
}
