namespace BistroFoodReview.Api.Models.Dto;

public class CreateRatingDto
{
    public Guid UserId { get; set; }
    public Guid MealId { get; set; }
    public int Stars { get; set; }
}