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
    public class SendController : Controller
    {
        MicrosoftAppCredentials _botCredentials;

        public SendController(IOptions<BotFrameworkCredentials> options)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
        }
        
        public string Get()
        {
            var client = new ConnectorClient(new Uri("http://localhost:9000/"), _botCredentials);

            var userAccount = new ChannelAccount(name: "FabricioCatae", id: "@fcatae");
            var botAccount = new ChannelAccount(name: "b", id: "@b");

            var conversationId = client.Conversations.CreateDirectConversation(botAccount, userAccount);

            IMessageActivity message = Activity.CreateMessageActivity();
                        message.From = botAccount;
                        message.Recipient = userAccount;
                        message.Conversation = new ConversationAccount(id: conversationId.Id);
                        message.Text = "Hello";

            client.Conversations.SendToConversation((Activity)message);
            
            return "OK!";
        }

        [HttpGet("{recipient_id}")]
        public string Get(string recipient_id)
        {
            var client = new ConnectorClient(new Uri("http://localhost:9000/"), _botCredentials);

            var userAccount = new ChannelAccount(name: "FabricioCatae", id: "@fcatae");
            var botAccount = new ChannelAccount(name: "b", id: "@b");

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
