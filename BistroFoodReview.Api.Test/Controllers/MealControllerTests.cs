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
        // Check only meals for today are returned
        Assert.All(returnedMeals, m => Assert.Equal(today, sampleMeals.First(x => x.Id == m.Id).Date.Date));
    }
}