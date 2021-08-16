using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediPlus.Migrations
{
    public partial class CapacidadPacientesCedula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESPECIALIDADES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESPECIALIDADES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Clave = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: true),
                    Confirmado = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK__USUARIOS__RolId__3B75D760",
                        column: x => x.RolId,
                        principalTable: "ROLES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CODIGOSVALIDACION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Enviado = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CODIGOSVALIDACION", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CODIGOSVA__Usuar__403A8C7D",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MEDICOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Apellidos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Especialidad = table.Column<int>(type: "int", nullable: true),
                    Oficina = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    CapacidadPacientes = table.Column<int>(type: "int", nullable: false),
                    CedulaIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK__MEDICOS__Especia__45F365D3",
                        column: x => x.Especialidad,
                        principalTable: "ESPECIALIDADES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MEDICOS__IdUsuar__46E78A0C",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PACIENTES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Apellidos = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Cedula = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: true),
                    Lugar_de_nacimiento = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    Sexo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACIENTES", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PACIENTES__IdUsu__4316F928",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SECRETARIA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Apellidos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Oficina = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdDoctor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECRETARIA", x => x.ID);
                    table.ForeignKey(
                        name: "FK__SECRETARI__IdDoc__4E88ABD4",
                        column: x => x.IdDoctor,
                        principalTable: "MEDICOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__SECRETARI__IdUsu__4D94879B",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CITAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPacientes = table.Column<int>(type: "int", nullable: true),
                    IdMedico = table.Column<int>(type: "int", nullable: true),
                    Motivo_de_consulta = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FechaCita = table.Column<DateTime>(type: "date", nullable: true),
                    Comentario = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITAS", x => x.Id);
                    table.ForeignKey(
                        name: "fk_gestion_pacientes",
                        column: x => x.IdPacientes,
                        principalTable: "PACIENTES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_medico_consulta",
                        column: x => x.IdMedico,
                        principalTable: "MEDICOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CITAS_IdMedico",
                table: "CITAS",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_CITAS_IdPacientes",
                table: "CITAS",
                column: "IdPacientes");

            migrationBuilder.CreateIndex(
                name: "IX_CODIGOSVALIDACION_UsuarioId",
                table: "CODIGOSVALIDACION",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "UQ__CODIGOSV__06370DAC4DADD383",
                table: "CODIGOSVALIDACION",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MEDICOS_Especialidad",
                table: "MEDICOS",
                column: "Especialidad");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICOS_IdUsuario",
                table: "MEDICOS",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PACIENTES_IdUsuario",
                table: "PACIENTES",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_SECRETARIA_IdDoctor",
                table: "SECRETARIA",
                column: "IdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_SECRETARIA_IdUsuario",
                table: "SECRETARIA",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_RolId",
                table: "USUARIOS",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "UQ__USUARIOS__A9D10534E477E492",
                table: "USUARIOS",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CITAS");

            migrationBuilder.DropTable(
                name: "CODIGOSVALIDACION");

            migrationBuilder.DropTable(
                name: "SECRETARIA");

            migrationBuilder.DropTable(
                name: "PACIENTES");

            migrationBuilder.DropTable(
                name: "MEDICOS");

            migrationBuilder.DropTable(
                name: "ESPECIALIDADES");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropTable(
                name: "ROLES");
        }
    }
}
