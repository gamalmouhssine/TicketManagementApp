import axios from 'axios';
import { Ticket, TicketStatus } from '../models/Ticket';

const API_URL = 'https://localhost:44350/api/tickets';

class TicketService {
    getTickets(status?: TicketStatus, sortOrder = 'date_desc', page = 1, pageSize = 10) {
        let url = `${API_URL}?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
        if (status !== undefined) {
            url += `&status=${status}`;
        }
        return axios.get<Ticket[]>(url);
    }

    getTicketById(ticketID: number) {
        return axios.get<Ticket>(`${API_URL}/${ticketID}`);
    }

    createTicket(ticket: Ticket) {
        return axios.post(API_URL, ticket);
    }

    updateTicket(ticket: Ticket) {
        return axios.put(`${API_URL}/${ticket.ticketId}`, ticket);
    }

    deleteTicket(ticketID: number) {
        return axios.delete(`${API_URL}/${ticketID}`);
    }
}

export default new TicketService();
