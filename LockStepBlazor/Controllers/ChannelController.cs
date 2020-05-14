using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LockStepBlazor.Controllers
{
    public class ChannelController : Controller
    {
        public IActionResult Send()
        {
            Task.Run(() =>
            {
                Task.Delay(100).Wait();

                Task.Delay(200).Wait();
            });

            return Ok();
        }
        public async Task<bool> SendC([FromServices] Channel<string> channel)
        {
            await channel.Writer.WriteAsync("Hello");
            return true;
        }
    }
}
