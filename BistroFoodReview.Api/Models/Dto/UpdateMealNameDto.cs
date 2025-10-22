namespace BistroFoodReview.Api.Models.Dto;

public class UpdateMealNameDto
{
    public Guid MealId { get; set; }
    public Guid MealOptionId { get; set; } 
    public DateTime MealDate { get; set; }   
    public string EditedMealName { get; set; } = string.Empty;
}