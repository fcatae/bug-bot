using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Options;

namespace BugBot
{
    public interface IBotService
    {
        IBotMessage TryCreate(Activity activity);
    }

    public interface IBotMessage
    {
        string From { get; }
        string Text { get; }
        string Recipient { get; }
        void Reply(string message);
    }

    public class BotService : IBotService
    {
        private MicrosoftAppCredentials _botCredentials;

        public BotService(string microsoftAppId, string microsoftAppPassword)
        {
            if (microsoftAppId == null || microsoftAppPassword == null)
                throw new InvalidSecretsException("BotFrameworkCredentials");

            this._botCredentials = new MicrosoftAppCredentials(microsoftAppId, microsoftAppPassword);
        }

        public IBotMessage TryCreate(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            if (activity.GetActivityType() != ActivityTypes.Message)
            {
                return null;
            }

            return new BotMessage(activity, _botCredentials);
        }

        public class BotMessage : IBotMessage
        {
            Activity _activity;
            MicrosoftAppCredentials _credentials;

            public BotMessage(Activity activity, MicrosoftAppCredentials credentials)
            {
                if (activity == null)
                    throw new ArgumentNullException(nameof(activity));

                if (credentials == null)
                    throw new ArgumentNullException(nameof(credentials));

                if (activity.GetActivityType() != ActivityTypes.Message)
                    throw new ArgumentException("activity.GetActivityType() != ActivityTypes.Message");

                _activity = activity;
                _credentials = credentials;
            }

            public void Reply(string message)
            {
                var client = new ConnectorClient(new Uri(_activity.ServiceUrl), _credentials);

                var reply = _activity.CreateReply(message);

                client.Conversations.ReplyToActivity(reply);
            }

            public string From
            {
                get
                {
                    return $"[{_activity.From.Id}] {_activity.From.Name}";
                }
            }

            public string Recipient
            {
                get
                {
                    return $"[{_activity.ChannelId} {_activity.Recipient.Id}] {_activity.Recipient.Name}";
                }
            }

            public string Text
            {
                get
                {
                    return _activity.Text;
                }
            }
        }
    }

}
