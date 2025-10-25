namespace BistroFoodReview.Api.Models.Dto;

public class DailyMealMenuDto
{
    public Guid Id { get; set; }                   
    public DateTime Date { get; set; }    
    public Guid MealOptionId { get; set; }
    public string MealOptionName { get; set; } = string.Empty; 
    public string? EditedMealName { get; set; }    
    
    public List<MinimalisticRatingDto> Ratings { get; set; } = new();
}