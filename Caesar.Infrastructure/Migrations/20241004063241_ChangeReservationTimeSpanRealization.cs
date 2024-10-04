using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caesar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReservationTimeSpanRealization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReservationTime",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Kss3Iqso0GxGo3iDfS9/E.PqgAjCIgJJY.tcf5CTFYvs0Q1kYzICG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ReservationTime",
                table: "Reservations",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$nQfxDV5Ns17i5cn6lV/UZeYObmJtd7IptujBYByZPrJiU.cTZM8pK");
        }
    }
}
