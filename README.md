# Electronic Library Management System with AI Recommendation - Backend
## Overview
This is the backend of the Electronic Library Management System with AI Recommendation, developed using ASP.NET Core, Entity Framework Core, and SQL Server. It manages all core library operations including book borrowing/returning, inventory management, user and role management, notifications, overdue and fine management, and integrates an AI-powered recommendation engine that suggests books based on user behavior, borrowing history, and profile information such as academic majors or professional domains. The backend supports real-time communication, automated email notifications, secure online payments, cloud storage, and scheduled background tasks.
## Key Features
- AI Recommendation Engine: Provides personalized book suggestions using AI based on user activity, borrowing history, and profile information.
- Real-time Notifications: Implemented with SignalR to provide instant updates on borrowing status, approvals, and system alerts.
- OTP & Email Notifications: Sends OTP codes for authentication and due-date reminder emails automatically.
- VNPay Integration: Enables secure online payments for overdue fines.
- Cloudinary Integration: Stores and manages book images and document files in cloud storage.
- Automated Inventory Management: Allocates new books to storage locations and tracks inventory status in real-time.
- Background Task Scheduling: Uses Hangfire to handle recurring jobs like email reminders and system maintenance.
- Comprehensive Backend Workflows: Covers book management, user/role management, borrowing/return processes, reservations, overdue handling, notifications, import/export logs, and fine management.
## Project Structure & Applied Knowledge
The backend project demonstrates knowledge of clean architecture, dependency injection, authentication & authorization, background task scheduling, real-time communication, and RESTful API design.

/DoAnCuoiKy.Backend
│
├─ Controllers          // API endpoints for books, users, borrowing, notifications, etc.
├─ Configuration        // JWT, CORS, cookie, and other middleware configurations
├─ Data                 // ApplicationDbContext and database configurations
├─ Iservice             // Service interfaces defining business logic contracts
├─ Service              // Service implementations providing core functionalities
├─ Model
│   ├─ Entities         // Database entity models
│   ├─ Request          // Request DTOs for API input
│   ├─ Response         // Response DTOs for API output
│   └─ Enum             // Enums used across the project
├─ Mapper               // AutoMapper profiles for mapping between entities and DTOs
└─ Program.cs           // Main program, service registrations, middleware, and pipeline configuration
### Key Knowledge Applied:
- Dependency Injection (DI): AddTransient and AddScoped for modularity and testability.
- Authentication & Authorization: JWT Bearer with role claims and cookie support.
- SignalR: Real-time notifications to frontend.
- Hangfire: Recurring background tasks for emails and system jobs.
- Cloudinary Service: Management of book images and files.
- Database Design: EF Core with entities, relationships, and migrations.
- API Design: RESTful controllers with structured request/response DTOs.
- Middleware & Security: CORS, HTTPS redirection, cookie policies, and JWT validation.

