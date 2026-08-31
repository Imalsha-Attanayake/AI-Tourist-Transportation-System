using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AITouristTransport.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ProviderId",
                table: "Vehicles",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleProviders_ProviderId",
                table: "Vehicles",
                column: "ProviderId",
                principalTable: "VehicleProviders",
                principalColumn: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleProviders_ProviderId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ProviderId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Vehicles");
        }
    }
}
