using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class IdentityAndRolesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68e3b10e-044b-464d-bd53-10c852dcf01c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7da835f1-23c5-49ca-bebf-581d406e3204");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17b4409a-9739-4213-b7a4-c6cde8fba872", null, "Admin", "ADMIN" },
                    { "79f62746-1fc5-460d-9a6f-dc7765900a77", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17b4409a-9739-4213-b7a4-c6cde8fba872");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79f62746-1fc5-460d-9a6f-dc7765900a77");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68e3b10e-044b-464d-bd53-10c852dcf01c", null, "User", "USER" },
                    { "7da835f1-23c5-49ca-bebf-581d406e3204", null, "Admin", "ADMIN" }
                });
        }
    }
}
