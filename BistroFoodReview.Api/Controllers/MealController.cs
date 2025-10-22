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
    
    [HttpPost("createMeal")]
    public async Task<IActionResult> CreateMeal([FromBody] CreateMealDto dto)
    {
        try
        {
            var newMeal = await mealRepository.CreateMealAsync(dto.MealOptionId, dto.Date, dto.EditedMealName);
            return CreatedAtAction(nameof(CreateMeal), new { mealId = newMeal.Id }, newMeal);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    [HttpPut("editName/{id:Guid}")]
    public async Task<IActionResult>UpdateMealName([FromRoute] Guid id,[FromBody] UpdateMealNameDto dto)
    {
        var updatedMeal = await mealRepository.UpdateMealNameAsync(id, dto.EditedMealName);

        if (updatedMeal == null)
        {
            return NotFound("Meal Not Found");
        }

        return Ok(updatedMeal);
    }
}