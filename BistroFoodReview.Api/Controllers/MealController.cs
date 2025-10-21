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
        var dailyMealsMenu = await mealRepository.GetDailyMealMenuAsync(targetDate);
        var dailyMealsMenuDto = mapper.Map<List<DailyMealMenuDto>>(dailyMealsMenu);
        return Ok(dailyMealsMenuDto);
    }
    
    [HttpPut]
    [Route("{id:Guid}/editName")]
    public async Task<IActionResult> UpdateMealName([FromRoute] Guid id, [FromBody] UpdateMealNameDto updateDto)
    {
        var mealDomain = mapper.Map<Meal>(updateDto);
        var updatedMeal = await mealRepository.UpdateMealNameAsync(id, mealDomain);
        var mealDto = mapper.Map<DailyMealMenuDto>(updatedMeal);
        return Ok(mealDto);
    } 
}