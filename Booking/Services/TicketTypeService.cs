using Booking.Models;
using Booking.Dto;
using Booking.Dtos;
using Booking.Repositories;
using Booking.Exceptions;

namespace Booking.Services
{
    public interface ITicketTypeService
    {
        List<TicketType> GetAll(short activityID, short companyID);
        void Add(AddTicketTypeRequestDto ticketType, short companyID);
        void Delete(int id, short companyID);
        void Update(UpdateTicketTypeRequestDto ticketType, short companyID);


    }
    public class TicketTypeService : ITicketTypeService
    {
        private readonly TicketTypeRepository _ticketTypeRepository;
        public TicketTypeService(TicketTypeRepository ticketTypeRepository)
        {
            _ticketTypeRepository = ticketTypeRepository;
        }
        /// <summary>
        /// 新增票種，同一活動下票種名稱不可相同
        /// </summary>
        /// <param name="ticketType"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add(AddTicketTypeRequestDto ticketType, short companyID)
        {
            if (!_ticketTypeRepository.ActivityPermission(ticketType.Activity, companyID))
            {
                throw new TicketTypeSettingException("活動沒有權限");
            }

            if (_ticketTypeRepository.IsRepeated(ticketType.Name, ticketType.Activity))
            {
                throw new TicketTypeSettingException("票種命名重複");
            }

            _ticketTypeRepository.Add(ticketType);
        }

        public void Delete(int id, short companyID)
        {
            if (!_ticketTypeRepository.TicketTypePermission(id, companyID))
            {
                throw new TicketTypeSettingException("沒有權限");
            }
            if (_ticketTypeRepository.IsSold(id))
            {
                throw new TicketTypeSettingException("已售出不可任意刪除");
            }
            _ticketTypeRepository.Delete(id);
        }

        public List<TicketType> GetAll(short activityID, short companyID)
        {
            if (!_ticketTypeRepository.ActivityPermission(activityID, companyID))
            {
                throw new TicketTypeSettingException("沒有權限");
            }
            return _ticketTypeRepository.GetAllByActivityID(activityID);
        }

        public void Update(UpdateTicketTypeRequestDto ticketType, short companyID)
        {
            if (!_ticketTypeRepository.TicketTypePermission(ticketType.Id, companyID))
            {
                throw new TicketTypeSettingException("沒有權限");
            }
            if (_ticketTypeRepository.IsSold(ticketType.Id))
            {
                throw new TicketTypeSettingException("已售出不可任意更改");
            }
            _ticketTypeRepository.Update(ticketType);
        }
    }
}
