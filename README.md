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
* Config stored in config files, not hardcoded

---

## Quick Start

### Without Docker

1. Clone the repo:

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
```

2. Copy config templates:

```bash
cp BookingService/Presentation/appsettings.Example.json BookingService/Presentation/appsettings.json
cp PaymentService/Presentation/appsettings.Example.json PaymentService/Presentation/appsettings.json
```

3. Edit both `appsettings.json` files:

* `your_username` → `postgres`
* `your_password` → your PostgreSQL password
* `your_super_secret_key` → any strong secret, 32+ characters
  **The key must be identical in both services.**

4. Apply DB migrations:

```bash
cd BookingService
dotnet ef database update -p Infrastructure -s Presentation

cd ../PaymentService
dotnet ef database update -p Infrastructure -s Presentation
```

5. Start services (two terminals):

```bash
cd BookingService/Presentation
dotnet run
```

```bash
cd PaymentService/Presentation
dotnet run
```

6. Swagger UI:

* [http://localhost:5001/swagger](http://localhost:5001/swagger)
* [http://localhost:5079/swagger](http://localhost:5079/swagger)

---

### With Docker (easier)

```bash
git clone https://github.com/YOUR_USERNAME/EventBookingAPI.git
cd EventBookingAPI
docker-compose up -d
```

Run migrations (first time only):

```bash
docker-compose exec booking-service dotnet ef database update
docker-compose exec payment-service dotnet ef database update
```

Swagger:

* [http://localhost:5001/swagger](http://localhost:5001/swagger)
* [http://localhost:5079/swagger](http://localhost:5079/swagger)

Stop:

```bash
docker-compose down
# or remove data:
docker-compose down -v
```

---

## API (Short Overview)

### BookingService

#### Authentication

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| POST | /api/auth/register | Register new user | No |
| POST | /api/auth/login | Login and get JWT token | No |

#### Events

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/event | Get all events | Yes |
| GET | /api/event/{id} | Get event by ID | Yes |
| POST | /api/event | Create new event | Yes |
| PATCH | /api/event/{id} | Update event | Yes |
| DELETE | /api/event/{id} | Delete event | Yes |

#### Tickets

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/ticket | Get all tickets | Yes |
| GET | /api/ticket/{id} | Get ticket by ID | Yes |
| GET | /api/ticket/paged?page=1\&pageSize=10 | Get paginated tickets | Yes |
| POST | /api/ticket | Purchase ticket | Yes |
| PATCH | /api/ticket/{id} | Update ticket | Yes |
| DELETE | /api/ticket/{id} | Delete ticket | Yes |

#### Users

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/user | Get all users | Yes |
| GET | /api/user/{id} | Get user by ID | Yes |
| POST | /api/user | Create user | Yes |
| PATCH | /api/user/{id} | Update user | Yes |
| DELETE | /api/user/{id} | Delete user | Yes |

#### Venues

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/venue | Get all venues | Yes |
| GET | /api/venue/{id} | Get venue by ID | Yes |
| POST | /api/venue | Create venue | Yes |
| PATCH | /api/venue/{id} | Update venue | Yes |
| DELETE | /api/venue/{id} | Delete venue | Yes |

### PaymentService Endpoints

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| GET | /api/payment | Get all payments | No |
| GET | /api/payment/{id} | Get payment by ID | No |
| POST | /api/payment | Create payment (validates ticket) | No |


---

## Docker Commands

```bash
docker-compose up -d        # start
docker-compose down         # stop
docker-compose down -v      # stop + delete data
docker-compose logs -f      # logs
docker-compose exec booking-service /bin/bash
```