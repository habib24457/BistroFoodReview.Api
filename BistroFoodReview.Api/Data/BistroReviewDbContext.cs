using BistroFoodReview.Api.Models;
using BistroFoodReview.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BistroFoodReview.Api.Data;

public class BistroReviewDbContext : DbContext
{
    public BistroReviewDbContext(DbContextOptions<BistroReviewDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<MealOption> MealOptions { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Each user can have many rating (1:N)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Each meal can have many ratings (1:N)
        modelBuilder.Entity<Meal>()
            .HasMany(m => m.Ratings)
            .WithOne(r => r.Meal)
            .HasForeignKey(r => r.MealId)
            .OnDelete(DeleteBehavior.Cascade); //ratings cannot exist without a user/meal, so if the corresponding meal/user is deleted, rating is also deleted
        
        //One mealOption contains many meals
        modelBuilder.Entity<MealOption>()
            .HasMany(mo => mo.Meals)
            .WithOne(m => m.MealOption)
            .HasForeignKey(meal => meal.MealOptionId)
            .OnDelete(DeleteBehavior.Restrict); //Applying cescade here would automatically delete the meals if we delete mealOption. 
        
        //A user can rate only a meal once a day
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.MealId, r.UserId })
            .IsUnique();

    }
}