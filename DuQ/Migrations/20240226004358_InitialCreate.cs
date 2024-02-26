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
            migrationBuilder.CreateTable(
                name: "DuQueueStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuQueueStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuQueueTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuQueueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentNo = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QueueTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QueueStatusId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheckinTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CheckoutTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuQueues_DuQueueStatuses_QueueStatusId",
                        column: x => x.QueueStatusId,
                        principalTable: "DuQueueStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuQueues_DuQueueTypes_QueueTypeId",
                        column: x => x.QueueTypeId,
                        principalTable: "DuQueueTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuQueues_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuQueues_QueueStatusId",
                table: "DuQueues",
                column: "QueueStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DuQueues_QueueTypeId",
                table: "DuQueues",
                column: "QueueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DuQueues_StudentId",
                table: "DuQueues",
                column: "StudentId");

            migrationBuilder.InsertData(
                table: "DuQueueTypes",
                columns: new[] { "Id", "Name", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "Campus ID Card", DateTime.Now }
            );

            migrationBuilder.InsertData(
                table: "DuQueueTypes",
                columns: new[] { "Id", "Name", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "Cap and Gown", DateTime.Now }
            );

            migrationBuilder.InsertData(
                table: "DuQueueTypes",
                columns: new[] { "Id", "Name", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "Other", DateTime.Now }
            );

            migrationBuilder.InsertData(
                table: "DuQueueStatuses",
                columns: new[] { "Id", "Status", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "In Queue", DateTime.Now }
            );

            migrationBuilder.InsertData(
                table: "DuQueueStatuses",
                columns: new[] { "Id", "Status", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "Serving", DateTime.Now }
            );

            migrationBuilder.InsertData(
                table: "DuQueueStatuses",
                columns: new[] { "Id", "Status", "LastUpdated" },
                values: new object[]
                        { Guid.NewGuid(), "Finished", DateTime.Now }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuQueues");

            migrationBuilder.DropTable(
                name: "DuQueueStatuses");

            migrationBuilder.DropTable(
                name: "DuQueueTypes");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
