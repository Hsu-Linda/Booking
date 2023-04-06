using AutoMapper;
using Booking.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Booking.Services
{
    public class MemoryCacheService
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheService(
            BookingContext bookingContext,
            IMapper iMapper,
            IMemoryCache memoryCache
        ) 
        {
            _bookingContext = bookingContext;
            _iMapper = iMapper;
            _memoryCache = memoryCache;
        }


        public void getRemainAccount(int activityID)
        {

        }

        public void getFromDB(int activityID)
        {

        }
    }
}
