using Microsoft.EntityFrameworkCore;
namespace Workflow.Data
{
    public class WorkflowDbContext : DbContext
    {

        public DbSet<UserWorkflow> UserWorkflows { get; set; }
        public DbSet<UserWorkflowStep> UserWorkflowSteps { get; set; }

        public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options)
        {
        }
    }
}
