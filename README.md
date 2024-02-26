# Project Title

Forum Application for a .NET Assingment.

## Description

This project is a comprehensive .NET Assignment that showcases the integration of Blazor for front-end web development, .NET Web API for backend services, and Entity Framework Core (EFC) for database operations.

## Getting Started

### Dependencies

- .NET 7.0 or later
- Rider 2023.3 or later (recommended for development)

### Installing

1. Clone the repository:
    ```bash
    git clone https://github.com/DefNotGroup5/dnp_assignment.git
    ```
2. Navigate to the project directory:
    ```bash
    cd dnp_assignment
    ```
3. Restore the .NET dependencies:
    ```bash
    dotnet restore
    ```
4. Apply the database migrations (ensure your database server is running):
    ```bash
    dotnet ef database update
    ```

### Running the application

1. To run the .NET Web API project:
    ```bash
    dotnet run --project DNP1_Assignment
    ```
2. To run the Blazor application:
    ```bash
    dotnet run --project BlazorPresentation
    ```
   
## Usage

At minimum, the requierements given for the assignment are:
- Register, log in, and out
- Create a new post (when logged in), containing a title and a body (create a new page for this)
- View all posts, i.e. just a Title (either use the front page or create a new page)
- Click on a post in the post overview, to go to a new page, where you can view the entire post.

There is also a secret somewhere in the application, therefore left for users to discover it.


## Acknowledgements

- [Blazor](https://blazor.net)
- [.NET Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
