using Workflow.Core.Data;

namespace Workflow.Core.WorkflowSteps
{
    public class AddressStep : BaseWorkflowStep<Address>
    {
        public AddressStep(string step, string title, Address data)
            : base(step, title, data)
        {
        }

        public override bool IsValid()
        {
            if (Data != null)
            {
                return Data.IsValid();
            }

            return false;
        }

        public static AddressStep Create(string stepName, string title, string street, string city, string state, string zip)
        {
            var step = new AddressStep(stepName, title, new Address(street, city, state, zip));
            return step;
        }
    }
}