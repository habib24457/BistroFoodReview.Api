namespace BistroFoodReview.Api.Models.Dto;

public class MealDto
{
    public Guid Id { get; set; } 
    public Guid MealOptionId { get; set; }
    public string EditedMealName { get; set; }
    public DateTime Date { get; set; }
    public string MealOptionName { get; set; }  
}