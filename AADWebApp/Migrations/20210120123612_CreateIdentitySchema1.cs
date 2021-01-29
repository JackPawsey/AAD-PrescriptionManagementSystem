using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class CreateIdentitySchema1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "2bc38ac5-d723-4066-931b-43bc14cd99b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "7f4a3472-8dbd-4bd7-a1e6-2d3d4158d633");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "1381e249-0cdc-4da6-b9eb-2721e5f26805");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "0055086d-568e-4103-aa4a-3ff23926d626");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "5628dfc3-7f68-4d26-b4a4-15b77390d1b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "4758dc78-4d94-47a3-b263-b714508151ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "14f651ba-1988-4fed-b35b-fb082569fb1b", "AQAAAAEAACcQAAAAEIDTb5JrI4rhkEgxH62bmsqHsT+3IIV/CW4F6RyNbYWR7J6eQuXwBwd9zIozeFBwzw==", "2fa46dab-5ddf-440c-9196-a2a544094113" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "53fb236d-2b59-49d8-9ca2-65f6047c0940");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "f686439c-065c-448d-a604-a883f87a8efc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "4ba59364-8d51-4986-a739-145cd8174289");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "e9ae5e5d-58e6-49ef-919d-3d2f8e4fea21");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "ad83495f-f9ab-4b9b-bded-5464c6cadc0b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "efbbaa89-59ed-403f-ad2a-6c9ae4a33452");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cb729c3-7012-4054-8950-a25066646633", "AQAAAAEAACcQAAAAEMh/BpQtEGyKCHupqqzy1sh+qGl6ZxIfZD6WERROKnb/ZnhnvFBNZeU34FwDeXtiVg==", "a9e11a07-2bca-4af7-9cb2-b97afdbdf524" });
        }
    }
}
