using BistroFoodReview.Api.Models.Domain;

namespace BistroFoodReview.Api.Models.Dto;

public class TopMealDto
{
    public DateTime Date { get; set; }
    public string MealOptionName { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public string EditedMealName { get; set; }
}