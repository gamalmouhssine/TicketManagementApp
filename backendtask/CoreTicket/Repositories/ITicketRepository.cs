using CoreTicket.Entities;
using CoreTicket.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTicket.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTickets(TicketStatus? status=null,string sortOrder= "date_desc", int page=1,int pageSize=10);
        Task<Ticket> GetTicket(int id);
        Task AddTicket(Ticket ticket);
        Task UpdateTicket(Ticket ticket);
        Task DeleteTicket(int id);

    }
}
