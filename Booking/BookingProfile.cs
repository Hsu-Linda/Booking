using AutoMapper;
using Booking.Dto;
using Booking.Dtos;
using Booking.Models;

namespace Booking
{
    public class BookingProfile:Profile
    {
        public BookingProfile()
        {
            CreateMap<AddCompanyRequestDto, Company>()
                .ForMember(x => x.Salt, y => y.Ignore())
                .ForMember(x => x.Password, y => y.Ignore());
            CreateMap<Company, QueryCompanyInfoResponseDto>();
            //CreateMap<AddActivityRequestDto, Activity>()
            //    .ForMember(x => x.Company, y => y.Ignore())
            //    .ForMember(x => x.Deleted, y => y.Ignore())
            //    .ForMember(x => x.LastModified, y => y.Ignore());
            //CreateMap<AddShowingRequestDto, Showing>();
            //CreateMap<ShowingRequestDto, Showing>()
            //    .ForMember(s => s.Activity, y => y.Ignore());
            CreateMap<AddMemberRequestDto, Member>()
                .ForMember(x => x.Salt, y => y.Ignore())
                .ForMember(x => x.Password, y => y.Ignore());
            CreateMap<Activity, ActivityWithLikeDto>()
                .ForMember(x => x.Like ,y =>　y.Ignore());
            CreateMap<Like, ActivityWithLikeDto>()
                .ForMember(x => x.Like, y => y.MapFrom(opt => (opt.LikeId!=0)));
            //CreateMap<AddTicketTypeRequestDto, TicketType>()
            //    .ForMember(x => x.NumOfRemaining, y => y.MapFrom(opt => opt.NumOfTotal))
            //    .ForMember(x => x.Description, y => y.Ignore());
            //CreateMap<UpdateTicketTypeRequestDto, TicketType>()
            //    .ForMember(x => x.NumOfRemaining, y => y.MapFrom(opt => opt.NumOfTotal))
            //    .ForMember(x => x.Description, y => y.Ignore())
            //    .ForMember(x => x.ActivityId, y => y.Ignore())
            //    .ForMember(x => x.ShowingId, y => y.Ignore());

            //CreateMap<TicketType, TicketTypeAccount>()
            //    .ForMember(x => x.TicketType, y => y.MapFrom(opt => opt.TicketTypeId));
        }
    }
}
