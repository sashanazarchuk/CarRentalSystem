# Car Rental System

**Car Rental System** is a car rental management system developed with **ASP.NET Core Web API**.

---

## Features
- Car management (add, edit, delete, change status)
- Car brand and model management
- Car booking by users
- JWT authentication and authorization (Admin / User)
- Background services for automatic car status updates
- Automatic creation of Admin account during first migration
- Unit tests for main logic

---

## Technologies Used
- [.NET 9 (ASP.NET Core Web API)]
- [PostgreSQL 15+]
- Entity Framework Core
- Clean Architecture (Domain, Application, Infrastructure, API)
- CQRS
- AutoMapper
- FluentValidation
- Unit Of Work
- xUnit
- FakeItEasy
- Docker

---


### Setup Locally
```bash
git clone https://github.com/sashanazarchuk/CarRentalSystem.git
cd CarRentalSystem/CarRentalSystem.API
dotnet run
```
API will be available at:
```bash
http://localhost:5075/swagger/index.html
```

Before running the backend locally, configure your PostgreSQL connection string.
All sensitive data (database, JWT, admin credentials) should be stored in Environment Variables.
Example appsettings.json structure for reference (values are left empty):

```json
{
  "ConnectionStrings": {
    "sqlConnection": " "
  },
  "JwtSettings": {
    "Issuer": " ",
    "Audience": " ",
    "Secret": " ",
    "AccessTokenExpirationMinutes": 5,
    "RefreshTokenExpirationDays": 7
  },
  "AdminSettings": {
    "AdminName": "",
    "AdminEmail": "",
    "AdminPassword": ""
  }
}
```
## Run Unit Tests
Unit tests are run locally and are not included in Docker containers.
```bash
cd CarRentalSystem
cd CarRentalSystem.Tests
dotnet test
```
## Run Application using Docker
Create an .env file in the root folder. Using the template below, fill in your own values:
```bash
# PostgreSQL
DB_SERVER=your_db_host
DB_PORT=5432
DB_USER=your_db_user
DB_PASSWORD=your_db_password
DB_NAME=your_db_name

# Admin account
ADMIN_NAME=your_admin_name
ADMIN_EMAIL=your_admin_email
ADMIN_PASSWORD=your_admin_password

# JWT settings
JWT_ISSUER=your_jwt_issuer
JWT_AUDIENCE=your_jwt_audience
JWT_SECRET=your_jwt_secret
```
Build and run Docker containers:
```bash
cd CarRentalSystem
docker-compose up --build
```
After running, the API will be available at:
```bash
http://localhost:8080/swagger/index.html
```
