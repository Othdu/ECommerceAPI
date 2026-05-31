#  E-Commerce API

A production-ready RESTful API built with **ASP.NET Core 9** and **Entity Framework Core**, featuring JWT authentication, role-based authorization, and a full e-commerce flow.

##  Features

- **JWT Authentication** — register, login, token-based auth
- **Role-based Authorization** — Admin and User roles
- **Product Management** — full CRUD with category support
- **Order System** — place orders, automatic stock management
- **Admin Dashboard** — real-time stats, top products, recent orders
- **Input Validation** — data annotations on all DTOs
- **Error Handling** — global middleware returning clean JSON errors
- **Swagger UI** — interactive API documentation

##  Architecture

```
Controller → Service → Repository → Database
```

Clean separation of concerns — controllers handle HTTP, services handle business logic, repositories handle data access.

##  Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| Auth | JWT Bearer Tokens |
| Docs | Swagger / Swashbuckle |
| Language | C# |

##  Database Schema

```
Users ──────────────── Orders
         1               │
         │               │ has many
         │               ▼
     places         OrderItems ──── Products ──── Categories
```

##  API Endpoints

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

## ⚙️ Setup & Run

### Prerequisites
- .NET 9 SDK
- SQL Server
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
  "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDB;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
}
```

3. Run migrations
```bash
dotnet ef database update
```

4. Run the project
```bash
dotnet run
```

5. Open Swagger at `http://localhost:5253/swagger`

##  Authentication Flow

1. Register → `POST /api/auth/register`
2. Login → `POST /api/auth/login` → copy the token
3. Click **Authorize** in Swagger → enter `Bearer {token}`
4. All protected endpoints are now accessible

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
