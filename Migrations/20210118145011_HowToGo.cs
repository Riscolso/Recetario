using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recetario.Migrations
{
    public partial class HowToGo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "actor",
                columns: table => new
                {
                    idActor = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    NombreActor = table.Column<string>(type: "varchar(55)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    FechaNac = table.Column<DateTime>(type: "date", nullable: false),
                    Tipo = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idActor);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "etiqueta",
                columns: table => new
                {
                    idEtiqueta = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Etiqueta = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEtiqueta);
                });

            migrationBuilder.CreateTable(
                name: "ingrediente",
                columns: table => new
                {
                    idIngrediente = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idIngrediente);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_actor_UserId",
                        column: x => x.UserId,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_actor_UserId",
                        column: x => x.UserId,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_actor_UserId",
                        column: x => x.UserId,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "receta",
                columns: table => new
                {
                    idReceta = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(45)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    ProcentajePromedio = table.Column<int>(type: "int(11)", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    TiempoPrep = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.idReceta, x.Actor_idActor });
                    table.ForeignKey(
                        name: "fk_Receta_Actor1",
                        column: x => x.Actor_idActor,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_actor_UserId",
                        column: x => x.UserId,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lleva",
                columns: table => new
                {
                    idLleva = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Receta_idReceta = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    Ingrediente_idIngrediente = table.Column<int>(type: "int(11)", nullable: false),
                    IngredienteCrudo = table.Column<string>(type: "varchar(70)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idLleva);
                    table.ForeignKey(
                        name: "fk_Receta_has_Ingrediente_Ingrediente1",
                        column: x => x.Ingrediente_idIngrediente,
                        principalTable: "ingrediente",
                        principalColumn: "idIngrediente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Receta_has_Ingrediente_Receta1",
                        columns: x => new { x.Receta_idReceta, x.Receta_Actor_idActor },
                        principalTable: "receta",
                        principalColumns: new[] { "idReceta", "Actor_idActor" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paso",
                columns: table => new
                {
                    NoPaso = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_idReceta = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    Texto = table.Column<string>(type: "varchar(600)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    TiempoTemporizador = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.NoPaso, x.Receta_idReceta, x.Receta_Actor_idActor });
                    table.ForeignKey(
                        name: "fk_Paso_Receta1",
                        columns: x => new { x.Receta_idReceta, x.Receta_Actor_idActor },
                        principalTable: "receta",
                        principalColumns: new[] { "idReceta", "Actor_idActor" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usa",
                columns: table => new
                {
                    Receta_idReceta = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    Etiqueta_idEtiqueta = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Receta_idReceta, x.Receta_Actor_idActor, x.Etiqueta_idEtiqueta });
                    table.ForeignKey(
                        name: "fk_Receta_has_Etiqueta_Etiqueta1",
                        column: x => x.Etiqueta_idEtiqueta,
                        principalTable: "etiqueta",
                        principalColumn: "idEtiqueta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Receta_has_Etiqueta_Receta1",
                        columns: x => new { x.Receta_idReceta, x.Receta_Actor_idActor },
                        principalTable: "receta",
                        principalColumns: new[] { "idReceta", "Actor_idActor" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visualizacion",
                columns: table => new
                {
                    Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_idReceta = table.Column<int>(type: "int(11)", nullable: false),
                    Receta_Actor_idActor = table.Column<int>(type: "int(11)", nullable: false),
                    ProcentajeCompl = table.Column<int>(type: "int(11)", nullable: true),
                    Calificacion = table.Column<bool>(nullable: true),
                    PorCocinar = table.Column<sbyte>(type: "tinyint(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Actor_idActor, x.Receta_idReceta, x.Receta_Actor_idActor });
                    table.ForeignKey(
                        name: "fk_Actor_has_Receta_Actor1",
                        column: x => x.Actor_idActor,
                        principalTable: "actor",
                        principalColumn: "idActor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Actor_has_Receta_Receta1",
                        columns: x => new { x.Receta_idReceta, x.Receta_Actor_idActor },
                        principalTable: "receta",
                        principalColumns: new[] { "idReceta", "Actor_idActor" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "actor",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "actor",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "fk_Receta_has_Ingrediente_Ingrediente1_idx",
                table: "lleva",
                column: "Ingrediente_idIngrediente");

            migrationBuilder.CreateIndex(
                name: "fk_Receta_has_Ingrediente_Receta1_idx",
                table: "lleva",
                columns: new[] { "Receta_idReceta", "Receta_Actor_idActor" });

            migrationBuilder.CreateIndex(
                name: "fk_Paso_Receta1_idx",
                table: "paso",
                columns: new[] { "Receta_idReceta", "Receta_Actor_idActor" });

            migrationBuilder.CreateIndex(
                name: "fk_Receta_Actor1_idx",
                table: "receta",
                column: "Actor_idActor");

            migrationBuilder.CreateIndex(
                name: "fk_Receta_has_Etiqueta_Etiqueta1_idx",
                table: "usa",
                column: "Etiqueta_idEtiqueta");

            migrationBuilder.CreateIndex(
                name: "fk_Receta_has_Etiqueta_Receta1_idx",
                table: "usa",
                columns: new[] { "Receta_idReceta", "Receta_Actor_idActor" });

            migrationBuilder.CreateIndex(
                name: "fk_Actor_has_Receta_Actor1_idx",
                table: "visualizacion",
                column: "Actor_idActor");

            migrationBuilder.CreateIndex(
                name: "fk_Actor_has_Receta_Receta1_idx",
                table: "visualizacion",
                columns: new[] { "Receta_idReceta", "Receta_Actor_idActor" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "lleva");

            migrationBuilder.DropTable(
                name: "paso");

            migrationBuilder.DropTable(
                name: "usa");

            migrationBuilder.DropTable(
                name: "visualizacion");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ingrediente");

            migrationBuilder.DropTable(
                name: "etiqueta");

            migrationBuilder.DropTable(
                name: "receta");

            migrationBuilder.DropTable(
                name: "actor");
        }
    }
}
