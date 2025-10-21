using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Repositories;

public interface IMealRepository
{
    public Task<List<Meal>> GetAllMealAsync();
    public Task<Meal> GetMealByIdAsync(Guid id);
    public Task<List<Meal>> GetTopMealsAsync();
    public Task<List<Meal>> GetDailyMealMenuAsync(DateTime date);

    public Task<Meal?> UpdateMealNameAsync(Guid mealId, Meal mealDomain);
    //Autocomplete
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

    public async Task<List<Meal>> GetDailyMealMenuAsync(DateTime date)
    {
        return await bistroReviewDbContext.Meals
            .Include(m => m.MealOption)
            .Include(m => m.Ratings)
            .Where(m => m.Date.Date == date.Date)
            .OrderBy(m => m.MealOption.Name)
            .ToListAsync();
    }
    
    public async Task<Meal?> UpdateMealNameAsync(Guid mealId, Meal mealDomain)
    {
        var meal = await bistroReviewDbContext.Meals
            .Include(m => m.MealOption)
            .FirstOrDefaultAsync(m => m.Id == mealId);

        if (meal == null) return null;
        
        meal.EditedMealName = mealDomain.EditedMealName;
        await bistroReviewDbContext.SaveChangesAsync();
        return meal;
    }

}