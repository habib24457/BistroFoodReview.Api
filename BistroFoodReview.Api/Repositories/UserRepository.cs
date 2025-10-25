using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User> AddUserAsync(User user);
    Task<User?> DeleteUserAsync(Guid id); 
}

public class UserRepository(BistroReviewDbContext bistroReviewDbContext):IUserRepository
{
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await bistroReviewDbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await bistroReviewDbContext.Users
            .Include(u => u.Ratings)
            .ThenInclude(r=>r.Meal)
            .ThenInclude(m=>m.MealOption)
            .FirstOrDefaultAsync(u => u.Id == id);    
    }
    
    public async Task<User> AddUserAsync(User user)
    {
        bistroReviewDbContext.Users.Add(user);
        await bistroReviewDbContext.SaveChangesAsync();
        return user;    
    }
    
    public async Task<User?> DeleteUserAsync(Guid id)
    {
        var user = await bistroReviewDbContext.Users.FindAsync(id);
        if (user == null) return null;
        bistroReviewDbContext.Users.Remove(user);
        await bistroReviewDbContext.SaveChangesAsync();
        return user;
    }
}