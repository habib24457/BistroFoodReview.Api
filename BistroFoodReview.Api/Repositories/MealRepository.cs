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
    public Task<Meal> CreateMealAsync(Guid mealOptionId, DateTime mealDate, string mealName);
    public Task<Meal?> UpdateMealNameAsync(Guid mealId, string editedName);
    public Task<List<string>> GetMealNeamesForAutoCompleteByQuery(string mealNameQuery);
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
    
    public async Task<Meal> CreateMealAsync(Guid mealOptionId, DateTime mealDate, string mealName)
    {
        var existingMeal = await bistroReviewDbContext.Meals
            .FirstOrDefaultAsync(m => m.MealOptionId == mealOptionId && m.Date.Date == mealDate.Date);
        if (existingMeal != null)
        {
            throw new InvalidOperationException("Meal already exists for this MealOption and for date: "+mealDate.Date);
        }

        var newMeal = new Meal
        {
            Id = Guid.NewGuid(),
            MealOptionId = mealOptionId,
            Date = mealDate.Date,
            EditedMealName = mealName
        };
        
        bistroReviewDbContext.Meals.AddAsync(newMeal);
        await bistroReviewDbContext.SaveChangesAsync();
        return newMeal;
    }

    public async Task<Meal?> UpdateMealNameAsync(Guid mealId, string editedName)
    {
        var meal = await bistroReviewDbContext.Meals.FindAsync(mealId);
        if (meal == null)
            return null;
        meal.EditedMealName = editedName;
        await bistroReviewDbContext.SaveChangesAsync();
        return meal;
    }
    

    public async Task<List<string?>> GetMealNeamesForAutoCompleteByQuery(string mealNameQuery)
    {
        return await bistroReviewDbContext.Meals
            .Where(m => !string.IsNullOrEmpty(m.EditedMealName) &&
                        m.EditedMealName.ToLower().Contains(mealNameQuery.ToLower()))
            .Select(m => m.EditedMealName)
            .Distinct()
            .Take(3)
            .ToListAsync();
    }
}