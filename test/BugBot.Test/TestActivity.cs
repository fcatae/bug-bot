using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BugBot.Test
{
    public class TestActivity
    {
        public Activity Activity;

        public TestActivity(string text)
        {
            Activity activity = new Activity()
            {
                Recipient = new ChannelAccount(Tests.BOTID, Tests.BOTNAME),
                Type = "message",
                Text = text
            };

            this.Activity = activity;
        }

        public static TestActivity DebugCall(string text)
        {
            return new TestActivity($"/dbg {text}");
        }

        public static TestActivity CommandCall(string text)
        {
            return new TestActivity($"{Tests.BOTID} {text}");
        }
    }
}
