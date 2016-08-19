using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Options;

namespace BugBot.Controllers
{    
    [Route("api/[controller]")]
    public class FeedbackController : Controller
    {
        public FeedbackController()
        {
        }
        
        [HttpPost("form")]
        public IActionResult Post([FromForm] string feedback, [FromQuery] string user, [FromQuery] string redirect)
        {
            return Redirect(redirect);
        }
        
    }
}
