using CoreTicket.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTicket.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public required string Description { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
