using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Repositories;

public interface IMealRepository
{
    public Task<List<Meal>> GetAllMealAsync();
    public Task<List<MealOption>> GetAllMealOptionAsync();
    public Task<Meal> GetMealByIdAsync(Guid id);
    public Task<List<Meal>> GetTopMealsAsync();
    public Task<List<Meal>> GetDailyMenuAsync(DateTime date);
    public Task<Meal?> AddOrUpdateMealNameAsync(Guid mealId,Guid mealOptionId, DateTime mealDate, string editedMealName);
}

public class MealRepository(BistroReviewDbContext bistroReviewDbContext):IMealRepository
{
    public async Task<List<Meal>> GetAllMealAsync()
    {
        var meals = await bistroReviewDbContext.Meals
            .Include(m => m.MealOption)
            .ToListAsync();
        return meals;
    }

    public async Task<List<MealOption>> GetAllMealOptionAsync()
    {
        return await bistroReviewDbContext.MealOptions.ToListAsync();
    }

    public async Task<Meal> GetMealByIdAsync(Guid id)
    {
        return await bistroReviewDbContext.Meals
                .Include(m => m.MealOption)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Meal>> GetTopMealsAsync()
    {
        var topMeals = await bistroReviewDbContext.Meals
            .Include(m => m.MealOption)
            .Include(m => m.Ratings)
            .Where(m => m.Ratings.Any())
            .OrderByDescending(m => m.Ratings.Average(r => r.Stars))
            .ThenByDescending(m=>m.Ratings.Count)
            .ToListAsync();

        return topMeals;
    }

    public async Task<List<Meal>> GetDailyMenuAsync(DateTime date)
    {
        return await bistroReviewDbContext.Meals
            .Include(m => m.MealOption)
            .Include(m => m.Ratings)
            .Where(m => m.Date.Date == date.Date)
            .OrderBy(m => m.MealOption.Name)
            .ToListAsync();
    }
    
    public async Task<Meal?> AddOrUpdateMealNameAsync(Guid mealId,Guid mealOptionId, DateTime mealDate, string editedMealName)
    {
        var meal = await bistroReviewDbContext.Meals.FindAsync(mealId);
        
        if (meal != null)
        {
            meal.EditedMealName = editedMealName;
        }
        else
        {
            meal = new Meal
            {
                Id = Guid.NewGuid(),
                MealOptionId = mealOptionId,
                Date = mealDate,
                EditedMealName = editedMealName
            };
            bistroReviewDbContext.Meals.Add(meal);
        }

        await bistroReviewDbContext.SaveChangesAsync();
        return meal;
    }
}