using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Regira.Fleet.Data.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ConcurrencyTokensAndActionAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_intervention_action_attachment_intervention_actions_intervention_action_id",
                table: "intervention_action_attachment");

            migrationBuilder.DropIndex(
                name: "ix_intervention_action_attachment_intervention_action_id",
                table: "intervention_action_attachment");

            migrationBuilder.DropColumn(
                name: "intervention_action_id",
                table: "intervention_action_attachment");

            migrationBuilder.AddColumn<Guid>(
                name: "concurrency_token",
                table: "vehicles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "concurrency_token",
                table: "interventions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "concurrency_token",
                table: "intervention_operators",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_intervention_action_attachment_object_id",
                table: "intervention_action_attachment",
                column: "object_id");

            migrationBuilder.AddForeignKey(
                name: "fk_intervention_action_attachment_intervention_actions_object_id",
                table: "intervention_action_attachment",
                column: "object_id",
                principalTable: "intervention_actions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_intervention_action_attachment_intervention_actions_object_id",
                table: "intervention_action_attachment");

            migrationBuilder.DropIndex(
                name: "ix_intervention_action_attachment_object_id",
                table: "intervention_action_attachment");

            migrationBuilder.DropColumn(
                name: "concurrency_token",
                table: "vehicles");

            migrationBuilder.DropColumn(
                name: "concurrency_token",
                table: "interventions");

            migrationBuilder.DropColumn(
                name: "concurrency_token",
                table: "intervention_operators");

            migrationBuilder.AddColumn<int>(
                name: "intervention_action_id",
                table: "intervention_action_attachment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_intervention_action_attachment_intervention_action_id",
                table: "intervention_action_attachment",
                column: "intervention_action_id");

            migrationBuilder.AddForeignKey(
                name: "fk_intervention_action_attachment_intervention_actions_intervention_action_id",
                table: "intervention_action_attachment",
                column: "intervention_action_id",
                principalTable: "intervention_actions",
                principalColumn: "id");
        }
    }
}
