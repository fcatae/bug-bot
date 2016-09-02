using System;
using BugBot;
using Xunit;

namespace BugBot.Test
{
    public class FeedbackTests
    {
        public void ListFeedbacks()
        {
            TestMessageController controller = new TestMessageController();
            TestActivity activity = TestActivity.DebugCall("list");

            controller.Post(activity);
        }
    }
}
