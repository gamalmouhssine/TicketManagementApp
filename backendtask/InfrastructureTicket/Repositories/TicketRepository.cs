using CoreTicket.Entities;
using CoreTicket.Enums;
using CoreTicket.Repositories;
using InfrastructureTicket.Data;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureTicket.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<Ticket?> FindTicket(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }
        public async Task<IEnumerable<Ticket>> GetTickets(TicketStatus? status = null, string sortOrder = "date_desc", int page = 1, int pageSize = 10)
        {
            var query = _context.Tickets.AsQueryable();

            // Filter by status if provided
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            // Sort the tickets based on the specified sort order
            switch (sortOrder.ToLower())
            {
                case "date_desc":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "date_asc":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(x => x.TicketId);
                    break;
                case "id_asc":
                    query = query.OrderBy(x => x.TicketId);
                    break;
                default:
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Adjust pageSize if total records are less than the current pageSize
            if (totalCount < pageSize)
            {
                pageSize = totalCount; // Set pageSize to totalCount if it's less
            }

            // Calculate the number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Adjust the page number if it exceeds the total number of pages
            if (page > totalPages)
            {
                page = totalPages;
            }

            // Fetch the records for the current page
            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        public async Task<Ticket> GetTicket(int id)
        {
            Ticket? ticket = await FindTicket(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            return ticket;
        }

        public async Task AddTicket(Ticket ticket)
        {
            var result = _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicket(int id)
        {
            var ticket = await FindTicket(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Ticket not found");
            }
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            var result = _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
