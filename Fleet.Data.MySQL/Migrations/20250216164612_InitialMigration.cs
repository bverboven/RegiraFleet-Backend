using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Regira.Fleet.Data.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    file_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content_type = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    path = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachments", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identification_number = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_archived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    normalized_title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_identification_number = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_content = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operators", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_archived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    normalized_title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_archived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    normalized_title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_brands", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_archived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    normalized_title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operator_addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    street = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    number = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    box = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    post_box = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    postal_code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country_code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    normalized_content = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_addresses_intervention_operators_opera",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operator_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    attachment_id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_attachments_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_intervention_operator_attachments_intervention_operators_obj",
                        column: x => x.object_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operator_contactdata",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_value = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_type = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    operator_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_contactdata", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_contactdata_intervention_operators_ope",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operator_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    label_type = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    normalized_content = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_labels", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_labels_intervention_operators_object_id",
                        column: x => x.object_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_operator_intervention_types",
                columns: table => new
                {
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    intervention_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_intervention_types", x => new { x.operator_id, x.intervention_type_id });
                    table.ForeignKey(
                        name: "fk_intervention_operator_intervention_types_intervention_operat",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_intervention_operator_intervention_types_intervention_types_",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_type_translations",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    culture = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    intervention_type_id = table.Column<int>(type: "int", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_type_translations", x => new { x.object_id, x.culture });
                    table.ForeignKey(
                        name: "fk_intervention_type_translations_intervention_types_interventi",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_type_translations",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "int", nullable: false),
                    culture = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_type_id = table.Column<int>(type: "int", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_type_translations", x => new { x.object_id, x.culture });
                    table.ForeignKey(
                        name: "fk_vehicle_type_translations_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalTable: "vehicle_types",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    vehicle_type_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identification_number = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_identification_number = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_archived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    normalized_title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    normalized_content = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "vehicle_brands",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalTable: "vehicle_types",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "interventions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    intervention_type_id = table.Column<int>(type: "int", nullable: true),
                    intervention_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    mileage = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    normalized_content = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interventions", x => x.id);
                    table.ForeignKey(
                        name: "fk_interventions_intervention_operators_operator_id",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_interventions_intervention_types_intervention_type_id",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_interventions_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    attachment_id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_attachments_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vehicle_attachments_vehicles_object_id",
                        column: x => x.object_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_intervention_types",
                columns: table => new
                {
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    intervention_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_intervention_types", x => new { x.vehicle_id, x.intervention_type_id });
                    table.ForeignKey(
                        name: "fk_vehicle_intervention_types_intervention_types_intervention_t",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vehicle_intervention_types_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicle_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    label_type = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    normalized_content = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_labels", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_labels_vehicles_object_id",
                        column: x => x.object_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    attachment_id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_attachments_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_intervention_attachments_interventions_object_id",
                        column: x => x.object_id,
                        principalTable: "interventions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    intervention_id = table.Column<int>(type: "int", nullable: false),
                    invoice_number = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    invoice_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    tax_category = table.Column<int>(type: "int", nullable: true),
                    tax_amount = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    price_excl = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    price_incl = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_invoices_interventions_intervention_id",
                        column: x => x.intervention_id,
                        principalTable: "interventions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    label_type = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    normalized_content = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_labels", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_labels_interventions_object_id",
                        column: x => x.object_id,
                        principalTable: "interventions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_actions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    guid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    operator_id = table.Column<int>(type: "int", nullable: false),
                    invoice_id = table.Column<int>(type: "int", nullable: false),
                    mileage = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    intervention_type_id = table.Column<int>(type: "int", nullable: true),
                    normalized_content = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    intervention_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_actions", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_actions_intervention_types_intervention_type_id",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_intervention_actions_interventions_intervention_id",
                        column: x => x.intervention_id,
                        principalTable: "interventions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_intervention_actions_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "intervention_invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "intervention_action_attachment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    intervention_action_id = table.Column<int>(type: "int", nullable: true),
                    object_id = table.Column<int>(type: "int", nullable: false),
                    attachment_id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_action_attachment", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_action_attachment_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_intervention_action_attachment_intervention_actions_interven",
                        column: x => x.intervention_action_id,
                        principalTable: "intervention_actions",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_action_attachment_attachment_id",
                table: "intervention_action_attachment",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_action_attachment_intervention_action_id",
                table: "intervention_action_attachment",
                column: "intervention_action_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_actions_intervention_id",
                table: "intervention_actions",
                column: "intervention_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_actions_intervention_type_id",
                table: "intervention_actions",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_actions_invoice_id",
                table: "intervention_actions",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_attachments_attachment_id",
                table: "intervention_attachments",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_attachments_object_id",
                table: "intervention_attachments",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_invoices_intervention_id",
                table: "intervention_invoices",
                column: "intervention_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_invoices_invoice_number",
                table: "intervention_invoices",
                column: "invoice_number");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_labels_object_id",
                table: "intervention_labels",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_addresses_operator_id",
                table: "intervention_operator_addresses",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_attachments_attachment_id",
                table: "intervention_operator_attachments",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_attachments_object_id",
                table: "intervention_operator_attachments",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_contactdata_normalized_value",
                table: "intervention_operator_contactdata",
                column: "normalized_value");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_contactdata_operator_id",
                table: "intervention_operator_contactdata",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_intervention_types_intervention_type_id",
                table: "intervention_operator_intervention_types",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_labels_object_id",
                table: "intervention_operator_labels",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_client_id",
                table: "intervention_operators",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_client_id_code",
                table: "intervention_operators",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_normalized_title",
                table: "intervention_operators",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_type_translations_intervention_type_id",
                table: "intervention_type_translations",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_client_id",
                table: "intervention_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_client_id_code",
                table: "intervention_types",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_client_id_title",
                table: "intervention_types",
                columns: new[] { "client_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_normalized_title",
                table: "intervention_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_client_id",
                table: "interventions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_intervention_type_id",
                table: "interventions",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_operator_id",
                table: "interventions",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_vehicle_id",
                table: "interventions",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_attachments_attachment_id",
                table: "vehicle_attachments",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_attachments_object_id",
                table: "vehicle_attachments",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_client_id",
                table: "vehicle_brands",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_client_id_code",
                table: "vehicle_brands",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_client_id_title",
                table: "vehicle_brands",
                columns: new[] { "client_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_normalized_title",
                table: "vehicle_brands",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_intervention_types_intervention_type_id",
                table: "vehicle_intervention_types",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_labels_object_id",
                table: "vehicle_labels",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_type_translations_vehicle_type_id",
                table: "vehicle_type_translations",
                column: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_client_id",
                table: "vehicle_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_client_id_code",
                table: "vehicle_types",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_client_id_title",
                table: "vehicle_types",
                columns: new[] { "client_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_normalized_title",
                table: "vehicle_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_brand_id",
                table: "vehicles",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_client_id",
                table: "vehicles",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_client_id_code",
                table: "vehicles",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_normalized_title",
                table: "vehicles",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_vehicle_type_id",
                table: "vehicles",
                column: "vehicle_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "intervention_action_attachment");

            migrationBuilder.DropTable(
                name: "intervention_attachments");

            migrationBuilder.DropTable(
                name: "intervention_labels");

            migrationBuilder.DropTable(
                name: "intervention_operator_addresses");

            migrationBuilder.DropTable(
                name: "intervention_operator_attachments");

            migrationBuilder.DropTable(
                name: "intervention_operator_contactdata");

            migrationBuilder.DropTable(
                name: "intervention_operator_intervention_types");

            migrationBuilder.DropTable(
                name: "intervention_operator_labels");

            migrationBuilder.DropTable(
                name: "intervention_type_translations");

            migrationBuilder.DropTable(
                name: "vehicle_attachments");

            migrationBuilder.DropTable(
                name: "vehicle_intervention_types");

            migrationBuilder.DropTable(
                name: "vehicle_labels");

            migrationBuilder.DropTable(
                name: "vehicle_type_translations");

            migrationBuilder.DropTable(
                name: "intervention_actions");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "intervention_invoices");

            migrationBuilder.DropTable(
                name: "interventions");

            migrationBuilder.DropTable(
                name: "intervention_operators");

            migrationBuilder.DropTable(
                name: "intervention_types");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "vehicle_brands");

            migrationBuilder.DropTable(
                name: "vehicle_types");
        }
    }
}
