using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations
{
    /// <inheritdoc />
    public partial class OopsGuid2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Current",
                table: "DuQueueTypes");

            migrationBuilder.DropColumn(
                name: "Next",
                table: "DuQueueTypes");

            migrationBuilder.DropColumn(
                name: "Previous",
                table: "DuQueueTypes");

            migrationBuilder.CreateTable(
                name: "DuQueuePosition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PreviousId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CurrentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    NextId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuQueuePosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuQueuePosition_DuQueues_CurrentId",
                        column: x => x.CurrentId,
                        principalTable: "DuQueues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DuQueuePosition_DuQueues_NextId",
                        column: x => x.NextId,
                        principalTable: "DuQueues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DuQueuePosition_DuQueues_PreviousId",
                        column: x => x.PreviousId,
                        principalTable: "DuQueues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuQueuePosition_CurrentId",
                table: "DuQueuePosition",
                column: "CurrentId");

            migrationBuilder.CreateIndex(
                name: "IX_DuQueuePosition_NextId",
                table: "DuQueuePosition",
                column: "NextId");

            migrationBuilder.CreateIndex(
                name: "IX_DuQueuePosition_PreviousId",
                table: "DuQueuePosition",
                column: "PreviousId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuQueuePosition");

            migrationBuilder.AddColumn<Guid>(
                name: "Current",
                table: "DuQueueTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Next",
                table: "DuQueueTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Previous",
                table: "DuQueueTypes",
                type: "TEXT",
                nullable: true);
        }
    }
}
