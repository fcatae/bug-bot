using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;

namespace BugBot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Activity activity)
        {
            if(activity.GetActivityType() == ActivityTypes.Message)
            {
                var client = new ConnectorClient(new Uri(activity.ServiceUrl));

                var reply = activity.CreateReply("I am here");

                client.Conversations.ReplyToActivity(reply);
            }
        }
    }
}
