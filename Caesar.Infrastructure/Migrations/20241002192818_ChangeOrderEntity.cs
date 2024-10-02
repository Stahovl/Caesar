using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caesar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$nQfxDV5Ns17i5cn6lV/UZeYObmJtd7IptujBYByZPrJiU.cTZM8pK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4Wh0vhcx7jMcnnGrXe2Pw.rG95WpG.p6Qi6Jhx1UBU9.CV.dK6.fG");
        }
    }
}
