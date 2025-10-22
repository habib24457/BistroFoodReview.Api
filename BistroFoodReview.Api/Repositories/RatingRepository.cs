using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Repositories;

public interface IRatingRepository
{
    public Task<List<Rating>> GetAllRatingAsync();
    public Task<Rating> GetExistingRatingForUserAndMealOptionAsync(Guid userId, Guid mealOptionId);
    public Task<Rating> SaveRatingAsync(Rating rating);
}

public class RatingRepository(BistroReviewDbContext bistroReviewDbContext):IRatingRepository
{
    public async Task<List<Rating>> GetAllRatingAsync()
    {
        return await bistroReviewDbContext.Ratings
            .Include(r=> r.Meal)
                .ThenInclude(m => m.MealOption)
            .Include(r=>r.User)
            .ToListAsync();
    }
    
    public Task<Rating> GetExistingRatingForUserAndMealOptionAsync(Guid userId, Guid mealOptionId)
    {
        var existingRating = bistroReviewDbContext.Ratings
            .Include(r => r.Meal)
            .FirstOrDefaultAsync(r =>
                r.UserId == userId &&
                r.Meal.MealOptionId == mealOptionId &&
                r.Meal.Date.Date == DateTime.UtcNow.Date
            );
        return existingRating;
    }

    public async Task<Rating> SaveRatingAsync(Rating rating)
    {
        await bistroReviewDbContext.Ratings.AddAsync(rating);
        await bistroReviewDbContext.SaveChangesAsync();
        return rating;
    }
    
}