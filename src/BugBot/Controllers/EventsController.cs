using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BugBot.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        MicrosoftAppCredentials _botCredentials;
        private IEventActivity _eventActivity;

        public EventsController(IOptions<BotFrameworkCredentials> options, IEventActivity eventActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
            this._eventActivity = eventActivity;
        }
        
        public string[] Get()
        {
            string[] eventList = null; // _eventActivity.GetAll();            

            return eventList;
        }

        [HttpGet("test", Order = -1)]
        public string Test([FromBody] string body)
        {
            // JsonConvert.DeserializeObject(body);
            return body;
        }

        [HttpPost("webhook", Order = -1)]
        public string Webhook([FromBody] string body)
        {
            // JsonConvert.DeserializeObject(body);
            Console.WriteLine(body);

            return body;
        }

        [HttpGet("ev={event_name}")]
        public string Get(string event_name)
        {
            EventModel eventData = _eventActivity.Get(event_name);

            if(eventData == null )
            {
                return "Not registered";
            }

            var client = new ConnectorClient(new Uri(eventData.serviceUrl), _botCredentials);

            var botAccount = new ChannelAccount(name: "BotAccount", id: eventData.botAccount);

            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = botAccount;
            message.Conversation = new ConversationAccount(id: eventData.conversation);
            message.Text = eventData.messageTemplate;

            client.Conversations.SendToConversation((Activity)message);

            return "OK";
        }

        [HttpGet("{sender_id}/{recipient_id}")]
        public string Get(string sender_id, string recipient_id)
        {
            string serviceUrl = "";
            var client = new ConnectorClient(new Uri(serviceUrl), _botCredentials);

            var botAccount = new ChannelAccount(name: "SenderBot", id: sender_id);
            var userAccount = new ChannelAccount(name: "User", id: recipient_id);

            var conversationId = client.Conversations.CreateDirectConversation(botAccount, userAccount);

            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId.Id);
            message.Text = "Hello";

            client.Conversations.SendToConversation((Activity)message);

            return "OK";
        }
    }
}
