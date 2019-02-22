namespace Workflow.Core
{
    public interface IWorkflowStep
    {
        string Step { get; }
        string Title { get; }
        bool IsValid();
    }
}
