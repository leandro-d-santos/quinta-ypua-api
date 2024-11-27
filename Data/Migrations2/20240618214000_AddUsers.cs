using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    password = table.Column<string>(type: "VARCHAR(36)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.userName);
                }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "users");
        }
    }
}