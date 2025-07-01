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
            modelBuilder.Entity<Asset>().HasKey(a => a.AssetId);
            modelBuilder.Entity<RiskAnalysis>().HasKey(r => r.RiskAnalysisId);
            modelBuilder.Entity<Incident>().HasKey(i => i.IncidentId);
            modelBuilder.Entity<ControlMeasure>().HasKey(c => c.ControlMeasureId);
            modelBuilder.Entity<AssetControl>().HasKey(ac => ac.AssetControlId);
            modelBuilder.Entity<Company>().HasKey(c => c.CompanyId);
            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
            modelBuilder.Entity<AssetType>().HasKey(t => t.AssetTypeId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}


