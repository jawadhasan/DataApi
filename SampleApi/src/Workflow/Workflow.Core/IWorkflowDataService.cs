using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Core.Data;

namespace Workflow.Core
{
    public interface IWorkflowDataService
    {
        Task<List<BaseWorkflow>> GetWorkflows();
        Task<BaseWorkflow> GetWorkflowByRequestId(string requestId);
        Task SaveWorkflowAsync(BaseWorkflow workflow);

        Task UpdatePersonalInfo(string id, Personal personalInfo);
        Task UpdateWorkInfo(string id, Work workInfo);
        Task UpdateAddressInfo(string id, Address addressInfo);
        Task UpdateResult(string id, FormData result);
    }
}
