using AutoMapper;
using StockAppAPI.DTO;
using StockAppAPI.Models;

namespace StockAppAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Stock, StockDetailDTO>()
                .ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(src => src.StockSymbol))
                .ForMember(dest => dest.StockHistory, opt => opt.MapFrom(src => src.HistoricalData));

            CreateMap<Stock, OverviewDataDTO>()
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src =>
                    src.HistoricalData != null && src.HistoricalData.Any() ?
                    src.HistoricalData.Last().CurrentPrice : 0))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                    src.HistoricalData != null && src.HistoricalData.Any() ?
                    src.HistoricalData.Last().Date : string.Empty));

            CreateMap<UpdateStockDTO, HistoricalData>().ReverseMap();

            CreateMap<HistoricalDataDTO, HistoricalData>().ReverseMap();

            CreateMap<User, UserOverviewDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.Admin));

            CreateMap<User, UserDetailDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.Admin));

            
            CreateMap<UserDetailDTO, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.IsAdmin));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            
            CreateMap<RegisterUserDTO, User>().ReverseMap();
            CreateMap<LoginUserDTO, User>().ReverseMap();
        }
    }
}
