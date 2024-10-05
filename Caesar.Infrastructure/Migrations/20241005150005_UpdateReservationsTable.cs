using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caesar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$8QLY4bKo.ta6b3zDDxQi2.mhhkDbEJKtHAYBNzhZgeneI901ngshS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$ytj5h0isn52VvvVEpH7TPOBBcn9sLtcY.fv3l1oPyuLYp6PkDKJKK");
        }
    }
}
