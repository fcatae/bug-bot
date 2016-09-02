using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Bot.Connector;
using BugBot.Controllers;

namespace BugBot.Test
{
    public class TestMessageController
    {
        MessagesController _controller;

        public TestMessageController()
        {
            BotFrameworkCredentials credentials = new BotFrameworkCredentials()
            {
                MicrosoftAppId = "appid",
                MicrosoftAppPassword = "appsecret"
            };

            this._controller = new MessagesController(Options.Create(credentials), null, null);
        }

        public void Post(TestActivity testActivity)
        {
            _controller.Post(testActivity.Activity);
        }
    }
}
