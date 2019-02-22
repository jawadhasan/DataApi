using System.Collections.Generic;
using Workflow.Core.WorkflowSteps;

namespace Workflow.Core
{
    public class BasicWorkflow : BaseWorkflow
    {
        public BasicWorkflow(string sourceEmailAddress, string requestId, int expiresIn, List<IWorkflowStep> steps) 
            : base(sourceEmailAddress, requestId, expiresIn, steps)
        {
            Name = SupportedWorkflows.Basic.ToString();
        }

        public static BasicWorkflow Create(string sourceEmailAddress, string requestId, int expiresIn)
        {
            var workflow = new BasicWorkflow(
                sourceEmailAddress,
                requestId,
                expiresIn,
                new List<IWorkflowStep>
                {
                    PersonalStep.Create(WorkflowSteps.Steps.Personal.ToString(), "Please tell us about yourself.",string.Empty, string.Empty, string.Empty),
                    ResultStep.Create(WorkflowSteps.Steps.Result.ToString(), "Thank you for your time", false)
                });
            return workflow;
        }
    }
}
