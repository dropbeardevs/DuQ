using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "duqueue");

            migrationBuilder.CreateTable(
                name: "du_queue_locations",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_du_queue_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "du_queue_statuses",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_du_queue_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "du_queue_wait_times",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    wait_time = table.Column<int>(type: "integer", nullable: false),
                    queue_no_students = table.Column<int>(type: "integer", nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_du_queue_wait_times", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_no = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    contact_details = table.Column<string>(type: "text", nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "du_queues",
                schema: "duqueue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    queue_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    queue_location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    checkin_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    checkout_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_du_queues", x => x.id);
                    table.ForeignKey(
                        name: "fk_du_queues_du_queue_locations_queue_location_id",
                        column: x => x.queue_location_id,
                        principalSchema: "duqueue",
                        principalTable: "du_queue_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_du_queues_du_queue_statuses_queue_status_id",
                        column: x => x.queue_status_id,
                        principalSchema: "duqueue",
                        principalTable: "du_queue_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_du_queues_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "duqueue",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_du_queues_queue_location_id",
                schema: "duqueue",
                table: "du_queues",
                column: "queue_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_du_queues_queue_status_id",
                schema: "duqueue",
                table: "du_queues",
                column: "queue_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_du_queues_student_id",
                schema: "duqueue",
                table: "du_queues",
                column: "student_id");


            // du_queue_statuses
            migrationBuilder.InsertData(
                table: "du_queue_statuses",
                columns: new[] { "id", "status", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "In Queue", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_statuses",
                columns: new[] { "id", "status", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Serving", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_statuses",
                columns: new[] { "id", "status", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Finished", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_statuses",
                columns: new[] { "id", "status", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "Deleted", DateTime.UtcNow }
            );

            // du_queue_locations
            migrationBuilder.InsertData(
                table: "du_queue_locations",
                columns: new[] { "id", "location", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "HRC", DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "du_queue_locations",
                columns: new[] { "id", "location", "modified_utc" },
                values: new object[]
                        { Guid.NewGuid(), "A&R", DateTime.UtcNow }
            );

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
            migrationBuilder.DropTable(
                name: "du_queue_wait_times",
                schema: "duqueue");

            migrationBuilder.DropTable(
                name: "du_queues",
                schema: "duqueue");

            migrationBuilder.DropTable(
                name: "du_queue_locations",
                schema: "duqueue");

            migrationBuilder.DropTable(
                name: "du_queue_statuses",
                schema: "duqueue");

            migrationBuilder.DropTable(
                name: "students",
                schema: "duqueue");
        }
    }
}
