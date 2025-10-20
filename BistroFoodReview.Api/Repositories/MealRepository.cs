using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Repositories;

public interface IMealRepository
{
    public Task<List<Meal>> GetAllMealAsync();
    //Top Meals
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
}