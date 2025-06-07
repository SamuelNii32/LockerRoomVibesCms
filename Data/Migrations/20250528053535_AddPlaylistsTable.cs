using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LockerRoomVibesCms.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaylistsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlist_Teams_TeamId",
                table: "Playlist");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlist_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Track_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Track",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Playlist",
                table: "Playlist");

            migrationBuilder.RenameTable(
                name: "Track",
                newName: "Tracks");

            migrationBuilder.RenameTable(
                name: "Playlist",
                newName: "Playlists");

            migrationBuilder.RenameIndex(
                name: "IX_Playlist_TeamId",
                table: "Playlists",
                newName: "IX_Playlists_TeamId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Playlists",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playlists",
                table: "Playlists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Teams_TeamId",
                table: "Playlists",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Teams_TeamId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Playlists",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.RenameTable(
                name: "Tracks",
                newName: "Track");

            migrationBuilder.RenameTable(
                name: "Playlists",
                newName: "Playlist");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_TeamId",
                table: "Playlist",
                newName: "IX_Playlist_TeamId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Playlist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Track",
                table: "Track",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playlist",
                table: "Playlist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlist_Teams_TeamId",
                table: "Playlist",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlist_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Track_TrackId",
                table: "PlaylistTracks",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
