using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Extensions.Logging;

namespace Veri.System.Xrm.Sdk
{
    public abstract class AbstractCrmProxy
    {
        private string token;
        private static CrmServiceClient _serviceProxy;
        protected ILogger Logger { get; set; }


        public AbstractCrmProxy(string connectionString, ILogger logger)
        {
            this.Logger = logger;

            try
            {
                _serviceProxy = new CrmServiceClient(connectionString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occured while trying to connect Dynamics 365");
            }
        }

        public virtual async Task RunAsync()
        {
            token = GetToken();

            RetrieveEntityChangesRequest request = new RetrieveEntityChangesRequest
            {
                EntityName = GetEntityName().ToLower(),
                Columns = new ColumnSet(GetColumns().ToArray()),
                PageInfo = new PagingInfo() { Count = 5000, PageNumber = 1, ReturnTotalRecordCount = false },
                DataVersion = token
            };

            List<Task> tasks = new List<Task>();

            while (true)
            {
                RetrieveEntityChangesResponse response = (RetrieveEntityChangesResponse)_serviceProxy.Execute(request);
                if (response.EntityChanges.Changes.Any(x => x.Type == ChangeType.NewOrUpdated))
                {
                    var changes = response.EntityChanges.Changes.Where(x => x.Type == ChangeType.NewOrUpdated);
                    var entityIds = GetEntityIds(changes.Select(x => (x as NewOrUpdatedItem).NewOrUpdatedEntity).ToArray());
                    if (entityIds == null || entityIds.Count == 0)
                    {
                        Logger.LogInformation($"{GetEntityName()}: no changes detected.");
                        break;
                    }
                    Logger.LogInformation($"{GetEntityName()}: Changes have been detected on {entityIds.Count} records.");
                }
                if (!response.EntityChanges.MoreRecords)
                {
                    SetToken(response.EntityChanges.DataToken);
                    await Task.WhenAll(tasks.ToArray());
                    break;
                }
                if (response.EntityChanges.Changes.Any(x => x.Type == ChangeType.RemoveOrDeleted))
                {
                    var changes = response.EntityChanges.Changes.Where(x => x.Type == ChangeType.RemoveOrDeleted);
                    var entityIds = GetEntityIds(changes.Select(x => (x as RemovedOrDeletedItem).RemovedItem).ToArray());
                    Logger.LogInformation($"{GetEntityName()}: {entityIds.Count} records are removed.");
                }

                request.PageInfo.PageNumber++;
                request.PageInfo.PagingCookie = response.EntityChanges.PagingCookie;
            }
        }

        protected virtual List<string> GetEntityIds(Entity[] entityArray)
        {
            List<string> entityIds = new List<string>();
            entityArray.ToList().ForEach(i => {
                entityIds.Add(i.GetConvertedValue(GetEntityIdFieldName()));
            });
            return entityIds;
        }
        protected virtual List<string> GetEntityIds(EntityReference[] entityArray)
        {
            List<string> entityIds = new List<string>();
            entityArray.ToList().ForEach(i => {
                entityIds.Add(i.Id.ToString());
            });
            return entityIds;
        }
        public abstract List<string> GetColumns();
        public abstract string GetEntityName();
        public abstract string GetEntityIdFieldName();
        public abstract void SetToken(string token);
        public abstract string GetToken();
    }
}
