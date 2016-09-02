using System;
using BugBot;
using Xunit;
using Microsoft.Extensions.Options;
using Microsoft.Bot.Connector;

namespace Tests
{
    public class Tests
    {
        public static void Main(string[] args)
        {
            var test = new Tests();

            test.TestPost();
        }

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

        [Fact]
        public void TestMainCommand()
        {
            Assert.Equal((new CommandLine("program")).Command, "program");
            Assert.Equal((new CommandLine("   program 1 2 3")).Command, "program");
            Assert.Equal((new CommandLine("")).Command, "");
        }

        [Fact]
        public void TestParameters() 
        {
            var cmd = new CommandLine("dbg teste teste2  teste3");

            Assert.Equal(cmd.GetValue(0), "teste");
            Assert.Equal(cmd.GetValue(1), "teste2");
            Assert.Equal(cmd.GetValue(2), "teste3");
        }

        [Fact]
        public void TestShortNamedParameters()
        {
            var cmd = new CommandLine("prog -a teste2 -b teste4 -c -d");

            Assert.Equal(cmd.GetValue("a"), "teste2");
            Assert.Equal(cmd.GetValue("b"), "teste4");

            Assert.Equal(cmd.GetValue("c"), "");
            Assert.Equal(cmd.GetValue("d"), "");

            Assert.Equal(cmd.GetValue("i"), null);
        }

        [Fact]
        public void TestNamedParameters()
        {
            var cmd = new CommandLine("prog --teste teste2 --teste3 teste4 --empty1 --empty_end");

            Assert.Equal(cmd.GetValue("teste"), "teste2");
            Assert.Equal(cmd.GetValue("teste3"), "teste4");

            Assert.Equal(cmd.GetValue("empty1"), "");
            Assert.Equal(cmd.GetValue("empty_end"), "");

            Assert.Equal(cmd.GetValue("invalid"), null);           
        }

        [Fact]
        public void TestMultipleShortParameters()
        {
            var cmd = new CommandLine("prog -abc def");

            Assert.Equal(cmd.GetValue("a"), "");
            Assert.Equal(cmd.GetValue("b"), "");
            Assert.Equal(cmd.GetValue("c"), "");

            Assert.Equal(cmd.GetValue(0), "def");
        }
    }
}
