export enum TicketStatus {
    Open = 1,
    Closed = 2
}

export interface Ticket {
    ticketId: number;
    description: string;
    status: TicketStatus;
    createdDate: string;
}
