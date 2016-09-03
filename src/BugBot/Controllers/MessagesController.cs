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
        private IBotService _botService;
        IDataActivity _dataActivity;
        IEventActivity _eventActivity;

        public MessagesController(IOptions<BotFrameworkCredentials> options, IBotService botService, IDataActivity dataActivity, IEventActivity eventActivity)
        {
            if (options.Value.MicrosoftAppId == null || options.Value.MicrosoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(options.Value.MicrosoftAppId, options.Value.MicrosoftAppPassword);
            this._botService = botService;
            this._dataActivity = dataActivity;
            this._eventActivity = eventActivity;
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody] Activity activity)
        {
            SendController.LAST_SERVICE_URL = activity.ServiceUrl;

            var message = _botService.TryCreate(activity);

            if( message != null )
            {
                if( message.Text.StartsWith("/") )
                {
                    CommandLine cmdLine = new CommandLine(message.Text);

                    string command = message.Text.Substring(1);

                    switch (command)
                    {
                        case "ping":
                            break;
                        case "help":
                            break;

                        // Feedback
                        case "list":
                            break;
                        case "ignore":
                            break;
                        case "promote":
                            break;

                        // Feedback v2
                        case "notify":
                            break;

                        // Events
                        case "event":
                            break;

                        // Security v2
                        case "admin":
                            break;

                        default:
                            break;
                    }

                    // commandline = new commandLine(args)
                }
            }

            // Legacy

            CommandLine cmd = new CommandLine(message.Text);

            if (activity.GetActivityType() == ActivityTypes.Message)
            {                
                if (cmd.Command == "/dbg") // activity.Text.StartsWith("dbg ") == true
                {
                    HandleDebug(message, activity);
                }
                else if (activity.Text.Contains("#bug") == true)
                {
                    MessageCreateBug(message);
                }
                else if(activity.Conversation.IsGroup == false)
                {
                    MessageIamReady(message);
                }
            }
        }

        void Reply(Activity activity, string message)
        {
            var bmessage = _botService.TryCreate(activity);
            bmessage.Reply(message);
        }

        void HandleDebug(IBotMessage message, Activity activity)
        {
            string input = message.Text;

            CommandLine cmd = new CommandLine(message.Text);

            if (cmd.GetValue(0) == "show")
            {
                Reply(activity, JsonConvert.SerializeObject(activity));
            }

            if (cmd.GetValue(0) == "whoami")
            {
                message.Reply(message.Recipient);

                //Reply(activity, JsonConvert.SerializeObject(new {
                //    serviceUrl = activity.ServiceUrl,
                //    botAccount = activity.Recipient.Name,
                //    botAccountId = activity.Recipient.Id
                //}
                //));
            }

            if (cmd.GetValue(0) == "subscribe")
            {
                // dbg(1) subscribe(2) eventname(3) message(4)
                var cmds = input.Split(new char[] { '\t', ' ' }, 4, StringSplitOptions.RemoveEmptyEntries);

                if(cmds.Length == 4)
                {
                    string eventName = cmds[2];
                    string template = cmds[3].Trim(new char[] { ' ', '\t', '"' });

                    _eventActivity.Add(
                        eventName,
                        activity.ServiceUrl,
                        activity.Recipient.Id,
                        activity.Conversation.Id,
                        template
                        );

                    Reply(activity, "Done");
                }
                else
                {
                    Reply(activity, "Wrong number of parameters");
                }
            }

            if (cmd.GetValue(0) == "createvsts")
            { 
                var cmds = input.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                VstsCredentials cred;

                if (cmds.Length == 4)
                {
                    string account = cmds[2];
                    string password = cmds[3];

                    cred = new VstsCredentials(account, password);
                }
                else
                {
                    string account = cmds[2];
                    string project = cmds[3];
                    string password = cmds[4];

                    cred = new VstsCredentials(account, project, password);
                }

                StoreVSTSData(cred);

                message.Reply("Cred Stored");
            }

            if (cmd.GetValue(0) == "createbug")                
            {
                var cmds = input.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                VstsCredentials cred = StoreVSTSData(null);                

                var vsClient = new VstsClient(cred);

                int bugid = vsClient.CreateBugAsync(cmds[2], cmds[3]).Result;
                string link = vsClient.GetLink(bugid);

                message.Reply($"Created bug at {link}");
            }

            if (cmd.GetValue(0) == "list")
            {
                _dataActivity.List();
            }
        }

        static VstsCredentials _StoreVSTSDataObject;

        VstsCredentials StoreVSTSData(VstsCredentials cred)
        {
            if(cred != null)
            {
                _StoreVSTSDataObject = cred;
            }
            return _StoreVSTSDataObject;
        }

        void MessageIamReady(IBotMessage message)
        {
            message.Reply($"I am ready");
        }

        void MessageCreateBug(IBotMessage message)
        {
            int messageId = _dataActivity.Add(message.From, message.Text);

            message.Reply($"Created bug #{messageId} in the database");
        }
    }
}
