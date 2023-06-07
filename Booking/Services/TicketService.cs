using Booking.Models;
using Booking.Repositories;

namespace Booking.Services
{
    public interface ITicketService
    {
        public List<Ticket> GetByMemberID(short memberID);
    };
    public class TicketService : ITicketService
    {
        private readonly TicketRepository _ticketRepository;
        public TicketService(
            TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public List<Ticket> GetByMemberID(short memberID)
        {
            return _ticketRepository.GetTicketsByMemberID(memberID);
        }
    }

}
