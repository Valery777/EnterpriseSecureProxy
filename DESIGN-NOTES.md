# DESIGN-NOTES.md

# Enterprise Secure Proxy - Design Notes

## Overview

The Enterprise Secure Proxy provides a controlled and secure 
mechanism for routing requests to downstream services.

The primary goals of the solution are:

- Centralized request validation
- Secure payload processing
- Configurable operation routing
- Destination control
- Auditability
- Resilience and fault handling

The solution was implemented using .NET 8 and follows a 
layered architecture to maintain separation of concerns and testability.

---

# Architecture

The project is organized into the following layers:

## API Layer

Responsibilities:

- Receiving HTTP requests
- Authentication and authorization
- Request deserialization
- Error handling
- Returning standardized responses

Main components:

- Controllers
- Middleware
- Program.cs configuration

---

## Application Layer

Responsibilities:

- Business rules
- Validation
- Operation execution workflow
- Audit orchestration
- Security services abstraction

Main components:

- Use cases
- Application services
- Interfaces
- Validation logic

The Application layer does not directly depend on Infrastructure implementations.

---

## Infrastructure Layer

Responsibilities:

- Database access
- External HTTP communication
- Encryption implementation
- Secret management
- Audit persistence

Main components:

- Repository implementations
- ProxyHttpForwarder
- Encryption services
- Secret providers

---

## Persistence Layer

Entity Framework Core is used for persistence.

Primary entities include:

- Operations
- Destinations
- RequestLogs

Database access is performed through repositories to
isolate persistence logic from business rules.

---

# Routing Design

Requests are routed using operation identifiers stored in the database.

Execution flow:

Client Request

↓

Operation Lookup

↓

Operation Validation

↓

Destination Validation

↓

Payload Validation

↓

Forwarding

↓

Audit Logging

↓

Response

Benefits:

- No hardcoded destination URLs
- Runtime configurability
- Centralized request management

---

# Security Design

## AES-GCM Encryption

AES-GCM was selected because it provides:

- Confidentiality
- Integrity protection
- Authentication tags

Requests with invalid authentication tags are rejected.

---

## Operation Validation

Unknown operations are rejected before any forwarding occurs.

Benefits:

- Prevents unauthorized routing
- Reduces attack surface

---

## Destination Validation

Only enabled destinations can be used.

Disabled destinations return a controlled error response.

Benefits:

- Administrative control
- Safe emergency shutdown capability

---

## Path Traversal Protection

Input validation prevents malicious path manipulation attempts.

Benefits:

- Protects destination endpoints
- Reduces SSRF-style attack risks

---

## Request Validation

Validation occurs before forwarding.

Examples:

- Missing payloads
- Missing parameters
- Invalid request formats

Benefits:

- Consistent behaviour
- Reduced downstream failures

---
# JWT Authentication

JWT Bearer authentication infrastructure has been configured 
within the application pipeline. Authentication middleware, 
authorization middleware and Swagger security definitions are
registered to support future secured deployments. Protected 
endpoints and token issuance were outside the scope of the assessment
requirements and therefore authentication is not actively 
enforced by business endpoints.

# Auditing Design

Every processed request generates audit information.

Captured information includes:

- Correlation ID
- Operation Key
- Outcome
- Duration
- Rejection reason
- Status information

Benefits:

- Traceability
- Troubleshooting support
- Compliance support

---

# Error Handling Strategy

Exceptions are translated into consistent HTTP responses.

Examples:

| Scenario | Response |
|-----------|-----------|
| Unknown Operation | 404 |
| Disabled Operation | 403 |
| Disabled Destination | 503 |
| Invalid Parameters | 400 |
| Invalid Payload | 400 |

This prevents leaking internal implementation details.

---

# Dependency Injection

The application uses ASP.NET Core dependency injection throughout.

Examples:

- Repositories
- Encryption services
- Secret providers
- Audit services
- HTTP forwarding services

Benefits:

- Loose coupling
- Easier testing
- Easier replacement of implementations

---

# Testing Strategy

Integration tests focus on behaviour visible to system consumers.

Coverage includes:

- Startup validation
- Unknown operations
- Disabled operations
- Disabled destinations
- Path traversal protection
- Invalid encrypted payloads
- Invalid request methods
- Audit record creation
- Concurrency handling

A mock forwarding infrastructure was implemented using:

- RecordingHttpMessageHandler
- FakeHttpClientFactory
- Test WebApplicationFactory

This enables isolated testing of outbound HTTP communication without 
depending on external services.

---

# Technical Decisions

## Why Entity Framework Core

Chosen because:

- Strong .NET integration
- Migration support
- Repository compatibility
- PostgreSQL/SQL Server support

---

## Why Repository Pattern

Chosen because:

- Separation of concerns
- Easier testing
- Reduced persistence coupling

---

## Why Dependency Injection

Chosen because:

- Improved maintainability
- Testability
- Flexibility of implementations

---

## Why Integration Tests

Integration tests were prioritized because assessment requirements focused on:

- Request validation
- Security controls
- Routing behaviour
- Audit correctness

These concerns are best verified through end-to-end execution paths.

---

# Future Improvements

Potential future enhancements:

- Full forwarding verification with active mock destinations
- Automated timeout simulation
- Header filtering verification
- Retry and resilience policies
- Expanded audit analytics
- Enhanced monitoring and telemetry