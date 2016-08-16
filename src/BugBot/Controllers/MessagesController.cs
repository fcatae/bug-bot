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
    [BotAuthentication]
    public class MessagesController : Controller
    {
        MicrosoftAppCredentials _botCredentials;
        IDataActivity _dataActivity;
        IEventActivity _eventActivity;

        public MessagesController(IOptions<BotFrameworkCredentials> options, IDataActivity dataActivity, IEventActivity eventActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
            this._dataActivity = dataActivity;
            this._eventActivity = eventActivity;
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
            SendController.LAST_SERVICE_URL = activity.ServiceUrl;

            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                if (activity.Text.StartsWith("dbg ") == true)
                {
                    HandleDebug(activity);
                }
                else if (activity.Text.Contains("#bug") == true)
                {
                    // OLD: activity.MentionsRecipient()

                    MessageCreateBug(activity);
                }
                else if(activity.Conversation.IsGroup == false)
                {
                    MessageIamReady(activity);
                }
            }
        }

        void Reply(Activity activity, string message)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl), this._botCredentials);
            var reply = activity.CreateReply(message);
            client.Conversations.ReplyToActivity(reply);
        }

        void HandleDebug(Activity activity)
        {
            string input = activity.Text;

            if (input.Contains("show"))
            {
                Reply(activity, JsonConvert.SerializeObject(activity));
            }

            if (input.Contains("whoami"))
            {                
                Reply(activity, JsonConvert.SerializeObject(new {
                    serviceUrl = activity.ServiceUrl,
                    botAccount = activity.Recipient.Name,
                    botAccountId = activity.Recipient.Id
                }
                ));
            }
            
            if (input.Contains("subscribe"))
            {
                string eventName = "general";
                string template = "general template blablabla";

                _eventActivity.Add(
                    eventName,
                    activity.ServiceUrl,
                    activity.Recipient.Id,
                    activity.Conversation.Id,
                    template
                    );
            }
        }


        void MessageIamReady(Activity activity)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl), this._botCredentials);
            var reply = activity.CreateReply($"I am ready");
            client.Conversations.ReplyToActivity(reply);
        }

        void MessageCreateBug(Activity activity)
        {
            var client = new ConnectorClient(new Uri(activity.ServiceUrl), this._botCredentials);
            int messageId;

            string user = activity.From.Id;
            string message = activity.Text;
            string name = activity.From.Name;

            messageId = _dataActivity.Add($"[{name}] {user}", message);

            var reply = activity.CreateReply($"Created bug #{messageId} in the database");

            client.Conversations.ReplyToActivity(reply);
        }
    }
}
