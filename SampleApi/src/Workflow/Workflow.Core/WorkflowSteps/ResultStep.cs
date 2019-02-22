namespace Workflow.Core.WorkflowSteps
{
    public class ResultStep : BaseWorkflowStep<bool>
    {
        public ResultStep(string step, string title, bool data)
            : base(step, title, data)
        {
        }

        public override bool IsValid()
        {
            return Data;
        }

        public static ResultStep Create(string stepName,string title, bool isValid)
        {
            var step = new ResultStep(stepName, title, isValid);
            return step;
        }
    }
}