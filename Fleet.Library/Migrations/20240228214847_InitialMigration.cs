using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Regira.Fleet.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "car_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
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
                name: "supplier_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    brand_id = table.Column<int>(type: "integer", nullable: true),
                    car_type_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    model = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cars", x => x.id);
                    table.ForeignKey(
                        name: "fk_cars_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cars_car_types_car_type_id",
                        column: x => x.car_type_id,
                        principalTable: "car_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    supplier_type_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    identification_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    normalized_identification_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    normalized_title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id);
                    table.ForeignKey(
                        name: "fk_suppliers_supplier_types_supplier_type_id",
                        column: x => x.supplier_type_id,
                        principalTable: "supplier_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "interventions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guid = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    car_id = table.Column<int>(type: "integer", nullable: true),
                    supplier_id = table.Column<int>(type: "integer", nullable: true),
                    intervention_type_id = table.Column<int>(type: "integer", nullable: true),
                    invoice_id = table.Column<int>(type: "integer", nullable: true),
                    mileage = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interventions", x => x.id);
                    table.ForeignKey(
                        name: "fk_interventions_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_interventions_intervention_types_intervention_type_id",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_interventions_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supplier_addresses",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    street = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    street_number = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    box_number = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    po_box = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    municipality = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    country_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    normalized_content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_addresses", x => new { x.supplier_id, x.id });
                    table.ForeignKey(
                        name: "fk_supplier_addresses_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "supplier_contact_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplier_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    data_type = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_contact_data", x => new { x.supplier_id, x.id });
                    table.ForeignKey(
                        name: "fk_supplier_contact_data_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    id1 = table.Column<int>(type: "integer", nullable: false),
                    invoice_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    invoice_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    tax_category = table.Column<int>(type: "integer", maxLength: 1, nullable: true),
                    tax_amount = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    price_excl = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    price_incl = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_interventions_id",
                        column: x => x.id,
                        principalTable: "interventions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_brands_client_id",
                table: "brands",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_brands_client_id_code",
                table: "brands",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_brands_normalized_title",
                table: "brands",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_car_types_client_id",
                table: "car_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_car_types_client_id_code",
                table: "car_types",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_car_types_normalized_title",
                table: "car_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_cars_brand_id",
                table: "cars",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_cars_car_type_id",
                table: "cars",
                column: "car_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_cars_client_id",
                table: "cars",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_cars_client_id_code",
                table: "cars",
                columns: new[] { "client_id", "code" },
                unique: true);

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
                name: "ix_intervention_types_normalized_title",
                table: "intervention_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_car_id",
                table: "interventions",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_client_id",
                table: "interventions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_intervention_type_id",
                table: "interventions",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_interventions_supplier_id",
                table: "interventions",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_invoice_date",
                table: "invoices",
                column: "invoice_date");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_invoice_number",
                table: "invoices",
                column: "invoice_number");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_addresses_normalized_content",
                table: "supplier_addresses",
                column: "normalized_content");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_contact_data_data_type",
                table: "supplier_contact_data",
                column: "data_type");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_contact_data_normalized_value",
                table: "supplier_contact_data",
                column: "normalized_value");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_types_client_id",
                table: "supplier_types",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_types_client_id_code",
                table: "supplier_types",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supplier_types_normalized_title",
                table: "supplier_types",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_client_id",
                table: "suppliers",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_client_id_code",
                table: "suppliers",
                columns: new[] { "client_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_normalized_title",
                table: "suppliers",
                column: "normalized_title");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_supplier_type_id",
                table: "suppliers",
                column: "supplier_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "supplier_addresses");

            migrationBuilder.DropTable(
                name: "supplier_contact_data");

            migrationBuilder.DropTable(
                name: "interventions");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "intervention_types");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "car_types");

            migrationBuilder.DropTable(
                name: "supplier_types");
        }
    }
}
