using Microsoft.EntityFrameworkCore.Migrations;

namespace DomoCore.Engine.Data.Migrations.DomoCore
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HWValue = table.Column<int>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false),
                    CurrentState = table.Column<string>(nullable: false),
                    PreviousState = table.Column<string>(nullable: false),
                    Changed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inputs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Outputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HWValue = table.Column<int>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Changed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outputs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowerOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    InputId = table.Column<int>(nullable: false),
                    OutputId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowerOutputs_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowerOutputs_Outputs_OutputId",
                        column: x => x.OutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorControlledOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    InputId = table.Column<int>(nullable: false),
                    OutputId = table.Column<int>(nullable: false),
                    AutoOff = table.Column<bool>(nullable: false),
                    AutoOffTimeSecs = table.Column<int>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorControlledOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorControlledOutputs_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorControlledOutputs_Outputs_OutputId",
                        column: x => x.OutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShutterOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    InputId = table.Column<int>(nullable: false),
                    DirectionOutputId = table.Column<int>(nullable: false),
                    PowerOutputId = table.Column<int>(nullable: false),
                    Direction = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    RunTimeSecs = table.Column<int>(nullable: false),
                    Counter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShutterOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShutterOutputs_Outputs_DirectionOutputId",
                        column: x => x.DirectionOutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShutterOutputs_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShutterOutputs_Outputs_PowerOutputId",
                        column: x => x.PowerOutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimpleOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    InputId = table.Column<int>(nullable: false),
                    OutputId = table.Column<int>(nullable: false),
                    AutoOff = table.Column<bool>(nullable: false),
                    AutoOffTimeSecs = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleOutputs_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimpleOutputs_Outputs_OutputId",
                        column: x => x.OutputId,
                        principalTable: "Outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SwitchTimes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DayOfWeek = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    SwitchOn = table.Column<bool>(nullable: false),
                    Hour = table.Column<int>(nullable: false),
                    Minute = table.Column<int>(nullable: false),
                    Second = table.Column<int>(nullable: false),
                    ShutterOutputId = table.Column<int>(nullable: true),
                    ShutterOutputdId = table.Column<int>(nullable: false),
                    SimpleOutputId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchTimes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SwitchTimes_ShutterOutputs_ShutterOutputId",
                        column: x => x.ShutterOutputId,
                        principalTable: "ShutterOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwitchTimes_SimpleOutputs_SimpleOutputId",
                        column: x => x.SimpleOutputId,
                        principalTable: "SimpleOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[] { 1, "192.168.0.236", "Domo1" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 1, false, "Released", 1, 1, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 31, false, "Released", 1, 1073741824, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 30, false, "Released", 1, 536870912, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 29, false, "Released", 1, 268435456, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 28, false, "Released", 1, 134217728, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 27, false, "Released", 1, 67108864, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 26, false, "Released", 1, 33554432, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 25, false, "Released", 1, 16777216, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 24, false, "Released", 1, 8388608, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 23, false, "Released", 1, 4194304, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 22, false, "Released", 1, 2097152, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 21, false, "Released", 1, 1048576, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 20, false, "Released", 1, 524288, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 19, false, "Released", 1, 262144, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 18, false, "Released", 1, 131072, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 17, false, "Released", 1, 65536, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 32, false, "Released", 1, -2147483648, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 15, false, "Released", 1, 16384, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 16, false, "Released", 1, 32768, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 2, false, "Released", 1, 2, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 3, false, "Released", 1, 4, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 4, false, "Released", 1, 8, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 6, false, "Released", 1, 32, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 7, false, "Released", 1, 64, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 5, false, "Released", 1, 16, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 9, false, "Released", 1, 256, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 10, false, "Released", 1, 512, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 11, false, "Released", 1, 1024, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 12, false, "Released", 1, 2048, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 13, false, "Released", 1, 4096, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 14, false, "Released", 1, 8192, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "Changed", "CurrentState", "DeviceId", "HWValue", "PreviousState" },
                values: new object[] { 8, false, "Released", 1, 128, "Released" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 22, false, 1, 262144, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 18, false, 1, 4194304, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 19, false, 1, 2097152, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 20, false, 1, 1048576, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 21, false, 1, 524288, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 23, false, 1, 131072, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 30, false, 1, 67108864, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 25, false, 1, -2147483648, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 26, false, 1, 1073741824, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 27, false, 1, 536870912, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 28, false, 1, 268435456, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 29, false, 1, 134217728, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 17, false, 1, 8388608, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 24, false, 1, 65536, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 16, false, 1, 256, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 9, false, 1, 32768, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 14, false, 1, 1024, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 13, false, 1, 2048, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 12, false, 1, 4096, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 11, false, 1, 8192, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 10, false, 1, 16384, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 8, false, 1, 1, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 7, false, 1, 2, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 6, false, 1, 4, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 5, false, 1, 8, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 4, false, 1, 16, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 3, false, 1, 32, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 2, false, 1, 64, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 1, false, 1, 128, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 31, false, 1, 33554432, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 15, false, 1, 512, "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "Changed", "DeviceId", "HWValue", "State" },
                values: new object[] { 32, false, 1, 16777216, "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 1, false, 0, 1, "Licht 1", 1, "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 2, false, 0, 2, "Licht 2", 2, "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 3, false, 0, 3, "Licht 3", 3, "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 4, false, 0, 4, "Licht 4", 4, "Off" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerOutputs_InputId",
                table: "FollowerOutputs",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerOutputs_OutputId",
                table: "FollowerOutputs",
                column: "OutputId");

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_DeviceId",
                table: "Inputs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_DeviceId",
                table: "Outputs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorControlledOutputs_InputId",
                table: "SensorControlledOutputs",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorControlledOutputs_OutputId",
                table: "SensorControlledOutputs",
                column: "OutputId");

            migrationBuilder.CreateIndex(
                name: "IX_ShutterOutputs_DirectionOutputId",
                table: "ShutterOutputs",
                column: "DirectionOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_ShutterOutputs_InputId",
                table: "ShutterOutputs",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_ShutterOutputs_PowerOutputId",
                table: "ShutterOutputs",
                column: "PowerOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_SimpleOutputs_InputId",
                table: "SimpleOutputs",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_SimpleOutputs_OutputId",
                table: "SimpleOutputs",
                column: "OutputId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchTimes_ShutterOutputId",
                table: "SwitchTimes",
                column: "ShutterOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchTimes_SimpleOutputId",
                table: "SwitchTimes",
                column: "SimpleOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowerOutputs");

            migrationBuilder.DropTable(
                name: "SensorControlledOutputs");

            migrationBuilder.DropTable(
                name: "SwitchTimes");

            migrationBuilder.DropTable(
                name: "ShutterOutputs");

            migrationBuilder.DropTable(
                name: "SimpleOutputs");

            migrationBuilder.DropTable(
                name: "Inputs");

            migrationBuilder.DropTable(
                name: "Outputs");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
