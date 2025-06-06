using Riva_Backend_Developer.Models;
using Riva_Backend_Developer.Services;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("=== Riva CRM Data Sync Engine ===\n");
// Sample Test data
var saleforceUser = new CrmUser
{
    UserId = Guid.CreateVersion7(),
    Email = "sample@company.com",
    Platform = "Salesforce",
    Token = "Randommmm"
};

var outlookUser = new CrmUser
{
    UserId = Guid.CreateVersion7(),
    Email = "outlookUser@company.com",
    Platform = "outlook",
    Token = "RandommmmOutlook"
};

var outlookUser2 = new CrmUser
{
    UserId = Guid.CreateVersion7(),
    Email = "outlookUser@company.com",
    Platform = "outlook",
    Token = ""
};

var syncJob = new List<SyncJob>
{
    new SyncJob
    {
        JobId = Guid.CreateVersion7(),
        User = saleforceUser,
        ObjectType = "Contact",
        Payload = "123",
        Status = SyncJobStatus.Pending },
    new SyncJob
    {
        JobId = Guid.CreateVersion7(),
        User = outlookUser,
        ObjectType = "Contact",
        Payload = "123",
        Status = SyncJobStatus.Pending },
       new SyncJob
    {
        JobId = Guid.CreateVersion7(),
        User = outlookUser2,
        ObjectType = "Contact",
        Payload = "123",
        Status = SyncJobStatus.Pending }

};

var tokenValidator = new SimpleTokenValidator();
var batchSyncProcessor = new BatchSyncProcessor(tokenValidator);

batchSyncProcessor.ProcessAll(syncJob).GetAwaiter().GetResult();
Console.WriteLine("\n=== Sync Process Completed ===\n");

