using Workflow.Core.Data;

namespace Workflow.Core.WorkflowSteps
{
    public class PersonalStep : BaseWorkflowStep<Personal>
    {
        public PersonalStep(string step, string title, Personal data) 
            : base(step, title, data)
        {
        }

        public override bool IsValid()
        {
            return Data.IsValid();
        }
        public static PersonalStep Create(string stepName, string title, string firstName, string lastName, string email)
        {
            var step = new PersonalStep(stepName, title, new Personal(firstName, lastName,email));
            return step;
        }
    }
}