import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import TicketManagementPage from './components/pages/TicketManagementPage';

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<TicketManagementPage />} />
            </Routes>
        </Router>
    );
};

export default App;
