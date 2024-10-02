using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caesar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Reservations",
                newName: "ReservationTime");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reservations",
                newName: "ReservationDate");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Reservations",
                newName: "UserId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4Wh0vhcx7jMcnnGrXe2Pw.rG95WpG.p6Qi6Jhx1UBU9.CV.dK6.fG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservations",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "ReservationTime",
                table: "Reservations",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "Reservations",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$nNX.csO.WhzMoshjLl4Ofu9iL560kNeWBOnJxaOMxADMYP0DhiVG.");
        }
    }
}
