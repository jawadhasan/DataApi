using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SampleApi.Web.Helpers;

namespace SampleApi.Web.Hubs
{
    public class CoffeeHub : Hub
    {
      private readonly OrderChecker _orderChecker;

      public CoffeeHub(OrderChecker orderChecker)
      {
        _orderChecker = orderChecker;
      }

      //Called by Angular-Client
      public async Task SendMessage(string user, string message)
      {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
      }

     //Currently being invoked by coffee-order.component on client side.
      public async Task GetUpdateForOrder(int orderId)
      {
        CheckResult result;
        do
        {
          result = _orderChecker.GetUpdate(orderId);
          Thread.Sleep(1000);
          if (result.New)
            await Clients.Caller.SendAsync("ReceiveOrderUpdate", result.Update);
        } while (!result.Finished);

        await Clients.Caller.SendAsync("Finished");
      }
  }
}
