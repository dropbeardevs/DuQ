using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations.Duq
{
    /// <inheritdoc />
    public partial class AddDeptUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dept_url",
                schema: "duqueue",
                table: "du_queue_wait_times",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "du_queue_wait_times",
                columns: new[] { "id", "location", "wait_time", "queue_no_students", "modified_utc", "is_open", "dept_url"},
                values: new object[]
                        { Guid.NewGuid(), "Hornets Resource Center", 0, 0, DateTime.UtcNow, true, "https://fc.xtours.io/#WC-1" }
            );

            migrationBuilder.InsertData(
                table: "du_queue_wait_times",
                columns: new[] { "id", "location", "wait_time", "queue_no_students", "modified_utc", "is_open", "dept_url"},
                values: new object[]
                        { Guid.NewGuid(), "Admissions & Records Campus ID", 0, 0, DateTime.UtcNow, true, "https://fc.xtours.io/#2000" }
            );

            migrationBuilder.InsertData(
                table: "du_queue_wait_times",
                columns: new[] { "id", "location", "wait_time", "queue_no_students", "modified_utc", "is_open", "dept_url"},
                values: new object[]
                        { Guid.NewGuid(), "Admissions & Records Window", 0, 0, DateTime.UtcNow, true, "https://fc.xtours.io/#2000" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dept_url",
                schema: "duqueue",
                table: "du_queue_wait_times");
        }
    }
}
