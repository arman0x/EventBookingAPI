# Event Booking API

A simple event booking system with tickets and payments.
Built as microservices. Uses JWT for auth and Redis for caching. Can run in Docker.

## Architecture

Two main services:

* **BookingService** (port 5001) — events, tickets, users, venues
* **PaymentService** (port 5079) — payment handling and ticket validation
* **PostgreSQL** (port 5432) — main database
* **Redis** (port 6379) — caching layer

## Tech Stack

* ASP.NET Core 10.0
* Clean Architecture (Domain / Application / Infrastructure / Presentation)
* PostgreSQL + EF Core
* Redis (StackExchange.Redis)
* JWT auth with User/Admin roles
* Docker + Docker Compose
* Swagger for API docs
* AutoMapper
* BCrypt for password hashing

## Security Features

* JWT-based login
* Passwords hashed with BCrypt
* Role checks (User, Admin)
* Secrets passed via environment variables, not hardcoded in config

---

## Quick Start

### With Docker (recommended)

1. Clone the repo:

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
```

2. Create `.env` file in the project root:

```env
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_postgres_password
JWT_SECRET=your_super_secret_key_at_least_32_chars
INTERNAL_SECRET=your_internal_service_secret
```

> The same `JWT_SECRET` is used by both services — they must match.

3. Start everything:

```bash
docker compose up -d --build
```

Migrations run automatically on first start. Swagger UI:

* http://localhost:5001/swagger
* http://localhost:5079/swagger

---

### Without Docker

1. Clone the repo and copy config templates:

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
cp BookingService/Presentation/appsettings.Example.json BookingService/Presentation/appsettings.json
cp PaymentService/Presentation/appsettings.Example.json PaymentService/Presentation/appsettings.json
```

2. Edit both `appsettings.json` files — fill in your PostgreSQL credentials.

3. Set the `JWT_SECRET` environment variable (must be the same for both services):

```bash
# Linux/macOS
export JWT_SECRET=your_super_secret_key_at_least_32_chars

# Windows (PowerShell)
$env:JWT_SECRET="your_super_secret_key_at_least_32_chars"
```

4. Apply DB migrations:

```bash
dotnet ef database update -p BookingService/Infrastructure -s BookingService/Presentation
dotnet ef database update -p PaymentService/Infrastructure -s PaymentService/Presentation
```

5. Start services (two terminals):

```bash
cd BookingService/Presentation && dotnet run
```

```bash
cd PaymentService/Presentation && dotnet run
```

---

## How to Use the API

### 1. Register

```
POST /api/auth/register
{ "email": "user@example.com", "password": "YourPassword123!" }
```

### 2. Login and get token

```
POST /api/auth/login
{ "email": "user@example.com", "password": "YourPassword123!" }
```

Response:
```json
{ "token": "eyJhbGci..." }
```

### 3. Authorize in Swagger

Click the **Authorize** button at the top of the Swagger page, paste the token, click **Authorize**.

### 4. Use protected endpoints

All endpoints except `/api/auth/*` require `Authorization: Bearer <token>`.

---

## API Overview

### BookingService

#### Authentication

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| POST | /api/auth/register | No |
| POST | /api/auth/login | No |

#### Events

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| GET | /api/event | Yes |
| GET | /api/event/{id} | Yes |
| POST | /api/event | Yes |
| PATCH | /api/event/{id} | Yes |
| DELETE | /api/event/{id} | Yes |

#### Tickets

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| GET | /api/ticket | Yes |
| GET | /api/ticket/{id} | Yes |
| GET | /api/ticket/paged?page=1&pageSize=10 | Yes |
| POST | /api/ticket | Yes |
| PATCH | /api/ticket/{id} | Yes |
| DELETE | /api/ticket/{id} | Yes |

#### Users

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| GET | /api/user | Yes |
| GET | /api/user/{id} | Yes |
| POST | /api/user | Yes |
| PATCH | /api/user/{id} | Yes |
| DELETE | /api/user/{id} | Yes |

#### Venues

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| GET | /api/venue | Yes |
| GET | /api/venue/{id} | Yes |
| POST | /api/venue | Yes |
| PATCH | /api/venue/{id} | Yes |
| DELETE | /api/venue/{id} | Yes |

### PaymentService

| Method | Endpoint | Auth |
| :--- | :--- | :--- |
| GET | /api/payment | Yes |
| GET | /api/payment/{id} | Yes |
| POST | /api/payment | Yes |

---

## Docker Commands

```bash
docker compose up -d --build   # build and start
docker compose up -d           # start (no rebuild)
docker compose down            # stop
docker compose down -v         # stop + delete all data
docker compose logs -f         # stream logs
docker compose logs -f booking-service   # logs for one service
docker compose exec booking-service /bin/bash
```
