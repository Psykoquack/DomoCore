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
    public class DomoCoreDbContext : DbContext
    {
        public DbSet<Input> Inputs { get; set; }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<FollowerOutput> FollowerOutputs { get; set; }
        public DbSet<SensorControlledOutput> SensorControlledOutputs { get; set; }
        public DbSet<ShutterOutput> ShutterOutputs { get; set; }
        public DbSet<SimpleOutput> SimpleOutputs { get; set; }
        public DbSet<SwitchTime> SwitchTimes { get; set; }

        public DomoCoreDbContext()
        {

        }

        public DomoCoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = domocore.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region EnumToStringConversion
            modelBuilder
                .Entity<Input>()
                .Property(e => e.State)
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
            #endregion EnumToStringConversion

            #region TestData
            modelBuilder
                .Entity<Input>()
                .HasData(
                    new Input { Id = 1, State = InputState.Released, HWValue = 0x00000001},
                    new Input { Id = 2, State = InputState.Released, HWValue = 0x00000002}
                );

            modelBuilder
                .Entity<Output>()
                .HasData(
                    new Output { Id = 1, Name = "Licht 1", State = OutputState.Off, HWValue = 0x00000001 },
                    new Output { Id = 2, Name = "Licht 2", State = OutputState.Off, HWValue = 0x00000002 }
                    );

            modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                    new SimpleOutput { Id = 1, Name = "Licht 1", State = SimpleOutputState.Off, AutoOff = false, InputId = 2, OutputId = 1 }
                );

            modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                    new SimpleOutput { Id = 2, Name = "Licht 2", State = SimpleOutputState.Off, AutoOff = false, InputId = 1, OutputId = 2 }
                );
            #endregion TestData
        }

    }
}
