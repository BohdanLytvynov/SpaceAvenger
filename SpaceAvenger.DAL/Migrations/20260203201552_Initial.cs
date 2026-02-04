using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceAvenger.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortNameKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescriptionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSaveTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelNameKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    MinExperience = table.Column<int>(type: "int", nullable: false),
                    RankType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranks_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commanders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaleFemale = table.Column<bool>(type: "bit", nullable: false),
                    MissionsCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    Points = table.Column<float>(type: "real", nullable: false),
                    RankId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commanders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commanders_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commanders_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commanders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Factions",
                columns: new[] { "Id", "DescriptionKey", "NameKey", "ShortDescriptionKey", "ShortNameKey" },
                values: new object[] { 1, "UEF_DescKey_DescKey", "UEF_FullNameKey", "UEF_ShortDescKey", "UEF_ShortNameKey" });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "DescriptionKey", "FactionId", "LevelNameKey", "MinExperience", "RankType", "SortOrder" },
                values: new object[,]
                {
                    { 1, "UEF_L1_Rank_Desc", 1, "UEF_L1", 0, "f", 1 },
                    { 2, "UEF_L2_Rank_Desc", 1, "UEF_L2", 1000, "f", 2 },
                    { 3, "UEF_L3_Rank_Desc", 1, "UEF_L3", 2500, "f", 3 },
                    { 4, "UEF_L4_Rank_Desc", 1, "UEF_L4", 5000, "f", 4 },
                    { 5, "UEF_L5_Rank_Desc", 1, "UEF_L5", 9000, "f", 5 },
                    { 6, "UEF_L6_Rank_Desc", 1, "UEF_L6", 15000, "f", 6 },
                    { 7, "UEF_L7_Rank_Desc", 1, "UEF_L7", 25000, "f", 7 },
                    { 8, "UEF_L8_Rank_Desc", 1, "UEF_L8", 45000, "f", 8 },
                    { 9, "UEF_L9_Rank_Desc", 1, "UEF_L9", 75000, "f", 9 },
                    { 10, "UEF_L10_Rank_Desc", 1, "UEF_L10", 120000, "f", 10 },
                    { 11, "UEF_L11_Rank_Desc", 1, "UEF_L11", 200000, "f", 11 },
                    { 12, "UEF_L12_Rank_Desc", 1, "UEF_L12", 350000, "f", 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_FactionId",
                table: "Commanders",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_RankId",
                table: "Commanders",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_UserId",
                table: "Commanders",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_FactionId",
                table: "Ranks",
                column: "FactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commanders");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Factions");
        }
    }
}
