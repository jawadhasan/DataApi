using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampleApi.Core;
using SampleApi.Data;

namespace SampleApi.Web.Controllers
{

  [Authorize]
  [Route("api/[controller]")]
  public class CompaniesController : BaseController
  {
    private readonly AppDbContext _db;
    private readonly ILogger<CompaniesController> _logger;

    public CompaniesController(AppDbContext db, ILogger<CompaniesController> logger)
    {
      _db = db;
      _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var result = await _db.Companies.ToListAsync();
        return Ok(result);
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return Error(e.Message);
      }
    }


    [HttpGet("{id}", Name = "GetCompanyRoute")]
    public async Task<IActionResult> Companies(int id)
    {
      try
      {
        var result = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
        return Ok(result);
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return Error(e.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Company createCompany)
    {
      try
      {
        _db.Companies.Add(createCompany);
        await _db.SaveChangesAsync();
        return Ok(createCompany);
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return Error(e.Message);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]Company updateCompany)
    {
      try
      {
        _db.Companies.Update(updateCompany);
        await _db.SaveChangesAsync();
        return Ok(updateCompany);
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return Error(e.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var removableCompany = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
        if (removableCompany != null)
        {
          _db.Companies.Remove(removableCompany);
          await _db.SaveChangesAsync();
        }
        return Ok();
      }
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return Error(e.Message);
      }
    }

  }
}
