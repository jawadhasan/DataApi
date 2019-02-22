using System;

namespace Workflow.Core.Factories
{
    public static class WorkflowFactory
    {
        public static BaseWorkflow CreateNewWorkflow(string workflow, string sourceEmailAddress, string requestId, int expiresIn)
        {
            switch (workflow)
            {
                case "Basic":
                    return BasicWorkflow.Create(sourceEmailAddress, requestId, expiresIn);
                case "Simple":
                    return SimpleWorkflow.Create(sourceEmailAddress, requestId, expiresIn);
                case "Onboarding":
                    return OnboardingWorkflow.Create(sourceEmailAddress, requestId, expiresIn);

                case "OnboardingGiver":
                    return OnboardingWorkflow.Create(sourceEmailAddress, requestId, expiresIn); //should be dedicate workflow

                case "OnboardingTaker":
                    return OnboardingWorkflow.Create(sourceEmailAddress, requestId, expiresIn); //should be dedicate workflow

                default:
                    throw new ArgumentOutOfRangeException(nameof(workflow), workflow, null);
            }
        }
    }
}