namespace BistroFoodReview.Api.Models.Domain;

public class Rating
{
    public Guid Id { get; set; }
    public Guid MealId { get; set; }
    public Meal Meal { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public int Stars { get; set; }
}