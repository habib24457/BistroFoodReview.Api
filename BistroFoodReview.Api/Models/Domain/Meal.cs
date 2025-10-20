namespace BistroFoodReview.Api.Models.Domain;

public class Meal
{
    public Guid Id  { get; set; }
    public DateTime Date { get; set; }
    public Guid MealOptionId { get; set; }
    public MealOption MealOption { get; set; }
    public string? EditedMealName { get; set; }
    public ICollection<Rating> Ratings { get; set; }
}