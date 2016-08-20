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
        private MicrosoftAppCredentials _botCredentials;
        private IDataActivity _dataActivity;
        private IEventActivity _eventActivity;

        public FeedbackController(IOptions<BotFrameworkCredentials> options, IDataActivity dataActivity, IEventActivity eventActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);

            this._dataActivity = dataActivity;
            this._eventActivity = eventActivity;
        }
        
        [HttpGet("teste")]
        public void Teste()
        {
            dynamic objFeedback = new { user = "usr", feedback = "text" };
            string  strFeedback = Newtonsoft.Json.JsonConvert.SerializeObject(objFeedback);
            object jsonFeedback = Newtonsoft.Json.JsonConvert.DeserializeObject(strFeedback);

            string s = FormatString("teste {user} ok", jsonFeedback);
        }

        [HttpPost("form")]
        public IActionResult Post([FromForm] string feedback, [FromQuery] string user, [FromQuery] string redirect)
        {
            var messageId = _dataActivity.Add(user, feedback);

            EventModel eventData = _eventActivity.Get("bugreport");

            if(eventData != null)
            {
                dynamic objFeedback = new { user = user, feedback = feedback };
                string strFeedback = Newtonsoft.Json.JsonConvert.SerializeObject(objFeedback);
                object jsonFeedback = Newtonsoft.Json.JsonConvert.DeserializeObject(strFeedback);

                SendMessage(eventData, jsonFeedback);
            }

            return Redirect(redirect);
        }

        // duplicated code
        string FormatString(string template, object obj)
        {
            Newtonsoft.Json.Linq.JObject jObject = (Newtonsoft.Json.Linq.JObject)obj;

            return jObject.ToString(template);
        }

        // duplicated code
        void SendMessage(EventModel eventData, object messageData)
        {
            var client = new ConnectorClient(new Uri(eventData.serviceUrl), _botCredentials);

            var botAccount = new ChannelAccount(name: "BotAccount", id: eventData.botAccount);

            string messageText = (messageData != null) ? FormatString(eventData.messageTemplate, messageData) : eventData.messageTemplate;

            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = botAccount;
            message.Conversation = new ConversationAccount(id: eventData.conversation);
            message.Text = messageText;

            client.Conversations.SendToConversation((Activity)message);
        }
    }
}
