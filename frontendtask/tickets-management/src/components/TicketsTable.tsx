import React, { useEffect, useState } from 'react';
import { Ticket, TicketStatus } from '../models/Ticket';
import TicketService from '../Services/TicketService';
import { Form, Pagination, Button, Modal } from 'react-bootstrap';
import { format } from 'date-fns';



const TicketTable: React.FC = () => {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [statusFilter, setStatusFilter] = useState<TicketStatus | undefined>(undefined);
    const [searchId, setSearchId] = useState<string>('');
    const [page, setPage] = useState<number>(1);
    const [pageSize] = useState<number>(10);
    const [totalPages, setTotalPages] = useState<number>(1);
    const [sortOrder, setSortOrder] = useState<string>('date_desc');  

    
    const [showModal, setShowModal] = useState<boolean>(false);
    const [isEditMode, setIsEditMode] = useState<boolean>(false);
    const [currentTicket, setCurrentTicket] = useState<Partial<Ticket>>({
        description: '',
        status: TicketStatus.Open,
    });

    useEffect(() => {
        fetchTickets();
    }, [page, statusFilter, sortOrder]); 

    const fetchTickets = async () => {
        try {
            setLoading(true);
            const response = await TicketService.getTickets(statusFilter, sortOrder, page, pageSize);
            setTickets(response.data);
         
            setTotalPages(Math.ceil(100 / pageSize)); 
        } catch (error) {
            console.error('Failed to fetch tickets', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSearchById = async () => {
        if (searchId.trim() === '') {
            alert('Please enter a Ticket ID.');
            return;
        }

        try {
            setLoading(true);
            const response = await TicketService.getTicketById(parseInt(searchId, 10));
            setTickets(response.data ? [response.data] : []);
            setTotalPages(1);  
        } catch (error) {
            console.error('Ticket not found', error);
            setTickets([]);
        } finally {
            setLoading(false);
        }
    };

    const handleStatusFilterChange = (e: React.ChangeEvent<any>) => {
        const value = (e.target as HTMLSelectElement).value;
        setStatusFilter(value === '' ? undefined : parseInt(value) as TicketStatus);
        setPage(1);  // Reset to first page when filtering
    };

    const handleSortChange = (e: React.ChangeEvent<any>) => {
        setSortOrder(e.target.value);
        setPage(1);  // Reset to first page when sorting
    };

    const handlePageChange = (newPage: number) => {
        setPage(newPage);
    };

    const handleDelete = async (ticketID: number) => {
        if (window.confirm('Are you sure you want to delete this ticket?')) {
            try {
                await TicketService.deleteTicket(ticketID);
                fetchTickets();  // Refresh ticket list
            } catch (error) {
                console.error('Failed to delete ticket', error);
            }
        }
    };

    const openAddModal = () => {
        setCurrentTicket({ description: '', status: TicketStatus.Open });
        setIsEditMode(false);
        setShowModal(true);
    };

    const openEditModal = (ticket: Ticket) => {
        setCurrentTicket(ticket);
        setIsEditMode(true);
        setShowModal(true);
    };

    const handleModalClose = () => {
        setShowModal(false);
        setCurrentTicket({ description: '', status: TicketStatus.Open });
    };

    const handleModalSave = async () => {
        try {
            if (isEditMode && currentTicket.ticketId) {
                await TicketService.updateTicket(currentTicket as Ticket);
            } else {
                await TicketService.createTicket(currentTicket as Ticket);
            }
            fetchTickets();
            setShowModal(false);
        } catch (error) {
            console.error('Failed to save ticket', error);
        }
    };

    const handleInputChange = (e: React.ChangeEvent<any>) => {
        const { name, value } = e.target;
        setCurrentTicket((prev) => ({
            ...prev,
            [name]: name === 'status' ? parseInt(value) : value,
        }));
    };

    if (loading) {
        return <p>Loading...</p>;
    }

    return (
        <div>
            <div className="d-flex justify-content-between mb-3">
                <Form.Group controlId="statusFilter">
                    <Form.Label>Status Filter</Form.Label>
                    <Form.Control as="select" onChange={handleStatusFilterChange} value={statusFilter || ''}>
                        <option value="">All</option>
                        <option value={TicketStatus.Open}>Open</option>
                        <option value={TicketStatus.Closed}>Closed</option>
                    </Form.Control>
                </Form.Group>

                <Form.Group controlId="sortOrder" className="ml-3">
                    <Form.Label>Sort By</Form.Label>
                    <Form.Control as="select" onChange={handleSortChange} value={sortOrder}>
                        <option value="date_desc">Date Descending</option>
                        <option value="date_asc">Date Ascending</option>
                        <option value="id_desc">ID Descending</option>
                        <option value="id_asc">ID Ascending</option>
                    </Form.Control>
                </Form.Group>

                <Form.Group controlId="searchId" className="ml-3">
                    <Form.Label>Search by Ticket ID</Form.Label>
                    <div className="d-flex">
                        <Form.Control
                            type="text"
                            placeholder="Enter Ticket ID"
                            value={searchId}
                            onChange={(e) => setSearchId(e.target.value)}
                        />
                        <Button variant="primary" className="ml-2" onClick={handleSearchById}>Search</Button>
                    </div>
                </Form.Group>

                <Button variant="success" onClick={openAddModal}>Add New Ticket</Button>
            </div>

            <div className="table-responsive">
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>Ticket ID</th>
                            <th>Description</th>
                            <th>Status</th>
                            <th>Date Created</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tickets.map((ticket) => (
                            <tr key={ticket.ticketId}>
                                <td>{ticket.ticketId}</td>
                                <td>{ticket.description}</td>
                                <td>{ticket.status === TicketStatus.Open ? 'Open' : 'Closed'}</td>
                                <td>{new Date(ticket.createdDate).toLocaleDateString()}</td>
                                <td>
                                    <Button className="btn btn-primary btn-sm" onClick={() => openEditModal(ticket)}>Edit</Button>
                                    <Button className="btn btn-danger btn-sm" onClick={() => handleDelete(ticket.ticketId)}>Delete</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <div className="d-flex justify-content-center">
                <Pagination>
                    {[...Array(totalPages)].map((_, i) => (
                        <Pagination.Item
                            key={i}
                            active={i + 1 === page}
                            onClick={() => handlePageChange(i + 1)}
                        >
                            {i + 1}
                        </Pagination.Item>
                    ))}
                </Pagination>
            </div>

            <Modal show={showModal} onHide={handleModalClose}>
                <Modal.Header closeButton>
                    <Modal.Title>{isEditMode ? 'Edit Ticket' : 'Add New Ticket'}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Group>
                        <Form.Label>Description</Form.Label>
                        <Form.Control
                            type="text"
                            name="description"
                            value={currentTicket.description || ''}
                            onChange={handleInputChange}
                            required  
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Status</Form.Label>
                        <Form.Control
                            as="select"
                            name="status"
                            value={currentTicket.status}
                            onChange={handleInputChange}
                            required  
                        >
                            <option value={TicketStatus.Open}>Open</option>
                            <option value={TicketStatus.Closed}>Closed</option>
                        </Form.Control>
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleModalClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleModalSave} disabled={!currentTicket.description || !currentTicket.status}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default TicketTable;
