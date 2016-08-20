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
        private IDataActivity _dataActivity;

        public FeedbackController(IDataActivity dataActivity)
        {
            this._dataActivity = dataActivity;
        }
        
        [HttpPost("form")]
        public IActionResult Post([FromForm] string feedback, [FromQuery] string user, [FromQuery] string redirect)
        {
            Console.WriteLine("FeedbackController::POST");
            Console.WriteLine("FeedbackController::POST");
            Console.WriteLine("FeedbackController::POST");
            Console.WriteLine("FeedbackController::POST");
            Console.WriteLine("FeedbackController::POST");

            var messageId = _dataActivity.Add(user, feedback);

            return Redirect(redirect);
        }
        
    }
}
