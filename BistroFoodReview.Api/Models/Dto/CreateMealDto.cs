namespace BistroFoodReview.Api.Models.Dto;

public class CreateMealDto
{
    public Guid MealOptionId { get; set; }
    public DateTime Date { get; set; }
    public string EditedMealName { get; set; } = string.Empty; 
}