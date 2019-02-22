using System.Collections.Generic;
using Workflow.Core.WorkflowSteps;

namespace Workflow.Core
{
    public class OnboardingWorkflow : BaseWorkflow
    {
        public OnboardingWorkflow(string sourceEmailAddress, string requestId, int expiresIn, List<IWorkflowStep> steps) 
            : base(sourceEmailAddress, requestId, expiresIn, steps)
        {
            Name = SupportedWorkflows.OnBoarding.ToString();
        }

        public static OnboardingWorkflow Create(string sourceEmailAddress, string requestId, int expiresIn)
        {
            var workflow = new OnboardingWorkflow(
                sourceEmailAddress,
                requestId,
                expiresIn,
                new List<IWorkflowStep>
                {
                    PersonalStep.Create(WorkflowSteps.Steps.Personal.ToString(), "Please tell us about yourself.",string.Empty, string.Empty, string.Empty),
                    WorkStep.Create(WorkflowSteps.Steps.Work.ToString(), "What do u do?",string.Empty),
                    AddressStep.Create(WorkflowSteps.Steps.Address.ToString(), "Where do u live?", string.Empty, string.Empty,string.Empty, string.Empty),
                    ResultStep.Create(WorkflowSteps.Steps.Result.ToString(), "Thank you for your time", false)
                });

            return workflow;
        }
    }
}
