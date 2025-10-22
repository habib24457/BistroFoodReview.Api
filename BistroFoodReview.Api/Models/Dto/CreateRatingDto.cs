namespace BistroFoodReview.Api.Models.Dto;

public class CreateRatingDto
{
    public Guid UserId { get; set; }
    public Guid MealId { get; set; }
    public double Stars { get; set; }
}