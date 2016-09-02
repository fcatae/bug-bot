using System;
using Xunit;

namespace BugBot.DatabaseTest
{
    public class CommentsTests
    {
        DataActivity _activity;

        public CommentsTests()
        {
            _activity = new DataActivity(Tests.CONNECTION_STRING);
        }

        [Fact]
        public void ListComments()
        {
            var list = _activity.List();

            Assert.NotEmpty(list);
        }
    }
}
