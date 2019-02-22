using System;
using System.Collections.Generic;
using System.Linq;
using Workflow.Core;

namespace Workflow.Data
{
    public class UserWorkflow
    {
        protected UserWorkflow()
        {
            Steps = new List<UserWorkflowStep>();
        }

        public UserWorkflow(string workflowType, string sourceEmailAddress, string requestId, int expiresIn, DateTime createdOn) :this()
        {
            WorkflowType = workflowType; //for Conveinience e.g.. basic, simple, onBoarding
            SourceEmailAddress = sourceEmailAddress;
            RequestId = requestId;
            ExpiresIn = expiresIn;
            CreatedOn = createdOn;
        }

        public long Id { get; set; }

        public string WorkflowType { get; private set; }
        public string SourceEmailAddress { get; private set; } //Email address of the invitation
        public string RequestId { get; private set; } //external system forigenKey (InvitationId)
        public int ExpiresIn { get; private set; } //Invitation Expires-in Days (if not started?)
        public DateTime CreatedOn { get; private set; }
        public WorkflowStatus Status { get; private set; }

        public string FirstName { get;  private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Work { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }
        public List<UserWorkflowStep> Steps { get; set; }

        //Link back to Management
        //public long WorkflowId { get; set; }
        //public virtual Management.Workflow Workflow { get; set; }


        public void AddWorkflowStep(UserWorkflowStep step)
        {
            step.TrackingState = TrackingState.Created;
            Steps.Add(step);
        }

        public void SetPersonal(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Steps.FirstOrDefault(s => s.Step.ToLower() == "personal")?.MarkIsStepComplete(true);
        }

        public void SetWork(string work)
        {
            Work = work;
            Steps.FirstOrDefault(s => s.Step.ToLower() == "work")?.MarkIsStepComplete(true);
        }

        public void SetAddress(string street, string city, string zip, string state)
        {
            Street = street;
            City = city;
            Zip = zip;
            State = state;
            Steps.FirstOrDefault(s => s.Step.ToLower() == "address")?.MarkIsStepComplete(true);
        }

        public void SetResult()
        {
            Steps.FirstOrDefault(s => s.Step.ToLower() == "result")?.MarkIsStepComplete(true);
        }

        public void UpdateStatus(WorkflowStatus status)
        {
            Status = status;
        }
    }
}
