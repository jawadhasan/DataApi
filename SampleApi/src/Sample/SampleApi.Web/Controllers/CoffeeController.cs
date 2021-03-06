using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SampleApi.Web.Hubs;
using SampleApi.Web.Models;

namespace SampleApi.Web.Controllers
{
    [Route("api/[controller]")]
    public class CoffeeController : Controller
    {
      private readonly IHubContext<CoffeeHub> _coffeeHub;
      public CoffeeController(IHubContext<CoffeeHub> coffeeHub)
      {
        _coffeeHub = coffeeHub;
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody]Order order)
      {
        await _coffeeHub.Clients.All.SendAsync("NewOrder", order);
        //Save order somewhere and get order id
        return Accepted(1); //return order id
      }
  }
}
