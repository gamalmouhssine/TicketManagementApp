# Ticket Management Web Application

### Overview 
This is a web application designed to manage tickets with basic CRUD (Create, Read, Update, Delete) functionality. It is developed using .NET 8 for the backend and a TypeScript-based frontend framework (React). The system allows users to create, view, update, and delete tickets.

### Features
- Create, Read, Update, Delete (CRUD) operations for managing tickets.
- Pagination for efficient ticket listing.
- Responsive design using modern UI libraries Bootstrap.
- Client-side form validation to ensure valid ticket entries.
- Sorting and filtering .
- Unit tests to ensure application reliability.

### Key Details for Each Ticket
- Ticket ID: Unique identifier for each ticket.
- Description: Summary of the ticket issue.
- Status: Indicates whether the ticket is "Open" or "Closed."
- Date: Date when the ticket was created.

## Technologies Used
### Backend
- .NET 8: Used to build a RESTful API for ticket management.
- Entity Framework: To handle database interactions.
- SQL Server: For storing ticket data.
### Frontend
- React: To create a responsive and interactive user interface.
- TypeScript: For strict typing and better maintainability
### UI Design
- Bootstrap: For modern and visually appealing design.

### Requirements
- .NET 8 SDK
- Node.js (for frontend development) React 
- A database SQL Server
- Entity Framework Core

## Setup Instructions

### Clone the repository:
```bash
git clone https://github.com/gamalmouhssine/TicketManagementApp.git
```

### Frontend Setup
- Navigate to the frontend folder:

```bash
cd frontend/tickets-management
```
- Install dependencies:

```bash
npm install
```
- Start the development server:
```bash
npm start
```
### Backend Setup
- Open the TicketManagementTests.csproj in your project.
- Navigate to the `appsettings.json` file.
- Add your database connection configuration in the `ConnectionStrings` section.
- Save the `appsettings.json` file after updating the connection string.
- Run the following command to apply migrations and create the database:
```bash
dotnet ef migrations add InitialCreate
```
- Update the database using the following command:
```bash
dotnet ef database update
```



