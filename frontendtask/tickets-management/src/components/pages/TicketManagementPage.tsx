import React from 'react';
import TicketTable from '../TicketsTable';

const TicketManagementPage: React.FC = () => {
    return (
        <div className="container mt-5">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h1>Ticket Management</h1>
            </div>
            <TicketTable />
        </div>
    );
};

export default TicketManagementPage;
