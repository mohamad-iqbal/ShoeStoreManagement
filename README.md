# Shoe Store Management API

A RESTful Web API built with ASP.NET Core implementing Clean Architecture principles, Repository Pattern, Service Layer, Unit of Work, JWT Authentication, and Role-Based Authorization.

This project was built as a portfolio project to practice enterprise-level backend development using ASP.NET Core and SQL Server.

---

## Features

### Authentication

- JWT Authentication
- Login
- Password Hashing (BCrypt)
- Role-Based Authorization

### User Management

- Create User
- Get Users
- Update User
- Delete User

### Store Management

- Create Store
- Get Store
- Update Store
- Delete Store

### Customer Management

- Create Customer
- Get Customer
- Update Customer
- Delete Customer

### Product Management

- Create Product
- Product Variants
- Stock Management
- Update Product
- Delete Product

### Sales

- Create Order
- Automatic Stock Reduction
- Order Detail
- Order Validation

### Returns

- Return Order
- Partial Return
- Full Return
- Automatic Stock Restoration

### Inventory

- Inventory Transaction History
- Sale Transaction
- Return Transaction
- Stock Audit Trail

---

## Tech Stack

- ASP.NET Core
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- LINQ
- Swagger
- BCrypt

---

## Architecture

```
Controller
      │
      ▼
Service
      │
      ▼
Repository
      │
      ▼
Entity Framework Core
      │
      ▼
SQL Server
```

### Design Patterns

- Clean Architecture
- Repository Pattern
- Service Layer
- Unit of Work
- Dependency Injection
- DTO Pattern

---

## Project Structure

```
ShoeStoreManagement.API
│
├── Controllers
├── Middleware
├── Extensions
└── Program.cs

ShoeStoreManagement.Application
│
├── DTOs
├── Interfaces
├── Services 
└── Exceptions

ShoeStoreManagement.Domain
│
├── Entities
├── Enums
└── Interfaces

ShoeStoreManagement.Infrastructure
│
├── Data
├── Repositories
└── Migration
```

---

## API Features

### Authentication

- Login
- JWT Token

### Authorization

Admin

- Manage Stores
- Manage Users

Sales

- Create Orders
- Create Returns
- Access only resources belonging to their assigned Store

---

## Business Rules

### Order

- Order number must be unique.
- Order must contain at least one item.
- Quantity must be greater than zero.
- Stock must be sufficient.
- Stock is automatically reduced after a successful order.

### Return

- An order can only be returned once.
- Return can be partial or full.
- Returned quantity cannot exceed purchased quantity.
- Stock is automatically restored after a successful return.

### Product

- SKU must be unique.
- Product belongs to one Store.
- Product Variant belongs to one Product.

---

## Security

- JWT Authentication
- Role-Based Authorization
- Store-Level Authorization
- Password Hashing using BCrypt

---

## Refactoring

During API testing, several improvements were made to align the project with common ASP.NET Core best practices.

- Moved SaveChangesAsync responsibility to Unit of Work.
- Refactored Repository implementation.
- Improved Dependency Injection configuration.
- Improved Response DTO mapping.
- Simplified Role Validation.
- Improved API error handling.
- Refined business validation.

---

## Getting Started

### Clone Repository

```bash
git clone https://github.com/mohamad-iqbal/ShoeStoreManagement.git
```

### Restore Packages

```bash
dotnet restore
```

### Update Database

```bash
dotnet ef database update
```

### Run

```bash
dotnet run
```

Swagger

```
https://localhost:7256/swagger
```

---

## Future Improvements

- Refresh Token
- Pagination
- Filtering
- Search
- Unit Testing
- Docker

---

## Learning Objectives

This project was created to practice:

- ASP.NET Core Web API
- Entity Framework Core
- Clean Architecture
- Repository Pattern
- Service Layer
- Unit of Work
- JWT Authentication
- LINQ
- SQL Server
- REST API Design
- Business Logic Implementation

---

## Author
Mohamad Iqbal Ali Ramadhan

Backend Developer (ASP.NET Core)
