using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceAvenger.DAL.Migrations
{
    /// <inheritdoc />
    public partial class BaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifierValue = table.Column<float>(type: "REAL", precision: 18, scale: 4, nullable: false),
                    Duration = table.Column<float>(type: "REAL", nullable: false),
                    IsPercentage = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPositive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(type: "TEXT", nullable: false),
                    ResourceKey = table.Column<string>(type: "TEXT", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionCode = table.Column<string>(type: "TEXT", nullable: false),
                    ShortNameKey = table.Column<string>(type: "TEXT", nullable: false),
                    ShortDescriptionKey = table.Column<string>(type: "TEXT", nullable: false),
                    ShipPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    IsAlive = table.Column<bool>(type: "INTEGER", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileName = table.Column<string>(type: "TEXT", nullable: false),
                    LastSaveTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BonusParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParameterName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    BonusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusParameters_Bonuses_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Bonuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionBonuses",
                columns: table => new
                {
                    BonusId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionBonuses", x => new { x.FactionId, x.BonusId });
                    table.ForeignKey(
                        name: "FK_FactionBonuses_Bonuses_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Bonuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionBonuses_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionCurrencies",
                columns: table => new
                {
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExchangeRateModifier = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionCurrencies", x => new { x.CurrencyId, x.FactionId });
                    table.ForeignKey(
                        name: "FK_FactionCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionCurrencies_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Population = table.Column<float>(type: "REAL", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LevelNameKey = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    MinExperience = table.Column<int>(type: "INTEGER", nullable: false),
                    RankType = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranks_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SubFactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubFactionCode = table.Column<string>(type: "TEXT", nullable: false),
                    ShortNameKey = table.Column<string>(type: "TEXT", nullable: false),
                    ShortDescriptionKey = table.Column<string>(type: "TEXT", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFactions_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionHomeWorlds",
                columns: table => new
                {
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    HomePlanetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionHomeWorlds", x => new { x.FactionId, x.HomePlanetId });
                    table.ForeignKey(
                        name: "FK_FactionHomeWorlds_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionHomeWorlds_Planets_HomePlanetId",
                        column: x => x.HomePlanetId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Armors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubFactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    KineticResist = table.Column<float>(type: "REAL", nullable: false),
                    EnergyResist = table.Column<float>(type: "REAL", nullable: false),
                    ExplosiveResist = table.Column<float>(type: "REAL", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armors_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Armors_SubFactions_SubFactionId",
                        column: x => x.SubFactionId,
                        principalTable: "SubFactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Commanders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaleFemale = table.Column<bool>(type: "INTEGER", nullable: false),
                    MissionsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Points = table.Column<float>(type: "REAL", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubFactionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commanders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commanders_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Commanders_SubFactions_SubFactionId",
                        column: x => x.SubFactionId,
                        principalTable: "SubFactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Commanders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projectiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubFactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    KineticDamage = table.Column<float>(type: "REAL", nullable: false),
                    EnergyDamage = table.Column<float>(type: "REAL", nullable: false),
                    ExplosiveDamage = table.Column<float>(type: "REAL", nullable: false),
                    Maintainable = table.Column<bool>(type: "INTEGER", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projectiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projectiles_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projectiles_SubFactions_SubFactionId",
                        column: x => x.SubFactionId,
                        principalTable: "SubFactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubFactionBonuses",
                columns: table => new
                {
                    SubFactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFactionBonuses", x => new { x.SubFactionId, x.BonusId });
                    table.ForeignKey(
                        name: "FK_SubFactionBonuses_Bonuses_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Bonuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubFactionBonuses_SubFactions_SubFactionId",
                        column: x => x.SubFactionId,
                        principalTable: "SubFactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubFactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Maintainable = table.Column<bool>(type: "INTEGER", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Weapons_SubFactions_SubFactionId",
                        column: x => x.SubFactionId,
                        principalTable: "SubFactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceShipClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShipClass = table.Column<string>(type: "TEXT", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArmorId = table.Column<int>(type: "INTEGER", nullable: true),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceShipClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceShipClasses_Armors_ArmorId",
                        column: x => x.ArmorId,
                        principalTable: "Armors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpaceShipClasses_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CommanderBonus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommanderId = table.Column<int>(type: "INTEGER", nullable: true),
                    BonusId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommanderBonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommanderBonus_Bonuses_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Bonuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommanderBonus_Commanders_CommanderId",
                        column: x => x.CommanderId,
                        principalTable: "Commanders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommanderRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StarFleetRankId = table.Column<int>(type: "INTEGER", nullable: true),
                    CommanderId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommanderRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommanderRanks_Commanders_CommanderId",
                        column: x => x.CommanderId,
                        principalTable: "Commanders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommanderRanks_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CommanderRanks_Ranks_StarFleetRankId",
                        column: x => x.StarFleetRankId,
                        principalTable: "Ranks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommanderWalet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CommanderId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommanderWalet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommanderWalet_Commanders_CommanderId",
                        column: x => x.CommanderId,
                        principalTable: "Commanders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommanderWalet_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceShips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpaceShipClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CommanderId = table.Column<int>(type: "INTEGER", nullable: true),
                    MapableObject = table.Column<string>(type: "TEXT", nullable: false),
                    NameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceShips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpaceShips_Commanders_CommanderId",
                        column: x => x.CommanderId,
                        principalTable: "Commanders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpaceShips_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SpaceShips_SpaceShipClasses_SpaceShipClassId",
                        column: x => x.SpaceShipClassId,
                        principalTable: "SpaceShipClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Factions",
                columns: new[] { "Id", "DescriptionKey", "FactionCode", "IsAlive", "NameKey", "ShipPrefix", "ShortDescriptionKey", "ShortNameKey" },
                values: new object[,]
                {
                    { 1, "F10_DescKey", "F10", false, "F10_FullNameKey", "F10_ShipPrefix", "F10_ShortDescKey", "F10_ShortNameKey" },
                    { 2, "F1_DescKey", "F1", false, "F1_FullNameKey", null, "F1_ShortDescKey", "F1_ShortNameKey" }
                });

            migrationBuilder.InsertData(
                table: "Planets",
                columns: new[] { "Id", "DescriptionKey", "FactionId", "NameKey", "Population", "Status" },
                values: new object[] { 1, "P_F10_0__DescKey", 1, "P_F10_0__FullNameKey", 0f, 0 });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "DescriptionKey", "FactionId", "LevelNameKey", "MinExperience", "RankType", "SortOrder" },
                values: new object[,]
                {
                    { 1, "F10_L1_Rank_Desc", 1, "F10_L1", 0, "f", 1 },
                    { 2, "F10_L2_Rank_Desc", 1, "F10_L2", 1000, "f", 2 },
                    { 3, "F10_L3_Rank_Desc", 1, "F10_L3", 2500, "f", 3 },
                    { 4, "F10_L4_Rank_Desc", 1, "F10_L4", 5000, "f", 4 },
                    { 5, "F10_L5_Rank_Desc", 1, "F10_L5", 9000, "f", 5 },
                    { 6, "F10_L6_Rank_Desc", 1, "F10_L6", 15000, "f", 6 },
                    { 7, "F10_L7_Rank_Desc", 1, "F10_L7", 25000, "f", 7 },
                    { 8, "F10_L8_Rank_Desc", 1, "F10_L8", 45000, "f", 8 },
                    { 9, "F10_L9_Rank_Desc", 1, "F10_L9", 75000, "f", 9 },
                    { 10, "F10_L10_Rank_Desc", 1, "F10_L10", 120000, "f", 10 },
                    { 11, "F10_L11_Rank_Desc", 1, "F10_L11", 200000, "f", 11 },
                    { 12, "F10_L12_Rank_Desc", 1, "F10_L12", 350000, "f", 12 }
                });

            migrationBuilder.InsertData(
                table: "SubFactions",
                columns: new[] { "Id", "DescriptionKey", "FactionId", "NameKey", "ShortDescriptionKey", "ShortNameKey", "SubFactionCode" },
                values: new object[,]
                {
                    { 1, "F10_NAA_DescKey", 1, "F10_NAA_FullNameKey", "F10_NAA_ShortDescKey", "F10_NAA_ShortNameKey", "F10_NAA" },
                    { 2, "F10_ECU_DescKey", 1, "F10_ECU_FullNameKey", "F10_ECU_ShortDescKey", "F10_ECU_ShortNameKey", "F10_ECU" },
                    { 3, "F10_ECU_DescKey", 1, "F10_PAS_FullNameKey", "F10_ECU_ShortDescKey", "F10_ECU_ShortNameKey", "F10_PAS" },
                    { 4, "F10_STC_DescKey", 1, "F10_STC_FullNameKey", "F10_STC_ShortDescKey", "F10_STC_ShortNameKey", "F10_STC" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armors_FactionId",
                table: "Armors",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Armors_SubFactionId",
                table: "Armors",
                column: "SubFactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusParameters_BonusId",
                table: "BonusParameters",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusParameters_ParameterName",
                table: "BonusParameters",
                column: "ParameterName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommanderBonus_BonusId",
                table: "CommanderBonus",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_CommanderBonus_CommanderId",
                table: "CommanderBonus",
                column: "CommanderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommanderRanks_CommanderId",
                table: "CommanderRanks",
                column: "CommanderId");

            migrationBuilder.CreateIndex(
                name: "IX_CommanderRanks_FactionId",
                table: "CommanderRanks",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommanderRanks_StarFleetRankId",
                table: "CommanderRanks",
                column: "StarFleetRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_FactionId",
                table: "Commanders",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_SubFactionId",
                table: "Commanders",
                column: "SubFactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Commanders_UserId",
                table: "Commanders",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommanderWalet_CommanderId_CurrencyId",
                table: "CommanderWalet",
                columns: new[] { "CommanderId", "CurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommanderWalet_CurrencyId",
                table: "CommanderWalet",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionBonuses_BonusId",
                table: "FactionBonuses",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionCurrencies_FactionId",
                table: "FactionCurrencies",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionHomeWorlds_HomePlanetId",
                table: "FactionHomeWorlds",
                column: "HomePlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_FactionId",
                table: "Planets",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_FactionId",
                table: "Projectiles",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projectiles_SubFactionId",
                table: "Projectiles",
                column: "SubFactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_FactionId",
                table: "Ranks",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceShipClasses_ArmorId",
                table: "SpaceShipClasses",
                column: "ArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceShipClasses_FactionId",
                table: "SpaceShipClasses",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceShips_CommanderId",
                table: "SpaceShips",
                column: "CommanderId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceShips_FactionId",
                table: "SpaceShips",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceShips_SpaceShipClassId",
                table: "SpaceShips",
                column: "SpaceShipClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFactionBonuses_BonusId",
                table: "SubFactionBonuses",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFactions_FactionId",
                table: "SubFactions",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_FactionId",
                table: "Weapons",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_SubFactionId",
                table: "Weapons",
                column: "SubFactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusParameters");

            migrationBuilder.DropTable(
                name: "CommanderBonus");

            migrationBuilder.DropTable(
                name: "CommanderRanks");

            migrationBuilder.DropTable(
                name: "CommanderWalet");

            migrationBuilder.DropTable(
                name: "FactionBonuses");

            migrationBuilder.DropTable(
                name: "FactionCurrencies");

            migrationBuilder.DropTable(
                name: "FactionHomeWorlds");

            migrationBuilder.DropTable(
                name: "Projectiles");

            migrationBuilder.DropTable(
                name: "SpaceShips");

            migrationBuilder.DropTable(
                name: "SubFactionBonuses");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Commanders");

            migrationBuilder.DropTable(
                name: "SpaceShipClasses");

            migrationBuilder.DropTable(
                name: "Bonuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Armors");

            migrationBuilder.DropTable(
                name: "SubFactions");

            migrationBuilder.DropTable(
                name: "Factions");
        }
    }
}
