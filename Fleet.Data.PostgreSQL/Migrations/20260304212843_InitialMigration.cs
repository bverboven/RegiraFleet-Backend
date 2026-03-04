using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Regira.Fleet.Data.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    content_type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intervention_operators",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    identification_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_identification_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intervention_operator_addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    street = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    number = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    box = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    post_box = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    city = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_addresses_intervention_operators_oper",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "intervention_operator_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
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
                        name: "fk_intervention_operator_attachments_intervention_operators_ob",
                        column: x => x.object_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "intervention_operator_contactdata",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    data_type = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    operator_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_contactdata", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_contactdata_intervention_operators_op",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "intervention_operator_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    label_type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_labels", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_labels_intervention_operators_object_",
                        column: x => x.object_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "intervention_operator_intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_operator_intervention_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_intervention_operator_intervention_types_intervention_opera",
                        column: x => x.operator_id,
                        principalTable: "intervention_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_intervention_operator_intervention_types_intervention_types",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "intervention_type_translations",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    culture = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: true),
                    id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_type_translations", x => new { x.object_id, x.culture });
                    table.ForeignKey(
                        name: "fk_intervention_type_translations_intervention_types_intervent",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type_translations",
                columns: table => new
                {
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    culture = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    vehicle_type_id = table.Column<int>(type: "integer", nullable: true),
                    id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_type_translations", x => new { x.object_id, x.culture });
                    table.ForeignKey(
                        name: "fk_vehicle_type_translations_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalTable: "vehicle_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    brand_id = table.Column<int>(type: "integer", nullable: true),
                    vehicle_type_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    model = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    identification_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    normalized_identification_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "interventions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: true),
                    intervention_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    mileage = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "vehicle_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "vehicle_intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_intervention_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_intervention_types_intervention_types_intervention_",
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
                });

            migrationBuilder.CreateTable(
                name: "vehicle_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    label_type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "intervention_attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "intervention_invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    intervention_id = table.Column<int>(type: "integer", nullable: false),
                    invoice_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    invoice_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    tax_category = table.Column<int>(type: "integer", nullable: true),
                    tax_amount = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    price_excl = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    price_incl = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "intervention_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    label_type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "intervention_actions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    invoice_id = table.Column<int>(type: "integer", nullable: false),
                    mileage = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    intervention_id = table.Column<int>(type: "integer", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "intervention_action_attachment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    intervention_action_id = table.Column<int>(type: "integer", nullable: true),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
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
                        name: "fk_intervention_action_attachment_intervention_actions_interve",
                        column: x => x.intervention_action_id,
                        principalTable: "intervention_actions",
                        principalColumn: "id");
                });

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
                name: "ix_intervention_operator_addresses_normalized_content",
                table: "intervention_operator_addresses",
                column: "normalized_content");

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
                name: "ix_intervention_operator_intervention_types_intervention_type_",
                table: "intervention_operator_intervention_types",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_intervention_types_operator_id",
                table: "intervention_operator_intervention_types",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operator_labels_object_id",
                table: "intervention_operator_labels",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_normalized_title",
                table: "intervention_operators",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_tenant_id",
                table: "intervention_operators",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_operators_tenant_id_code",
                table: "intervention_operators",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_type_translations_intervention_type_id",
                table: "intervention_type_translations",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_normalized_title",
                table: "intervention_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_tenant_id",
                table: "intervention_types",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_tenant_id_code",
                table: "intervention_types",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_tenant_id_title",
                table: "intervention_types",
                columns: new[] { "tenant_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_interventions_intervention_type_id",
                table: "interventions",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_operator_id",
                table: "interventions",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_tenant_id",
                table: "interventions",
                column: "tenant_id");

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
                name: "ix_vehicle_brands_normalized_title",
                table: "vehicle_brands",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_tenant_id",
                table: "vehicle_brands",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_tenant_id_code",
                table: "vehicle_brands",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_brands_tenant_id_title",
                table: "vehicle_brands",
                columns: new[] { "tenant_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_intervention_types_intervention_type_id",
                table: "vehicle_intervention_types",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_intervention_types_vehicle_id",
                table: "vehicle_intervention_types",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_labels_object_id",
                table: "vehicle_labels",
                column: "object_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_type_translations_vehicle_type_id",
                table: "vehicle_type_translations",
                column: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_normalized_title",
                table: "vehicle_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_tenant_id",
                table: "vehicle_types",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_tenant_id_code",
                table: "vehicle_types",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_tenant_id_title",
                table: "vehicle_types",
                columns: new[] { "tenant_id", "title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_brand_id",
                table: "vehicles",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_normalized_title",
                table: "vehicles",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_tenant_id",
                table: "vehicles",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_tenant_id_code",
                table: "vehicles",
                columns: new[] { "tenant_id", "code" },
                unique: true);

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
