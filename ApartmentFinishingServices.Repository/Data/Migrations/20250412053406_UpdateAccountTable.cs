using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentFinishingServices.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDays_Workers_WorkerId",
                table: "AvailableDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AppUser_AppUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Workers_WorkerId",
                table: "PortfolioItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Workers_WorkerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedWorkers_Customers_CustomerId",
                table: "SavedWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedWorkers_Workers_ProviderId",
                table: "SavedWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AppUser_AppUserId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Services_ServiceId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workers",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Workers",
                newName: "Worker");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_ServiceId",
                table: "Worker",
                newName: "IX_Worker_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_AppUserId",
                table: "Worker",
                newName: "IX_Worker_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_AppUserId",
                table: "Customer",
                newName: "IX_Customer_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Worker",
                table: "Worker",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDays_Worker_WorkerId",
                table: "AvailableDays",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AppUser_AppUserId",
                table: "Customer",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Worker_WorkerId",
                table: "PortfolioItems",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customer_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Worker_WorkerId",
                table: "Requests",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customer_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Worker_WorkerId",
                table: "Reviews",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedWorkers_Customer_CustomerId",
                table: "SavedWorkers",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedWorkers_Worker_ProviderId",
                table: "SavedWorkers",
                column: "ProviderId",
                principalTable: "Worker",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_AppUser_AppUserId",
                table: "Worker",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Services_ServiceId",
                table: "Worker",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDays_Worker_WorkerId",
                table: "AvailableDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AppUser_AppUserId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Worker_WorkerId",
                table: "PortfolioItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customer_CustomerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Worker_WorkerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customer_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Worker_WorkerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedWorkers_Customer_CustomerId",
                table: "SavedWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedWorkers_Worker_ProviderId",
                table: "SavedWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_AppUser_AppUserId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Services_ServiceId",
                table: "Worker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Worker",
                table: "Worker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Worker",
                newName: "Workers");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Worker_ServiceId",
                table: "Workers",
                newName: "IX_Workers_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Worker_AppUserId",
                table: "Workers",
                newName: "IX_Workers_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_AppUserId",
                table: "Customers",
                newName: "IX_Customers_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workers",
                table: "Workers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDays_Workers_WorkerId",
                table: "AvailableDays",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AppUser_AppUserId",
                table: "Customers",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Workers_WorkerId",
                table: "PortfolioItems",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Workers_WorkerId",
                table: "Requests",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Workers_WorkerId",
                table: "Reviews",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedWorkers_Customers_CustomerId",
                table: "SavedWorkers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedWorkers_Workers_ProviderId",
                table: "SavedWorkers",
                column: "ProviderId",
                principalTable: "Workers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AppUser_AppUserId",
                table: "Workers",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Services_ServiceId",
                table: "Workers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
