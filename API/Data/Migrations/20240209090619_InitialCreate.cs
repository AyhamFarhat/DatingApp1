// it is generated file and should not be modified, if you need to modify it, please create a new migration file.
// our database is sqlite, so we are using sqlite specific code here.
// our database will have a table named Users with two columns Id and UserName
// our database will created in the file named data.db in the root of the project
// this file is used to create the database schema, to apply the schema to the database,
// run the following command: dotnet ef database update.
// and to revert the schema, run the following command: dotnet ef database update 0. and 
// and run the following command: dotnet ef database drop. to drop the database.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
