using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class SeedUserDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc",
                column: "ConcurrencyStamp",
                value: "6c1bab71-343d-4d75-b3ae-767b603fadd1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                column: "ConcurrencyStamp",
                value: "7518c7b2-1909-4cd3-8c5c-dd98e97eed91");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                column: "ConcurrencyStamp",
                value: "fb6dff4f-cdf5-4792-9894-5fefb6cdae0f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846",
                column: "ConcurrencyStamp",
                value: "dae2d224-3c93-46a4-9156-79e979b76cb2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                column: "ConcurrencyStamp",
                value: "113d2740-b2ad-46e9-aa7c-18ab43b82ee6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                column: "ConcurrencyStamp",
                value: "7968a070-4691-4fe8-8f40-5eca177b88b2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01734a51-05b1-4c95-8d21-6820014332e9",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "0230274e-b72b-4de0-949f-ec7ad20cd250", "Demo", "Steve", "Technician", "3", "AQAAAAEAACcQAAAAEAIX9I0tPxcBVHKaAvw1aBK2/kMcaaiZ1Ws5a6EbB/bW/30vdj9tn28k4dB45++OpQ==", "07123456789", "9d2902a0-0935-4556-9b3b-df17d61a6e02" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "250f3fea-59bd-4f65-ba6a-a08b7afad55a",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "5afd50d1-7629-464a-a38b-1e8a23641367", "Demo", "Steve", "Patient", "5", "AQAAAAEAACcQAAAAELNYOmNiLO/773i7jdycDrdjBfZ9z/Lyu97hmgObka5O4koEZZqkU2T511QMwE6s9Q==", "07123456789", "7519ce4a-a06c-4adb-8985-77827c135451" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "dbf3eed0-5bb5-4d07-8b20-be58c93f231c", "Demo", "Steve", "Authorised Carer", "6", "AQAAAAEAACcQAAAAEBMKd5esBy5+lg+xCQ0K3j4IeyavWznMSJsKPyaMuhMGLeZHgx5ySe0tiY2maYW0Uw==", "07123456789", "4eb93520-c34d-4718-9308-87e81d99f017" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "929d9063-e187-4722-80d0-642bb2d3f1a5", "Demo", "Steve", "Admin", "1", "AQAAAAEAACcQAAAAEKgc3PLBb2RqvgOfg2yCY86mVqsE0VRnkKcBMXv96x4OhO6SgeyX2iqpo1e3NCq/7g==", "07123456789", "795ab7c3-3211-4d41-8ece-2ef59fd21c26" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c299b237-a197-454d-b474-587e7fe61656",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "1f19d16e-0ea8-42a4-914a-2a0ecf9ce8b0", "Demo", "Steve", "G.P", "4", "AQAAAAEAACcQAAAAEDPYxWUc2MU3/FNLN0vFi+6Sb5G58q/KGJ59pCYj1VUI8B1rUMM2SCrt7vEW1x24zQ==", "07123456789", "cb92f124-69a5-49d6-bf2c-294c00ed04fd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fd064d4e-7457-4287-a3f4-5b99580ef2ab",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "Nottingham", "2ffb4353-0f4e-4e72-8d07-71e4eaad7c18", "Demo", "Steve", "Pharmacist", "2", "AQAAAAEAACcQAAAAELGNBl7aWgSQBAk1XZZvt5648e0+FxB3gav1roqX2QG2+YCVbzWGGlyjibpuiKjT3A==", "07123456789", "2f2bf165-0e0a-44ef-bf80-05f2b94bcc4b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: "01734a51-05b1-4c95-8d21-6820014332e9",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "706765a7-28ac-4ad4-8935-a760d8eeb9a9", null, null, null, null, "AQAAAAEAACcQAAAAEEblYKMALLLQZcwA8o3PGBvAb7J427YkLmH/DLhtDGHxFUj/PzCIhepUYk6s+FvrxA==", null, "93f594c0-4bd9-4555-8c72-18de913cda5a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "250f3fea-59bd-4f65-ba6a-a08b7afad55a",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "302af0a2-7197-4c26-9fa8-14ab10061f51", null, null, null, null, "AQAAAAEAACcQAAAAEBea/EhS/KCwmHrs/I0N9XocNVZEhd/wzeJB8tR4PHc94AldxleS08vaJxVy/RaKoA==", null, "379e1c18-8f4a-45f5-b47d-04b01c0af6d4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "7a8266bc-c78e-461c-867d-adc4e1bc073c", null, null, null, null, "AQAAAAEAACcQAAAAEHXkN1vsFuhRDXz4TEW+rv+b+Cs1enulWo6enKtPIlyzrUdILVn9ia8Xj5+BX4KNYw==", null, "945befd3-2621-44f7-834f-a3b6389f2ea6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "2a4673d9-4280-4f31-abf5-5c61c83129c8", null, null, null, null, "AQAAAAEAACcQAAAAEN/QGFXZopgiLSihLbM0His3Tv2Vkue1+jt0D87bDjpKqA2HHBAKQjZApvamvROaVA==", null, "ec71e725-02cb-4dcf-9127-d95ba13e0a6d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c299b237-a197-454d-b474-587e7fe61656",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "13e0da14-6a03-475b-9cb5-7de13d6e1295", null, null, null, null, "AQAAAAEAACcQAAAAECwvOvvOOOcwIN6kmx5e+V1ue1V7Zc+shUUqU0MlWLn0ARIu3m+6DLLq+3sqTGOdlg==", null, "fcdee77f-0cd1-4fe0-82a5-77fb2bfb0bbe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fd064d4e-7457-4287-a3f4-5b99580ef2ab",
                columns: new[] { "City", "ConcurrencyStamp", "FirstName", "GeneralPractioner", "LastName", "NHSNumber", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "e986e8e6-f748-4524-ae50-7c6360086395", null, null, null, null, "AQAAAAEAACcQAAAAECjNQIdDn0uDxl97ci6jgJRc4an4ZrM2FbxNs/fab4Pwh2aUu0QRU+S/Wmu/4VAUKw==", null, "f3940753-4cdc-4c05-8ae1-6966ba93d934" });
        }
    }
}
