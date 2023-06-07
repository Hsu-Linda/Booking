using AutoMapper;
using Booking.Dto;
using Booking.Dtos;
using Booking.Models;

namespace Booking
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Activity, ActivityInfoResponse>();
            CreateMap<Order, OrderDetailResponseDto>()
                .ForMember(x => x.Items, y => y.Ignore());
            CreateMap<UpdateActivityRequest, UpdateLimitActivityRequest>();
            CreateMap<AddCompanyRequestDto, Company>()
                .ForMember(x => x.Salt, y => y.Ignore())
                .ForMember(x => x.Password, y => y.Ignore());
            CreateMap<AddMemberRequestDto, Member>()
                .ForMember(x => x.Salt, y => y.Ignore())
                .ForMember(x => x.Password, y => y.Ignore());
            CreateMap<Activity, ActivityWithLikeDto>()
                .ForMember(x => x.Like, y => y.Ignore());
            CreateMap<Like, ActivityWithLikeDto>()
                .ForMember(x => x.Like, y => y.MapFrom(opt => (opt.LikeId != 0)));
        }
    }
}
