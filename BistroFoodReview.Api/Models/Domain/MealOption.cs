namespace BistroFoodReview.Api.Models.Domain;

public class MealOption
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Meal> Meals { get; set; }
}