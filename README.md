# Out of Office Application

This project is an application designed to manage employees, projects, leave requests, and approval requests within an organization. It provides role-based access control for different users: Employee, HR Manager, Project Manager, and Administrator.

## Features

### Employee Management
- **View Employees**: List all employees with their details including full name, subdivision, position, status, and more.
- **Add Employee**: Ability to add new employees with necessary details.
- **Edit Employee**: Modify existing employee details.
- **Delete Employee**: Remove employees from the system.
- **Activate/Deactivate Employee**: Change the status of an employee between Active and Inactive.

### Project Management
- **View Projects**: List all projects with details such as project type, start date, end date, project manager, and status.
- **Add Project**: Add new projects with relevant information.
- **Edit Project**: Modify project details including its manager and status.
- **Delete Project**: Remove projects from the system.

### Leave Request Management
- **View Leave Requests**: Display all leave requests showing employee, absence reason, start date, end date, status, and comments.
- **Add Leave Request**: Submit new leave requests specifying the reason, duration, and additional notes.
- **Edit Leave Request**: Modify leave request details such as dates and comments.
- **Cancel Leave Request**: Withdraw submitted leave requests.

### Approval Request Management
- **View Approval Requests**: List all pending approval requests showing requester, type (leave or project), status, and comments.
- **Approve/Reject Requests**: Allow HR Managers and Project Managers to approve or reject pending requests.
- **Add Comments**: Include comments while approving or rejecting requests.

### Role-Based Access Control
- **Employee Role**: Can create and manage leave requests.
- **HR Manager Role**: Manages employees, approves/rejects leave requests.
- **Project Manager Role**: Manages projects, approves/rejects project-related requests.
- **Administrator Role**: Grants permissions, manages all application data and roles.

## Endpoints

### Implemented Swagger Documentation with authorization
![Swagger Documentation](/pics/swagger-documentation.png)

### Employee
![Employee](/pics/employee.png)

### Project
![Project](/pics/project.png)

### Leave Request
![Leave Request](/pics/leave-request.png)

### Approval Request
![Approval Request](/pics/approval-request.png)

## Databases

### Out of office Api data
![Approval Requests](/pics/api-data.png)

### Identity Server data
![Approval Requests](/pics/identity-server-data.png)

## Technologies Used

- ASP.NET Core Web API for backend services.
- IdentityServer4 for authentication and authorization.
- Entity Framework Core for data access.
