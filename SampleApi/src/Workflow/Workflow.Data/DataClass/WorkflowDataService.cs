using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workflow.Core;
using Workflow.Core.Data;
using Workflow.Data.Adapter;

namespace Workflow.Data.DataClass
{
    public class WorkflowDataService : IWorkflowDataService
    {
        private readonly WorkflowDbContext _db;

        public WorkflowDataService(WorkflowDbContext db)
        {
            _db = db;
        }

        public async Task<BaseWorkflow> GetWorkflowByRequestId(string requestId)
        {
            var workflowfromDb = await _db.UserWorkflows
                .Include(wf => wf.Steps)
                //.Include(m=> m.Workflow)
                //.ThenInclude(mw=> mw.WorkflowSteps)

                .FirstOrDefaultAsync(wf => wf.RequestId == requestId);

            return workflowfromDb != null ? new ApplicationWorkflow(workflowfromDb) : null;
        }


        public async Task UpdatePersonalInfo(string id, Personal personalInfo)
        {
            var workflowfromDb = await _db.UserWorkflows
              .Include(wf => wf.Steps)
              .FirstOrDefaultAsync(wf => wf.RequestId == id);

            workflowfromDb.SetPersonal(personalInfo.FirstName, personalInfo.LastName, personalInfo.Email);
            await UpdateDbWorkflow(workflowfromDb);
        }
        public async Task UpdateWorkInfo(string id, Work workInfo)
        {
            var workflowfromDb = await _db.UserWorkflows
                .Include(wf => wf.Steps)
                .FirstOrDefaultAsync(wf => wf.RequestId == id);

            workflowfromDb.SetWork(workInfo.WorkType);
            await UpdateDbWorkflow(workflowfromDb);
        }
        public async Task UpdateAddressInfo(string id, Address addressInfo)
        {
            var workflowfromDb = await _db.UserWorkflows
                .Include(wf => wf.Steps)
                .FirstOrDefaultAsync(wf => wf.RequestId == id);

            workflowfromDb.SetAddress(addressInfo.Street, addressInfo.City, addressInfo.Zip, addressInfo.State);
            await UpdateDbWorkflow(workflowfromDb);
        }
        public async Task UpdateResult(string id, FormData result)
        {
            //Not using result here, but kept it just incase

            var workflowfromDb = await _db.UserWorkflows
                .Include(wf => wf.Steps)
                .FirstOrDefaultAsync(wf => wf.RequestId == id);

            workflowfromDb.SetResult();

            await UpdateDbWorkflow(workflowfromDb);
        }

        private async Task UpdateDbWorkflow(UserWorkflow userWorkflow)
        {
            foreach (var step in userWorkflow.Steps)
            {
                if (step.TrackingState == TrackingState.Updated)
                {
                    _db.Entry(step).State = EntityState.Modified;
                }
            }
            _db.Entry(userWorkflow).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        
        public async Task SaveWorkflowAsync(BaseWorkflow workflow)
        {
            if (workflow.Id > 0)
            {
                var existingWorkflow = _db.UserWorkflows
                    .Include(wf => wf.Steps)
                    .FirstOrDefault(wf => wf.Id == workflow.Id);

                if (existingWorkflow != null)
                {
                    UpdateWorkflow(workflow, existingWorkflow); //do we need this, coz we are not updating the whole workflow, instead now its task based update.. this could be removed.
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Id", $"can not find workflow for Id {workflow.Id} for update/save");
                }
            }
            else
            {
               await SaveNewUserWorkflow(workflow);
            }
        }


        //TODO: SRP
        private async Task SaveNewUserWorkflow(BaseWorkflow workflow)
        {
            //Building
            var dbWorkflow = new UserWorkflow(workflow.Name, workflow.SourceEmailAddress, workflow.RequestId,workflow.ExpiresIn,workflow.CreatedOn);
            foreach (var step in workflow.Steps)
            {
                dbWorkflow.AddWorkflowStep(new UserWorkflowStep(step.Step));
            }
           
            //Persisting
            foreach (var step in dbWorkflow.Steps)
            {
                if (step.TrackingState == TrackingState.Created)
                {
                    _db.Entry(step).State = EntityState.Added;
                }
            }
            _db.UserWorkflows.Add(dbWorkflow);
            await _db.SaveChangesAsync();
        }

        private void UpdateWorkflow(BaseWorkflow workflow, UserWorkflow existingUserWorkflow)
        {
            //not updating the primary properties e.g. sourceEmailAddress, expiresIn etc.
            existingUserWorkflow.SetPersonal(workflow.WorkflowData.FirstName, workflow.WorkflowData.LastName,workflow.WorkflowData.Email);
            existingUserWorkflow.SetWork(workflow.WorkflowData.Work);
            existingUserWorkflow.SetAddress(workflow.WorkflowData.Street, workflow.WorkflowData.City, workflow.WorkflowData.Zip,workflow.WorkflowData.State);
            existingUserWorkflow.UpdateStatus(workflow.Status);

            foreach (var step in existingUserWorkflow.Steps)
            {
                if (step.TrackingState == TrackingState.Updated)
                {
                    _db.Entry(step).State = EntityState.Modified;
                }
            }

            _db.Entry(existingUserWorkflow).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
