using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Regira.Fleet.Aca.Library.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 3, nullable: true),
                    title = table.Column<string>(maxLength: 64, nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "car_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 3, nullable: true),
                    title = table.Column<string>(maxLength: 64, nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intervention_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 3, nullable: true),
                    title = table.Column<string>(maxLength: 64, nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intervention_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "supplier_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 3, nullable: true),
                    title = table.Column<string>(maxLength: 64, nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    brand_id = table.Column<int>(nullable: true),
                    car_type_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(maxLength: 8, nullable: true),
                    model = table.Column<string>(maxLength: 64, nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cars", x => x.id);
                    table.ForeignKey(
                        name: "fk_cars_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_cars_car_types_car_type_id",
                        column: x => x.car_type_id,
                        principalTable: "car_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    supplier_type_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(maxLength: 10, nullable: true),
                    name = table.Column<string>(maxLength: 128, nullable: true),
                    name2 = table.Column<string>(maxLength: 128, nullable: true),
                    address1 = table.Column<string>(maxLength: 128, nullable: true),
                    address2 = table.Column<string>(maxLength: 128, nullable: true),
                    postal_code = table.Column<string>(maxLength: 32, nullable: true),
                    location = table.Column<string>(maxLength: 128, nullable: true),
                    country_code = table.Column<string>(maxLength: 2, nullable: true),
                    email = table.Column<string>(maxLength: 128, nullable: true),
                    phone1 = table.Column<string>(maxLength: 64, nullable: true),
                    phone2 = table.Column<string>(maxLength: 64, nullable: true),
                    vat_number = table.Column<string>(maxLength: 32, nullable: true),
                    notes = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: true),
                    last_modified = table.Column<DateTime>(nullable: true),
                    is_archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id);
                    table.ForeignKey(
                        name: "fk_suppliers_supplier_types_supplier_type_id",
                        column: x => x.supplier_type_id,
                        principalTable: "supplier_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    car_id = table.Column<int>(nullable: true),
                    supplier_id = table.Column<int>(nullable: true),
                    intervention_type_id = table.Column<int>(nullable: true),
                    invoice_number = table.Column<string>(maxLength: 32, nullable: true),
                    invoice_date = table.Column<DateTime>(nullable: true),
                    tax_category = table.Column<string>(maxLength: 1, nullable: true),
                    tax_amount = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    price_excl = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    price_incl = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    mileage = table.Column<int>(nullable: true),
                    hyperlink = table.Column<string>(maxLength: 256, nullable: true),
                    comments = table.Column<string>(maxLength: 256, nullable: true),
                    notes = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    last_modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookings", x => x.id);
                    table.ForeignKey(
                        name: "fk_bookings_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bookings_intervention_types_intervention_type_id",
                        column: x => x.intervention_type_id,
                        principalTable: "intervention_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bookings_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_car_id",
                table: "bookings",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_intervention_type_id",
                table: "bookings",
                column: "intervention_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_invoice_date",
                table: "bookings",
                column: "invoice_date");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_invoice_number",
                table: "bookings",
                column: "invoice_number");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_supplier_id",
                table: "bookings",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_brands_code",
                table: "brands",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_brands_title",
                table: "brands",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_car_types_code",
                table: "car_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_car_types_title",
                table: "car_types",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_cars_brand_id",
                table: "cars",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_cars_car_type_id",
                table: "cars",
                column: "car_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_cars_code",
                table: "cars",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_code",
                table: "intervention_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_types_title",
                table: "intervention_types",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_types_code",
                table: "supplier_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supplier_types_title",
                table: "supplier_types",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_code",
                table: "suppliers",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_name",
                table: "suppliers",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_supplier_type_id",
                table: "suppliers",
                column: "supplier_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

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
