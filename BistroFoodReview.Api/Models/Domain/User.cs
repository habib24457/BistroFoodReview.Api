namespace BistroFoodReview.Api.Models.Domain;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Rating> Ratings { get; set; }
}