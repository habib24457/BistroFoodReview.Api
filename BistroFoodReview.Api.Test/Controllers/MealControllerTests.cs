using AutoMapper;
using BistroFoodReview.Api.Controllers;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace BistroFoodReview.Api.Test.Controllers;

public class MealControllerTests
{
    [Fact]
    public async Task GetDailyMenu_ShouldReturnOnlyMealsForToday()
    {
        // Arrange
        var mealRepo = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        var today = DateTime.UtcNow.Date;
        
        var sampleMeals = new List<Meal>
        {
            new Meal { Id = Guid.NewGuid(), EditedMealName = "Today Meal", Date = today },
            new Meal { Id = Guid.NewGuid(), EditedMealName = "Yesterday Meal", Date = today.AddDays(-1) }
        };
        mealRepo.GetDailyMenuAsync(today).Returns(Task.FromResult(sampleMeals.FindAll(m => m.Date.Date == today)));

        var sampleMealsDto = sampleMeals
            .FindAll(m => m.Date.Date == today)
            .Select(m => new DailyMealMenuDto { Id = m.Id, EditedMealName = m.EditedMealName })
            .ToList();
        mapper.Map<List<DailyMealMenuDto>>(Arg.Any<List<Meal>>()).Returns(sampleMealsDto);
        var controller = new MealController(mealRepo, mapper);

        // Act
        var result = await controller.GetDailyMealMenu();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMeals = Assert.IsAssignableFrom<List<DailyMealMenuDto>>(okResult.Value);
        Assert.All(returnedMeals, m => Assert.Equal(today, sampleMeals.First(x => x.Id == m.Id).Date.Date));
    }

    [Fact]
    public async Task GetTopMeal_ShouldReturn_TopAverageRatedMeal()
    {
        // Arrange
        var mealRepo = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        
        var meal1 = new Meal
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            EditedMealName = "Meal 1",
            Ratings = new List<Rating>
            {
                new Rating { Stars = 5 },
                new Rating { Stars = 4 }
            },
            MealOption = new MealOption { Name = "Option A" }
        };

        var meal2 = new Meal
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            EditedMealName = "Meal 2",
            Ratings = new List<Rating>
            {
                new Rating { Stars = 3 },
                new Rating { Stars = 4 }
            },
            MealOption = new MealOption { Name = "Option B" }
        };

        var topMeals = new List<Meal> { meal1, meal2 };
        mealRepo.GetTopMealsAsync().Returns(Task.FromResult(topMeals));
        var topMealsDto = topMeals.Select(m => new TopMealDto
        {
            Date = m.Date,
            MealOptionName = m.MealOption.Name,
            AverageRating = m.Ratings.Average(r => r.Stars)
        }).ToList();

        mapper.Map<List<TopMealDto>>(Arg.Any<List<Meal>>()).Returns(topMealsDto);

        var controller = new MealController(mealRepo, mapper);

        // Act
        var result = await controller.GetTopMeals();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsAssignableFrom<List<TopMealDto>>(okResult.Value);
        Assert.Equal(2, returned.Count);
        Assert.Equal("Option A", returned[0].MealOptionName);
        Assert.Equal(4.5, returned[0].AverageRating);
        Assert.Equal("Option B", returned[1].MealOptionName);
        Assert.Equal(3.5, returned[1].AverageRating);
    }
    
    [Fact]
public async Task GetTopMeal_ShouldHandleTieInAverageRating()
{
    // Arrange
    var mealRepo = Substitute.For<IMealRepository>();
    var mapper = Substitute.For<IMapper>();
    var now = DateTime.UtcNow;
    
    var meal1 = new Meal
    {
        Id = Guid.NewGuid(),
        Date = now,
        EditedMealName = "Meal 1",
        Ratings = new List<Rating> { new Rating { Stars = 4 }, new Rating { Stars = 5 } }, //Avg 4.5
        MealOption = new MealOption { Name = "Option A" }
    };

    var meal2 = new Meal
    {
        Id = Guid.NewGuid(),
        Date = now,
        EditedMealName = "Meal 2",
        Ratings = new List<Rating> { new Rating { Stars = 5 }, new Rating { Stars = 4 } }, // avg 4.5
        MealOption = new MealOption { Name = "Option B" }
    };

    var topMeals = new List<Meal> { meal1, meal2 };
    mealRepo.GetTopMealsAsync().Returns(Task.FromResult(topMeals));
    var topMealsDto = topMeals.Select(m => new TopMealDto
    {
        Date = m.Date,
        MealOptionName = m.MealOption.Name,
        AverageRating = m.Ratings.Average(r => r.Stars)
    }).ToList();
    
    mapper.Map<List<TopMealDto>>(Arg.Any<List<Meal>>()).Returns(topMealsDto);
    var controller = new MealController(mealRepo, mapper);

    // Act
    var result = await controller.GetTopMeals();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returned = Assert.IsAssignableFrom<List<TopMealDto>>(okResult.Value);
    Assert.Equal(2, returned.Count);
    Assert.Equal(4.5, returned[0].AverageRating);
    Assert.Equal(4.5, returned[1].AverageRating);
    var names = returned.Select(m => m.MealOptionName).ToList();
    Assert.Contains("Option A", names);
    Assert.Contains("Option B", names);
}
}