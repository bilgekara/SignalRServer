using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Business;
using SignalRServer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly MyBusiness _myBusiness;
        readonly IHubContext<MyHub> _hubContext;

        public HomeController(MyBusiness myBusiness)
        {
            _myBusiness = myBusiness;
            //_hubContext = hubContext;
        }

        [HttpGet("{message}/{message2}")]
        public async Task<IActionResult> Index(string message, string message2)
        {
            await _myBusiness.SendMessageAsync(message, message2);
            return Ok();
        }
    }
}
