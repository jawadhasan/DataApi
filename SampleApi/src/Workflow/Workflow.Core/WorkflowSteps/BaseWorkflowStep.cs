using System;

namespace Workflow.Core.WorkflowSteps
{
    public abstract class BaseWorkflowStep<T> : IWorkflowStep
    {
        public string Step { get; protected set; }
        public string Title { get; protected set; }
        public T Data { get; protected set; }

        protected BaseWorkflowStep(string step, string title, T data)
        {
            Step = step;
            Title = title;
            Data = data;
        }
        public virtual bool IsValid()
        {
            throw new NotImplementedException($"Drived class should implement {nameof(IsValid)} method");
        }

        public virtual void SetStepData(T stepData)
        {
            Data = stepData;
        }
    }
}