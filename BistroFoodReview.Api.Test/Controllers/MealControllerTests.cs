using AutoMapper;
using BistroFoodReview.Api.Controllers;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BistroFoodReview.Api.Test.Controllers;

public class MealControllerTests
{
    [Fact]
    public async Task GetAllMeals_ShouldReturnAllMeals()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<MealController>>();

        var meals = new List<Meal>
        {
            new Meal { Id = Guid.NewGuid(), EditedMealName = "Meal 1" },
            new Meal { Id = Guid.NewGuid(), EditedMealName = "Meal 2" }
        };

        var mealsDto = meals.Select(m => new MealDto
        {
            EditedMealName = m.EditedMealName,
            Date = m.Date,
        }).ToList();

        mealRepository.GetAllMealAsync().Returns(Task.FromResult(meals));
        mapper.Map<List<MealDto>>(meals).Returns(mealsDto);

        var controller = new MealController(mealRepository, mapper, logger);

        // Act
        var result = await controller.GetAllMeals();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMeals = Assert.IsAssignableFrom<List<MealDto>>(okResult.Value);
        Assert.Equal(2, returnedMeals.Count);
        Assert.Equal("Meal 1", returnedMeals[0].EditedMealName);
        Assert.Equal("Meal 2", returnedMeals[1].EditedMealName);
    }
    
    [Fact]
    public async Task GetMealOptions_ShouldReturnAllMealOptions()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<MealController>>();

        var mealOptions = new List<MealOption>
        {
            new MealOption { Id = Guid.NewGuid(), Name = "Salad" },
            new MealOption { Id = Guid.NewGuid(), Name = "Soup" }
        };

        var mealOptionsDto = mealOptions.Select(mo => new MealOptionDto
        {
            Id = mo.Id,
            Name = mo.Name
        }).ToList();

        mealRepository.GetAllMealOptionAsync().Returns(Task.FromResult(mealOptions));
        mapper.Map<List<MealOptionDto>>(mealOptions).Returns(mealOptionsDto);

        var controller = new MealController(mealRepository, mapper, logger);

        // Act
        var result = await controller.GetMealOptions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMealOptions = Assert.IsAssignableFrom<List<MealOptionDto>>(okResult.Value);

        Assert.Equal(2, returnedMealOptions.Count);
        Assert.Equal("Salad", returnedMealOptions[0].Name);
        Assert.Equal("Soup", returnedMealOptions[1].Name);
    }
    
    [Fact]
    public async Task GetDailyMenu_ShouldReturnOnlyMealsForToday()
    {
        // Arrange
        var mealRepo = Substitute.For<IMealRepository>();
        var logger = Substitute.For<ILogger<MealController>>();
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
        var controller = new MealController(mealRepo, mapper,logger);

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
        var logger = Substitute.For<ILogger<MealController>>();

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

        var controller = new MealController(mealRepo, mapper, logger);

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
        var logger = Substitute.For<ILogger<MealController>>();

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
            var controller = new MealController(mealRepo, mapper, logger);

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

    [Fact]
    public async Task UpdateMealName_ShouldUpdate_IfMealExist()
    {
        //Arrange
        var mealId = Guid.NewGuid();
        var updatedName = "Updated Meal Name";
        var logger = Substitute.For<ILogger<MealController>>();

        var meal = new Meal
        {
            Id = mealId,
            EditedMealName = "Old Name",
            MealOptionId = Guid.NewGuid(),
            Date = DateTime.UtcNow
        };
        var mapper = Substitute.For<IMapper>();
        var mealRepo = Substitute.For<IMealRepository>();
        mealRepo.UpdateMealNameAsync(mealId, updatedName)
            .Returns(callInfo =>
            {
                meal.EditedMealName = updatedName;
                return Task.FromResult(meal);
            });
        
        var controller = new MealController(mealRepo,mapper, logger);
        
        //Act
        var result = await controller.UpdateMealName(mealId, new UpdateMealNameDto
        {
            EditedMealName = updatedName
        });
        
        //Assert
        var expectedMeal = meal;
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualMeal = Assert.IsType<Meal>(okResult.Value);
        Assert.Equal(expectedMeal.Id, actualMeal.Id);
        Assert.Equal(updatedName, actualMeal.EditedMealName);
        await mealRepo.Received(1).UpdateMealNameAsync(mealId, updatedName);
    }
    
    [Fact]
    public async Task UpdateMealName_ShouldReturnNotFound_IfMealDoesNotExist()
    {
        // Arrange
        var mealId = Guid.NewGuid();
        var updatedName = "Updated Meal Name";
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<MealController>>();
        var mealRepo = Substitute.For<IMealRepository>();
        mealRepo.UpdateMealNameAsync(mealId, updatedName)
            .Returns(Task.FromResult<Meal?>(null)); 

        var controller = new MealController(mealRepo,mapper, logger);

        // Act
        var result = await controller.UpdateMealName(mealId, new UpdateMealNameDto
        {
            EditedMealName = updatedName
        });

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Meal Not Found", notFoundResult.Value);
        await mealRepo.Received(1).UpdateMealNameAsync(mealId, updatedName);
    }
    
    [Fact]
    public async Task UpdateMealName_ShouldReturnUpdatedMeal_WhenMealExists()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var logger = Substitute.For<ILogger<MealController>>();
        var mapper = Substitute.For<IMapper>();

        var mealId = Guid.NewGuid();
        var updatedMealDto = new UpdateMealNameDto
        {
            EditedMealName = "Updated Meal Name"
        };

        var updatedMeal = new Meal
        {
            Id = mealId,
            EditedMealName = updatedMealDto.EditedMealName
        };

        mealRepository.UpdateMealNameAsync(mealId, updatedMealDto.EditedMealName)
            .Returns(Task.FromResult(updatedMeal));

        var controller = new MealController(mealRepository, mapper, logger);

        // Act
        var result = await controller.UpdateMealName(mealId, updatedMealDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMeal = Assert.IsType<Meal>(okResult.Value);
        Assert.Equal(mealId, returnedMeal.Id);
        Assert.Equal("Updated Meal Name", returnedMeal.EditedMealName);
    }
    
    
    [Fact]
    public async Task UpdateMealName_ShouldReturnNotFound_WhenMealDoesNotExist()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var logger = Substitute.For<ILogger<MealController>>();
        var mapper = Substitute.For<IMapper>();

        var mealId = Guid.NewGuid();
        var updatedMealDto = new UpdateMealNameDto
        {
            EditedMealName = "Updated Meal Name"
        };

        mealRepository.UpdateMealNameAsync(mealId, updatedMealDto.EditedMealName)
            .Returns(Task.FromResult<Meal>(null));

        var controller = new MealController(mealRepository, mapper, logger);

        // Act
        var result = await controller.UpdateMealName(mealId, updatedMealDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Meal Not Found", notFoundResult.Value);
    }
    
    
    [Fact]
    public async Task Autocomplete_ShouldReturnMealNames_WhenQueryIsValid()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<MealController>>();

        var query = "Pizza";
        var matchingNames = new List<string> { "Pizza Margherita", "Pizza Salami" };
        mealRepository.GetMealNeamesForAutoCompleteByQuery(query)
            .Returns(Task.FromResult(matchingNames));

        var controller = new MealController(mealRepository,mapper, logger);

        // Act
        var result = await controller.Autocomplete(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedList = Assert.IsAssignableFrom<List<AutoCompleteMealNameDto>>(okResult.Value);

        Assert.Equal(2, returnedList.Count);
        Assert.Equal("Pizza Margherita", returnedList[0].MealName);
        Assert.Equal("Pizza Salami", returnedList[1].MealName);
    }
    
    [Fact]
    public async Task Autocomplete_ShouldReturnBadRequest_WhenQueryIsEmpty()
    {
        // Arrange
        var mealRepository = Substitute.For<IMealRepository>();
        var mapper = Substitute.For<IMapper>();
        var logger = Substitute.For<ILogger<MealController>>();
        var controller = new MealController(mealRepository, mapper, logger);

        // Act
        var result = await controller.Autocomplete("");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Query cannot be empty.", badRequestResult.Value);
    }
}