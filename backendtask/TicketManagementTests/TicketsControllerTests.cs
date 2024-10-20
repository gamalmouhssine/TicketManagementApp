using CoreTicket.Entities;
using CoreTicket.Enums;
using CoreTicket.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketApp.Controllers;

namespace TicketManagementTests
{
    public class TicketsControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepo;
        private readonly TicketsController _controller;

        public TicketsControllerTests()
        {
            _mockRepo = new Mock<ITicketRepository>();
            _controller = new TicketsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetTickets_ReturnsOkResult_WithTicketList()
        {
            var mockTickets = new List<Ticket>
            {
                new Ticket {  Description = "Test Ticket 1", Status = TicketStatus.open },
                new Ticket {  Description = "Test Ticket 2", Status = TicketStatus.closed }
            };

            _mockRepo.Setup(repo => repo.GetTickets(null, "date_desc", 1, 10))
                .ReturnsAsync(mockTickets);

            var result = await _controller.GetTickets();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            var mockTicket = new Ticket { TicketId = 1, Description = "Test Ticket 1", Status = TicketStatus.open };

            _mockRepo.Setup(repo => repo.GetTicket(1)).ReturnsAsync(mockTicket);

            var result = await _controller.GetTicketById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(1, returnValue.TicketId);
        }
        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetTicket(1)).ReturnsAsync((Ticket)null);

            var result = await _controller.GetTicketById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddTicket_ReturnsCreatedAtActionResult()
        {
            var newTicket = new Ticket { TicketId = 3, Description = "New Ticket", Status = TicketStatus.open };

            var result = await _controller.AddTicket(newTicket);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetTicketById", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsNoContent_WhenValidUpdate()
        {
            var updatedTicket = new Ticket { TicketId = 1, Description = "Updated Ticket", Status = TicketStatus.open };

            _mockRepo.Setup(repo => repo.UpdateTicket(updatedTicket)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateTicket(1, updatedTicket);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsBadRequest_WhenIdMismatch()
        {
            var updatedTicket = new Ticket { TicketId = 1, Description = "Updated Ticket", Status = TicketStatus.open };

            var result = await _controller.UpdateTicket(2, updatedTicket); // Mismatch in ID

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsNoContent_WhenValidDeletion()
        {
            _mockRepo.Setup(repo => repo.DeleteTicket(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTicket(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
