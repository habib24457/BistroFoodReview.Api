using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BistroFoodReview.Api.Controllers;

[Route("api/meal")]
[ApiController]
public class MealController(IMealRepository mealRepository, IMapper mapper):ControllerBase
{
    [HttpGet("allMeal")]
    public async Task<IActionResult> GetAllMeals()
    {
        var meals = await mealRepository.GetAllMealAsync();
        var mealsDto = mapper.Map<List<MealDto>>(meals);
        return Ok(mealsDto);
    }
    
    [HttpGet("mealOptions")]
    public async Task<IActionResult> GetMealOptions()
    {
        var mealOptions = await mealRepository.GetAllMealOptionAsync();
        var mealOptionsDto = mapper.Map<List<MealOptionDto>>(mealOptions);
        return Ok(mealOptionsDto);
    }

    [HttpGet("topMeal")]
    public async Task<IActionResult> GetTopMeals()
    {
        var topMeals = await mealRepository.GetTopMealsAsync();
        var topMealsDto = mapper.Map<List<TopMealDto>>(topMeals);
        return Ok(topMealsDto);
    }

    [HttpGet("dailyMenu")]
    public async Task<IActionResult> GetDailyMealMenu()
    {
        var targetDate = DateTime.UtcNow.Date;
        var dailyMealsMenu = await mealRepository.GetDailyMenuAsync(targetDate);
        var dailyMealsMenuDto = mapper.Map<List<DailyMealMenuDto>>(dailyMealsMenu);
        return Ok(dailyMealsMenuDto);
    }
    
    [HttpPut("editName")]
    public async Task<IActionResult> AddOrUpdateMealName([FromBody] UpdateMealNameDto dto)
    {
        var updatedMeal = await mealRepository.AddOrUpdateMealNameAsync(
            dto.MealId,
            dto.MealOptionId, 
            dto.MealDate, 
            dto.EditedMealName
        );

        var mealDto = mapper.Map<DailyMealMenuDto>(updatedMeal);
        return Ok(mealDto);
    }
}