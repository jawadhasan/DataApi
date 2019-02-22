using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Workflow.Core;
using Workflow.Core.Data;
using Workflow.Core.Factories;
using Workflow.Web.Models;

namespace Workflow.Web.Controllers
{
  [Route("api/[controller]")]
  public class WorkflowController : Controller
  {
    private readonly IWorkflowDataService _workflowDataService;
    private readonly ILogger<WorkflowController> _logger;

    public WorkflowController(IWorkflowDataService workflowDataService, ILogger<WorkflowController> logger)
    {
      _workflowDataService = workflowDataService;
      _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
      try
      {
        var workflow = await _workflowDataService.GetWorkflowByRequestId(id);
        return Ok(workflow);
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateWorkflow createWorkflow)
    {
      try
      {
        var workFlow = WorkflowFactory.CreateNewWorkflow(createWorkflow.WorkflowName, createWorkflow.SourceEmailAddress, createWorkflow.RequestId, 30);
        await _workflowDataService.SaveWorkflowAsync(workFlow);
        return Ok(createWorkflow);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    //may be need to change the url scheme e.g. id/personal
    [HttpPut("{id}/personal")]
    public async Task<IActionResult> Personal(string id, [FromBody] UpdatePersonalStep updatePersonalStep)
    {
      try
      {
        var workflow = await _workflowDataService.GetWorkflowByRequestId(id);
        if (workflow.CanContinue() && !workflow.IsExpired())
        {
          await _workflowDataService.UpdatePersonalInfo(id, new Personal(updatePersonalStep.FirstName, updatePersonalStep.LastName, updatePersonalStep.Email));
        }
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    
    [HttpPut("{id}/work")]
    public async Task<IActionResult> Work(string id, [FromBody] UpdateWorkStep updateWorkStep)
    {
      try
      {
        var workflow = await _workflowDataService.GetWorkflowByRequestId(id);
        if (workflow.CanContinue() && !workflow.IsExpired())
        {
          await _workflowDataService.UpdateWorkInfo(id, new Work(updateWorkStep.Work));
        }
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    
    [HttpPut("{id}/address")]
    public async Task<IActionResult> Address(string id, [FromBody] UpdateAddressStep updateAddressStep)
    {
      try
      {
        var workflow = await _workflowDataService.GetWorkflowByRequestId(id);
        if (workflow.CanContinue() && !workflow.IsExpired())
        {
          await _workflowDataService.UpdateAddressInfo(id,

            new Address(updateAddressStep.Street, updateAddressStep.City, updateAddressStep.State,
              updateAddressStep.Zip));
        }
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    
    [HttpPut("{id}/result")]
    public async Task<IActionResult> Result(string id, [FromBody] UpdateResultStep updateResultStep)
    {
      try
      {
        var workflow = await _workflowDataService.GetWorkflowByRequestId(id);

        if (workflow.CanContinue() && !workflow.IsExpired())
        {
          await _workflowDataService.UpdateResult(id, workflow.WorkflowData); //here second parameter is un-necessary, may be we can remove it.
        }
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}
