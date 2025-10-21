using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BistroFoodReview.Api.Controllers;

[Route("api/[controller]")]
[Controller]
public class RatingController(IRatingRepository ratingRepository,IMealRepository mealRepository,IMapper mapper):ControllerBase
{

    [HttpGet("ratings")]
    public async Task<IActionResult> GetAllRatings()
    {
        var ratings = await ratingRepository.GetAllRatingAsync();
        if (ratings == null)
            return NotFound();
        var ratingsDto = mapper.Map<List<RatingDto>>(ratings);
        return Ok(ratingsDto);
    }
    
    
    /// <summary>
    /// Creates a new rating for a meal (rating must be in between 1–5 stars).
    /// Business logic:
    /// - User can only rate meals that are available for today
    /// - User can rate each mealOption once per day
    /// - Example:
    ///      - On 21st Oct, UserA can rate "Grill Sandwiches" once.
    ///      - On 22nd Oct, UserA can rate "Grill Sandwiches" again (new day)
    /// </summary>
    /// <param name="createRatingDto">Rating input data (UserId, MealId, Stars)</param>
    /// <returns>Returns the created rating as RatingDto</returns>
    [HttpPost("saveRating")]
    public async Task<IActionResult> SaveRating([FromBody] CreateRatingDto createRatingDto)
    {
        await ValidateRatingAsync(createRatingDto);
        var ratingEntity = mapper.Map<Rating>(createRatingDto);
        ratingEntity.Id = Guid.NewGuid();
        var createdRating = await ratingRepository.SaveRatingAsync(ratingEntity);
        var ratingDto = mapper.Map<RatingDto>(createdRating);
        return CreatedAtAction(nameof(SaveRating), new { id = ratingDto.Id }, ratingDto);
    }

    private async Task<Meal?> ValidateRatingAsync(CreateRatingDto dto)
    {
        if (dto.Stars < 1 || dto.Stars > 5)
            throw new BadHttpRequestException("Stars must be in between 1 and 5");

        var meal = await mealRepository.GetMealByIdAsync(dto.MealId);
        if (meal == null)
            throw new BadHttpRequestException("The selected meal is not available");

        if (meal.Date.Date != DateTime.UtcNow.Date)
            throw new BadHttpRequestException("You can only rate meals that are available for today");

        var existingRating = await ratingRepository
            .GetExistingRatingForUserAndMealOptionAsync(dto.UserId, meal.MealOptionId);

        if (existingRating != null)
            throw new BadHttpRequestException("You have already rated this meal option today.");

        return meal;
    }        
    
}