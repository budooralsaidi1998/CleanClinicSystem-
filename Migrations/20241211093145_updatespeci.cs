using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanCllinicSystem.Migrations
{
    /// <inheritdoc />
    public partial class updatespeci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_clinics_cid",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clinics",
                table: "clinics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_cid",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "cid",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "spe",
                table: "clinics",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "spec",
                table: "bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clinics",
                table: "clinics",
                column: "spe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                columns: new[] { "pid", "spec", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_spec",
                table: "bookings",
                column: "spec");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_clinics_spec",
                table: "bookings",
                column: "spec",
                principalTable: "clinics",
                principalColumn: "spe",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_clinics_spec",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clinics",
                table: "clinics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_spec",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "spec",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "spe",
                table: "clinics",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "cid",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_clinics",
                table: "clinics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                columns: new[] { "pid", "cid", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_cid",
                table: "bookings",
                column: "cid");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_clinics_cid",
                table: "bookings",
                column: "cid",
                principalTable: "clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
