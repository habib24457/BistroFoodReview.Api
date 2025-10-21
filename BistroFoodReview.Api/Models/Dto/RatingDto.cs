namespace BistroFoodReview.Api.Models.Dto;

public class RatingDto
{
    public Guid Id { get; set; }
    public Guid MealId { get; set; }         
    public DateTime MealDate { get; set; }  
    public string MealOptionName { get; set; } = string.Empty;
    public string? EditedMealName { get; set; }
    public int Stars { get; set; }  
}