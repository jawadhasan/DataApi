namespace Workflow.Core.Data
{
    public class Work : IStepData
    {
        public string WorkType { get; private set; }
        public Work(string worktype)
        {
            WorkType = worktype;
        }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(WorkType);
        }
    }
}
