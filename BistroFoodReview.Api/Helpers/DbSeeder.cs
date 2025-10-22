using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;

namespace BistroFoodReview.Api.Helpers;

public static class DbSeeder
{
    public static void InitializeSeeding(BistroReviewDbContext context)
    {
        if (!context.MealOptions.Any())
        {
            context.MealOptions.AddRange(
                new MealOption { Id = Guid.NewGuid(), Name = "Grill Sandwiches" },
                new MealOption { Id = Guid.NewGuid(), Name = "Smuts Leibspeise" },
                new MealOption { Id = Guid.NewGuid(), Name = "Just Good Food" }
            );
            context.SaveChanges();
        } 
        
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Id = Guid.NewGuid(), FirstName = "Habibur", LastName = "Rahman" },
                new User { Id = Guid.NewGuid(), FirstName = "Markus", LastName = "Uhlar" }
            );
            context.SaveChanges();
        }
        
        if (!context.Meals.Any())
        {
            var options = context.MealOptions.ToList();
            context.Meals.AddRange(
                new Meal { Id = Guid.NewGuid(), Date = DateTime.UtcNow, MealOptionId = options[0].Id, EditedMealName = "EditedName1"},
                new Meal { Id = Guid.NewGuid(), Date = DateTime.UtcNow, MealOptionId = options[1].Id,EditedMealName = "EditedName2" },
                new Meal { Id = Guid.NewGuid(), Date = DateTime.UtcNow, MealOptionId = options[2].Id,EditedMealName = "EditedName3" }
            );
            context.SaveChanges();
        }
        
        if (!context.Ratings.Any())
        {
            var meals = context.Meals.ToList();
            var users = context.Users.ToList();
            context.Ratings.AddRange(
                new Rating { Id = Guid.NewGuid(), MealId = meals[0].Id, UserId = users[0].Id, Stars = 4 },
                new Rating { Id = Guid.NewGuid(), MealId = meals[2].Id, UserId = users[0].Id, Stars = 3.5 },
                new Rating { Id = Guid.NewGuid(), MealId = meals[1].Id, UserId = users[1].Id, Stars = 4.5 },
                new Rating { Id = Guid.NewGuid(), MealId = meals[0].Id, UserId = users[1].Id, Stars = 5 }
            );
            context.SaveChanges();
        }
    }
}