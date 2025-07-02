using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activos",
                columns: table => new
                {
                    id_activo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_activo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_tipo_activo = table.Column<int>(type: "int", nullable: true),
                    id_empresa = table.Column<int>(type: "int", nullable: true),
                    id_equipo_proprietario = table.Column<int>(type: "int", nullable: true),
                    contiene_pii = table.Column<bool>(type: "bit", nullable: true),
                    fuente_datos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ingresos_por_minuto_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    porcentaje_flujos_criticos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    registros_pii_totales = table.Column<long>(type: "bigint", nullable: true),
                    coste_licencia_anual_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    horas_soporte_anual = table.Column<int>(type: "int", nullable: true),
                    tarifa_ingeniero_hora_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    downtime_mensual_min = table.Column<int>(type: "int", nullable: true),
                    vulnerabilidades_criticas_ano = table.Column<int>(type: "int", nullable: true),
                    errores_corrupcion_datos = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_ultima_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos", x => x.id_activo);
                });

            migrationBuilder.CreateTable(
                name: "Activos_controles",
                columns: table => new
                {
                    id_activo_control = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_activo = table.Column<int>(type: "int", nullable: true),
                    id_control = table.Column<int>(type: "int", nullable: true),
                    fecha_aplicacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos_controles", x => x.id_activo_control);
                });

            migrationBuilder.CreateTable(
                name: "Analisis_riesgo",
                columns: table => new
                {
                    id_analysis = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_activo = table.Column<int>(type: "int", nullable: true),
                    fecha_analisis = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VNA_calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SI_calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LEF_calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LM_calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ALE_calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notas_analista = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analisis_riesgo", x => x.id_analysis);
                });

            migrationBuilder.CreateTable(
                name: "Controles",
                columns: table => new
                {
                    id_control = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_control = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion_control = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    costo_anual_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    horas_implementacion = table.Column<int>(type: "int", nullable: true),
                    reduccion_frecuencia_est = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    reduccion_magnitud_est = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controles", x => x.id_control);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    id_empresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_empresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sector = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.id_empresa);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    id_equipo_propetario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_equipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lider_equipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email_contacto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.id_equipo_propetario);
                });

            migrationBuilder.CreateTable(
                name: "Incidentes",
                columns: table => new
                {
                    id_incidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_activo = table.Column<int>(type: "int", nullable: true),
                    fecha_incidente = table.Column<DateTime>(type: "datetime2", nullable: true),
                    descripcion_incidente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duracion_resolucion_horas = table.Column<int>(type: "int", nullable: true),
                    duracion_impacto_min = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidentes", x => x.id_incidente);
                });

            migrationBuilder.CreateTable(
                name: "Tipos_activo",
                columns: table => new
                {
                    id_tipo_activo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion_tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos_activo", x => x.id_tipo_activo);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_empresa = table.Column<int>(type: "int", nullable: true),
                    nombre_usuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contraseña = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activos");

            migrationBuilder.DropTable(
                name: "Activos_controles");

            migrationBuilder.DropTable(
                name: "Analisis_riesgo");

            migrationBuilder.DropTable(
                name: "Controles");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Incidentes");

            migrationBuilder.DropTable(
                name: "Tipos_activo");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
