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
        
        public string Get()
        {
            //string serviceUrl = "";
            //var client = new ConnectorClient(new Uri(serviceUrl), _botCredentials);

            //var userAccount = new ChannelAccount(name: "FabricioCatae", id: "@fcatae");
            //var botAccount = new ChannelAccount(name: "b", id: "@b");

            //var conversationId = client.Conversations.CreateDirectConversation(botAccount, userAccount);

            //IMessageActivity message = Activity.CreateMessageActivity();
            //            message.From = botAccount;
            //            message.Recipient = userAccount;
            //            message.Conversation = new ConversationAccount(id: conversationId.Id);
            //            message.Text = "Hello";

            //client.Conversations.SendToConversation((Activity)message);
            
            return "OK!";
        }

        [HttpGet("{sender_id}/{recipient_id}/{conversation_id}")]
        public string Get(string sender_id, string recipient_id, string conversation_id)
        {
            //string serviceUrl = "";
            //var client = new ConnectorClient(new Uri(serviceUrl), _botCredentials);

            //var botAccount = new ChannelAccount(name: "SenderBot", id: sender_id);
            //var userAccount = new ChannelAccount(name: "User", id: recipient_id);            

            //IMessageActivity message = Activity.CreateMessageActivity();
            //message.From = botAccount;
            //message.Recipient = userAccount;
            //message.Conversation = new ConversationAccount(id: conversation_id);
            //message.Text = "Hello Conversation";

            //client.Conversations.SendToConversation((Activity)message);

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
