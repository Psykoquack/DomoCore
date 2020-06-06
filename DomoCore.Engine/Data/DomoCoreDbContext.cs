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
        public DbSet<Device> Devices { get; set; }

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
            #endregion EnumToStringConversion

            #region TestData

            List<Input> inputs = new List<Input>();
            for (int i = 0; i < 32; i++)
            {
                inputs.Add(new Input
                {
                    Id = i + 1,
                    CurrentState = InputState.Released,
                    PreviousState = InputState.Released,
                    HWValue = 0x00000001 << i,
                    Changed = false,
                    DeviceId = 1
                });
            }
            modelBuilder
                .Entity<Input>()
                .HasData(inputs);

            List<Output> outputs = new List<Output>();
            int counter = 1;
            for (int j = 1; j <= 4; j++)
            {
                uint mask = (uint)(0x00000001 << ((j * 8) - 1));

                for (int i = 0; i < 8; i++)
                {
                    outputs.Add(new Output
                    {
                        Id = counter,
                        DeviceId = 1,
                        State = OutputState.Off,
                        HWValue = (int)(mask >> i),
                        Changed = false
                    });
                    counter++;
                }
            }
            modelBuilder
                .Entity<Output>()
                .HasData(outputs);

            modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                    new SimpleOutput { Id = 1, Name = "Licht 1", State = SimpleOutputState.Off, AutoOff = false, InputId = 1, OutputId = 1 }
                );

            modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                    new SimpleOutput { Id = 2, Name = "Licht 2", State = SimpleOutputState.Off, AutoOff = false, InputId = 2, OutputId = 2 }
                );

             modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                    new SimpleOutput { Id = 3, Name = "Licht 3", State = SimpleOutputState.Off, AutoOff = false, InputId = 3, OutputId = 3 }
                );

            modelBuilder
                .Entity<SimpleOutput>()
                .HasData(
                     new SimpleOutput { Id = 4, Name = "Licht 4", State = SimpleOutputState.Off, AutoOff = false, InputId = 4, OutputId = 4 }
                );

            modelBuilder
                .Entity<Device>()
                .HasData(
                    new Device { Id = 1, Address = "192.168.0.236", Name = "Domo1" }
                );
            #endregion TestData
        }

    }
}
