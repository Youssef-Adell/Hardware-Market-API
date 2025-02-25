<h1 align="center">Hardware Market: E-Commerce (API)</h1>
<p align="center">
  <a href="https://bidx.runasp.net/swagger/index.html">Swagger Documentation</a>
</p>

## 📖 Overview  
**Hardware Market** is a RESTful API for a tech-focused e-commerce platform, offering features for user authentication, product and order management, payment processing, and customer reviews. It supports both customer and admin roles, ensuring a secure and efficient shopping experience.

## ✨ Key Features  
- **Product Management**: Admins can create, update, and delete products; customers can search, filter, and review products.  
- **Order Management**: Customers can place and track orders; admins can manage and update order statuses.  
- **Payment Integration**: Secure payment processing for seamless checkout experiences.  
- **Product Reviews**: Customers can leave reviews and ratings for purchased products.  
- **Coupons**: Admins can create and manage discount coupons; customers can validate and apply them.
- **Authentication**: Registration, login, email confirmation, password recovery, and token refresh.
- **Pagination & Sorting**: Supports pagination and sorting for products, orders, and reviews. 

## ⚙️ Tech Stack  
- [ASP.NET Core 9](https://dotnet.microsoft.com/en-us/apps/aspnet/) - A free, cross-platform and open-source web-development framework.
- [Entity Framework Core 9](https://learn.microsoft.com/en-us/ef/core/) - An open source object–relational mapping framework.
- [Microsoft SQL Server](https://hub.docker.com/_/microsoft-mssql-server) - A relational database management system.
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) - A membership system for managing users, authentication, and authorization in ASP.NET Core applications.
- [JWT](https://jwt.io/) - A secure, compact token format used for transmitting information between parties as a JSON object.
- [Serilog](https://serilog.net/) - A logging library that allows logging to various outputs like files, console, etc.
- [Docker](https://www.docker.com/) - A containerization platform for packaging applications and their dependencies to ensure consistency across different environments.

## 🔗 Third-Party Services
- [Stripe](https://stripe.com/) - A payment processing platform.
- [Brevo](https://www.brevo.com/) - An email sending service.  


## 🛠️ Setup & Run 
### 1. Prerequisites  
- Install [Docker](https://www.docker.com/) and [Docker Compose](https://docs.docker.com/compose/install/).  
- Clone the repository:  
```bash  
git clone https://github.com/Youssef-Adell/Hardware-Market-API.git
cd Hardware-Market-API
```
### 2. Configure Environment Variables  
- Rename the example files:  
  - `webapi.env.example` → `webapi.env`  
  - `sqlserver.env.example` → `sqlserver.env`  
- Update the `.env` files with your credentials:  
  - `webapi.env`: Add database connection strings, JWT secret, and third-party API keys.  
  - `sqlserver.env`: Set the SQL Server admin password.  

### 3. Start the Application  
Run the following command to build and start the containers:  
```bash  
docker-compose up --build  
```

- The API will be available at http://localhost:5000.
- SQL Server will be accessible at localhost:1433.

> [!NOTE]
> Database, logs, and DataProtection keys are stored in Docker volumes (sqlserver-data, webapi-logs, dataprotection-keys) to ensure data persistence and consistency across container restarts or rebuilds.