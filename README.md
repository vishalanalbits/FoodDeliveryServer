# FoodDeliveryServer
# API Development with .NET, Entity Framework, and PostgreSQL

This project is a demonstration of building an API using **.NET 6**, **Entity Framework**, and **PostgreSQL 16**. Additionally, it integrates **PostGIS 3.5** to provide geospatial capabilities.

---

## Features
- **Entity Framework Core** for database interaction
- PostgreSQL 16 as the database
- PostGIS 3.5 for spatial data support
- Authentication and Authorization using **JWT (JSON Web Tokens)**
- Password hashing with **BCrypt.Net-Next**
- Request validation using **FluentValidation**
- Object mapping using **AutoMapper**
- Cross-Origin Resource Sharing (**CORS**) setup
- Image storage using **Cloudinary**
- RESTful API design
- Scalable and modular architecture(N-tier)

---

## Prerequisites
Before you begin, ensure you have the following installed:
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [PostgreSQL 16](https://www.postgresql.org/download/)
- [PostGIS 3.5](https://postgis.net/install/) or (https://gis.stackexchange.com/questions/41060/installing-postgis-on-windows)
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Cloudinary Account](https://cloudinary.com/)

---

## Setting Up the Environment

### 1. Install PostgreSQL 16 and PostGIS 3.5
1. Download and install PostgreSQL 16.
2. During the installation, enable the PostGIS bundle(https://gis.stackexchange.com/questions/41060/installing-postgis-on-windows) or manually install it later:
   ``bash
   CREATE EXTENSION postgis;


### 2. Clone the Repository
    ``bash
    git clone <repository-url>
    cd <project-folder>


### 3. Update the Connection String
Modify the `appsettings.json` file with your PostgreSQL connection details:

    {
      "ConnectionStrings": {
        "FoodDeliveryDbConnectionString": "Host=localhost;Port=5432;Database=your_database;Username=your_user;Password=your_password"
      }
    },
    "ClientSettings": {
    "ClientDomain": "http://localhost:7269"
    },
    "JWTSettings": {
    "SecretKey": "your_secret_key",
    "ValidIssuer": "your_secret_key"
    },
    "CloudinarySettings": {
    "CloudinaryUrl": "cloudinary://<API_Key>:<API_Secret>@<Cloud_Name>"
    }


### 4. Apply Database Migrations
This project uses `Entity Framework Core` for data access. Follow the steps below to add migrations or update the database schema.

#### 1. Add Migrations
    ``bash
    dotnet ef migrations add <MigrationName>

#### 2. Update Database
    ``bash
    dotnet ef database update


### 5. Run the Application
Launch the application:
    ``bash
    dotnet run


### Notes
- Replace <repository-url> and <project-folder> with actual repository details.
- Adjust `appsettings.json` for database, JWT, and Cloudinary configuration.
- Update the allowed origins in the CORS configuration to match your frontend
- Ensure validators and mappings are updated whenever new models or DTOs are added.
- Use hashed passwords for all user accounts.
- Consider adding a LICENSE file to complement the license section.