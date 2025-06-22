using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LockerRoomVibesCms.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAudioUrlToTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "Tracks");
        }
    }
}
