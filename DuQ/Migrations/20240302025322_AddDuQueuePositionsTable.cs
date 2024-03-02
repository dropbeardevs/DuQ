using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations
{
    /// <inheritdoc />
    public partial class AddDuQueuePositionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DuQueuePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: false),
                    DuQueueId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DuQueueTypeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuQueuePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuQueuePositions_DuQueueTypes_DuQueueTypeId",
                        column: x => x.DuQueueTypeId,
                        principalTable: "DuQueueTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DuQueuePositions_DuQueues_DuQueueId",
                        column: x => x.DuQueueId,
                        principalTable: "DuQueues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuQueuePositions_DuQueueId",
                table: "DuQueuePositions",
                column: "DuQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_DuQueuePositions_DuQueueTypeId",
                table: "DuQueuePositions",
                column: "DuQueueTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuQueuePositions");
        }
    }
}
