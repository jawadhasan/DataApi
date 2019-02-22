namespace Workflow.Data
{
    public class UserWorkflowStep
    {
        public long Id { get; private set; }
        public string Step { get; private set; }
        public bool IsCompleted { get; private set; }

        //Not mapped
        public TrackingState TrackingState { get; set; }
        protected UserWorkflowStep() { }
        public UserWorkflowStep(string step)
        {
            Step = step;
            TrackingState = TrackingState.Created;
        }

        public void MarkIsStepComplete(bool isCompleted)
        {
            IsCompleted = isCompleted;
            TrackingState = TrackingState.Updated;
        }
    }
}