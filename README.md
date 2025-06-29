# Yungching Interview WebAPI

---
- Located at: `apps/backend`
- Built with ASP.NET Core (.NET 8)
- Features:
  - RESTful APIs
  - Dapper-based database access
  - Serilog logging
  - Encapsulated database transactions via [`TransactionContext`](https://github.com/aeg6430/yungching_interview/blob/main/apps/backend/Yungching.Infrastructure/Contexts/TransactionContext.cs)
- Architecture:
  - Layered structure: WebAPI, Application, Infrastructure, and Domain
  - Repository pattern for data access abstraction
