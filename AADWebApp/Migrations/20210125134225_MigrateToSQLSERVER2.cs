using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class MigrateToSQLSERVER2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "5eecdbfd-b234-4a15-b1f2-9f1cfb11a9f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "e0960a86-2476-48a5-8a9b-534acec7ffd9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "d5eace2f-7945-4d3c-b6cf-8c3e3d6729c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "e684556f-8b8b-426e-8c2a-c7fe730c9b40");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "9d75e86e-6bb6-4c33-aa29-9ea6f4c68da7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "d0345c9e-c412-4609-a373-63539f693c9f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02602d0c-7043-4e0d-aba5-4e53e3ab0701", "AQAAAAEAACcQAAAAELNN51W608u99LD14YLGecHJQ+1jC8bNaKB+wK6HVx/2D2i3g5h3Lf0EYjv1/xCHQg==", "3fb4b5ea-c203-4126-8567-239ec11d949e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "80f0c740-d849-4702-be57-e28c74fb96e7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "470c12ec-efc5-49cb-9c1d-88f58de530d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "adcea726-8ac7-4c35-98dc-7af1203b4a4a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "e3c3f94d-94fe-4f24-a729-028062b45371");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "0a39de96-2e8e-4fa7-ae1e-29b64dd696ea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "cefe3773-e358-4e6a-9685-c3c61d380b77");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b138394-799c-4d8a-a64a-3ca4028ad687", "AQAAAAEAACcQAAAAEGACst0XeulMVnylDrSysWlb2XSa9xy5DIJfiOP8gXqAeXzapCO+cvyO7wU/oIpCJw==", "6780f7a7-88f8-4370-b066-9f6fe308972d" });
        }
    }
}
