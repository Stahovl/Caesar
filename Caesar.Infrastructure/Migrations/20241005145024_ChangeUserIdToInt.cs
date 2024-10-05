using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caesar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$ytj5h0isn52VvvVEpH7TPOBBcn9sLtcY.fv3l1oPyuLYp6PkDKJKK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Kss3Iqso0GxGo3iDfS9/E.PqgAjCIgJJY.tcf5CTFYvs0Q1kYzICG");
        }
    }
}
