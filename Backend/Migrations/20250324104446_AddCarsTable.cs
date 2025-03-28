using Microsoft.EntityFrameworkCore.Migrations;

namespace Parkingapp.Migrations // Ensure this matches your project's migration namespace
{
    public partial class AddCarsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the invalid RenameColumn since PasswordHash already exists
            // migrationBuilder.RenameColumn(
            //     name: "Password",
            //     table: "Users",
            //     newName: "PasswordHash");

            // Create the Cars table
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<decimal>(type: "REAL", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Cars table
            migrationBuilder.DropTable(name: "Cars");

            // Remove the reverse rename since it’s not needed
            // migrationBuilder.RenameColumn(
            //     name: "PasswordHash",
            //     table: "Users",
            //     newName: "Password");
        }
    }
}