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
        private IEventActivity _eventActivity;

        public FeedbackController(IDataActivity dataActivity, IEventActivity eventActivity)
        {
            this._dataActivity = dataActivity;
            this._eventActivity = eventActivity;
        }
        
        [HttpPost("form")]
        public IActionResult Post([FromForm] string feedback, [FromQuery] string user, [FromQuery] string redirect)
        {
            var messageId = _dataActivity.Add(user, feedback);

            EventModel eventData = _eventActivity.Get("bugreport");

            if(eventData != null)
            {

            }

            return Redirect(redirect);
        }
        
    }
}
