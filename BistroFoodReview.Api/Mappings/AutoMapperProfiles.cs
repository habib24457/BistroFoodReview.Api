using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;

namespace BistroFoodReview.Api.Mappings;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Meal, MealDto>()
            .ForMember(dest => dest.MealOptionId, opt => opt.MapFrom(src => src.MealOptionId))
            .ForMember(dest => dest.EditedMealName, opt => opt.MapFrom(src => src.EditedMealName))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.MealOptionName, opt => opt.MapFrom(src => src.MealOption.Name));

        CreateMap<Meal, TopMealDto>()
            .ForMember(dest => dest.MealOptionName,
                opt => opt.MapFrom(src => src.MealOption.Name))
            .ForMember(dest => dest.AverageRating,
                opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Stars) : 0));
        
        CreateMap<Meal, DailyMealMenuDto>()
            .ForMember(dest => dest.MealOptionName,
                opt => opt.MapFrom(src => src.MealOption.Name))
            .ForMember(dest => dest.AverageRating,
                opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Stars) : 0))
            .ForMember(dest => dest.EditedMealName,
                opt => opt.MapFrom(src => src.EditedMealName));
        
        CreateMap<UpdateMealNameDto, Meal>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MealId)) 
            .ForMember(dest => dest.MealOptionId, opt => opt.MapFrom(src => src.MealOptionId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.MealDate))
            .ForMember(dest => dest.EditedMealName, opt => opt.MapFrom(src => src.EditedMealName));

        CreateMap<Meal, DailyMealMenuDto>()
            .ForMember(dest => dest.MealOptionName, opt => opt.MapFrom(src => src.MealOption.Name));
        
        CreateMap<MealOption, MealOptionDto>();

        CreateMap<User, UserDto>();
        CreateMap<User, UserWithRatingsDto>();

        CreateMap<Rating, RatingDto>()
            .ForMember(dest => dest.MealDate,
                opt => opt.MapFrom(src => src.Meal.Date))
            .ForMember(dest => dest.MealOptionName,
                opt => opt.MapFrom(src => src.Meal.MealOption.Name))
            .ForMember(dest => dest.EditedMealName,
                opt => opt.MapFrom(src => src.Meal.EditedMealName));

        CreateMap<CreateRatingDto, Rating>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}