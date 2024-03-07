using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations
{
    /// <inheritdoc />
    public partial class DuQueuePositionsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePosition_DuQueues_CurrentId",
                table: "DuQueuePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePosition_DuQueues_NextId",
                table: "DuQueuePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePosition_DuQueues_PreviousId",
                table: "DuQueuePosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DuQueuePosition",
                table: "DuQueuePosition");

            migrationBuilder.RenameTable(
                name: "DuQueuePosition",
                newName: "DuQueuePositions");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePosition_PreviousId",
                table: "DuQueuePositions",
                newName: "IX_DuQueuePositions_PreviousId");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePosition_NextId",
                table: "DuQueuePositions",
                newName: "IX_DuQueuePositions_NextId");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePosition_CurrentId",
                table: "DuQueuePositions",
                newName: "IX_DuQueuePositions_CurrentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DuQueuePositions",
                table: "DuQueuePositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePositions_DuQueues_CurrentId",
                table: "DuQueuePositions",
                column: "CurrentId",
                principalTable: "DuQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePositions_DuQueues_NextId",
                table: "DuQueuePositions",
                column: "NextId",
                principalTable: "DuQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePositions_DuQueues_PreviousId",
                table: "DuQueuePositions",
                column: "PreviousId",
                principalTable: "DuQueues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePositions_DuQueues_CurrentId",
                table: "DuQueuePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePositions_DuQueues_NextId",
                table: "DuQueuePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_DuQueuePositions_DuQueues_PreviousId",
                table: "DuQueuePositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DuQueuePositions",
                table: "DuQueuePositions");

            migrationBuilder.RenameTable(
                name: "DuQueuePositions",
                newName: "DuQueuePosition");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePositions_PreviousId",
                table: "DuQueuePosition",
                newName: "IX_DuQueuePosition_PreviousId");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePositions_NextId",
                table: "DuQueuePosition",
                newName: "IX_DuQueuePosition_NextId");

            migrationBuilder.RenameIndex(
                name: "IX_DuQueuePositions_CurrentId",
                table: "DuQueuePosition",
                newName: "IX_DuQueuePosition_CurrentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DuQueuePosition",
                table: "DuQueuePosition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePosition_DuQueues_CurrentId",
                table: "DuQueuePosition",
                column: "CurrentId",
                principalTable: "DuQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePosition_DuQueues_NextId",
                table: "DuQueuePosition",
                column: "NextId",
                principalTable: "DuQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DuQueuePosition_DuQueues_PreviousId",
                table: "DuQueuePosition",
                column: "PreviousId",
                principalTable: "DuQueues",
                principalColumn: "Id");
        }
    }
}
