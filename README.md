**Riva CRM Data Sync Engine**

A refactored, modular CRM data synchronization system that connects to platforms like Salesforce, Outlook, and HubSpot to sync user and object data.

**Architecture Overview**
The system has been refactored from a monolithic IntelSyncEntry class into a clean, modular architecture with clear separation of concerns:

**Domain Models**

CrmUser: Represents a CRM user with authentication information
SyncJob: Represents individual sync tasks with status tracking

**Services**

ISyncValidator: Interface for validation logic (easily extensible)
SimpleTokenValidator: Concrete implementation for token validation
BatchSyncProcessor: Handles batch processing of multiple sync jobs

**Key Design Principles**

Single Responsibility: Each class has one clear purpose
Interface Segregation: ISyncValidator allows for different validation strategies
Dependency Injection: Services depend on abstractions, not concretions
Open/Closed Principle: Easy to extend with new validators or processors
