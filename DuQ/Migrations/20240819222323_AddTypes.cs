using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations.Duq
{
    /// <inheritdoc />
    public partial class AddTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "queue_type_id",
                schema: "duqueue",
                table: "du_queues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "du_queue_types",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_du_queue_types", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_du_queues_queue_type_id",
                schema: "duqueue",
                table: "du_queues",
                column: "queue_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_du_queues_du_queue_types_queue_type_id",
                schema: "duqueue",
                table: "du_queues",
                column: "queue_type_id",
                principalSchema: "duqueue",
                principalTable: "du_queue_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);


            // du_queue_types
            migrationBuilder.InsertData(
                table: "du_queue_types",
                columns: new[] { "id", "type", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Campus ID Card", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_types",
                columns: new[] { "id", "type", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Cap and Gown", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_types",
                columns: new[] { "id", "type", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Other", DateTime.UtcNow }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_du_queues_du_queue_types_queue_type_id",
                schema: "duqueue",
                table: "du_queues");

            migrationBuilder.DropTable(
                name: "du_queue_types",
                schema: "duqueue");

            migrationBuilder.DropIndex(
                name: "ix_du_queues_queue_type_id",
                schema: "duqueue",
                table: "du_queues");

            migrationBuilder.DropColumn(
                name: "queue_type_id",
                schema: "duqueue",
                table: "du_queues");
        }
    }
}
