using System;
using BugBot;
using Xunit;

namespace Tests
{
    public class Tests
    {
        public static void Main(string[] args)
        {
            var test = new BugBot.Test.FeedbackTests();

            test.ListFeedbacks();
        }

    }
}
