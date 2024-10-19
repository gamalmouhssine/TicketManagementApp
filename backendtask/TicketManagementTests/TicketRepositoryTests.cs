using CoreTicket.Entities;
using CoreTicket.Enums;
using InfrastructureTicket.Data;
using InfrastructureTicket.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementTests
{
    public class TicketRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            if (!context.Tickets.Any())
            {
                context.Tickets.AddRange(
                    new Ticket { Description = "Test Ticket 1", Status = TicketStatus.open, CreatedDate = DateTime.Now.AddDays(-5) },
                    new Ticket { Description = "Test Ticket 2", Status = TicketStatus.closed, CreatedDate = DateTime.Now.AddDays(-2) }
                );
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAllTickets_ReturnsAllTickets()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);

            var tickets = await repository.GetTickets();

            Assert.Equal(2, tickets.Count());
        }
        [Fact]
        public async Task GetTicketById_ReturnsTicketWithGivenId()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);

            var ticket = await repository.GetTicket(1);

            Assert.NotNull(ticket);
            Assert.Equal(1, ticket.TicketId);
            Assert.Equal("Test Ticket 1", ticket.Description);
        }
        [Fact]
        public async Task AddTicket_AddsTicketToDatabase()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);
            var newTicket = new Ticket { Description = "New Test Ticket", Status = TicketStatus.open, CreatedDate = DateTime.Now };

            await repository.AddTicket(newTicket);
            var tickets = await repository.GetTickets();

            Assert.Equal(3, tickets.Count());
            Assert.Contains(tickets, t => t.Description == "New Test Ticket");

        }
        [Fact]
        public async Task UpdateTicket_UpdatesTicketInDatabase()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);
            var existingTicket = await repository.GetTicket(1);
            existingTicket.Description = "Updated Ticket Description";

            await repository.UpdateTicket(existingTicket);
            var updatedTicket = await repository.GetTicket(1);

            Assert.Equal("Updated Ticket Description", updatedTicket.Description);
        }
        [Fact]
        public async Task DeleteTicket_DeletesTicketFromDatabase()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);

            await repository.DeleteTicket(1);
            var tickets = await repository.GetTickets();

            Assert.Single(tickets);
        }
        [Fact]
        public async Task GetTickets_ApplySortingAndFiltering()
        {
            var context = await GetInMemoryDbContext();
            var repository = new TicketRepository(context);

            var ticketsSortedByDateDesc = await repository.GetTickets(null, "date_desc");
            var ticketsSortedByIdAsc = await repository.GetTickets(null, "id_asc");

            Assert.Equal(2, ticketsSortedByDateDesc.First().TicketId);
            Assert.Equal(1, ticketsSortedByIdAsc.First().TicketId);
        }
    }
}
