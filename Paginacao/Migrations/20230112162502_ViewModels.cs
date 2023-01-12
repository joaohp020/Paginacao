using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paginacao.Migrations
{
    /// <inheritdoc />
    public partial class ViewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Todos",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Todos",
                newName: "Created");
        }
    }
}
