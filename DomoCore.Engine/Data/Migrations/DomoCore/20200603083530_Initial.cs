using Microsoft.EntityFrameworkCore.Migrations;

namespace DomoCore.Engine.Data.Migrations.DomoCore
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HWValue = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outputs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    HWValue = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.Id);
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
                table: "Inputs",
                columns: new[] { "Id", "HWValue", "State" },
                values: new object[] { 1, 1, "Released" });

            migrationBuilder.InsertData(
                table: "Inputs",
                columns: new[] { "Id", "HWValue", "State" },
                values: new object[] { 2, 2, "Released" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "HWValue", "Name", "State" },
                values: new object[] { 1, 1, "Licht 1", "Off" });

            migrationBuilder.InsertData(
                table: "Outputs",
                columns: new[] { "Id", "HWValue", "Name", "State" },
                values: new object[] { 2, 2, "Licht 2", "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 1, false, 0, 2, "Licht 1", 1, "Off" });

            migrationBuilder.InsertData(
                table: "SimpleOutputs",
                columns: new[] { "Id", "AutoOff", "AutoOffTimeSecs", "InputId", "Name", "OutputId", "State" },
                values: new object[] { 2, false, 0, 1, "Licht 2", 2, "Off" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerOutputs_InputId",
                table: "FollowerOutputs",
                column: "InputId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerOutputs_OutputId",
                table: "FollowerOutputs",
                column: "OutputId");

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
        }
    }
}
