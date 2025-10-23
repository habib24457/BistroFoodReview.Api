using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BistroFoodReview.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                // Seed MealOptions
                var mealOption1Id = new Guid("11111111-1111-1111-1111-111111111111");
                var mealOption2Id = new Guid("22222222-2222-2222-2222-222222222222");
                var mealOption3Id = new Guid("33333333-3333-3333-3333-333333333333");

                migrationBuilder.InsertData(
                    table: "MealOptions",
                    columns: new[] { "Id", "Name" },
                    values: new object[,]
                    {
                        { mealOption1Id, "Grill Sandwiches" },
                        { mealOption2Id, "Smuts Leibspeise" },
                        { mealOption3Id, "Just Good Food" }
                    });

                // Seed Users
                var user1Id = new Guid("44444444-4444-4444-4444-444444444444");
                var user2Id = new Guid("55555555-5555-5555-5555-555555555555");

                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "Id", "FirstName", "LastName" },
                    values: new object[,]
                    {
                        { user1Id, "Finn", "Tekk" },
                        { user2Id, "Al", "Berto" }
                    });

                // Seed Meals
                var meal1Id = new Guid("66666666-6666-6666-6666-666666666666");
                var meal2Id = new Guid("77777777-7777-7777-7777-777777777777");
                var meal3Id = new Guid("88888888-8888-8888-8888-888888888888");

                // Use a fixed UTC date for deterministic migration
                var fixedDate = new DateTime(2025, 10, 23, 0, 0, 0, DateTimeKind.Utc);

                migrationBuilder.InsertData(
                    table: "Meals",
                    columns: new[] { "Id", "Date", "MealOptionId", "EditedMealName" },
                    values: new object[,]
                    {
                        { meal1Id, fixedDate, mealOption1Id, "EditedName1" },
                        { meal2Id, fixedDate, mealOption2Id, "EditedName2" },
                        { meal3Id, fixedDate, mealOption3Id, "EditedName3" }
                    });

                // Seed Ratings
                var rating1Id = new Guid("99999999-9999-9999-9999-999999999999");
                var rating2Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
                var rating3Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
                var rating4Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");

                migrationBuilder.InsertData(
                    table: "Ratings",
                    columns: new[] { "Id", "MealId", "UserId", "Stars" },
                    values: new object[,]
                    {
                        { rating1Id, meal1Id, user1Id, 4.0 },
                        { rating2Id, meal3Id, user1Id, 3.5 },
                        { rating3Id, meal2Id, user2Id, 4.5 },
                        { rating4Id, meal1Id, user2Id, 5.0 }
                    });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                // Delete seeded Ratings, Meals, Users and MealOptions by Id
                migrationBuilder.DeleteData(
                    table: "Ratings",
                    keyColumn: "Id",
                    keyValues: new object[] {
                        new Guid("99999999-9999-9999-9999-999999999999"),
                        new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")
                    });

                migrationBuilder.DeleteData(
                    table: "Meals",
                    keyColumn: "Id",
                    keyValues: new object[] {
                        new Guid("66666666-6666-6666-6666-666666666666"),
                        new Guid("77777777-7777-7777-7777-777777777777"),
                        new Guid("88888888-8888-8888-8888-888888888888")
                    });

                migrationBuilder.DeleteData(
                    table: "Users",
                    keyColumn: "Id",
                    keyValues: new object[] {
                        new Guid("44444444-4444-4444-4444-444444444444"),
                        new Guid("55555555-5555-5555-5555-555555555555")
                    });

                migrationBuilder.DeleteData(
                    table: "MealOptions",
                    keyColumn: "Id",
                    keyValues: new object[] {
                        new Guid("11111111-1111-1111-1111-111111111111"),
                        new Guid("22222222-2222-2222-2222-222222222222"),
                        new Guid("33333333-3333-3333-3333-333333333333")
                    });
        }
    }
}
