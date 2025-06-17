using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "guests",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    phoneNumber = table.Column<string>(type: "TEXT", maxLength: 22, nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    number = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    floor = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userName);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    checkIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    checkOut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    numberOfAdults = table.Column<int>(type: "INTEGER", nullable: false),
                    numberOfChildren = table.Column<int>(type: "INTEGER", nullable: false),
                    roomId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    guestId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservations_guests_guestId",
                        column: x => x.guestId,
                        principalTable: "guests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_rooms_roomId",
                        column: x => x.roomId,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
               name: "financial",
               columns: table => new
               {
                   id = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                   reservationValue = table.Column<string>(type: "Double", nullable: false),
                   additionalValue = table.Column<string>(type: "Double", nullable: false),
                   payment = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                   status = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                   createdAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                   updatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_financial", x => x.id);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "guests");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "financial");
        }
    }
}
