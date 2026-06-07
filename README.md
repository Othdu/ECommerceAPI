# 🛒 E-Commerce REST API

A production-ready RESTful API built with **ASP.NET Core 9** and **Entity Framework Core**, featuring JWT authentication, role-based authorization, and a full e-commerce flow.

🌐 **Live API:** https://ecommerceap.up.railway.app/swagger

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-purple)
![Docker](https://img.shields.io/badge/Docker-deployed-2496ED)
![Railway](https://img.shields.io/badge/Railway-live-success)
![JWT](https://img.shields.io/badge/Auth-JWT-orange)

---

## ✨ Features

- **JWT Authentication** — register, login, token-based auth with SHA-256 password hashing
- **Role-based Authorization** — Admin and User roles with protected endpoints
- **Product Management** — full CRUD with category support (Admin only)
- **Order System** — place orders with automatic stock management
- **Admin Dashboard** — real-time stats: total revenue, top products, recent orders
- **Input Validation** — Data Annotations on all DTOs
- **Error Handling** — global middleware returning clean JSON errors
- **Swagger UI** — interactive API documentation with Bearer token support
- **Docker Deployment** — containerized and deployed to Railway cloud

---

## 🏗️ Architecture

```
Controller → Service → Repository → Database
```

Clean separation of concerns:
- **Controllers** — handle HTTP only, zero business logic
- **Services** — business logic and DTO mapping
- **Repositories** — database access only
- **Middleware** — cross-cutting concerns (error handling)

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL (Railway) |
| Authentication | JWT Bearer Tokens |
| Mapping | AutoMapper |
| Documentation | Swagger / Swashbuckle |
| Deployment | Docker + Railway |
| Language | C# |

---

## 📦 Database Schema

```
Users ──────────────── Orders
         1               │
         │               │ has many
         │               ▼
     places         OrderItems ──── Products ──── Categories
```

---

## 🔑 API Endpoints

### Auth
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | /api/auth/register | Create account | Public |
| POST | /api/auth/login | Login + get token | Public |

### Categories
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | /api/categories | Get all categories | Public |
| POST | /api/categories | Create category | Admin |
| DELETE | /api/categories/{id} | Delete category | Admin |

### Products
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | /api/products | Get all products | Public |
| GET | /api/products/{id} | Get product by id | Public |
| POST | /api/products | Create product | Admin |
| PUT | /api/products/{id} | Update product | Admin |
| DELETE | /api/products/{id} | Delete product | Admin |

### Orders
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | /api/orders | Get my orders | User |
| GET | /api/orders/{id} | Get order by id | User |
| POST | /api/orders | Place new order | User |

### Dashboard
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | /api/dashboard | Admin stats | Admin |

---

## ⚙️ Setup & Run Locally

### Prerequisites
- .NET 9 SDK
- PostgreSQL
- Visual Studio 2022

### Steps

1. Clone the repo
```bash
git clone https://github.com/Othdu/ECommerceAPI.git
cd ECommerceAPI
```

2. Update connection string in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ECommerceDB;Username=postgres;Password=yourpassword"
}
```

3. Update JWT settings
```json
"JwtSettings": {
  "SecretKey": "YourSuperSecretKeyHere",
  "Issuer": "ECommerceAPI",
  "Audience": "ECommerceAPIUsers",
  "ExpiryHours": 1
}
```

4. Run migrations
```bash
dotnet ef database update
```

5. Run the project
```bash
dotnet run
```

6. Open Swagger at `http://localhost:5000/swagger`

---

## 🔐 Authentication Flow

1. Register → `POST /api/auth/register`
2. Login → `POST /api/auth/login` → copy the token
3. Click **Authorize** in Swagger → enter `Bearer {token}`
4. All protected endpoints are now accessible

### Make yourself Admin
After registering, update your role in the database:
```sql
UPDATE "Users" SET "Role" = 'Admin' WHERE "Email" = 'your@email.com';
```

---

## 📊 Dashboard Response Example

```json
{
  "totalUsers": 10,
  "totalOrders": 25,
  "totalRevenue": 15420.50,
  "totalProducts": 8,
  "topProducts": [
    { "productName": "iPhone 15", "totalSold": 12, "totalRevenue": 11988 }
  ],
  "recentOrders": [
    { "id": 25, "totalAmount": 999.99, "createdAt": "2026-05-31", "userEmail": "user@test.com" }
  ]
}
```
