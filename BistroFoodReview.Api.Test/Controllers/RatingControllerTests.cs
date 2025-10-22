using AutoMapper;
using BistroFoodReview.Api.Controllers;
using BistroFoodReview.Api.Data;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace BistroFoodReview.Api.Test.Controllers;

public class RatingControllerTests
{
    
    [Fact]
    public async Task SaveRating_ShouldSaveRating_WhenValidInput()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BistroReviewDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        await using var context = new BistroReviewDbContext(options);

        var mealOption = new MealOption { Id = Guid.NewGuid(), Name = "Grill Sandwiches" };
        context.MealOptions.Add(mealOption);

        var meal = new Meal
            { 
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                MealOptionId = mealOption.Id,
                MealOption = mealOption,
                EditedMealName = "Test Meal",
                Ratings = new List<Rating>()
            };
        context.Meals.Add(meal);

        var user = new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User" };
        context.Users.Add(user);

        await context.SaveChangesAsync();

        var mealRepo = new MealRepository(context);
        var ratingRepo = new RatingRepository(context);

        // Mock mapper
        var mapper = Substitute.For<IMapper>();
        var createRatingDto = new CreateRatingDto
        {
            MealId = meal.Id,
            UserId = user.Id,
            Stars = 5
        };

        var ratingEntity = new Rating
        {
            Id = Guid.NewGuid(),
            MealId = meal.Id,
            UserId = user.Id,
            Stars = createRatingDto.Stars,
            Meal = meal,
            User = user
        };

        var ratingDto = new RatingDto
        {
            Id = ratingEntity.Id,
            MealId = meal.Id,
            Stars = createRatingDto.Stars,
            MealDate = meal.Date,
            EditedMealName = meal.EditedMealName,
            MealOptionName = meal.MealOption.Name
        };

        mapper.Map<Rating>(createRatingDto).Returns(ratingEntity);
        mapper.Map<RatingDto>(Arg.Any<Rating>()).Returns(ratingDto);

        var controller = new RatingController(ratingRepo, mealRepo, mapper);

        // Act
        controller.SaveRating(createRatingDto);

        // Assert
        var ratingInDb = await context.Ratings.FirstOrDefaultAsync(r => r.MealId == meal.Id && r.UserId == user.Id);
        Assert.NotNull(ratingInDb);
        Assert.Equal(5, ratingInDb.Stars);
    }
}