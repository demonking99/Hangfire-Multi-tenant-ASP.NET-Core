# Hangfire-Multi-tenant-ASP.NET-Core
Hangfire - Multi tenant ASP.NET Core - Resolving the correct tenant

We got a SaaS project that needs the use Hangfire. We already implemented the requirements to identify a tenant.

Architecture

Persistence Layer
Each tenant has it's own database
.NET Core
We already have a service TenantCurrentService which returns the ID of the tenant, from a list of source [hostname, query string, etc]
We already have a DbContextFactory for Entity Framework which return a DB context with the correct connection string for the client
We are currently using ASP.NET Core DI (willing to change if that helps)
Hangfire
Using single storage (eg: Postgresql), no matter the tenant count
Execute the job in an appropriate Container/ServiceCollection, so we retrieve the right database, right settings, etc.
The problem

I'm trying to stamp a TenantId to a job, retrieved from TenantCurrentService (which is a Scoped service).

When the job then gets executed, we need to retrieve the TenantId from the Job and store it in HangfireContext, so then the TenantCurrentService knows the TenantId retrieved from Hangfire. And from there, our application layer will be able to connect to the right database from our DbContextFactory
