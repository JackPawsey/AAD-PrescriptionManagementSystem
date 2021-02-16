using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class SeedAdditionalUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "7d6f205b-86e1-4d1f-a1e6-2741a27d84a5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "945881ad-1491-4a5c-b92a-ad5941fbb8b7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "4b73f4f6-8e56-41fe-bca9-43c5c932dac1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "7687c2be-3203-462f-9174-45b8f3ac33b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "d33f1ace-4e33-474d-aca9-874a9b145c32");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "4c35b654-2e49-439e-b2c5-e2bfead9cc23");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2a4673d9-4280-4f31-abf5-5c61c83129c8", "AQAAAAEAACcQAAAAEN/QGFXZopgiLSihLbM0His3Tv2Vkue1+jt0D87bDjpKqA2HHBAKQjZApvamvROaVA==", "ec71e725-02cb-4dcf-9127-d95ba13e0a6d" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GeneralPractioner", "LastName", "LockoutEnabled", "LockoutEnd", "NHSNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "fd064d4e-7457-4287-a3f4-5b99580ef2ab", 0, null, "e986e8e6-f748-4524-ae50-7c6360086395", "cloudcrusaderssystems+pharmacist@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS+PHARMACIST@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS+PHARMACIST@GMAIL.COM", "AQAAAAEAACcQAAAAECjNQIdDn0uDxl97ci6jgJRc4an4ZrM2FbxNs/fab4Pwh2aUu0QRU+S/Wmu/4VAUKw==", null, false, "f3940753-4cdc-4c05-8ae1-6966ba93d934", false, "cloudcrusaderssystems+pharmacist@gmail.com" },
                    { "01734a51-05b1-4c95-8d21-6820014332e9", 0, null, "706765a7-28ac-4ad4-8935-a760d8eeb9a9", "cloudcrusaderssystems+technician@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS+TECHNICIAN@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS+TECHNICIAN@GMAIL.COM", "AQAAAAEAACcQAAAAEEblYKMALLLQZcwA8o3PGBvAb7J427YkLmH/DLhtDGHxFUj/PzCIhepUYk6s+FvrxA==", null, false, "93f594c0-4bd9-4555-8c72-18de913cda5a", false, "cloudcrusaderssystems+technician@gmail.com" },
                    { "c299b237-a197-454d-b474-587e7fe61656", 0, null, "13e0da14-6a03-475b-9cb5-7de13d6e1295", "cloudcrusaderssystems+general.practitioner@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS+GENERAL.PRACTITIONER@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS+GENERAL.PRACTITIONER@GMAIL.COM", "AQAAAAEAACcQAAAAECwvOvvOOOcwIN6kmx5e+V1ue1V7Zc+shUUqU0MlWLn0ARIu3m+6DLLq+3sqTGOdlg==", null, false, "fcdee77f-0cd1-4fe0-82a5-77fb2bfb0bbe", false, "cloudcrusaderssystems+general.practitioner@gmail.com" },
                    { "250f3fea-59bd-4f65-ba6a-a08b7afad55a", 0, null, "302af0a2-7197-4c26-9fa8-14ab10061f51", "cloudcrusaderssystems+patient@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS+PATIENT@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS+PATIENT@GMAIL.COM", "AQAAAAEAACcQAAAAEBea/EhS/KCwmHrs/I0N9XocNVZEhd/wzeJB8tR4PHc94AldxleS08vaJxVy/RaKoA==", null, false, "379e1c18-8f4a-45f5-b47d-04b01c0af6d4", false, "cloudcrusaderssystems+patient@gmail.com" },
                    { "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b", 0, null, "7a8266bc-c78e-461c-867d-adc4e1bc073c", "cloudcrusaderssystems+authorised.carer@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS+AUTHORISED.CARER@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS+AUTHORISED.CARER@GMAIL.COM", "AQAAAAEAACcQAAAAEHXkN1vsFuhRDXz4TEW+rv+b+Cs1enulWo6enKtPIlyzrUdILVn9ia8Xj5+BX4KNYw==", null, false, "945befd3-2621-44f7-834f-a3b6389f2ea6", false, "cloudcrusaderssystems+authorised.carer@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "fd064d4e-7457-4287-a3f4-5b99580ef2ab", "a0abf262-1a77-4b9d-bac5-ec293928f9ae" },
                    { "01734a51-05b1-4c95-8d21-6820014332e9", "5cf92bcd-61c7-40be-bf40-857cd7e94679" },
                    { "c299b237-a197-454d-b474-587e7fe61656", "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42" },
                    { "250f3fea-59bd-4f65-ba6a-a08b7afad55a", "89363d4b-e187-4c02-8959-c3fa597d0846" },
                    { "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b", "4d2715ee-88a0-4631-8339-cf24311bafbc" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "01734a51-05b1-4c95-8d21-6820014332e9", "5cf92bcd-61c7-40be-bf40-857cd7e94679" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "250f3fea-59bd-4f65-ba6a-a08b7afad55a", "89363d4b-e187-4c02-8959-c3fa597d0846" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b", "4d2715ee-88a0-4631-8339-cf24311bafbc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "c299b237-a197-454d-b474-587e7fe61656", "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "fd064d4e-7457-4287-a3f4-5b99580ef2ab", "a0abf262-1a77-4b9d-bac5-ec293928f9ae" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01734a51-05b1-4c95-8d21-6820014332e9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "250f3fea-59bd-4f65-ba6a-a08b7afad55a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c299b237-a197-454d-b474-587e7fe61656");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fd064d4e-7457-4287-a3f4-5b99580ef2ab");

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
    }
}
