using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hubtel.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWalletColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Wallets",
                newName: "wallet_type");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Wallets",
                newName: "owner_phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Wallets",
                newName: "wallet_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Wallets",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AccountScheme",
                table: "Wallets",
                newName: "account_scheme");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "Wallets",
                newName: "account_number");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Wallets",
                newName: "wallet_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wallet_type",
                table: "Wallets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "wallet_name",
                table: "Wallets",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "owner_phone",
                table: "Wallets",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Wallets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "account_scheme",
                table: "Wallets",
                newName: "AccountScheme");

            migrationBuilder.RenameColumn(
                name: "account_number",
                table: "Wallets",
                newName: "AccountNumber");

            migrationBuilder.RenameColumn(
                name: "wallet_id",
                table: "Wallets",
                newName: "Id");
        }
    }
}
