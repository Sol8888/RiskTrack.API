using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using RiskTrack.API.RiskTrack.Domain.Entities;

namespace RiskTrack.Infrastructure.Data
{
    public class RiskTrackDbContext : DbContext
    {
        public RiskTrackDbContext(DbContextOptions<RiskTrackDbContext> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<RiskAnalysis> RiskAnalyses { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<ControlMeasure> ControlMeasures { get; set; }
        public DbSet<AssetControl> AssetControls { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabla: Activos
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Activos");

                entity.HasKey(e => e.AssetId);
                entity.Property(e => e.AssetId).HasColumnName("id_activo");
                entity.Property(e => e.Name).HasColumnName("nombre_activo");
                entity.Property(e => e.AssetTypeId).HasColumnName("id_tipo_activo");
                entity.Property(e => e.CompanyId).HasColumnName("id_empresa");
                entity.Property(e => e.OwnerTeamId).HasColumnName("id_equipo_proprietario");
                entity.Property(e => e.ContainsPII).HasColumnName("contiene_pii");
                entity.Property(e => e.DataSource).HasColumnName("fuente_datos");
                entity.Property(e => e.RevenuePerMinuteUsd).HasColumnName("ingresos_por_minuto_usd");
                entity.Property(e => e.CriticalFlowPercentage).HasColumnName("porcentaje_flujos_criticos");
                entity.Property(e => e.TotalPiiRecords).HasColumnName("registros_pii_totales");
                entity.Property(e => e.AnnualLicenseCostUsd).HasColumnName("coste_licencia_anual_usd");
                entity.Property(e => e.AnnualSupportHours).HasColumnName("horas_soporte_anual");
                entity.Property(e => e.EngineerHourlyRateUsd).HasColumnName("tarifa_ingeniero_hora_usd");
                entity.Property(e => e.MonthlyDowntimeMin).HasColumnName("downtime_mensual_min");
                entity.Property(e => e.AnnualCriticalVulnerabilities).HasColumnName("vulnerabilidades_criticas_ano");
                entity.Property(e => e.DataCorruptionErrors).HasColumnName("errores_corrupcion_datos");
                entity.Property(e => e.CreatedAt).HasColumnName("fecha_creacion");
                entity.Property(e => e.UpdatedAt).HasColumnName("fecha_ultima_actualizacion");
            });

            // Tabla: Analisis_Riesgo
            modelBuilder.Entity<RiskAnalysis>(entity =>
            {
                entity.ToTable("Analisis_riesgo");
                entity.HasKey(e => e.RiskAnalysisId);
                entity.Property(e => e.RiskAnalysisId).HasColumnName("id_analysis");
                entity.Property(e => e.AssetId).HasColumnName("id_activo");
                entity.Property(e => e.AnalysisDate).HasColumnName("fecha_analisis");
                entity.Property(e => e.VnaValue).HasColumnName("VNA_calculado");
                entity.Property(e => e.SiValue).HasColumnName("SI_calculado");
                entity.Property(e => e.LefValue).HasColumnName("LEF_calculado");
                entity.Property(e => e.LmValue).HasColumnName("LM_calculado");
                entity.Property(e => e.AleValue).HasColumnName("ALE_calculado");
                entity.Property(e => e.AnalystNotes).HasColumnName("Notas_analista");
            });

            // Tabla: Incidentes
            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("Incidentes");
                entity.HasKey(e => e.IncidentId);
                entity.Property(e => e.IncidentId).HasColumnName("id_incidente");
                entity.Property(e => e.AssetId).HasColumnName("id_activo");
                entity.Property(e => e.IncidentDate).HasColumnName("fecha_incidente");
                entity.Property(e => e.Description).HasColumnName("descripcion_incidente");
                entity.Property(e => e.ResolutionDurationHours).HasColumnName("duracion_resolucion_horas");
                entity.Property(e => e.ImpactDurationMinutes).HasColumnName("duracion_impacto_min");
            });

            // Tabla: Controles
            modelBuilder.Entity<ControlMeasure>(entity =>
            {
                entity.ToTable("Controles");

                entity.HasKey(e => e.ControlMeasureId);

                entity.Property(e => e.ControlMeasureId).HasColumnName("id_control");
                entity.Property(e => e.Name).HasColumnName("nombre_control");
                entity.Property(e => e.Description).HasColumnName("descripcion_control");
                entity.Property(e => e.AnnualCostUsd).HasColumnName("costo_anual_usd");
                entity.Property(e => e.ImplementationHours).HasColumnName("horas_implementacion");
                entity.Property(e => e.EstimatedFrequencyReduction).HasColumnName("reduccion_frecuencia_est");
                entity.Property(e => e.EstimatedMagnitudeReduction).HasColumnName("reduccion_magnitud_est");
            });

            // Tabla: Activos_controles
            modelBuilder.Entity<AssetControl>(entity =>
            {
                entity.ToTable("Activos_controles");
                entity.HasKey(e => e.AssetControlId);
                entity.Property(e => e.AssetControlId).HasColumnName("id_activo_control");
                entity.Property(e => e.AssetId).HasColumnName("id_activo");
                entity.Property(e => e.ControlMeasureId).HasColumnName("id_control");
                entity.Property(e => e.AppliedDate).HasColumnName("fecha_aplicacion");
                entity.Property(e => e.Status).HasColumnName("estado");
            });

            // Tabla: Empresas
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Empresas");
                entity.HasKey(e => e.CompanyId);
                entity.Property(e => e.CompanyId).HasColumnName("id_empresa");
                entity.Property(e => e.Name).HasColumnName("nombre_empresa");
                entity.Property(e => e.RUC).HasColumnName("RUC"); 
                entity.Property(e => e.Sector).HasColumnName("sector"); 
            });

            // Tabla: Equipos
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Equipos");
                entity.HasKey(e => e.TeamId);
                entity.Property(e => e.TeamId).HasColumnName("id_equipo_propetario");
                entity.Property(e => e.Name).HasColumnName("nombre_equipo");
                entity.Property(e => e.Leader).HasColumnName("lider_equipo");
                entity.Property(e => e.ContactEmail).HasColumnName("email_contacto");
            });

            // Tabla: Tipos_activo
            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.ToTable("Tipos_activo"); 
                entity.HasKey(e => e.AssetTypeId);
                entity.Property(e => e.AssetTypeId).HasColumnName("id_tipo_activo");
                entity.Property(e => e.Name).HasColumnName("nombre_tipo");
                entity.Property(e => e.Description).HasColumnName("descripcion_tipo"); 
            });

            // Tabla: Usuarios
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("id_usuario");
                entity.Property(e => e.CompanyId).HasColumnName("id_empresa");
                entity.Property(e => e.Username).HasColumnName("nombre_usuario");
                entity.Property(e => e.Email).HasColumnName("correo");
                entity.Property(e => e.Password).HasColumnName("contraseña");
                entity.Property(e => e.Role).HasColumnName("rol");
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}


