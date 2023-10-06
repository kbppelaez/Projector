using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projector.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationLinkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ActivationToken = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: false),
                    ActivationLink = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationLinks_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationLinks");
        }
    }
}
