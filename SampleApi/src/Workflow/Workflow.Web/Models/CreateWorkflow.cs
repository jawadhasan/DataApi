using System;
using System.ComponentModel.DataAnnotations;

namespace Workflow.Web.Models
{
    public class CreateWorkflow
    {
        public string RequestId { get; set; } 

        [Required]
        public string WorkflowName { get; set; }

        [Required]
        public string SourceEmailAddress { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public CreateWorkflow()
        {
            RequestId = Guid.NewGuid().ToString("N");
        }

    }

    public class UpdatePersonalStep
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class UpdateWorkStep
    {
        public string Work { get; set; }
    }

    public class UpdateAddressStep
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class UpdateResultStep
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Work { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
