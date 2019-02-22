using System;
using Workflow.Core.Data;
using Workflow.Core.WorkflowSteps;

namespace Workflow.Core.Factories
{
    public static class WorkflowStepFactory
    {
        public static IWorkflowStep Create(string stepName, string title, FormData formData)
        {
            switch (stepName)
            {
                case "Personal":
                    return PersonalStep.Create(stepName,title, formData.FirstName, formData.LastName, formData.Email);

                case "Work":
                    return WorkStep.Create(stepName, title, formData.Work );

                case "Address":
                    return AddressStep.Create(stepName,title, formData.Street, formData.City, formData.State, formData.Zip);

                case "Result":
                    return ResultStep.Create(stepName, title, formData.IsValid); //may be if needed we can bring this value in from db.
                   
                default:
                    throw new ArgumentOutOfRangeException(stepName, $"{stepName} is not valid.");
            }
        }
    }
}
