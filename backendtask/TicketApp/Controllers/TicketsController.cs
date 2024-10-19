using CoreTicket.Entities;
using CoreTicket.Enums;
using CoreTicket.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace TicketApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets([FromQuery] TicketStatus? status = null,
            string sortOrder = "date_desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var tickets = await _ticketRepository.GetTickets(status, sortOrder, page, pageSize);

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(int id)
        {
            var ticket = await _ticketRepository.GetTicket(id);

            if (ticket == null) return NotFound();

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult> AddTicket([FromBody] Ticket ticket)
        {
            await _ticketRepository.AddTicket(ticket);

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.TicketId }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.TicketId) return BadRequest();

            await _ticketRepository.UpdateTicket(ticket);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            await _ticketRepository.DeleteTicket(id);

            return NoContent();
        }

    }
}
