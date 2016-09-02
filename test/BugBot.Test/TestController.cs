using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Bot.Connector;

namespace BugBot.Test
{
    public class TestController
    {
        public void TestPost()
        {
            BotFrameworkCredentials credentials = new BotFrameworkCredentials()
            {
                MicrosoftAppId = "appid",
                MicrosoftAppPassword = "appsecret"
            };

            var controller = new BugBot.Controllers.MessagesController(Options.Create(credentials), null, null);

            Activity activity = new Activity()
            {
                Type = "message",
                Text = "/dbg a b c"
            };

            controller.Post(activity);
        }
    }
}
