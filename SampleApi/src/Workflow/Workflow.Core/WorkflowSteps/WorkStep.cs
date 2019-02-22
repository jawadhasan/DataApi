using Workflow.Core.Data;

namespace Workflow.Core.WorkflowSteps
{
    public class WorkStep : BaseWorkflowStep<Work>
    {
        
        public WorkStep(string step, string title, Work data)
            : base(step, title, data)
        {
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Data.WorkType);
        }

        public static WorkStep Create(string stepName, string title, string work)
        {
            var workStep = new WorkStep(stepName, title, new Work(work));
            return workStep;
        }
    }
}