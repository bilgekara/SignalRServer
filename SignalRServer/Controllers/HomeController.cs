using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Business;
using SignalRServer.Hubs;
using SignalRServer.TimerFeatures;
using System.Threading.Tasks;

// my business ı kaldır hub ve controller üzerinden yürüsün işler
namespace SignalRServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly  MyBusiness _myBusiness;
        readonly IHubContext<MyHub> _hubContext;

        public HomeController(IHubContext<MyHub> hubContext)
        {
            _myBusiness = new MyBusiness(hubContext);

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var timerManager = new TimerManager(() => _hubContext.Clients.All.SendAsync("transferchartdata", "slm"));

            var timerManager = new TimerManager(() => _myBusiness.SendTopicAsync());
            //var message = new Nessage() { typeof = "warning", Information = "text message" + Guid.NewGuid().ToString() };
            return Ok(new { Message = "Request Completed" });
        }


        [HttpGet("{userName}/{passw}/{urlx}")]
        public async Task<IActionResult> Get(string userName, string passw, string urlx)
        {
            await _myBusiness.SendMessageAsync(userName, passw, urlx);
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> SendMessage()
        //{
        //    //await _myBusiness.SendMessageAsync(userName, passw, urlx);
        //    return Ok();
        //}
    }
}

//protected IHubContext<SystemHub> _context { get; }

//public SystemHub(IServiceProvider serviceProvider, IHubContext<SystemHub> context)
//{
//    Id = Guid.NewGuid();
//    Console.WriteLine($"SystemHub({Id}) Started");

//    ServiceProvider = serviceProvider;
//    _context = context;
//}

//_context.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);