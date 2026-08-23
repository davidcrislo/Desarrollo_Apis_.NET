using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCuentas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cuentas",
                columns: table => new
                {
                    id_cuenta = table.Column<string>(type: "text", nullable: false),
                    numero_cuenta = table.Column<string>(type: "text", nullable: false),
                    titular = table.Column<string>(type: "text", nullable: false),
                    tipo_cuenta = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    saldo = table.Column<decimal>(type: "numeric", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cuentas", x => x.id_cuenta);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cuentas");
        }
    }
}
