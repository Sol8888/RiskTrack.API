using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using RiskTrack.API.Domain.Entities;

namespace RiskTrack.Infrastructure.Data
{
    public class RiskTrackDbContext : DbContext
    {
        public RiskTrackDbContext(DbContextOptions<RiskTrackDbContext> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<RiskAnalysis> RiskAnalysis { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<ControlMeasure> ControlMeasures { get; set; }
        public DbSet<AssetControl> AssetControls { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}


