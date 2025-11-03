using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice Trainer", "pw1" },
                    { 2, "bob@example.com", "Bob Coach", "pw2" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CoachId", "Name", "TimeSlot" },
                values: new object[,]
                {
                    { 1, 1, "Yoga", new DateTime(2025, 11, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, "Boxing", new DateTime(2025, 11, 3, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "ClassId", "ClientName", "CoachId", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Оксана", 1, true },
                    { 2, 2, "Назар", 2, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
