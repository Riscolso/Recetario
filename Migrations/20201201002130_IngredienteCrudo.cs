using Microsoft.EntityFrameworkCore.Migrations;

namespace Recetario.Migrations
{
    public partial class IngredienteCrudo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IngredienteCrudo",
                table: "lleva",
                type: "varchar(45)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredienteCrudo",
                table: "lleva");
        }
    }
}
