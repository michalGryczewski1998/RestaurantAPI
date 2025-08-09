using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class PoprawaNazw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContackEmail",
                table: "Restaurants",
                newName: "ContactNumber");

            migrationBuilder.RenameColumn(
                name: "ContacNumber",
                table: "Restaurants",
                newName: "ContactEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactNumber",
                table: "Restaurants",
                newName: "ContackEmail");

            migrationBuilder.RenameColumn(
                name: "ContactEmail",
                table: "Restaurants",
                newName: "ContacNumber");
        }
    }
}
