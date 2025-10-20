using AutoMapper;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BistroFoodReview.Api.Controllers;

[Route("api/meal")]
[ApiController]
public class MealController(IMealRepository mealRepository, IMapper mapper):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMealsAsync()
    {
        var meals = await mealRepository.GetAllMealAsync();
        var mealsDto = mapper.Map<List<MealDto>>(meals);
        return Ok(mealsDto);
    }
}