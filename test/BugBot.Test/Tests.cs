using System;
using BugBot;
using Xunit;

namespace BugBot.Test
{
    public class Tests
    {
        public const string BOTID = "@test";
        public const string BOTNAME = "testbot";

        public static void Main(string[] args)
        {
            var test = new BugBot.Test.FeedbackTests();

            test.ListFeedbacks();
        }
    }
}
