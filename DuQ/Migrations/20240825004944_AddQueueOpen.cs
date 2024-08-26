using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuQ.Migrations.Duq
{
    /// <inheritdoc />
    public partial class AddQueueOpen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_open",
                schema: "duqueue",
                table: "du_queue_wait_times",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_open",
                schema: "duqueue",
                table: "du_queue_wait_times");
        }
    }
}
