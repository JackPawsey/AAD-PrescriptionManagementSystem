using Microsoft.EntityFrameworkCore.Migrations;

namespace AADWebApp.Migrations
{
    public partial class CreateSeedRolesAndAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a0abf262-1a77-4b9d-bac5-ec293928f9ae", "ad83495f-f9ab-4b9b-bded-5464c6cadc0b", "Pharmacist", "PHARMACIST" },
                    { "5cf92bcd-61c7-40be-bf40-857cd7e94679", "f686439c-065c-448d-a604-a883f87a8efc", "Technician", "TECHNICIAN" },
                    { "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42", "efbbaa89-59ed-403f-ad2a-6c9ae4a33452", "General Practitioner", "GENERAL PRACTITIONER" },
                    { "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31", "4ba59364-8d51-4986-a739-145cd8174289", "Admin", "ADMIN" },
                    { "89363d4b-e187-4c02-8959-c3fa597d0846", "e9ae5e5d-58e6-49ef-919d-3d2f8e4fea21", "Patient", "PATIENT" },
                    { "4d2715ee-88a0-4631-8339-cf24311bafbc", "53fb236d-2b59-49d8-9ca2-65f6047c0940", "Authorised Carer", "AUTHORISED CARER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GeneralPractioner", "LastName", "LockoutEnabled", "LockoutEnd", "NHSNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "be2497f5-ab1f-4824-9a94-a14747bcccd7", 0, null, "5cb729c3-7012-4054-8950-a25066646633", "cloudcrusaderssystems@gmail.com", false, null, null, null, false, null, null, "CLOUDCRUSADERSSYSTEMS@GMAIL.COM", "CLOUDCRUSADERSSYSTEMS@GMAIL.COM", "AQAAAAEAACcQAAAAEMh/BpQtEGyKCHupqqzy1sh+qGl6ZxIfZD6WERROKnb/ZnhnvFBNZeU34FwDeXtiVg==", null, false, "a9e11a07-2bca-4af7-9cb2-b97afdbdf524", false, "cloudcrusaderssystems@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "be2497f5-ab1f-4824-9a94-a14747bcccd7", "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d2715ee-88a0-4631-8339-cf24311bafbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cf92bcd-61c7-40be-bf40-857cd7e94679");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89363d4b-e187-4c02-8959-c3fa597d0846");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0abf262-1a77-4b9d-bac5-ec293928f9ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "be2497f5-ab1f-4824-9a94-a14747bcccd7", "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "be2497f5-ab1f-4824-9a94-a14747bcccd7");
        }
    }
}
