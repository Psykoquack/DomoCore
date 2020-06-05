using DomoCore.Engine.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.Data
{
    public class DomoCoreInMemDbContext : DbContext
    {
        public DbSet<Input> Inputs { get; set; }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<FollowerOutput> FollowerOutputs { get; set; }
        public DbSet<SensorControlledOutput> SensorControlledOutputs { get; set; }
        public DbSet<ShutterOutput> ShutterOutputs { get; set; }
        public DbSet<SimpleOutput> SimpleOutputs { get; set; }
        public DbSet<SwitchTime> SwitchTimes { get; set; }
        public DbSet<Device> Devices { get; set; }

        public DomoCoreInMemDbContext(DbContextOptions options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Input>()
                .Property(e => e.CurrentState)
                .HasConversion(
                    v => v.ToString(),
                    v => (InputState)Enum.Parse(typeof(InputState), v));

            modelBuilder
                .Entity<Input>()
                .Property(e => e.PreviousState)
                .HasConversion(
                    v => v.ToString(),
                    v => (InputState)Enum.Parse(typeof(InputState), v));

            modelBuilder
                .Entity<Output>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (OutputState)Enum.Parse(typeof(OutputState), v));

            modelBuilder
                .Entity<SensorControlledOutput>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (SensorCtrlOutputState)Enum.Parse(typeof(SensorCtrlOutputState), v));

            modelBuilder
                .Entity<ShutterOutput>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (ShutterOutputState)Enum.Parse(typeof(ShutterOutputState), v));

            modelBuilder
                .Entity<ShutterOutput>()
                .Property(e => e.Direction)
                .HasConversion(
                    v => v.ToString(),
                    v => (ShutterDirection)Enum.Parse(typeof(ShutterDirection), v));

            modelBuilder
                .Entity<SimpleOutput>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (SimpleOutputState)Enum.Parse(typeof(SimpleOutputState), v));


        }

    }
}
