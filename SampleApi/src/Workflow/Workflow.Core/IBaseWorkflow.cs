using System.Collections.Generic;
using Workflow.Core.Data;

namespace Workflow.Core
{
    public interface IBaseWorkflow
    {
        string Name { get; }
        FormData WorkflowData { get; }
        List<IWorkflowStep> Steps { get; }
        bool IsValid();
        string GetFirstInvalidStep();
    }
}
