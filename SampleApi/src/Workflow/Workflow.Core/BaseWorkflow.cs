using System;
using System.Collections.Generic;
using System.Linq;
using Workflow.Core.Data;

namespace Workflow.Core
{
    public class BaseWorkflow : IBaseWorkflow
    {
        public virtual long Id { get; }
        public virtual string Name { get; protected set; }
        public virtual string SourceEmailAddress { get; private set; } //Email address of the invitation
        public virtual string RequestId { get; private set; } //external system forigenKey (InvitationId)
        public virtual int ExpiresIn { get; private set; } //Invitation Expires-in Days (if not started?)
        public virtual DateTime CreatedOn { get; private set; }
        public virtual WorkflowStatus Status { get; private set; }
        
        public virtual FormData WorkflowData { get; protected set; }
        public virtual List<IWorkflowStep> Steps { get; private set; }

        protected BaseWorkflow()
        {
        }

        protected BaseWorkflow(string sourceEmailAddress, string requestId, int expiresIn, List<IWorkflowStep> steps) : this()
        {
            Guard.ForNullOrEmpty(sourceEmailAddress, nameof(sourceEmailAddress));
            Guard.ForNullOrEmpty(requestId, nameof(requestId));
            Guard.ForLessEqualZero(expiresIn, nameof(expiresIn));
           
            SourceEmailAddress = sourceEmailAddress;
            RequestId = requestId;
            ExpiresIn = expiresIn;
            Steps = steps;

            CreatedOn = DateTime.UtcNow;
            WorkflowData = new FormData();
        }



        //we should not have persistence here, put this comment for reminder. 
        //public virtual void SavePersonal(string firstName, string lastName, string email)
        //{
        //   WorkflowData.FirstName = firstName;
        //   WorkflowData.LastName = lastName;
        //   WorkflowData.Email = email;
        //}

        public bool IsExpired()
        {
            return CreatedOn.AddDays(ExpiresIn) <= DateTime.UtcNow;
        }
        public bool CanContinue() =>
            Status == WorkflowStatus.Created ||
            Status == WorkflowStatus.Started ||
            Status == WorkflowStatus.Completed;

        public void UpdateStatus(WorkflowStatus status)
        {
            Status = status;
            //StatusUpdated
        }

        public virtual bool IsValid()
        {
            return Steps.Where(s=> s.Step != WorkflowSteps.Steps.Result.ToString())
                .All(step => step.IsValid());
        }
        public virtual string GetFirstInvalidStep()
        {
            return Steps.FirstOrDefault(s => !s.IsValid())?.Step;
        }
        public override string ToString()
        {
            return $"WorkflowName: {Name},  IsValid: {IsValid()}";
        }


    }


    public enum WorkflowStatus
    {
        Created = 0,
        Started = 10,
        Completed = 20,
        Posted = 30, //Posted for background processing
        Success = 40,
        Expired = 50,
        Failed = 60,
        Cancelled = 100
    }
}