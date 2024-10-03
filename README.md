# ETIDrive

ETIDrive is a Document Management System (DMS) built using .NET technologies. It provides a robust solution for managing and organizing documents in a secure and efficient manner. With features like role-based access control, document versioning, and multi-storage support, it is tailored for enterprise-level document handling.

## Features

- **Document Storage & Organization**: Upload, categorize, and manage documents within a structured hierarchy.
- **Role-Based Access Control (RBAC)**: Secure access to documents based on user roles.
- **User Management**: Admin functionalities to create, manage, and assign roles to users.
- **Active Directory Integration**: Supports Active Directory (AD) for user authentication and role management in enterprise environments.

## Technology Stack

- **Backend**: ASP.NET Core, C#
- **Frontend**: HTML, CSS, JavaScript
- **Database**: SQL Server
- **Authentication**: Identity Framework, The Lightweight Directory Access Protocol (LDAP)
- **Testing**: xUnit Framework

## Screenshots

![Ekran görüntüsü 2024-10-03 224828](https://github.com/user-attachments/assets/b02dc325-b37b-484c-8766-218cc77e787a)
*Figure 1: The login page allows users to securely enter their credentials.*

![Ekran görüntüsü 2024-10-03 225823](https://github.com/user-attachments/assets/637f434c-358f-439c-bd8f-16bdda76eab5)
*Figure 2: Displays the user's main folders, enabling navigation and file management.*

![Ekran görüntüsü 2024-10-03 225859](https://github.com/user-attachments/assets/436eb306-be2f-43ae-ac45-6d43d0d32fd1)
*Figure 3: Shows how users can create subfolders and navigate using the breadcrumb bar.*

![Ekran görüntüsü 2024-10-03 225919](https://github.com/user-attachments/assets/8af9d284-3ca2-4e4a-80da-88a32178b1ad)
*Figure 4: Custom context menu that appears on right-click, providing options to create new folders.*

![Ekran görüntüsü 2024-10-03 225948](https://github.com/user-attachments/assets/6ae42d31-a868-46e8-acc5-0558306eef1c)
*Figure 5: New folder creation screen where users can specify the folder name and its location.*

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server or compatible database
- Visual Studio or Visual Studio Code

### Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/unsulliedd/.NET-ETIDrive.git

2. **Build the Solution**:
   Open the solution file in Visual Studio and build the project to restore dependencies.

4. **Database Setup**: Update the appsettings.json file with your SQL Server connection string and create the initial database schema using Entity Framework migrations:
   ```bash
   dotnet ef database update
5. **Run the Application**: Start the project from Visual Studio or using the .NET CLI:
   ```bash
   dotnet run
6. **Access the Web UI**: Navigate to http://localhost in your browser to access the document management interface.

## Project Structure

- ETIDrive_Data: Handles database context and Entity Framework operations.
- ETIDrive_Entity: Defines the core data models and entities.
- ETIDrive_WebUI: The main web user interface for document management.
- ETIDrive_xUnitTest: Unit testing for various components.
