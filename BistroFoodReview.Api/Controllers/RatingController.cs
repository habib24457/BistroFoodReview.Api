using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BistroFoodReview.Api.Controllers;

[Route("api/[controller]")]
[Controller]
public class RatingController(
    IRatingRepository ratingRepository,
    IMealRepository mealRepository,
    IMapper mapper, 
    ILogger<RatingController>logger):ControllerBase
{

    [HttpGet("ratings")]
    public async Task<IActionResult> GetAllRatings()
    {
        logger.LogInformation("Fetching all ratings");
        var ratings = await ratingRepository.GetAllRatingAsync();
        if (ratings == null)
        {
            logger.LogWarning("No ratings found in the database");
            return NotFound();
        }
        var ratingsDto = mapper.Map<List<RatingDto>>(ratings);
        logger.LogInformation("Retrieved {Count} ratings from the database", ratingsDto.Count);
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
        {
            logger.LogWarning("Invalid rating value {Stars} for MealId {MealId} by UserId {UserId}", 
                dto.Stars, dto.MealId, dto.UserId);
            throw new BadHttpRequestException("Stars must be in between 1 and 5");
        }

        var meal = await mealRepository.GetMealByIdAsync(dto.MealId);
        if (meal == null)
        {
            logger.LogWarning("Meal with Id {MealId} not found when UserId {UserId} attempted rating", 
                dto.MealId, dto.UserId);
            throw new BadHttpRequestException("The selected meal is not available");
        }

        if (meal.Date.Date != DateTime.UtcNow.Date)
        {
            logger.LogWarning("You can only rate meals that are available for today");
            throw new BadHttpRequestException("You can only rate meals that are available for today");
        }

        var existingRating = await ratingRepository
            .GetExistingRatingForUserAndMealOptionAsync(dto.UserId, meal.MealOptionId);

        if (existingRating != null)
        {
            logger.LogWarning("You have already rated this meal option today.");
            throw new BadHttpRequestException("You have already rated this meal option today.");
        }

        return meal;
    }        
    
}