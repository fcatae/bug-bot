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
    [BotAuthentication]
    public class MessagesController : Controller
    {
        MicrosoftAppCredentials _botCredentials;
        IDataActivity _dataActivity;

        public MessagesController(IOptions<BotFrameworkCredentials> options, IDataActivity dataActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
            this._dataActivity = dataActivity;
        }

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
                var client = new ConnectorClient(new Uri(activity.ServiceUrl), this._botCredentials);

                string user = activity.From.Id;
                string message = activity.Text;

                _dataActivity.Add(user, message);

                var reply = activity.CreateReply("I am here");

                client.Conversations.ReplyToActivity(reply);
            }
        }
    }
}
